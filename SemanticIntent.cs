namespace TheSingularityWorkshop.FSM_UserIO;

/// <summary>Represents application-owned semantic intent without assigning physical-device or execution authority.</summary>
/// <param name="Name">The application-defined semantic name of the intent.</param>
/// <param name="ProtocolId">An optional deterministic application protocol identity.</param>
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null)
{
    /// <summary>Gets the application-defined semantic name.</summary>
    public string Name { get; } = string.IsNullOrWhiteSpace(Name)
        ? throw new ArgumentException("An intent name is required.", nameof(Name))
        : Name;

    /// <summary>Gets the optional deterministic ProtocolAi identity associated with this intent.</summary>
    public ulong? ProtocolId { get; } = ProtocolId;
}