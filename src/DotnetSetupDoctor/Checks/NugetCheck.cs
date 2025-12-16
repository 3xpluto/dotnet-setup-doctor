using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class NugetCheck : ICheck
{
    public string Name => "NuGet";

    public async Task<CheckResult> RunAsync()
    {
        var r = await ProcessRunner.RunAsync("dotnet", "nuget locals global-packages -l");
        if (r.ExitCode != 0)
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "Couldn't query NuGet global packages location",
                new[] { string.IsNullOrWhiteSpace(r.Stderr) ? r.Stdout : r.Stderr },
                "Ensure dotnet is installed and runnable."
            );
        }

        return new CheckResult(Name, Status.Info, "NuGet local cache info", new[] { r.Stdout });
    }
}
