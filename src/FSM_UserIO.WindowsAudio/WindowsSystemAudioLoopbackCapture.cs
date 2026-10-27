using NAudio.Wave;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

/// <summary>Captures the default Windows playback endpoint using WASAPI loopback.</summary>
/// <remarks>Captures system playback only. The host remains responsible for disclosure and application-level authorization.</remarks>
public sealed class WindowsSystemAudioLoopbackCapture : IDisposable
{
    private WasapiLoopbackCapture? _capture;
    private AudioCaptureSourceStatus _status = new(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.NotRequested);
    private bool _disposed;

    /// <summary>Raised with a copied PCM buffer and format metadata.</summary>
    public event EventHandler<AudioCapturePcmDataEventArgs>? AudioDataAvailable;
    /// <summary>Raised when the source status changes.</summary>
    public event EventHandler<AudioCaptureSourceStatus>? StatusChanged;
    /// <summary>Gets the latest source-neutral lifecycle status.</summary>
    public AudioCaptureSourceStatus Status => _status;

    /// <summary>Starts default-output endpoint loopback capture.</summary>
    /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows.</exception>
    public AudioCaptureSourceStatus Start()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        if (!OperatingSystem.IsWindows())
        {
            SetStatus(new AudioCaptureSourceStatus(AudioCaptureSourceKind.SystemAudio,
                AudioCaptureSourceState.Unsupported, errorCode: "platform-unsupported",
                recoveryGuidance: "Use a Windows host for native system-audio loopback capture."));
            throw new PlatformNotSupportedException("WASAPI loopback capture is supported only on Windows.");
        }

        if (_status.State == AudioCaptureSourceState.Capturing)
            return _status;

        SetStatus(_status.TransitionTo(AudioCaptureSourceState.RequestPending));
        try
        {
            _capture = new WasapiLoopbackCapture();
            _capture.DataAvailable += OnDataAvailable;
            _capture.RecordingStopped += OnRecordingStopped;
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Ready));
            _capture.StartRecording();
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Capturing));
            return _status;
        }
        catch
        {
            ReleaseCapture();
            if (_status.State is AudioCaptureSourceState.RequestPending or AudioCaptureSourceState.Ready)
                SetStatus(_status.TransitionTo(AudioCaptureSourceState.Failed,
                    errorCode: "capture-start-failed",
                    recoveryGuidance: "Check that a Windows playback endpoint is available, then retry."));
            throw;
        }
    }

    /// <summary>Stops active capture and releases native resources.</summary>
    public void Stop()
    {
        if (_capture is null)
            return;
        if (_status.State == AudioCaptureSourceState.Capturing)
            _capture.StopRecording();
        ReleaseCapture();
        if (_status.State == AudioCaptureSourceState.Capturing)
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Ready));
    }

    /// <summary>Stops capture and releases native resources.</summary>
    public void Dispose()
    {
        if (_disposed)
            return;
        Stop();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs args)
    {
        if (args.BytesRecorded <= 0 || _capture is null)
            return;
        var format = _capture.WaveFormat;
        var bytes = new byte[args.BytesRecorded];
        Buffer.BlockCopy(args.Buffer, 0, bytes, 0, args.BytesRecorded);
        AudioDataAvailable?.Invoke(this, new AudioCapturePcmDataEventArgs(
            bytes, format.SampleRate, format.Channels, format.BitsPerSample, format.Encoding.ToString()));
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs args)
    {
        if (args.Exception is not null &&
            _status.State is AudioCaptureSourceState.Capturing or AudioCaptureSourceState.Ready)
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Failed,
                errorCode: "capture-failed",
                recoveryGuidance: "Check the playback endpoint and retry the capture session."));
    }

    private void SetStatus(AudioCaptureSourceStatus status)
    {
        _status = status;
        StatusChanged?.Invoke(this, status);
    }

    private void ReleaseCapture()
    {
        if (_capture is null)
            return;
        _capture.DataAvailable -= OnDataAvailable;
        _capture.RecordingStopped -= OnRecordingStopped;
        _capture.Dispose();
        _capture = null;
    }
}

/// <summary>A copied audio buffer and format metadata from the loopback source.</summary>
/// <param name="Buffer">The captured bytes owned by the event consumer.</param>
/// <param name="SampleRate">Samples per second.</param>
/// <param name="Channels">Number of interleaved channels.</param>
/// <param name="BitsPerSample">Bits per sample reported by the capture format.</param>
/// <param name="Encoding">Encoding reported by the capture format.</param>
public sealed record AudioCapturePcmDataEventArgs(
    byte[] Buffer, int SampleRate, int Channels, int BitsPerSample, string Encoding);
