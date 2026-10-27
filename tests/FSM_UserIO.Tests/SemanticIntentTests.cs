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
    [InlineData("\r")]
    [InlineData("\v")]
    [InlineData("\f")]
    [InlineData("\u00A0")]
    [InlineData("\u2003")]
    [InlineData("\u3000")]
    public void Intent_rejects_additional_unicode_whitespace_names(string name)
    {
        Assert.Throws<ArgumentException>(() => new SemanticIntent(name));
    }

    [Fact]
    public void Intent_equal_values_have_equal_hash_codes()
    {
        var first = new SemanticIntent("select", 42UL);
        var second = new SemanticIntent("select", 42UL);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Intent_name_comparison_is_case_sensitive()
    {
        Assert.NotEqual(new SemanticIntent("select"), new SemanticIntent("Select"));
    }

}
