using System.Diagnostics;

namespace DotnetSetupDoctor.Utils;

public sealed record ProcessResult(int ExitCode, string Stdout, string Stderr);

public static class ProcessRunner
{
    public static async Task<ProcessResult> RunAsync(string fileName, string arguments, string? workingDirectory = null)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory ?? Environment.CurrentDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var p = Process.Start(psi);
            if (p is null) return new ProcessResult(1, "", $"Failed to start {fileName}");

            var stdout = await p.StandardOutput.ReadToEndAsync();
            var stderr = await p.StandardError.ReadToEndAsync();
            await p.WaitForExitAsync();

            return new ProcessResult(p.ExitCode, stdout.Trim(), stderr.Trim());
        }
        catch (Exception ex)
        {
            return new ProcessResult(1, "", ex.Message);
        }
    }
}
