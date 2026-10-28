<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# Windows system-audio loopback: architecture decision

## Decision status

**Implemented prototype; hardware verification pending.** A Windows-targeted adapter and runnable sample now exist on `development`. The Windows CI workflow is intended to verify compilation; no real-device capture run has been claimed.

## Decision

Keep `TheSingularityWorkshop.FSM_UserIO` platform-neutral and add native loopback only in a separate Windows-specific adapter project. The adapter should use Windows Core Audio WASAPI loopback capture for the selected render endpoint, report lifecycle through `AudioCaptureSourceStatus`, and own all native/audio-library resources. The host remains responsible for source selection policy, consent UX, recording mode, and whether capture is allowed in its application.

The first adapter should capture **system playback only**. Microphone capture and mixing are separate operations; do not silently combine them.

## Why there are two paths

| Concern | Browser capture lab | Native Windows loopback |
|---|---|---|
| Source selection | Browser-controlled display/tab/window sharing flow; availability varies by browser and selected surface | Host/adapter selects or enumerates an output/render endpoint |
| Acquisition | Browser media-capture APIs and browser permission/share UI | Windows Core Audio loopback API through a Windows adapter |
| User-facing consent | Browser-controlled prompt and site permission behavior | Do not invent a universal loopback permission prompt; host must disclose capture and follow its own product/privacy policy |
| Device/surface changes | Track ended/muted events; browser-specific behavior | Endpoint changes, device removal, stream failures and format changes need explicit handling |
| Portability | Browser and HTTPS/localhost constraints | Windows only; other platforms report unsupported through a platform-neutral result |
| Verification | Manual browser matrix | Real Windows playback/capture test plus disposal and endpoint-change checks |

Neither path guarantees access to every sound. Protected content, endpoint configuration, exclusive-mode behavior, device changes, and OS/application restrictions can affect results.

## Project and dependency boundary

- Core remains the existing `net8.0` project and must not reference Windows SDK types, NAudio, WASAPI, or GUI/browser APIs.
- The adapter is a separate `net8.0-windows` project using NAudio 2.2.1. It enumerates active render endpoints and can capture either the default multimedia endpoint or an explicitly selected endpoint ID. Format conversion is not implemented; the reported native format is forwarded unchanged.
- Prefer a narrow adapter contract over exposing library-specific recorder, wave-format, endpoint, or COM types to consumers.
- Endpoint enumeration and explicit endpoint-ID selection are implemented. Injectable capture-source construction remains a possible testability improvement, not a public core contract.
- Do not add a Windows adapter reference to the core project or make the core package transitively platform-specific.

## Lifecycle and status mapping

The adapter reports one `AudioCaptureSourceStatus` for `SystemAudio`; it does not mutate status behind the host's back.

| Adapter event | Status intent |
|---|---|
| Adapter cannot run on this platform | `Unsupported` |
| Capture requested, endpoint/acquisition pending | `RequestPending` |
| A usable output endpoint and capture stream are established | `Ready` |
| Capture actively yields samples | `Capturing` |
| Caller cancels before or during capture | `Cancelled` |
| Selected endpoint disappears or cannot yield audio | `Unavailable` or `Failed`, with a stable source-neutral error code |
| Capture stops cleanly but the source remains acquired | `Ready` if restart is supported; otherwise return to `NotRequested` according to the adapter's documented lifecycle |
| Caller disposes the capture session | Stop callbacks, stop capture, release buffers/streams and native endpoint resources; disposal must be safe to repeat |

The exact status transition must follow the shared state machine. Do not label a stream `Capturing` merely because an endpoint exists: observe successful capture/start behavior. Host-reported estimates must be marked as estimates.

## Audio format, buffering, cancellation, and cleanup

- Preserve the endpoint's reported format at the native boundary, then expose a documented stable sample representation to consumers. Conversion must be explicit; never assume all endpoints use the same sample rate, channel count, or sample encoding.
- Make sample-rate/channel conversion a separate, testable stage. Document whether output is interleaved and the exact sample type/units before exposing buffers publicly.
- Use bounded buffering and define what happens on consumer backpressure (for example, report dropped frames rather than growing memory without limit).
- Cancellation must stop acquisition and unblock pending reads; disposal must release the capture stream and all owned resources even when startup fails.
- Device removal, stream failure, cancellation, and consumer exceptions must not leak native resources or leave the session reporting `Capturing`.
- Mixing with microphone input is deliberately out of scope for the first adapter. A future mixer must define clock drift, resampling, channel mapping, and per-source failure behavior.

## Privacy and authority

WASAPI loopback captures audio rendered through an output endpoint; it is not microphone capture. A separate universal OS permission prompt should not be claimed unless verified for the exact supported Windows versions and deployment model. This does not remove the application's responsibility to disclose recording, obtain any consent required by law/product policy, or respect protected-content and platform restrictions. The adapter provides technical capability, not authorization.

## Verification gate

Do not call the adapter complete until a Windows runner or manual Windows session proves all of the following:

1. Play known audio through a selected output endpoint and verify captured samples are non-empty and plausibly match playback.
2. Stop and dispose the session, then start a second session; verify no stale stream or duplicate callbacks.
3. Cancel during startup and during active capture; verify completion and resource cleanup.
4. Remove/change the selected output endpoint and verify a predictable status/error plus cleanup.
5. Verify unsupported-platform behavior separately from Windows capture behavior.
6. Record actual format, sample-rate/channel conversion behavior, and any limitations observed with protected/exclusive-mode content.

Unit tests can prove state mapping, cancellation orchestration, format conversion and buffer bounds without a physical endpoint. They cannot replace the real Windows playback test.

## Current evidence

On `development`, the source-neutral status contract, browser lab finalization guard, Windows-targeted adapter, endpoint enumeration/selection, runnable sample, and Windows-specific build workflow are present. The adapter emits copied buffers with the format reported by NAudio and maps basic start/stop/failure events to source-neutral status. The sample has not been run against a real output endpoint in this environment, and no real-device capture result is claimed. Device-removal behavior, cancellation during acquisition, and format conversion remain unverified or unimplemented. Capture data is delivered directly through events rather than an adapter-owned queue; consumer handlers should avoid blocking and should not retain unbounded numbers of copied buffers.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
