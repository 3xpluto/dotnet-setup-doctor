using Microsoft.Win32;

namespace DotnetSetupDoctor.Utils;

public static class RegistryReader
{
    public static int? TryReadDword(RegistryHive hive, string subKey, string valueName)
    {
        try
        {
            using var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Registry64);
            using var key = baseKey.OpenSubKey(subKey);
            if (key is null) return null;

            var val = key.GetValue(valueName);
            if (val is int i) return i;
            return null;
        }
        catch
        {
            return null;
        }
    }
}
