using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class DotnetSdkCheck : ICheck
{
    public string Name => ".NET SDK";

    public async Task<CheckResult> RunAsync()
    {
        var r = await ProcessRunner.RunAsync("dotnet", "--list-sdks");
        if (r.ExitCode != 0)
        {
            return new CheckResult(
                Name,
                Status.Fail,
                "dotnet not found or not runnable",
                new[] { string.IsNullOrWhiteSpace(r.Stderr) ? "dotnet command failed" : r.Stderr },
                "Install the .NET SDK (winget: Microsoft.DotNet.SDK.8) and ensure dotnet is on PATH."
            );
        }

        var lines = r.Stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (lines.Length == 0)
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "dotnet runs, but no SDKs were listed",
                Array.Empty<string>(),
                "Install at least one .NET SDK (recommended: 8.x LTS)."
            );
        }

        return new CheckResult(Name, Status.Pass, $"Found {lines.Length} SDK(s)", lines);
    }
}
