<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# WindowsAudioOutputEndpoint

**Evidence type:** Source-derived API reference; no dedicated test evidence was verified for this record.

**Namespace:** `TheSingularityWorkshop.FSM_UserIO.WindowsAudio`  
**Source:** [WindowsAudioOutputEndpoint.cs](../../../src/FSM_UserIO.WindowsAudio/WindowsAudioOutputEndpoint.cs)  
**Consumer:** [WindowsSystemAudioLoopbackCapture](WindowsSystemAudioLoopbackCapture.md)

## Purpose

`WindowsAudioOutputEndpoint` is immutable metadata describing an active Windows render endpoint that the Windows audio adapter can select. It is not a device handle and does not itself open, validate, or retain a native endpoint.

## Data contract

| Member | Meaning | Ownership / limit |
|---|---|---|
| `Id` | Opaque endpoint identifier understood by the Windows adapter | Treat as an adapter-issued selection token; do not parse or synthesize it. |
| `FriendlyName` | Display name reported by Windows | Presentation metadata, not a stable identity. |
| `IsDefault` | Whether Windows reports this endpoint as the default multimedia render endpoint | A snapshot fact at enumeration time; it may become stale after device/configuration changes. |

The public declaration is a sealed positional record:

```csharp
public sealed record WindowsAudioOutputEndpoint(
    string Id,
    string FriendlyName,
    bool IsDefault);
```

The record provides value-based equality through its record semantics. The declaration itself does not validate null/blank strings, refresh endpoint state, or guarantee that the endpoint remains active after enumeration.

## Lifecycle and authority boundary

1. The adapter enumerates active render endpoints.
2. The host presents the friendly name and retains the opaque ID.
3. The host may pass the selected ID to `WindowsSystemAudioLoopbackCapture`.
4. The adapter resolves the ID against Windows and checks that the selected device is active when capture starts.

Enumeration metadata is not authorization or a promise of future availability. The host remains responsible for disclosure, consent, and application policy.

## Complexity and freshness

This value object has no methods or internal collections. Construction and property access are constant-time reference/value operations. Endpoint enumeration and native device resolution belong to the adapter, not this record. No timing or hardware guarantees are implied.

## Verification

No dedicated test for this record was verified in the inspected test paths. Its current evidence is the source declaration and its use by the Windows adapter. Add focused tests if validation, normalization, or additional behavior is introduced.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
