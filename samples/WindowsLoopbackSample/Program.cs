using TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

using var capture = new WindowsSystemAudioLoopbackCapture();
long totalBytes = 0;
capture.StatusChanged += (_, status) =>
    Console.WriteLine($"Source status: {status.State} ({status.ErrorCode ?? "no error"})");
capture.AudioDataAvailable += (_, data) =>
{
    Interlocked.Add(ref totalBytes, data.Buffer.Length);
    Console.WriteLine($"Audio: {data.Buffer.Length} bytes, {data.SampleRate} Hz, {data.Channels} channel(s), {data.BitsPerSample}-bit, {data.Encoding}");
};

Console.WriteLine("Windows system-audio loopback sample");
Console.WriteLine("Play non-protected audio through the default Windows output device.");
Console.WriteLine("Capturing for 10 seconds. Press Ctrl+C to stop the sample process.");
try
{
    capture.Start();
    await Task.Delay(TimeSpan.FromSeconds(10));
    capture.Stop();
    Console.WriteLine($"Capture stopped. Total received: {Interlocked.Read(ref totalBytes):N0} bytes.");
}
catch (PlatformNotSupportedException ex)
{
    Console.Error.WriteLine(ex.Message);
    Environment.ExitCode = 2;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Loopback capture failed: {ex.Message}");
    Environment.ExitCode = 1;
}
