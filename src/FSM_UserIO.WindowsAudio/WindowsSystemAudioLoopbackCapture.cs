using NAudio.CoreAudioApi;
using NAudio.Wave;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

/// <summary>Captures the default Windows playback endpoint using WASAPI loopback.</summary>
/// <remarks>Captures system playback only. The host remains responsible for disclosure and application-level authorization.</remarks>
public sealed class WindowsSystemAudioLoopbackCapture : IDisposable
{
    private readonly string? _endpointId;
    private IWindowsLoopbackCapture? _capture;
    private readonly Func<string?, IWindowsLoopbackCapture> _captureFactory;
    private TaskCompletionSource _stopSignal = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private AudioCaptureSourceStatus _status = new(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.NotRequested);
    private bool _disposed;

    /// <summary>Creates a loopback capture for the default endpoint or a selected active output endpoint ID.</summary>
    /// <param name="endpointId">Optional ID returned by <see cref="GetOutputEndpoints"/>. When omitted, the default multimedia output endpoint is used.</param>
    public WindowsSystemAudioLoopbackCapture(string? endpointId = null)
        : this(endpointId, WindowsLoopbackCaptureFactory.Create)
    {
    }

    internal WindowsSystemAudioLoopbackCapture(
        string? endpointId,
        Func<string?, IWindowsLoopbackCapture> captureFactory)
    {
        if (endpointId is not null && string.IsNullOrWhiteSpace(endpointId))
            throw new ArgumentException("An endpoint ID must be nonblank when supplied.", nameof(endpointId));
        _endpointId = endpointId;
        _captureFactory = captureFactory ?? throw new ArgumentNullException(nameof(captureFactory));
    }

    /// <summary>Lists active Windows render endpoints without exposing NAudio types to callers.</summary>
    /// <exception cref="PlatformNotSupportedException">The current operating system is not Windows.</exception>
    public static IReadOnlyList<WindowsAudioOutputEndpoint> GetOutputEndpoints()
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Windows output endpoint enumeration is supported only on Windows.");

