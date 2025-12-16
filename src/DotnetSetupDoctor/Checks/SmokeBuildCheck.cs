using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class SmokeBuildCheck : ICheck
{
    public string Name => "Smoke build";

    public async Task<CheckResult> RunAsync()
    {
        var tempRoot = Path.Combine(Path.GetTempPath(), "dotnet-setup-doctor-smoke-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempRoot);

        try
        {
            var r1 = await ProcessRunner.RunAsync("dotnet", "new console -n SmokeApp", workingDirectory: tempRoot);
            if (r1.ExitCode != 0)
            {
                return new CheckResult(Name, Status.Fail, "dotnet new failed",
                    new[] { r1.Stdout, r1.Stderr }.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
                    "dotnet SDK install may be broken."
                );
            }

            var projDir = Path.Combine(tempRoot, "SmokeApp");
            var r2 = await ProcessRunner.RunAsync("dotnet", "build -c Release", workingDirectory: projDir);
            if (r2.ExitCode != 0)
            {
                return new CheckResult(Name, Status.Fail, "dotnet build failed",
                    new[] { r2.Stdout, r2.Stderr }.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
                    "Check workloads, SDK integrity, and restore sources."
                );
            }

            return new CheckResult(Name, Status.Pass, "dotnet new + build succeeded", Array.Empty<string>());
        }
        finally
        {
            try { Directory.Delete(tempRoot, recursive: true); } catch { /* ignore */ }
        }
    }
}
