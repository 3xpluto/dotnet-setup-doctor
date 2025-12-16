using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class WingetCheck : ICheck
{
    public string Name => "winget";

    public async Task<CheckResult> RunAsync()
    {
        var r = await ProcessRunner.RunAsync("winget", "--version");
        if (r.ExitCode != 0)
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "winget not found",
                new[] { string.IsNullOrWhiteSpace(r.Stderr) ? "winget command failed" : r.Stderr },
                "Install/update 'App Installer' from Microsoft Store (provides winget)."
            );
        }

        return new CheckResult(Name, Status.Pass, r.Stdout, Array.Empty<string>());
    }
}
