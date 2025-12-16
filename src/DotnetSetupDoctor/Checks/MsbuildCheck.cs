using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class MsbuildCheck : ICheck
{
    public string Name => "MSBuild / Visual Studio Build Tools";

    public async Task<CheckResult> RunAsync()
    {
        // Try msbuild first (if it's on PATH)
        var msbuild = await ProcessRunner.RunAsync("msbuild", "-version");
        if (msbuild.ExitCode == 0)
        {
            return new CheckResult(Name, Status.Pass, "msbuild found", new[] { msbuild.Stdout });
        }

        // Try vswhere (common location)
        var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        var vswherePath = Path.Combine(programFilesX86, "Microsoft Visual Studio", "Installer", "vswhere.exe");

        if (!File.Exists(vswherePath))
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "msbuild not found and vswhere not present",
                new[] { "Expected vswhere at: " + vswherePath },
                "Install Visual Studio Build Tools or Visual Studio (includes MSBuild)."
            );
        }

        var vs = await ProcessRunner.RunAsync(
            vswherePath,
            "-latest -products * -requires Microsoft.Component.MSBuild -property installationPath"
        );

        if (vs.ExitCode != 0 || string.IsNullOrWhiteSpace(vs.Stdout))
        {
            return new CheckResult(
                Name,
                Status.Warn,
                "vswhere found, but no MSBuild component detected",
                new[] { string.IsNullOrWhiteSpace(vs.Stderr) ? vs.Stdout : vs.Stderr },
                "Install Visual Studio Build Tools with MSBuild components/workloads."
            );
        }

        return new CheckResult(
            Name,
            Status.Pass,
            "Visual Studio installation with MSBuild detected",
            new[] { "VS install path: " + vs.Stdout }
        );
    }
}
