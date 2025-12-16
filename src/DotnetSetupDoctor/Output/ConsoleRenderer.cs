namespace DotnetSetupDoctor.Output;

public interface IRenderer
{
    void Render(IReadOnlyList<CheckResult> results);
}

public sealed class ConsoleRenderer : IRenderer
{
    public void Render(IReadOnlyList<CheckResult> results)
    {
        // Try to render emoji; users can still force their terminal encoding if needed.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Heuristic: if output encoding isn't UTF, use ASCII.
        var emoji = Console.OutputEncoding.WebName.Contains("utf", StringComparison.OrdinalIgnoreCase);

        Console.WriteLine(ReportFormatter.Format(results, emoji));
    }
}
