namespace TheSingularityWorkshop.FSM_UserIO;

/// <summary>Describes a named capability in the interaction lifecycle without granting permission to use it.</summary>
/// <param name="Name">Application-defined capability name.</param>
/// <param name="Mode">The kind of interaction the capability supports.</param>
public sealed record InteractionCapability(string Name, InteractionMode Mode)
{
    /// <summary>Gets the application-defined capability name.</summary>
    public string Name { get; } = SemanticValueValidation.Required(Name, nameof(Name));

    /// <summary>Gets the supported interaction mode.</summary>
    public InteractionMode Mode { get; } = SemanticValueValidation.Defined(Mode, nameof(Mode));
}

/// <summary>Identifies whether a capability concerns receiving information, expressing information, or acting.</summary>
public enum InteractionMode
{
    /// <summary>The form can sense, observe, or receive information.</summary>
    Observe = 0,
    /// <summary>The form can express or propose information.</summary>
    Express = 1,
    /// <summary>The form can perform an action; this does not imply application authorization.</summary>
    Act = 2
}

/// <summary>A source-neutral record of something observed by a participating form or adapter.</summary>
/// <param name="Name">Application-defined observation name.</param>
/// <param name="ObservedAt">Timestamp supplied by the observing source.</param>
/// <param name="Source">Optional source label; not an authenticated identity.</param>
/// <param name="Confidence">Optional confidence in the range from zero to one.</param>
public sealed record SemanticObservation(
    string Name,
    DateTimeOffset ObservedAt,
    string? Source = null,
    double? Confidence = null)
{
    /// <summary>Gets the application-defined observation name.</summary>
    public string Name { get; } = SemanticValueValidation.Required(Name, nameof(Name));

    /// <summary>Gets the source-supplied observation timestamp.</summary>
    public DateTimeOffset ObservedAt { get; } = ObservedAt;

    /// <summary>Gets the optional source label, which is not proof of identity or trust.</summary>
    public string? Source { get; } = SemanticValueValidation.Optional(Source, nameof(Source));

    /// <summary>Gets the optional confidence value between zero and one, inclusive.</summary>
    public double? Confidence { get; } = SemanticValueValidation.Confidence(Confidence, nameof(Confidence));
}

/// <summary>Represents an expression or proposal before semantic interpretation or authorization.</summary>
/// <param name="Name">Application-defined expression name.</param>
/// <param name="ExpressedAt">Timestamp supplied by the expressing source.</param>
/// <param name="Source">Optional source label; not an authenticated identity.</param>
public sealed record SemanticExpression(
    string Name,
    DateTimeOffset ExpressedAt,
    string? Source = null)
{
    /// <summary>Gets the application-defined expression name.</summary>
    public string Name { get; } = SemanticValueValidation.Required(Name, nameof(Name));

    /// <summary>Gets the source-supplied expression timestamp.</summary>
    public DateTimeOffset ExpressedAt { get; } = ExpressedAt;

    /// <summary>Gets the optional source label, which is not proof of identity or trust.</summary>
    public string? Source { get; } = SemanticValueValidation.Optional(Source, nameof(Source));
}

/// <summary>Correlation metadata that can travel with an interaction without owning session lifecycle.</summary>
/// <param name="CorrelationId">A non-empty correlation identifier.</param>
/// <param name="ParticipantId">Optional application-defined participant identifier.</param>
/// <param name="SessionId">Optional application-defined session identifier.</param>
public sealed record InteractionContext(
    Guid CorrelationId,
    string? ParticipantId = null,
    Guid? SessionId = null)
{
    /// <summary>Gets the correlation identifier.</summary>
    public Guid CorrelationId { get; } = CorrelationId == Guid.Empty
        ? throw new ArgumentException("A non-empty correlation identifier is required.", nameof(CorrelationId))
        : CorrelationId;

    /// <summary>Gets the optional application-defined participant identifier.</summary>
    public string? ParticipantId { get; } = SemanticValueValidation.Optional(ParticipantId, nameof(ParticipantId));

    /// <summary>Gets the optional application-defined session identifier.</summary>
    public Guid? SessionId { get; } = SemanticValueValidation.OptionalIdentifier(SessionId, nameof(SessionId));
}

