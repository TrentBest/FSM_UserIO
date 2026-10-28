using NAudio.CoreAudioApi;
using NAudio.Wave;
using TheSingularityWorkshop.FSM_UserIO;

namespace TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

/// <summary>Captures the default Windows playback endpoint using WASAPI loopback.</summary>
/// <remarks>Captures system playback only. The host remains responsible for disclosure and application-level authorization.</remarks>
public sealed class WindowsSystemAudioLoopbackCapture : IDisposable
{
    private readonly string? _endpointId;
    private MMDevice? _selectedDevice;
    private WasapiLoopbackCapture? _capture;
    private TaskCompletionSource _stopSignal = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private AudioCaptureSourceStatus _status = new(AudioCaptureSourceKind.SystemAudio, AudioCaptureSourceState.NotRequested);
    private bool _disposed;

    /// <summary>Creates a loopback capture for the default endpoint or a selected active output endpoint ID.</summary>
    /// <param name="endpointId">Optional ID returned by <see cref="GetOutputEndpoints"/>. When omitted, the default multimedia output endpoint is used.</param>
    public WindowsSystemAudioLoopbackCapture(string? endpointId = null)
    {
        if (endpointId is not null && string.IsNullOrWhiteSpace(endpointId))
            throw new ArgumentException("An endpoint ID must be nonblank when supplied.", nameof(endpointId));
        _endpointId = endpointId;
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
        foreach (var device in devices)
        {
            try
            {
                endpoints.Add(new WindowsAudioOutputEndpoint(
                    device.ID,
                    device.FriendlyName,
                    string.Equals(device.ID, defaultEndpoint.ID, StringComparison.OrdinalIgnoreCase)));
            }
            finally
            {
                device.Dispose();
            }
        }
        return endpoints;
    }

    /// <summary>Raised with a copied audio buffer and format metadata.</summary>
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
                AudioCaptureSourceState.Unsupported, ErrorCode: "platform-unsupported",
                RecoveryGuidance: "Use a Windows host for native system-audio loopback capture."));
            throw new PlatformNotSupportedException("WASAPI loopback capture is supported only on Windows.");
        }

        if (_status.State == AudioCaptureSourceState.Capturing)
            return _status;

        SetStatus(_status.TransitionTo(AudioCaptureSourceState.RequestPending));
        try
        {
            if (_endpointId is null)
            {
                _capture = new WasapiLoopbackCapture();
            }
            else
            {
                using var enumerator = new MMDeviceEnumerator();
                _selectedDevice = enumerator.GetDevice(_endpointId);
                if (_selectedDevice.State != DeviceState.Active)
                    throw new InvalidOperationException("The selected Windows output endpoint is not active.");
                _capture = new WasapiLoopbackCapture(_selectedDevice);
            }
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
        AudioDataAvailable?.Invoke(this, new AudioCapturePcmDataEventArgs(
            bytes, format.SampleRate, format.Channels, format.BitsPerSample, format.Encoding.ToString()));
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs args)
    {
        _stopSignal.TrySetResult();

        if (args.Exception is not null &&
            (_status.State is AudioCaptureSourceState.Capturing or AudioCaptureSourceState.Ready))
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
        try
        {
            _capture.Dispose();
        }
        finally
        {
            _capture = null;
            _selectedDevice?.Dispose();
            _selectedDevice = null;
        }
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
