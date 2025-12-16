using System.Reflection;
using DotnetSetupDoctor;
using DotnetSetupDoctor.Bundle;
using DotnetSetupDoctor.Cli;
using DotnetSetupDoctor.Fix;
using DotnetSetupDoctor.Output;

var parsed = CommandLine.Parse(args);

if (parsed.ShowHelp)
{
    Console.WriteLine(CommandLine.HelpText());
    return 0;
}

if (parsed.ShowVersion)
{
    var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
    var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    var ver = info ?? asm.GetName().Version?.ToString() ?? "unknown";
    Console.WriteLine(ver);
    return 0;
}

switch (parsed.Command)
{
    case "doctor":
    {
        var checks = CheckRegistry.BuildDefaultChecks(includeSmoke: parsed.Smoke);
        var results = await CheckRunner.RunAllAsync(checks);

        // Prefer UTF-8 output so emoji works in most terminals
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (parsed.Json)
        {
            new JsonRenderer().Render(results);
        }
        else
        {
            new ConsoleRenderer().Render(results);
        }

        return results.Any(r => r.Status == Status.Fail) ? 1 : 0;
    }

    case "bundle":
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var checks = CheckRegistry.BuildDefaultChecks(includeSmoke: parsed.Smoke);
        var results = await CheckRunner.RunAllAsync(checks);

        var bundlePath = await BundleWriter.WriteAsync(
            results: results,
            outDir: parsed.OutDir,
            zip: parsed.Zip
        );

        Console.WriteLine($"Wrote bundle: {bundlePath}");
        return results.Any(r => r.Status == Status.Fail) ? 1 : 0;
    }

    case "fix-script":
    {
        Console.WriteLine(FixScriptGenerator.GeneratePowerShell());
        return 0;
    }

    default:
    {
        Console.WriteLine(CommandLine.HelpText());
        return 0;
    }
}
