<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# AudioCaptureSourceState

**Evidence:** Source-derived from [\`AudioCaptureSourceStatus.cs\`](https://github.com/TrentBest/FSM_UserIO/blob/development/AudioCaptureSourceStatus.cs), including the explicit transition table in \`AudioCaptureTransitions.IsAllowed\`.

## Purpose

Describes the lifecycle/readiness state reported for one audio source. The enum is a vocabulary, not a guarantee that the operating system will grant access or that audio data is flowing.

## Values

| Value | Meaning |
|---|---|
| `NotRequested = 0` | No request/query has been made for this session. |
| `Unsupported = 1` | The source or mechanism is unsupported in this context. |
| `RequestPending = 2` | Permission or source acquisition is in progress. |
| `Denied = 3` | A request was denied or permission revoked. |
| `Unavailable = 4` | The source is unavailable or supplied no usable audio track. |
| `Ready = 5` | A usable source was acquired; recording has not started. |
| `Capturing = 6` | The source is actively contributing audio to a capture. |
| `Cancelled = 7` | The request or active source was cancelled. |
| `Failed = 8` | An unexpected or adapter-defined failure occurred. |

## Transition contract

A status changes only through `AudioCaptureSourceStatus.TransitionTo`, which returns a new immutable record. The current source state and requested next state are checked against the explicit transition map; invalid enum values raise `ArgumentOutOfRangeException`, and disallowed edges raise `InvalidOperationException`.

The permitted edges are:

- `NotRequested → Unsupported | RequestPending`
- `Unsupported → NotRequested | RequestPending`
- `RequestPending → Unsupported | Denied | Unavailable | Ready | Cancelled | Failed`
- `Denied → NotRequested | RequestPending`
- `Unavailable → NotRequested | RequestPending | Unsupported`
- `Ready → NotRequested | RequestPending | Denied | Unavailable | Capturing | Cancelled | Failed`
- `Capturing → Ready | Denied | Unavailable | Cancelled | Failed`
- `Cancelled → NotRequested | RequestPending`
- `Failed → NotRequested | RequestPending`

These are the library's modeled edges, not an assertion that every host emits every state or event. A host adapter must translate platform-specific behavior into this source-neutral vocabulary.

## Related contracts and evidence

- [AudioCaptureSourceStatus](AudioCaptureSourceStatus.md)
- [Audio capture contract](../../AUDIO_CAPTURE_CONTRACT.md)
- [Source implementation](../../AudioCaptureSourceStatus.cs)
- [Audio source status tests](../../tests/FSM_UserIO.Tests/AudioCaptureSourceStatusTests.cs)


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
