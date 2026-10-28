<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# Windows System-Audio Loopback Capture

This sample exercises the optional Windows-specific adapter for capturing audio played through either the default multimedia output endpoint or an explicitly selected active output endpoint. It is separate from the platform-neutral FSM_UserIO core and does not capture microphone input or mix sources.

## Requirements

- Windows 10 or later with a usable playback endpoint.
- .NET 8 SDK.
- A usable active Windows playback endpoint; the sample defaults to the multimedia endpoint unless an endpoint ID is supplied.
- Audio that is permitted to be captured in the current application and environment.

## Run the sample

From the repository root on Windows:

```powershell
# List active output endpoints. The asterisk marks the default multimedia endpoint.
dotnet run --project samples/WindowsLoopbackSample/WindowsLoopbackSample.csproj --configuration Release -- --list-endpoints

# Capture the default multimedia output endpoint for ten seconds.
dotnet run --project samples/WindowsLoopbackSample/WindowsLoopbackSample.csproj --configuration Release

# Capture a specific endpoint using the ID shown by --list-endpoints.
dotnet run --project samples/WindowsLoopbackSample/WindowsLoopbackSample.csproj --configuration Release -- --endpoint-id "{endpoint-id}"
```

Start non-protected playback through the selected output device. The sample reports observed capture format and received buffer sizes for ten seconds, then stops and disposes the capture object.

## What the adapter does

- Enumerates active render endpoints and marks the default multimedia endpoint.
- Uses WASAPI loopback through NAudio against the default or explicitly selected endpoint ID.
- Emits copied audio buffers with sample rate, channel count, bit depth, and encoding metadata.
- Reports source-neutral lifecycle states through `AudioCaptureSourceStatus`.
- Keeps Windows/NAudio types out of the core public contract.
- Does not combine microphone audio with system playback.

The sample now exposes endpoint enumeration and explicit endpoint selection by ID. It forwards the capture format as reported by the native library; it does not promise a particular sample rate or convert/resample buffers.

## Privacy and reliability boundaries

Loopback capture is not microphone capture and should not be described as a universal OS permission prompt. The host application must disclose capture, obtain any consent required by its context, and control how the captured content is used. Protected content and endpoint/device configurations may prevent capture or yield silence.

## Manual verification checklist

1. Build and run on a real Windows machine.
2. Run `--list-endpoints`, then play ordinary, non-protected audio through the default endpoint and, separately, a selected non-default endpoint where available.
3. Confirm that buffers arrive and report non-zero byte counts and plausible format metadata for each selected endpoint.
4. Stop playback and confirm capture stops without hanging.
5. Repeat start/stop sessions and verify that the output endpoint remains usable.
6. Disconnect or change the output device and observe the resulting error/status behavior.
7. Run on a non-Windows host and confirm the adapter reports `Unsupported` and throws `PlatformNotSupportedException`.
8. Verify application disclosure/consent and the actual behavior of the specific endpoint and content.

The Windows CI job validates compilation of the adapter and sample. It does not replace this hardware-dependent manual verification.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
