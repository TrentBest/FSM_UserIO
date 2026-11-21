using NAudio.Wave;
using TheSingularityWorkshop.FSM_UserIO;
using TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

namespace FSM_UserIO.WindowsAudio.Tests;

public sealed class WindowsSystemAudioLoopbackCaptureTests
{
    [Fact]
    public void Start_uses_injected_capture_and_reports_capturing()
    {
        var fake = new FakeCapture();
        using var adapter = new WindowsSystemAudioLoopbackCapture(null, _ => fake);

        var status = adapter.Start();

        Assert.Equal(AudioCaptureSourceState.Capturing, status.State);
        Assert.Equal(1, fake.StartCount);
        adapter.Stop();
        Assert.Equal(AudioCaptureSourceState.Ready, adapter.Status.State);
        Assert.Equal(1, fake.DisposeCount);
    }

    [Fact]
    public void External_stop_returns_adapter_to_ready_and_allows_retry()
    {
        var captures = new List<FakeCapture>();
        using var adapter = new WindowsSystemAudioLoopbackCapture(null, _ =>
        {
            var capture = new FakeCapture();
            captures.Add(capture);
            return capture;
        });

        adapter.Start();
        captures[0].RaiseStopped();

        Assert.Equal(AudioCaptureSourceState.Ready, adapter.Status.State);
        adapter.Start();

        Assert.Equal(2, captures.Count);
        Assert.Equal(AudioCaptureSourceState.Capturing, adapter.Status.State);
        adapter.Stop();
    }

    [Fact]
    public void Data_subscriber_failure_is_reported_without_blocking_later_subscribers()
    {
        var fake = new FakeCapture();
        using var adapter = new WindowsSystemAudioLoopbackCapture(null, _ => fake);
        var laterSubscriberCalled = false;
        Exception? reported = null;
        adapter.AudioDataAvailable += (_, _) => throw new InvalidOperationException("consumer failure");
        adapter.AudioDataAvailable += (_, _) => laterSubscriberCalled = true;
        adapter.SubscriberFaulted += (_, exception) => reported = exception;

        adapter.Start();
        fake.RaiseData();

        Assert.True(laterSubscriberCalled);
        Assert.IsType<InvalidOperationException>(reported);
        adapter.Stop();
    }

    [Fact]
    public void Dispose_releases_capture_and_prevents_restart()
    {
        var fake = new FakeCapture();
        var adapter = new WindowsSystemAudioLoopbackCapture(null, _ => fake);
        adapter.Start();

        adapter.Dispose();
        adapter.Dispose();

        Assert.Equal(1, fake.DisposeCount);
        Assert.Throws<ObjectDisposedException>(() => adapter.Start());
    }

    private sealed class FakeCapture : IWindowsLoopbackCapture
    {
        public WaveFormat WaveFormat { get; } = new(48_000, 16, 2);
        public int StartCount { get; private set; }
        public int DisposeCount { get; private set; }
        public event EventHandler<WaveInEventArgs>? DataAvailable;
        public event EventHandler<StoppedEventArgs>? RecordingStopped;

        public void StartRecording() => StartCount++;

        public void StopRecording() => RaiseStopped();

        public void RaiseData()
        {
            var buffer = new byte[] { 1, 2, 3, 4 };
            DataAvailable?.Invoke(this, new WaveInEventArgs(buffer, buffer.Length));
        }

        public void RaiseStopped() =>
            RecordingStopped?.Invoke(this, new StoppedEventArgs(null));

        public void Dispose() => DisposeCount++;
    }
}
