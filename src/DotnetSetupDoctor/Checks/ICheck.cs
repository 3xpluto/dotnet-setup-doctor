namespace DotnetSetupDoctor;

public enum Status { Pass, Warn, Fail, Info }

public sealed record CheckResult(
    string Name,
    Status Status,
    string Summary,
    IReadOnlyList<string> Details,
    string? FixHint = null
);

public interface ICheck
{
    string Name { get; }
    Task<CheckResult> RunAsync();
}
