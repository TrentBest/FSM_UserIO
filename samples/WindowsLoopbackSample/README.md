![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Windows System-Audio Loopback Capture

This sample exercises the optional Windows-specific adapter for capturing audio played through the default Windows multimedia output endpoint. It is separate from the platform-neutral FSM_UserIO core and does not capture microphone input or mix sources.

## Requirements

- Windows 10 or later with a usable playback endpoint.
- .NET 8 SDK.
- An output device selected as the default multimedia playback device.
- Audio that is permitted to be captured in the current application and environment.

## Run the sample

From the repository root on Windows:

```powershell
dotnet run --project samples/WindowsLoopbackSample/WindowsLoopbackSample.csproj --configuration Release
```

Start playback through the default output device when prompted. The sample reports observed capture format and received buffer sizes for ten seconds, then stops and disposes the capture object.

## What the adapter does

- Uses WASAPI loopback through NAudio against the default multimedia render endpoint.
- Emits copied audio buffers with sample rate, channel count, bit depth, and encoding metadata.
- Reports source-neutral lifecycle states through `AudioCaptureSourceStatus`.
- Keeps Windows/NAudio types out of the core public contract.
- Does not combine microphone audio with system playback.

The sample currently chooses the default endpoint. It does not yet expose endpoint enumeration or explicit endpoint selection. It also forwards the capture format as reported by the native library; it does not promise a particular sample rate or convert/resample buffers.

## Privacy and reliability boundaries

Loopback capture is not microphone capture and should not be described as a universal OS permission prompt. The host application must disclose capture, obtain any consent required by its context, and control how the captured content is used. Protected content and endpoint/device configurations may prevent capture or yield silence.

## Manual verification checklist

1. Build and run on a real Windows machine.
2. Play ordinary, non-protected audio through the default output endpoint.
3. Confirm that buffers arrive and report non-zero byte counts and plausible format metadata.
4. Stop playback and confirm capture stops without hanging.
5. Repeat start/stop sessions and verify that the output endpoint remains usable.
6. Disconnect or change the output device and observe the resulting error/status behavior.
7. Run on a non-Windows host and confirm the adapter reports `Unsupported` and throws `PlatformNotSupportedException`.
8. Verify application disclosure/consent and the actual behavior of the specific endpoint and content.

The Windows CI job validates compilation of the adapter and sample. It does not replace this hardware-dependent manual verification.

---

![The Singularity Workshop](../../docs/assets/Avatar.jpg)
