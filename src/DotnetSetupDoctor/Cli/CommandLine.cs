namespace DotnetSetupDoctor.Cli;

public sealed record ArgsParsed(
    string Command,
    bool Json,
    bool Smoke,
    bool Zip,
    string? OutDir,
    bool ShowHelp,
    bool ShowVersion
);

public static class CommandLine
{
    public static ArgsParsed Parse(string[] args)
    {
        var command = "doctor";
        var json = false;
        var smoke = false;
        var zip = false;
        string? outDir = null;
        var showHelp = false;
        var showVersion = false;

        for (int i = 0; i < args.Length; i++)
        {
            var a = args[i];

            if (a is "-h" or "--help" or "help") { showHelp = true; continue; }
            if (a is "-V" or "--version") { showVersion = true; continue; }

            if (a.Equals("--json", StringComparison.OrdinalIgnoreCase)) { json = true; continue; }
            if (a.Equals("--smoke", StringComparison.OrdinalIgnoreCase)) { smoke = true; continue; }
            if (a.Equals("--zip", StringComparison.OrdinalIgnoreCase)) { zip = true; continue; }

            if (a.Equals("--out", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < args.Length)
                {
                    outDir = args[i + 1];
                    i++;
                }
                continue;
            }

            // First non-flag token becomes command
            if (!a.StartsWith("-", StringComparison.Ordinal))
                command = a.ToLowerInvariant();
        }

        return new ArgsParsed(command, json, smoke, zip, outDir, showHelp, showVersion);
    }

    public static string HelpText() =>
"""
dotnet-setup-doctor

Usage:
  dotnet-setup-doctor [doctor] [--json] [--smoke]
  dotnet-setup-doctor bundle [--out <dir>] [--zip] [--smoke]
  dotnet-setup-doctor fix-script
  dotnet-setup-doctor --help
  dotnet-setup-doctor --version

Commands:
  doctor       Run environment checks and print a report (default)
  bundle       Write a diagnostics bundle (report + command outputs) to a folder (and optional zip)
  fix-script   Print a PowerShell script with recommended installs/fixes

Options:
  --json       Output doctor results as JSON (doctor command only)
  --smoke      Run an end-to-end build smoke test (slower)
  --out <dir>  Output folder for bundle (default: auto timestamped folder)
  --zip        Also create a .zip next to the bundle folder
""";
}
