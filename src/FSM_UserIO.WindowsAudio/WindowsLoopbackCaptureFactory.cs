using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

// Internal seam: the public adapter contract remains independent of NAudio types.
internal interface IWindowsLoopbackCapture : IDisposable
{
    WaveFormat WaveFormat { get; }
    event EventHandler<WaveInEventArgs>? DataAvailable;
    event EventHandler<StoppedEventArgs>? RecordingStopped;
    void StartRecording();
    void StopRecording();
}

internal static class WindowsLoopbackCaptureFactory
{
    internal static IWindowsLoopbackCapture Create(string? endpointId) =>
        new NAudioWindowsLoopbackCapture(endpointId);
}

internal sealed class NAudioWindowsLoopbackCapture : IWindowsLoopbackCapture
{
    private readonly MMDevice? _device;
    private readonly WasapiLoopbackCapture _capture;
    private bool _disposed;

    internal NAudioWindowsLoopbackCapture(string? endpointId)
    {
        if (endpointId is null)
        {
            _capture = new WasapiLoopbackCapture();
            return;
        }

        using var enumerator = new MMDeviceEnumerator();
        var device = enumerator.GetDevice(endpointId);
        _device = device;
        if (device.State != DeviceState.Active)
        {
            device.Dispose();
            throw new InvalidOperationException("The selected Windows output endpoint is not active.");
        }

        try
        {
            _capture = new WasapiLoopbackCapture(device);
        }
        catch
        {
            device.Dispose();
            throw;
        }
    }

    public WaveFormat WaveFormat => _capture.WaveFormat;

    public event EventHandler<WaveInEventArgs>? DataAvailable
    {
        add => _capture.DataAvailable += value;
        remove => _capture.DataAvailable -= value;
    }

    public event EventHandler<StoppedEventArgs>? RecordingStopped
    {
        add => _capture.RecordingStopped += value;
        remove => _capture.RecordingStopped -= value;
    }

    public void StartRecording() => _capture.StartRecording();

    public void StopRecording() => _capture.StopRecording();

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        try
        {
            _capture.Dispose();
        }
        finally
        {
            _device?.Dispose();
        }
    }
}
