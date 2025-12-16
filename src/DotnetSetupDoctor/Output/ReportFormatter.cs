using System.Text;

namespace DotnetSetupDoctor.Output;

public static class ReportFormatter
{
    public static string Format(IReadOnlyList<CheckResult> results, bool emoji)
    {
        var sb = new StringBuilder();

        foreach (var r in results)
        {
            var icon = (r.Status, emoji) switch
            {
                (Status.Pass, true) => "✅",
                (Status.Warn, true) => "⚠️",
                (Status.Fail, true) => "❌",
                (_, true) => "ℹ️",

                (Status.Pass, false) => "[OK ]",
                (Status.Warn, false) => "[WARN]",
                (Status.Fail, false) => "[FAIL]",
                (_, false) => "[INFO]"
            };

            sb.AppendLine($"{icon} {r.Name}: {r.Summary}");
            foreach (var d in r.Details) sb.AppendLine($"   - {d}");
            if (!string.IsNullOrWhiteSpace(r.FixHint))
                sb.AppendLine($"   Fix: {r.FixHint}");
            sb.AppendLine();
        }

        var pass = results.Count(x => x.Status == Status.Pass);
        var warn = results.Count(x => x.Status == Status.Warn);
        var fail = results.Count(x => x.Status == Status.Fail);

        sb.AppendLine($"Summary: {pass} pass, {warn} warn, {fail} fail");
        return sb.ToString();
    }
}
