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

    [Fact]
    public void Intent_rejects_null_name_with_a_stable_argument_contract()
    {
        var exception = Assert.Throws<ArgumentException>(() => new SemanticIntent(null!));

        Assert.Equal("Name", exception.ParamName);
    }

    [Fact]
    public void Intent_preserves_non_blank_name_exactly()
    {
        const string name = " select ";
        var intent = new SemanticIntent(name);

        Assert.Equal(name, intent.Name);
    }

    [Fact]
    public void Intent_equality_includes_name_and_protocol_identity()
    {
        Assert.NotEqual(new SemanticIntent("select", 42UL), new SemanticIntent("select", 43UL));
        Assert.NotEqual(new SemanticIntent("select", 42UL), new SemanticIntent("inspect", 42UL));
    }

    [Fact]
    public void Core_assembly_has_no_platform_or_upstream_product_references()
    {
        string[] forbiddenPrefixes =
        [
            "ProtocolAi",
            "GrammarAi",
            "FSM_COS",
            "TheSingularityWorkshop.GUI",
            "PresentationFramework",
            "PresentationCore",
            "WindowsBase",
            "UnityEngine",
            "Microsoft.AspNetCore.Components",
            "Microsoft.Web.WebView2"
        ];

        var unexpectedReferences = typeof(SemanticIntent).Assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name ?? string.Empty)
            .Where(name => forbiddenPrefixes.Any(prefix =>
                name.Equals(prefix, StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase)))
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        Assert.True(
            unexpectedReferences.Length == 0,
            $"The FSM_UserIO core assembly references forbidden platform or upstream assemblies: {string.Join(", ", unexpectedReferences)}");
    }
}
