using System.IO.Compression;
using System.Runtime.InteropServices;
using DotnetSetupDoctor.Output;
using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Bundle;

public static class BundleWriter
{
    public static async Task<string> WriteAsync(
        IReadOnlyList<CheckResult> results,
        string? outDir,
        bool zip
    )
    {
        var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
        var folder = outDir?.Trim();
        if (string.IsNullOrWhiteSpace(folder))
            folder = Path.Combine(Environment.CurrentDirectory, $"dotnet-setup-doctor-bundle-{stamp}");

        Directory.CreateDirectory(folder);

        // Reports
        await File.WriteAllTextAsync(Path.Combine(folder, "report.txt"),
            ReportFormatter.Format(results, emoji: true));

        // Always write JSON with string enums (same as JsonRenderer)
        var json = OutputJson(results);
        await File.WriteAllTextAsync(Path.Combine(folder, "report.json"), json);

        // Extra command outputs
        await WriteCommand(folder, "dotnet-info.txt", "dotnet", "--info");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            await WriteCommand(folder, "where-dotnet.txt", "where", "dotnet");
            await WriteCommand(folder, "where-msbuild.txt", "where", "msbuild");
            await WriteCommand(folder, "where-git.txt", "where", "git");
            await WriteCommand(folder, "where-winget.txt", "where", "winget");
        }

        await File.WriteAllTextAsync(Path.Combine(folder, "env.txt"),
            $"OS: {Environment.OSVersion}{Environment.NewLine}" +
            $"Machine: {Environment.MachineName}{Environment.NewLine}" +
            $"User: {Environment.UserName}{Environment.NewLine}" +
            $"64-bit OS: {Environment.Is64BitOperatingSystem}{Environment.NewLine}" +
            $"64-bit Proc: {Environment.Is64BitProcess}{Environment.NewLine}"
        );

        if (!zip)
            return folder;

        var zipPath = folder.TrimEnd(Path.DirectorySeparatorChar) + ".zip";
        if (File.Exists(zipPath)) File.Delete(zipPath);

        ZipFile.CreateFromDirectory(folder, zipPath, CompressionLevel.Optimal, includeBaseDirectory: false);
        return zipPath;
    }

    private static async Task WriteCommand(string folder, string fileName, string exe, string args)
    {
        var r = await ProcessRunner.RunAsync(exe, args);
        var content =
            $"$ {exe} {args}{Environment.NewLine}" +
            $"ExitCode: {r.ExitCode}{Environment.NewLine}" +
            $"{Environment.NewLine}--- STDOUT ---{Environment.NewLine}{r.Stdout}{Environment.NewLine}" +
            $"{Environment.NewLine}--- STDERR ---{Environment.NewLine}{r.Stderr}{Environment.NewLine}";

        await File.WriteAllTextAsync(Path.Combine(folder, fileName), content);
    }

    private static string OutputJson(IReadOnlyList<CheckResult> results)
    {
        // Local helper to avoid dependency on renderer for bundle.
        return System.Text.Json.JsonSerializer.Serialize(results, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        });
    }
}
