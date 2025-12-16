using System.Runtime.InteropServices;
using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class LongPathsCheck : ICheck
{
    public string Name => "Long Paths";

    public Task<CheckResult> RunAsync()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Task.FromResult(new CheckResult(Name, Status.Info, "Non-Windows OS", Array.Empty<string>()));
        }

        // HKLM\SYSTEM\CurrentControlSet\Control\FileSystem LongPathsEnabled (DWORD) 1 = enabled
        var val = RegistryReader.TryReadDword(
            Microsoft.Win32.RegistryHive.LocalMachine,
            @"SYSTEM\CurrentControlSet\Control\FileSystem",
            "LongPathsEnabled"
        );

        if (val is null)
        {
            return Task.FromResult(new CheckResult(
                Name,
                Status.Warn,
                "Couldn't read LongPathsEnabled",
                Array.Empty<string>(),
                "Enable Win32 long paths in Windows settings/group policy, then reboot/sign out."
            ));
        }

        return Task.FromResult(val == 1
            ? new CheckResult(Name, Status.Pass, "Enabled", new[] { "LongPathsEnabled=1" })
            : new CheckResult(Name, Status.Warn, "Disabled", new[] { $"LongPathsEnabled={val}" },
                "Enable Win32 long paths to avoid path-too-long build errors."));
    }
}
