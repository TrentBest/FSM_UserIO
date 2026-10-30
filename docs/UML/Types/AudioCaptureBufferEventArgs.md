<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# AudioCaptureBufferEventArgs

**Evidence type:** Source-derived API reference; no dedicated adapter test evidence was verified in the inspected test paths.

**Namespace:** `TheSingularityWorkshop.FSM_UserIO.WindowsAudio`  
**Source:** [WindowsSystemAudioLoopbackCapture.cs](../../../src/FSM_UserIO.WindowsAudio/WindowsSystemAudioLoopbackCapture.cs)  
**Producer:** [WindowsSystemAudioLoopbackCapture](WindowsSystemAudioLoopbackCapture.md)

## Purpose

`AudioCaptureBufferEventArgs` carries one audio buffer and the format metadata reported by the Windows loopback capture adapter. It is an adapter-level event payload, not a normalized cross-platform audio format.

## Data contract

| Member | Meaning |
|---|---|
| `Buffer` | Byte array copied from the NAudio callback buffer for this event. |
| `SampleRate` | Sample rate reported by the capture format, in samples per second. |
| `Channels` | Channel count reported by the capture format. |
| `BitsPerSample` | Bits per sample reported by the capture format. |
| `Encoding` | Encoding name reported by the capture format. |

The public declaration is a sealed positional record:

```csharp
public sealed record AudioCaptureBufferEventArgs(
    byte[] Buffer,
    int SampleRate,
    int Channels,
    int BitsPerSample,
    string Encoding);
```

## Buffer ownership

The adapter allocates a new byte array for each callback and copies exactly `BytesRecorded` bytes from the NAudio-owned buffer before raising `AudioDataAvailable`. This prevents the consumer from retaining a reference to NAudio's reusable callback buffer.

The event argument record does **not** make the array immutable or defensively copy it on construction/access. A subscriber can mutate the array, and subscribers receiving the same event-args instance share that array. Consumers that fan out data across trust or ownership boundaries should make their own copy or transfer ownership explicitly.

## Format and authority limits

- The format fields are forwarded from the capture device's reported `WaveFormat`; the adapter does not resample or convert the payload.
- The payload should be interpreted using the accompanying format metadata, not assumed to be a fixed PCM layout.
- The record does not validate buffer contents, dimensions, encoding names, or consistency between metadata and byte length.
- Receiving a buffer does not establish user consent, authorization, or permission to retain or transmit audio.

## Complexity

The adapter's callback copy is O(n) in `BytesRecorded`, with one new byte-array allocation of that size per non-empty callback. Record construction is constant-time aside from storing references/values. No throughput, latency, or allocation-rate guarantee is implied.

## Verification

No dedicated Windows adapter test suite was verified in the inspected paths. The source establishes the copy-before-event behavior; it does not prove actual WASAPI delivery, format correctness on hardware, callback timing, or device-removal behavior. Those remain hardware/integration verification concerns documented in [Windows system-audio loopback](../../WINDOWS_SYSTEM_AUDIO_LOOPBACK.md).

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
