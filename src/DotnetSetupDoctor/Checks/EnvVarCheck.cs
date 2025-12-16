namespace DotnetSetupDoctor.Checks;

public sealed class EnvVarCheck : ICheck
{
    public string Name => "Environment variables";

    public Task<CheckResult> RunAsync()
    {
        var vars = new[]
        {
            "DOTNET_ROOT",
            "MSBuildSDKsPath",
            "NUGET_PACKAGES"
        };

        var details = vars.Select(v =>
        {
            var val = Environment.GetEnvironmentVariable(v);
            return val is null ? $"{v}: <not set>" : $"{v}: {val}";
        }).ToList();

        return Task.FromResult(new CheckResult(Name, Status.Info, "Collected common env vars", details));
    }
}