/// <summary>Non-authoritative semantic feedback associated with an interaction.</summary>
/// <param name="CorrelationId">The correlation identifier for the interaction.</param>
/// <param name="Kind">The category of feedback.</param>
/// <param name="Message">Application-defined explanatory text.</param>
public sealed record SemanticFeedback(
    Guid CorrelationId,
    FeedbackKind Kind,
    string Message)
{
    /// <summary>Gets the interaction correlation identifier.</summary>
    public Guid CorrelationId { get; } = CorrelationId == Guid.Empty
        ? throw new ArgumentException("A non-empty correlation identifier is required.", nameof(CorrelationId))
        : CorrelationId;

    /// <summary>Gets the feedback category.</summary>
    public FeedbackKind Kind { get; } = SemanticValueValidation.Defined(Kind, nameof(Kind));

    /// <summary>Gets the explanatory message.</summary>
    public string Message { get; } = SemanticValueValidation.Required(Message, nameof(Message));
}

/// <summary>Classifies feedback without implying that an action was authorized or executed.</summary>
public enum FeedbackKind
{
    /// <summary>Information that does not assert acceptance or execution.</summary>
    Informational = 0,
    /// <summary>The host reports that it accepted the interaction for further handling.</summary>
    Accepted = 1,
    /// <summary>The host reports that it rejected the interaction.</summary>
    Rejected = 2,
    /// <summary>The host requests clarification before deciding how to proceed.</summary>
    ClarificationRequired = 3,
    /// <summary>The host reports that its application-defined operation completed.</summary>
    Completed = 4,
    /// <summary>The host reports an application-defined failure.</summary>
    Failed = 5
}

/// <summary>Opaque application-owned payload bytes with caller-defined media type and schema identifiers.</summary>
/// <remarks>
/// The payload does not prescribe a serializer, schema registry, or maximum size. Input bytes are copied,
/// and the data accessor returns a copy-backed read-only view so callers cannot mutate the stored value.
/// </remarks>
public sealed class SemanticPayload : IEquatable<SemanticPayload>
{
    private readonly byte[] _data;

    /// <summary>Creates a payload with application-defined content and schema labels.</summary>
    /// <param name="mediaType">A non-whitespace media/content type label.</param>
    /// <param name="schema">A non-whitespace application-defined schema identifier or version.</param>
    /// <param name="data">Payload bytes; an empty payload is valid.</param>
    public SemanticPayload(string mediaType, string schema, byte[] data)
    {
        MediaType = SemanticValueValidation.Required(mediaType, nameof(mediaType));
        Schema = SemanticValueValidation.Required(schema, nameof(schema));
        ArgumentNullException.ThrowIfNull(data);
        _data = data.ToArray();
    }

    /// <summary>Gets the application-defined content/media type label.</summary>
    public string MediaType { get; }

    /// <summary>Gets the application-defined schema identifier or version.</summary>
    public string Schema { get; }

    /// <summary>Gets a read-only view over a defensive copy of the payload bytes.</summary>
    public ReadOnlyMemory<byte> Data => _data.ToArray();

    /// <inheritdoc />
    public bool Equals(SemanticPayload? other) =>
        other is not null
        && StringComparer.Ordinal.Equals(MediaType, other.MediaType)
        && StringComparer.Ordinal.Equals(Schema, other.Schema)
        && _data.AsSpan().SequenceEqual(other._data);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is SemanticPayload other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(MediaType, StringComparer.Ordinal);
        hash.Add(Schema, StringComparer.Ordinal);
        foreach (var value in _data)
        {
            hash.Add(value);
        }

        return hash.ToHashCode();
    }
}

internal static class SemanticValueValidation
{
    internal static string Required(string? value, string parameterName) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("A non-empty, non-whitespace value is required.", parameterName)
            : value;

    internal static string? Optional(string? value, string parameterName) =>
        value is null ? null : Required(value, parameterName);

    internal static Guid? OptionalIdentifier(Guid? value, string parameterName) =>
        value == Guid.Empty
            ? throw new ArgumentException("An optional identifier must be null or non-empty.", parameterName)
            : value;

    internal static double? Confidence(double? value, string parameterName)
    {
        if (value is null)
        {
            return null;
        }

        if (!double.IsFinite(value.Value) || value.Value < 0d || value.Value > 1d)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, "Confidence must be finite and between zero and one, inclusive.");
        }

        return value;
    }

    internal static TEnum Defined<TEnum>(TEnum value, string parameterName)
        where TEnum : struct, Enum =>
        Enum.IsDefined(value)
            ? value
            : throw new ArgumentOutOfRangeException(parameterName, value, "The value is not a defined member.");
}
