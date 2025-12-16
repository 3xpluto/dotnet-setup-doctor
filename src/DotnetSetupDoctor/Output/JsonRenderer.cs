using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetSetupDoctor.Output;

public sealed class JsonRenderer : IRenderer
{
    public void Render(IReadOnlyList<CheckResult> results)
    {
        var json = JsonSerializer.Serialize(results, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        });

        Console.WriteLine(json);
    }
}
