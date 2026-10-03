using Xunit;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_UserIO.Tests;

public sealed class SemanticIntentTests
{
    [Fact]
    public void Intent_preserves_application_owned_identity()
    {
        var intent = new SemanticIntent("select", 42UL);

        Assert.Equal("select", intent.Name);
        Assert.Equal(42UL, intent.ProtocolId);
    }

    [Fact]
    public void Intent_does_not_require_protocol_identity()
    {
        var intent = new SemanticIntent("inspect");

        Assert.Null(intent.ProtocolId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Intent_requires_a_non_blank_name(string name)
    {
        Assert.Throws<ArgumentException>(() => new SemanticIntent(name));
    }

    [Fact]
    public void Intent_is_value_based()
    {
        var first = new SemanticIntent("select", 42UL);
        var second = new SemanticIntent("select", 42UL);

        Assert.Equal(first, second);
    }
}