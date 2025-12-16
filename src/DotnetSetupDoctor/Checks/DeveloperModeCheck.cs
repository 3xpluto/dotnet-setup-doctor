using System.Runtime.InteropServices;
using DotnetSetupDoctor.Utils;

namespace DotnetSetupDoctor.Checks;

public sealed class DeveloperModeCheck : ICheck
{
    public string Name => "Developer Mode";

    public Task<CheckResult> RunAsync()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Task.FromResult(new CheckResult(Name, Status.Info, "Non-Windows OS", Array.Empty<string>()));
        }

        // HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock AllowDevelopmentWithoutDevLicense (DWORD) 1 = enabled
        var val = RegistryReader.TryReadDword(
            Microsoft.Win32.RegistryHive.LocalMachine,
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
            "AllowDevelopmentWithoutDevLicense"
        );

        if (val is null)
        {
            return Task.FromResult(new CheckResult(
                Name,
                Status.Warn,
                "Couldn't read Developer Mode setting",
                Array.Empty<string>(),
                "Enable Developer Mode in Windows Settings (Privacy & security -> For developers)."
            ));
        }

        return Task.FromResult(val == 1
            ? new CheckResult(Name, Status.Pass, "Enabled", new[] { "AllowDevelopmentWithoutDevLicense=1" })
            : new CheckResult(Name, Status.Warn, "Disabled", new[] { $"AllowDevelopmentWithoutDevLicense={val}" },
                "Enable Developer Mode for smoother dev tooling experience (symlinks, dev features)."));
    }
}
