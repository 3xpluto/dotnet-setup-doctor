using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class GitCheck : ICheck
{
    public string Name => "Git";

    public async Task<CheckResult> RunAsync()
    {
        var r = await ProcessRunner.RunAsync("git", "--version");
        if (r.ExitCode != 0)
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "git not found",
                new[] { string.IsNullOrWhiteSpace(r.Stderr) ? "git command failed" : r.Stderr },
                "Install Git (winget: Git.Git)."
            );
        }

        return new CheckResult(Name, Status.Pass, r.Stdout, Array.Empty<string>());
    }
}
