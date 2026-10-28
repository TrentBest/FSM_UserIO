<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# Audio capture source contract

## Purpose

This is the platform-neutral contract shared by browser and native audio-capture adapters. It describes **one source at a time** so microphone and system/output audio can succeed or fail independently. It does not request device access, mix audio, own a recorder, grant application authority, or guarantee that an operating system permits capture.

## Source and state vocabulary

| Contract | Meaning |
|---|---|
| `AudioCaptureSourceKind.Microphone` | Microphone input |
| `AudioCaptureSourceKind.SystemAudio` | System playback, output endpoint, or browser-selected shared surface |
| `NotRequested` | No acquisition request has been made in this session |
| `Unsupported` | The adapter/host reports that this source or mechanism is unsupported |
| `RequestPending` | Permission or source acquisition is in progress |
| `Denied` | Request denied or permission revoked |
| `Unavailable` | Source is absent or acquisition yielded no usable audio track |
| `Ready` | A usable source is acquired; recording has not started |
| `Capturing` | Source is actively contributing to a capture |
| `Cancelled` | Pending request or active source was cancelled |
| `Failed` | Unexpected or adapter-defined failure |

The source status records whether evidence was `Observed` directly or a `HostReportedEstimate`. An estimate is not proof that capture will succeed. Browser APIs and Windows audio endpoints have different acquisition and privacy behavior, but neither set of platform types belongs in the core contract.

## Transition rules

Transitions are explicit and validated by `AudioCaptureSourceStatus.TransitionTo`:

```text
NotRequested -> RequestPending | Unsupported
Unsupported  -> NotRequested | RequestPending
RequestPending -> Ready | Unsupported | Denied | Unavailable | Cancelled | Failed
Ready -> Capturing | RequestPending | NotRequested | Denied | Unavailable | Cancelled | Failed
Capturing -> Ready | Denied | Unavailable | Cancelled | Failed
Denied | Unavailable | Cancelled | Failed -> RequestPending or NotRequested
Unavailable -> Unsupported is also allowed
```

Invalid transitions throw `InvalidOperationException`; undefined enum values are rejected. Each transition returns a new immutable status, leaving the previous value unchanged. Retry paths must begin a new acquisition request rather than jumping directly from failure to ready/capturing.

A source that disappears or has permission revoked during capture transitions out of `Capturing`; the adapter/host must stop or reconfigure the actual recorder and release native resources. This value contract does not itself stop streams, cancel an OS request, or dispose resources.

## Errors and recovery

`ErrorCode` and `RecoveryGuidance` are optional source-neutral strings. They should explain what failed without leaking browser exception types, OS handles, or native SDK types. Suggested stable codes include `permission-denied`, `permission-revoked`, `no-audio-track`, `source-ended`, and `capture-failed`.

Recovery guidance may suggest checking site or OS privacy settings, choosing an audio-capable surface, or retrying after a device becomes available. An adapter must not claim it can grant OS permission itself. Error messages are explanatory data, not proof of root cause and not authorization decisions.

## Partial success

Keep one status per source. For example, microphone can be `Denied` while system audio is `Ready`. The host's selected recording mode decides whether that combination is acceptable; UserIO does not silently downgrade the requested mode or choose which sources are required.

## Authority and privacy boundaries

- A `Ready` status means a source was acquired/reported ready; it does not authorize an application action.
- A microphone permission does not imply permission to capture system audio, or vice versa.
- UserIO does not infer a universal OS permission prompt for loopback capture.
- Source adapters own actual acquisition, track/device-ended events, cleanup, sample-format conversion, and native resource disposal.
- The host owns the selected mode, policy, consent UX, and whether a partial source set is acceptable.
- The core contains no browser, Windows, WASAPI, GUI, or device SDK dependency.

## Verification

`tests/FSM_UserIO.Tests/AudioCaptureSourceStatusTests.cs` covers normal lifecycle, invalid transitions, independent source status, partial success, source loss/revocation, direct observation versus host estimate, invalid enum values, and diagnostic metadata. These are deterministic contract tests; they do not claim to exercise a real microphone, browser permission prompt, or Windows output endpoint.

See the [Audio Capture Lab manual verification guide](demos/AUDIO_CAPTURE_TESTING.md) for browser-specific end-to-end steps and the [API Contracts](API_CONTRACTS.md) for the wider semantic boundary.

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://www.patreon.com/TheSingularityWorkshop">Support the Workshop</a>
  <br />
  <img src="assets/Avatar.jpg" alt="The Singularity Workshop avatar" width="96" />
</p>
