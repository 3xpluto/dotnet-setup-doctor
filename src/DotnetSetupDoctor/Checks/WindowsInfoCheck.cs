namespace DotnetSetupDoctor.Checks;

public sealed class WindowsInfoCheck : ICheck
{
    public string Name => "Windows info";

    public Task<CheckResult> RunAsync()
    {
        var details = new List<string>
        {
            $"OS: {Environment.OSVersion}",
            $"64-bit OS: {Environment.Is64BitOperatingSystem}",
            $"64-bit Proc: {Environment.Is64BitProcess}",
            $"Machine: {Environment.MachineName}",
            $"User: {Environment.UserName}"
        };

        return Task.FromResult(new CheckResult(Name, Status.Info, "System info", details));
    }
}
