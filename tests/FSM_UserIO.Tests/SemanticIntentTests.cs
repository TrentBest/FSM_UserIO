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
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Intent_rejects_null_empty_and_whitespace_names(string? name)
    {
        var exception = Assert.Throws<ArgumentException>(() => new SemanticIntent(name!));

        Assert.Equal("Name", exception.ParamName);
    }

    [Fact]
    public void Intent_preserves_meaningful_name_whitespace_exactly()
    {
        const string name = "  custom-intent  ";

        Assert.Equal(name, new SemanticIntent(name).Name);
    }

    [Theory]
    [InlineData(0UL)]
    [InlineData(ulong.MaxValue)]
    public void Intent_accepts_protocol_identity_boundaries(ulong protocolId)
    {
        Assert.Equal(protocolId, new SemanticIntent("boundary", protocolId).ProtocolId);
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
    public void Intent_equality_includes_name_and_protocol_identity()
    {
        Assert.NotEqual(new SemanticIntent("select", 42UL), new SemanticIntent("select", 43UL));
        Assert.NotEqual(new SemanticIntent("select", 42UL), new SemanticIntent("inspect", 42UL));
    }

    [Fact]
    public void Intent_name_comparison_is_case_sensitive()
    {
        Assert.NotEqual(new SemanticIntent("select"), new SemanticIntent("Select"));
    }
}