        using var enumerator = new MMDeviceEnumerator();
        using var defaultEndpoint = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        var endpoints = new List<WindowsAudioOutputEndpoint>(devices.Count);
        try
        {
            foreach (var device in devices)
            {
                endpoints.Add(new WindowsAudioOutputEndpoint(
                    device.ID,
                    device.FriendlyName,
                    string.Equals(device.ID, defaultEndpoint.ID, StringComparison.OrdinalIgnoreCase)));
            }
            return endpoints;
        }
        finally
        {
            foreach (var device in devices)
                device.Dispose();
        }
    }

    /// <summary>Raised with a copied audio buffer and format metadata.</summary>
    public event EventHandler<AudioCaptureBufferEventArgs>? AudioDataAvailable;
    /// <summary>Raised when the source status changes.</summary>
    public event EventHandler<AudioCaptureSourceStatus>? StatusChanged;
    /// <summary>
    /// Raised when an <see cref="AudioDataAvailable"/> or <see cref="StatusChanged"/> subscriber throws.
    /// Exceptions thrown by subscribers to this diagnostic event are isolated and ignored.
    /// </summary>
    public event EventHandler<Exception>? SubscriberFaulted;
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
                AudioCaptureSourceState.Unsupported, ErrorCode: "platform-unsupported",
                RecoveryGuidance: "Use a Windows host for native system-audio loopback capture."));
            throw new PlatformNotSupportedException("WASAPI loopback capture is supported only on Windows.");
        }

        if (_status.State == AudioCaptureSourceState.Capturing)
            return _status;

        // A capture can stop independently (for example, when its endpoint disappears).
        // Dispose that completed session before replacing it with a retry.
        if (_capture is not null)
            ReleaseCapture();

        SetStatus(_status.TransitionTo(AudioCaptureSourceState.RequestPending));
        try
        {
            _capture = _captureFactory(_endpointId);
            _capture.DataAvailable += OnDataAvailable;
            _capture.RecordingStopped += OnRecordingStopped;
            _stopSignal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
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

        try
        {
            if (_status.State == AudioCaptureSourceState.Capturing)
            {
                _capture.StopRecording();
                // NAudio signals completion asynchronously; allow its stop callback to settle before disposal.
                if (!_stopSignal.Task.Wait(TimeSpan.FromSeconds(5)))
                    throw new TimeoutException("The Windows audio capture stop callback did not arrive within five seconds.");
            }
        }
        catch
        {
            if (_status.State == AudioCaptureSourceState.Capturing)
                SetStatus(_status.TransitionTo(AudioCaptureSourceState.Failed,
                    errorCode: "capture-stop-failed",
                    recoveryGuidance: "The capture session could not stop cleanly; dispose the adapter and retry."));
            throw;
        }
        finally
        {
            ReleaseCapture();
            if (_status.State == AudioCaptureSourceState.Capturing)
                SetStatus(_status.TransitionTo(AudioCaptureSourceState.Ready));
        }
    }

    /// <summary>Stops capture and releases native resources.</summary>
    public void Dispose()
    {
        if (_disposed)
            return;
        try
        {
            Stop();
        }
        finally
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs args)
    {
        if (args.BytesRecorded <= 0 || _capture is null)
            return;
        var format = _capture.WaveFormat;
        var bytes = new byte[args.BytesRecorded];
        Buffer.BlockCopy(args.Buffer, 0, bytes, 0, args.BytesRecorded);
        NotifySubscribers(AudioDataAvailable, new AudioCaptureBufferEventArgs(
            bytes, format.SampleRate, format.Channels, format.BitsPerSample, format.Encoding.ToString()));
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs args)
    {
        _stopSignal.TrySetResult();

        if (_status.State is not (AudioCaptureSourceState.Capturing or AudioCaptureSourceState.Ready))
            return;

        if (args.Exception is not null)
        {
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Failed,
                errorCode: "capture-failed",
                recoveryGuidance: "Check the playback endpoint and retry the capture session."));
            return;
        }

        // RecordingStopped is authoritative even when the stop was initiated outside this adapter.
        // Keep the session restartable; Start() releases the completed native capture before retrying.
        if (_status.State == AudioCaptureSourceState.Capturing)
            SetStatus(_status.TransitionTo(AudioCaptureSourceState.Ready));
    }

    private void SetStatus(AudioCaptureSourceStatus status)
    {
        _status = status;
        NotifySubscribers(StatusChanged, status);
    }

    // Each consumer is isolated: a faulty handler must not escape a native callback,
    // prevent later handlers from receiving data, or mask a capture/start/stop failure.
    private void NotifySubscribers<TEventArgs>(EventHandler<TEventArgs>? handlers, TEventArgs args)
    {
        if (handlers is null)
            return;

        foreach (EventHandler<TEventArgs> handler in handlers.GetInvocationList())
        {
            try
            {
                handler(this, args);
            }
            catch (Exception exception)
            {
                NotifySubscriberFault(exception);
            }
        }
    }

    private void NotifySubscriberFault(Exception exception)
    {
        var handlers = SubscriberFaulted;
        if (handlers is null)
            return;

        foreach (EventHandler<Exception> handler in handlers.GetInvocationList())
        {
            try
            {
                handler(this, exception);
            }
            catch
            {
                // Diagnostics must never destabilize native capture or lifecycle cleanup.
            }
        }
    }

    private void ReleaseCapture()
    {
        try
        {
            if (_capture is not null)
            {
                _capture.DataAvailable -= OnDataAvailable;
                _capture.RecordingStopped -= OnRecordingStopped;
                _capture.Dispose();
            }
        }
        finally
        {
            _capture = null;
            // The capture implementation owns and releases any endpoint/device handles it acquires.
        }
    }
}

/// <summary>A copied native-format audio buffer and its reported format metadata from the loopback source.</summary>
/// <param name="Buffer">The captured bytes owned by the event consumer.</param>
/// <param name="SampleRate">Samples per second.</param>
/// <param name="Channels">Number of interleaved channels.</param>
/// <param name="BitsPerSample">Bits per sample reported by the capture format.</param>
/// <param name="Encoding">Encoding reported by the capture format.</param>
public sealed record AudioCaptureBufferEventArgs(
    byte[] Buffer, int SampleRate, int Channels, int BitsPerSample, string Encoding);
