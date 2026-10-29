<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# AudioCaptureStateEvidence

**Evidence:** Source-derived from [\`AudioCaptureSourceStatus.cs\`](https://github.com/TrentBest/FSM_UserIO/blob/development/AudioCaptureSourceStatus.cs).

## Purpose

Distinguishes direct observations from a host's estimate when describing an audio source's readiness or lifecycle. This distinction prevents a capability estimate from being mistaken for proof that a source was successfully acquired.

## Values

| Value | Meaning | Interpretation |
|---|---|---|
| `Observed = 0` | The adapter directly observed the state or event. | Evidence about the adapter's observation; not an authorization decision. |
| `HostReportedEstimate = 1` | A host reports an estimate. | Not proof that a source can be acquired or that audio is flowing. |

## Validation and boundaries

`AudioCaptureSourceStatus` validates the enum through `SemanticValueValidation.Defined`. `TransitionTo` also validates the requested evidence value. The enum does not attest to the reliability of the host, authenticate a source, or grant application/OS permission.

## Related contracts and evidence

- [AudioCaptureSourceStatus](AudioCaptureSourceStatus.md)
- [Audio capture contract](../../AUDIO_CAPTURE_CONTRACT.md)
- [Source implementation](../../AudioCaptureSourceStatus.cs)
- [Audio source status tests](../../tests/FSM_UserIO.Tests/AudioCaptureSourceStatusTests.cs)


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
