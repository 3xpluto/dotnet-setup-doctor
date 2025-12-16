using System.Runtime.InteropServices;
using DotnetSetupDoctor.Checks;

namespace DotnetSetupDoctor;

public static class CheckRegistry
{
    public static IReadOnlyList<ICheck> BuildDefaultChecks(bool includeSmoke)
    {
        var list = new List<ICheck>
        {
            new WindowsInfoCheck(),
            new PathCheck(),
            new EnvVarCheck(),
            new DotnetSdkCheck(),
            new MsbuildCheck(),
            new NugetCheck(),
            new GitCheck(),
        };

        // Windows-specific checks (tool is aimed at Windows)
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            list.Add(new WingetCheck());
            list.Add(new LongPathsCheck());
            list.Add(new DeveloperModeCheck());
        }

        if (includeSmoke)
            list.Add(new SmokeBuildCheck());

        return list;
    }
}
