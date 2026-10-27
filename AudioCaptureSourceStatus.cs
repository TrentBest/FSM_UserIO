namespace TheSingularityWorkshop.FSM_UserIO;

/// <summary>Identifies an audio source without exposing browser, operating-system, or device SDK types.</summary>
public enum AudioCaptureSourceKind
{
    /// <summary>Audio captured from a microphone input.</summary>
    Microphone = 0,
    /// <summary>Audio captured from system playback, an output endpoint, or a browser-selected shared surface.</summary>
    SystemAudio = 1
}

/// <summary>Describes the observed or host-reported readiness state of one audio capture source.</summary>
public enum AudioCaptureSourceState
{
    /// <summary>The source has not been queried or requested in this session.</summary>
    NotRequested = 0,
    /// <summary>The source or capture mechanism is not supported in the current context.</summary>
    Unsupported = 1,
    /// <summary>A permission or source-acquisition request is in progress.</summary>
    RequestPending = 2,
    /// <summary>The source request was denied or permission was revoked.</summary>
    Denied = 3,
    /// <summary>The source is unavailable or supplied no usable audio track.</summary>
    Unavailable = 4,
    /// <summary>A usable source has been acquired but recording has not started.</summary>
    Ready = 5,
    /// <summary>The source is actively contributing audio to a capture.</summary>
    Capturing = 6,
    /// <summary>The request or active source was cancelled.</summary>
    Cancelled = 7,
    /// <summary>The source encountered an unexpected or adapter-defined failure.</summary>
    Failed = 8
}

/// <summary>Distinguishes direct source observations from a host's estimate of capability or readiness.</summary>
public enum AudioCaptureStateEvidence
{
    /// <summary>The adapter observed the state or event directly.</summary>
    Observed = 0,
    /// <summary>A host reports an estimate; it is not proof that a source can be acquired.</summary>
    HostReportedEstimate = 1
}

/// <summary>
/// Source-neutral status for one audio source. It describes capability and lifecycle, not application authorization.
/// </summary>
/// <param name="Source">The independent source being described.</param>
/// <param name="State">The current source state.</param>
/// <param name="Evidence">Whether the state was observed directly or reported as an estimate.</param>
/// <param name="ErrorCode">Optional stable, source-neutral error code.</param>
/// <param name="RecoveryGuidance">Optional actionable recovery guidance supplied by the adapter or host.</param>
/// <remarks>
/// Error codes and recovery guidance must not contain platform-specific types. Adapters may use stable strings such
/// as <c>permission-denied</c> or <c>no-audio-track</c>; they should not expose browser exception classes or OS handles.
/// A ready source is not automatically authorized for an application-level action.
/// </remarks>
public sealed record AudioCaptureSourceStatus(
    AudioCaptureSourceKind Source,
    AudioCaptureSourceState State,
    AudioCaptureStateEvidence Evidence = AudioCaptureStateEvidence.Observed,
    string? ErrorCode = null,
    string? RecoveryGuidance = null)
{
    /// <summary>Gets the independent audio source.</summary>
    public AudioCaptureSourceKind Source { get; } = SemanticValueValidation.Defined(Source, nameof(Source));

    /// <summary>Gets the current state.</summary>
    public AudioCaptureSourceState State { get; } = SemanticValueValidation.Defined(State, nameof(State));

    /// <summary>Gets whether this state is directly observed or estimated by a host.</summary>
    public AudioCaptureStateEvidence Evidence { get; } = SemanticValueValidation.Defined(Evidence, nameof(Evidence));

    /// <summary>Gets an optional source-neutral error code.</summary>
    public string? ErrorCode { get; } = SemanticValueValidation.Optional(ErrorCode, nameof(ErrorCode));

    /// <summary>Gets optional recovery guidance without claiming the adapter can grant operating-system permission.</summary>
    public string? RecoveryGuidance { get; } = SemanticValueValidation.Optional(RecoveryGuidance, nameof(RecoveryGuidance));

    /// <summary>Creates a new status after validating that the requested lifecycle transition is allowed.</summary>
    /// <param name="nextState">The requested next state.</param>
    /// <param name="evidence">Evidence for the new state.</param>
    /// <param name="errorCode">A source-neutral error code for a denial, unavailable source, or failure.</param>
    /// <param name="recoveryGuidance">Recovery guidance for a denial, unavailable source, or failure.</param>
    /// <returns>A new immutable status value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The next state or evidence value is undefined.</exception>
    /// <exception cref="InvalidOperationException">The requested transition is not valid from the current state.</exception>
    public AudioCaptureSourceStatus TransitionTo(
        AudioCaptureSourceState nextState,
        AudioCaptureStateEvidence evidence = AudioCaptureStateEvidence.Observed,
        string? errorCode = null,
        string? recoveryGuidance = null)
    {
        SemanticValueValidation.Defined(nextState, nameof(nextState));
        SemanticValueValidation.Defined(evidence, nameof(evidence));

        if (!AudioCaptureTransitions.IsAllowed(State, nextState))
        {
            throw new InvalidOperationException($"Audio source transition from {State} to {nextState} is not allowed.");
        }

        return new AudioCaptureSourceStatus(Source, nextState, evidence, errorCode, recoveryGuidance);
    }
}

internal static class AudioCaptureTransitions
{
    internal static bool IsAllowed(AudioCaptureSourceState current, AudioCaptureSourceState next) =>
        current switch
        {
            AudioCaptureSourceState.NotRequested =>
                next is AudioCaptureSourceState.Unsupported or AudioCaptureSourceState.RequestPending,
            AudioCaptureSourceState.Unsupported =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending,
            AudioCaptureSourceState.RequestPending =>
                next is AudioCaptureSourceState.Unsupported or AudioCaptureSourceState.Denied
                    or AudioCaptureSourceState.Unavailable or AudioCaptureSourceState.Ready
                    or AudioCaptureSourceState.Cancelled or AudioCaptureSourceState.Failed,
            AudioCaptureSourceState.Denied =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending,
            AudioCaptureSourceState.Unavailable =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending
                    or AudioCaptureSourceState.Unsupported,
            AudioCaptureSourceState.Ready =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending
                    or AudioCaptureSourceState.Denied or AudioCaptureSourceState.Unavailable
                    or AudioCaptureSourceState.Capturing or AudioCaptureSourceState.Failed,
            AudioCaptureSourceState.Capturing =>
                next is AudioCaptureSourceState.Ready or AudioCaptureSourceState.Denied
                    or AudioCaptureSourceState.Unavailable or AudioCaptureSourceState.Cancelled
                    or AudioCaptureSourceState.Failed,
            AudioCaptureSourceState.Cancelled =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending,
            AudioCaptureSourceState.Failed =>
                next is AudioCaptureSourceState.NotRequested or AudioCaptureSourceState.RequestPending,
            _ => false
        };
}
