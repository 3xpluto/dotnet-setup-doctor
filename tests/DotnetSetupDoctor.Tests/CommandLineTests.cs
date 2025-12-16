using DotnetSetupDoctor.Cli;
using Xunit;

namespace DotnetSetupDoctor.Tests;

public class CommandLineTests
{
    [Fact]
    public void DefaultCommand_IsDoctor()
    {
        var parsed = CommandLine.Parse(Array.Empty<string>());
        Assert.Equal("doctor", parsed.Command);
        Assert.False(parsed.Json);
    }

    [Fact]
    public void JsonFlag_Works()
    {
        var parsed = CommandLine.Parse(new[] { "--json" });
        Assert.True(parsed.Json);
    }

    [Fact]
    public void FixScriptCommand_Works()
    {
        var parsed = CommandLine.Parse(new[] { "fix-script" });
        Assert.Equal("fix-script", parsed.Command);
    }
}
