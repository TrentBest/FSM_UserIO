using TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

if (args.Length == 1 && args[0] == "--list-endpoints")
{
    try
    {
        foreach (var endpoint in WindowsSystemAudioLoopbackCapture.GetOutputEndpoints())
            Console.WriteLine($"{(endpoint.IsDefault ? "* " : "  ")}{endpoint.FriendlyName}\n    ID: {endpoint.Id}");
        return;
    }
    catch (PlatformNotSupportedException ex)
    {
        Console.Error.WriteLine(ex.Message);
        Environment.ExitCode = 2;
        return;
    }
}

string? endpointId = null;
if (args.Length == 2 && args[0] == "--endpoint-id")
{
    endpointId = args[1];
}
else if (args.Length != 0)
{
    Console.Error.WriteLine("Usage: WindowsLoopbackSample [--list-endpoints | --endpoint-id <id>]");
    Environment.ExitCode = 64;
    return;
}

using var capture = new WindowsSystemAudioLoopbackCapture(endpointId);
long totalBytes = 0;
capture.StatusChanged += (_, status) =>
    Console.WriteLine($"Source status: {status.State} ({status.ErrorCode ?? "no error"})");
capture.AudioDataAvailable += (_, data) =>
{
    Interlocked.Add(ref totalBytes, data.Buffer.Length);
    Console.WriteLine($"Audio: {data.Buffer.Length} bytes, {data.SampleRate} Hz, {data.Channels} channel(s), {data.BitsPerSample}-bit, {data.Encoding}");
};

Console.WriteLine("Windows system-audio loopback sample");
Console.WriteLine("Play non-protected audio through the selected Windows output device.");
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
