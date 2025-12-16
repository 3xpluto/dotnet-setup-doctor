namespace DotnetSetupDoctor;

public static class CheckRunner
{
    public static async Task<List<CheckResult>> RunAllAsync(IReadOnlyList<ICheck> checks)
    {
        var results = new List<CheckResult>(checks.Count);

        foreach (var c in checks)
        {
            try
            {
                results.Add(await c.RunAsync());
            }
            catch (Exception ex)
            {
                results.Add(new CheckResult(
                    c.Name,
                    Status.Fail,
                    "Check crashed",
                    new[] { ex.ToString() },
                    "Report this issue and include the stack trace."
                ));
            }
        }

        return results;
    }
}
