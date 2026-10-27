using TheSingularityWorkshop.FSM_UserIO;
using Xunit;

namespace TheSingularityWorkshop.FSM_UserIO.Tests;

public sealed class AudioCaptureSourceStatusTests
{
    [Theory]
    [InlineData(AudioCaptureSourceKind.Microphone)]
    [InlineData(AudioCaptureSourceKind.SystemAudio)]
    public void Sources_are_independent_values(AudioCaptureSourceKind source)
    {
        var status = new AudioCaptureSourceStatus(source, AudioCaptureSourceState.NotRequested);

        Assert.Equal(source, status.Source);
        Assert.Equal(AudioCaptureSourceState.NotRequested, status.State);
        Assert.Equal(AudioCaptureStateEvidence.Observed, status.Evidence);
    }

    [Fact]
    public void Successful_capture_lifecycle_is_valid_and_immutable()
    {
        var initial = new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.NotRequested);
        var pending = initial.TransitionTo(AudioCaptureSourceState.RequestPending);
        var ready = pending.TransitionTo(AudioCaptureSourceState.Ready);
        var capturing = ready.TransitionTo(AudioCaptureSourceState.Capturing);
        var stopped = capturing.TransitionTo(AudioCaptureSourceState.Ready);

        Assert.Equal(AudioCaptureSourceState.NotRequested, initial.State);
        Assert.Equal(AudioCaptureSourceState.RequestPending, pending.State);
        Assert.Equal(AudioCaptureSourceState.Ready, ready.State);
        Assert.Equal(AudioCaptureSourceState.Capturing, capturing.State);
        Assert.Equal(AudioCaptureSourceState.Ready, stopped.State);
    }

    [Theory]
    [InlineData(AudioCaptureSourceState.Denied, "permission-denied", "Review site and OS privacy permissions.")]
    [InlineData(AudioCaptureSourceState.Unavailable, "no-audio-track", "Select a source that provides audio.")]
    [InlineData(AudioCaptureSourceState.Failed, "capture-failed", "Check source availability and retry.")]
    public void Failed_acquisition_preserves_source_neutral_diagnostics(
        AudioCaptureSourceState failureState, string errorCode, string recovery)
    {
        var pending = new AudioCaptureSourceStatus(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.RequestPending);
        var failed = pending.TransitionTo(failureState, errorCode: errorCode, recoveryGuidance: recovery);

        Assert.Equal(AudioCaptureSourceKind.SystemAudio, failed.Source);
        Assert.Equal(errorCode, failed.ErrorCode);
        Assert.Equal(recovery, failed.RecoveryGuidance);
    }

    [Fact]
    public void Cancellation_is_a_real_state_and_can_be_retried()
    {
        var pending = new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.RequestPending);
        var cancelled = pending.TransitionTo(AudioCaptureSourceState.Cancelled);
        var retry = cancelled.TransitionTo(AudioCaptureSourceState.RequestPending);
        var ready = retry.TransitionTo(AudioCaptureSourceState.Ready);
        var countdownCancelled = ready.TransitionTo(AudioCaptureSourceState.Cancelled);

        Assert.Equal(AudioCaptureSourceState.Cancelled, cancelled.State);
        Assert.Equal(AudioCaptureSourceState.RequestPending, retry.State);
        Assert.Equal(AudioCaptureSourceState.Cancelled, countdownCancelled.State);
    }

    [Fact]
    public void Host_estimate_is_not_misrepresented_as_direct_observation()
    {
        var initial = new AudioCaptureSourceStatus(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.NotRequested);
        var estimated = initial.TransitionTo(
            AudioCaptureSourceState.Unsupported,
            AudioCaptureStateEvidence.HostReportedEstimate);

        Assert.Equal(AudioCaptureStateEvidence.HostReportedEstimate, estimated.Evidence);
    }

    [Fact]
    public void Partial_success_keeps_microphone_and_system_audio_statuses_separate()
    {
        var microphone = new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.RequestPending)
            .TransitionTo(AudioCaptureSourceState.Denied, errorCode: "permission-denied", recoveryGuidance: "Check microphone permissions.");
        var system = new AudioCaptureSourceStatus(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.RequestPending)
            .TransitionTo(AudioCaptureSourceState.Ready);

        Assert.Equal(AudioCaptureSourceState.Denied, microphone.State);
        Assert.Equal(AudioCaptureSourceState.Ready, system.State);
    }

    [Theory]
    [InlineData(AudioCaptureSourceState.NotRequested, AudioCaptureSourceState.Capturing)]
    [InlineData(AudioCaptureSourceState.RequestPending, AudioCaptureSourceState.Capturing)]
    [InlineData(AudioCaptureSourceState.Denied, AudioCaptureSourceState.Capturing)]
    [InlineData(AudioCaptureSourceState.Capturing, AudioCaptureSourceState.RequestPending)]
    [InlineData(AudioCaptureSourceState.Failed, AudioCaptureSourceState.Ready)]
    public void Invalid_lifecycle_transitions_are_rejected(
        AudioCaptureSourceState current, AudioCaptureSourceState next)
    {
        var status = new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, current);

        Assert.Throws<InvalidOperationException>(() => status.TransitionTo(next));
    }

    [Fact]
    public void Undefined_source_state_and_evidence_are_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AudioCaptureSourceStatus((AudioCaptureSourceKind)99, AudioCaptureSourceState.NotRequested));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, (AudioCaptureSourceState)99));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.NotRequested,
                (AudioCaptureStateEvidence)99));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.NotRequested)
                .TransitionTo((AudioCaptureSourceState)99));
    }

    [Fact]
    public void Optional_error_metadata_rejects_whitespace()
    {
        Assert.Throws<ArgumentException>(() =>
            new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.NotRequested, ErrorCode: " "));
        Assert.Throws<ArgumentException>(() =>
            new AudioCaptureSourceStatus(AudioCaptureSourceKind.Microphone, AudioCaptureSourceState.NotRequested, RecoveryGuidance: " "));
    }

    [Fact]
    public void Permission_revocation_and_source_loss_can_end_active_capture()
    {
        var capturing = new AudioCaptureSourceStatus(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.Capturing);
        var revoked = capturing.TransitionTo(
            AudioCaptureSourceState.Denied,
            errorCode: "permission-revoked",
            recoveryGuidance: "Reacquire the source before restarting.");
        var lost = capturing.TransitionTo(
            AudioCaptureSourceState.Unavailable,
            errorCode: "source-ended",
            recoveryGuidance: "Select an available output source and retry.");

        Assert.Equal(AudioCaptureSourceState.Denied, revoked.State);
        Assert.Equal(AudioCaptureSourceState.Unavailable, lost.State);
    }
}
