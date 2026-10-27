using TheSingularityWorkshop.FSM_UserIO;
using Xunit;

namespace TheSingularityWorkshop.FSM_UserIO.Tests;

public sealed class SemanticInteractionTests
{
    private static readonly Guid Correlation = Guid.Parse("4a3a2b2c-8c36-4a56-9a90-34a32b21e001");

    [Theory]
    [InlineData("observe", InteractionMode.Observe)]
    [InlineData("express", InteractionMode.Express)]
    [InlineData("act", InteractionMode.Act)]
    public void Capability_preserves_name_and_defined_mode(string name, InteractionMode mode)
    {
        var capability = new InteractionCapability(name, mode);

        Assert.Equal(name, capability.Name);
        Assert.Equal(mode, capability.Mode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Capability_rejects_invalid_name(string? name)
    {
        Assert.Throws<ArgumentException>(() => new InteractionCapability(name!, InteractionMode.Observe));
    }

    [Fact]
    public void Capability_rejects_undefined_mode()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new InteractionCapability("move", (InteractionMode)99));
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(0.5d)]
    [InlineData(1d)]
    public void Observation_accepts_confidence_inclusive_range(double confidence)
    {
        var observation = new SemanticObservation("motion-detected", DateTimeOffset.UnixEpoch, Confidence: confidence);

        Assert.Equal(confidence, observation.Confidence);
    }

    [Theory]
    [InlineData(-0.01d)]
    [InlineData(1.01d)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    [InlineData(double.NegativeInfinity)]
    public void Observation_rejects_invalid_confidence(double confidence)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new SemanticObservation("motion-detected", DateTimeOffset.UnixEpoch, Confidence: confidence));
    }

    [Fact]
    public void Observation_preserves_timestamp_and_source_and_does_not_normalize()
    {
        var timestamp = new DateTimeOffset(2026, 10, 27, 12, 0, 0, TimeSpan.FromHours(-7));
        var observation = new SemanticObservation("  custom-event  ", timestamp, "sensor-a");

        Assert.Equal("  custom-event  ", observation.Name);
        Assert.Equal(timestamp, observation.ObservedAt);
        Assert.Equal("sensor-a", observation.Source);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Observation_rejects_blank_source_when_supplied(string source)
    {
        Assert.Throws<ArgumentException>(() =>
            new SemanticObservation("motion-detected", DateTimeOffset.UnixEpoch, source));
    }

    [Fact]
    public void Expression_is_not_an_intent_and_keeps_its_source_label()
    {
        var expression = new SemanticExpression("maybe-open-door", DateTimeOffset.UnixEpoch, "speech-adapter");

        Assert.Equal("maybe-open-door", expression.Name);
        Assert.Equal("speech-adapter", expression.Source);
    }

    [Fact]
    public void Context_allows_optional_participant_and_session_metadata()
    {
        var session = Guid.NewGuid();
        var context = new InteractionContext(Correlation, "participant-7", session);

        Assert.Equal(Correlation, context.CorrelationId);
        Assert.Equal("participant-7", context.ParticipantId);
        Assert.Equal(session, context.SessionId);
    }

    [Fact]
    public void Context_defaults_optional_metadata_to_null()
    {
        var context = new InteractionContext(Correlation);

        Assert.Null(context.ParticipantId);
        Assert.Null(context.SessionId);
    }

    [Fact]
    public void Context_rejects_empty_correlation_and_blank_participant()
    {
        Assert.Throws<ArgumentException>(() => new InteractionContext(Guid.Empty));
        Assert.Throws<ArgumentException>(() => new InteractionContext(Correlation, " "));
    }

    [Fact]
    public void Feedback_preserves_kind_message_and_correlation()
    {
        var feedback = new SemanticFeedback(Correlation, FeedbackKind.Rejected, "Host policy rejected this request.");

        Assert.Equal(Correlation, feedback.CorrelationId);
        Assert.Equal(FeedbackKind.Rejected, feedback.Kind);
        Assert.Equal("Host policy rejected this request.", feedback.Message);
    }

    [Fact]
    public void Feedback_rejects_invalid_correlation_kind_and_message()
    {
        Assert.Throws<ArgumentException>(() => new SemanticFeedback(Guid.Empty, FeedbackKind.Rejected, "Rejected"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new SemanticFeedback(Correlation, (FeedbackKind)99, "Rejected"));
        Assert.Throws<ArgumentException>(() => new SemanticFeedback(Correlation, FeedbackKind.Rejected, " "));
    }

    [Fact]
    public void Semantic_contract_records_use_value_equality()
    {
        Assert.Equal(
            new SemanticObservation("signal", DateTimeOffset.UnixEpoch, "source", 0.5),
            new SemanticObservation("signal", DateTimeOffset.UnixEpoch, "source", 0.5));
        Assert.Equal(
            new InteractionContext(Correlation, "actor"),
            new InteractionContext(Correlation, "actor"));
        Assert.Equal(
            new SemanticFeedback(Correlation, FeedbackKind.Accepted, "queued"),
            new SemanticFeedback(Correlation, FeedbackKind.Accepted, "queued"));
    }
}
