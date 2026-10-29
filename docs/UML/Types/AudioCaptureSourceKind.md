<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# AudioCaptureSourceKind

**Evidence:** Source-derived from [\`AudioCaptureSourceStatus.cs\`](https://github.com/TrentBest/FSM_UserIO/blob/development/AudioCaptureSourceStatus.cs); test coverage is linked below.

## Purpose

Identifies which independent audio input a status describes without exposing browser, operating-system, or device-SDK types.

## Values

| Value | Meaning | Boundary |
|---|---|---|
| `Microphone = 0` | Audio captured from a microphone input. | Names a source category; it does not prove permission, availability, or active capture. |
| `SystemAudio = 1` | Audio from system playback, an output endpoint, or a browser-selected shared surface. | Does not guarantee that the host or platform supports loopback capture. |

## Use and validation

`AudioCaptureSourceStatus` validates this enum with the shared `SemanticValueValidation.Defined` helper. Undefined numeric enum values are rejected when constructing a status. The enum itself does not carry a device handle, identity, permission, or lifecycle state.

## Related contracts and evidence

- [AudioCaptureSourceStatus](AudioCaptureSourceStatus.md)
- [Source implementation](../../AudioCaptureSourceStatus.cs)
- [Audio source status tests](../../tests/FSM_UserIO.Tests/AudioCaptureSourceStatusTests.cs)


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
