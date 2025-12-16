namespace DotnetSetupDoctor.Checks;

public sealed class PathCheck : ICheck
{
    public string Name => "PATH sanity";

    public Task<CheckResult> RunAsync()
    {
        var path = Environment.GetEnvironmentVariable("PATH") ?? "";
        var parts = path.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length == 0)
        {
            return Task.FromResult(new CheckResult(
                Name,
                Status.Fail,
                "PATH is empty",
                Array.Empty<string>(),
                "Restore your PATH environment variable (Windows Environment Variables settings)."
            ));
        }

        var details = new List<string> { $"Entries: {parts.Length}" };

        var hasDotnetHint = parts.Any(p => p.Contains(@"\dotnet", StringComparison.OrdinalIgnoreCase));
        if (!hasDotnetHint)
            details.Add("No obvious '\\dotnet' directory on PATH (dotnet may still be accessible via other PATH entries).");

        return Task.FromResult(new CheckResult(Name, Status.Info, "PATH looks present", details));
    }
}
