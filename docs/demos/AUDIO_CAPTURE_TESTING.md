<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# Audio Capture Lab — manual verification guide

This guide explains how to verify the browser prototype in `audio-capture-lab.html`. It is a manual browser test, not a claim that every browser/operating-system combination is supported.

## Run the demo

1. Use a current desktop browser with screen/tab capture and microphone support.
2. Serve the repository over **HTTPS** or **localhost**. Opening the HTML from an arbitrary `file://` path or viewing it in GitHub's source viewer is not a valid test.
3. Open `docs/demos/audio-capture-lab.html`.
4. Confirm the green border and **READY · CHILLING** state appear before capture starts.

For a local checkout, run a static server from the directory containing the HTML file, for example:

```sh
python -m http.server 8000
```

Then open `http://localhost:8000/docs/demos/audio-capture-lab.html` from the repository root.

## What the browser and operating system control

- **Microphone:** the browser may request site permission, while operating-system privacy settings can independently block microphone access.
- **System/tab audio:** `getDisplayMedia` opens the browser's own screen/tab/window picker. The browser and selected surface determine whether an audio option is available. Select a source and enable its audio option when offered.
- **The page cannot grant OS permissions for you.** If access is blocked, change the relevant browser or operating-system privacy setting yourself and retry.
- **No upload:** the demo records locally in the page, then offers playback and a local WebM download. It does not upload the recording.

The system-audio checkbox does not promise universal capture of all computer sound. Browser, selected surface, operating system, and protected-content restrictions affect what can be captured.

## Verification checklist

Run each case in the browsers and operating systems you intend to support. Record browser/version, OS/version, selected source, and observed result when a case differs.

### Normal capture paths

- [ ] **Microphone only:** select only the microphone, start, approve access, and confirm the magenta countdown starts only after a usable audio track is acquired.
- [ ] **System/tab audio only:** select only system/tab audio, choose a surface in the browser picker, enable audio if offered, and verify the recording contains that source.
- [ ] **Both sources:** select both, grant/choose both, and verify the resulting recording contains the expected sources and mix.
- [ ] **No source selected:** attempt to start and confirm the page explains that at least one source must be selected; no countdown or recording begins.
- [ ] **Playback and download:** stop a recording, play it back, download the WebM, and verify the file is playable.
- [ ] **Repeat sessions:** perform multiple start/stop cycles and confirm each session resets the timer, controls, source status, and output metrics without reusing stale recording data.

### Denial, cancellation, and failure paths

- [ ] **Preflight cancelled:** cancel the page's source-confirmation step and verify no browser capture request is made.
- [ ] **Display picker dismissed:** dismiss the browser picker and confirm the page returns to a usable ready state with a clear message.
- [ ] **Microphone denied or OS-blocked:** deny site access or use the OS privacy control to block it; confirm the message distinguishes the likely cause where the browser exposes enough information and gives recovery steps.
- [ ] **Display selected without audio:** choose a surface that does not provide an audio track, or leave audio disabled when the picker offers that choice. Confirm the page reports that no usable audio track was supplied.
- [ ] **One source fails when both are required:** force one selected source to fail. Confirm partial streams are released and the page does not silently record a different source combination.
- [ ] **Source ends during capture:** stop sharing or disconnect the source where possible. Confirm the recording stops or returns to a safe state, and the user receives a clear explanation.
- [ ] **Retry after failure:** correct the permission/source problem and start again. Confirm the previous failure does not leave a countdown, active capture, disabled start button, or stale source status.
- [ ] **Close or reload during capture:** confirm the page stops acquired tracks and clears active timers on unload.

## Expected state behavior

| State | Border | Expected meaning |
|---|---|---|
| Ready / chilling | Green | No active recording; the page can accept a new capture request. |
| Countdown | Magenta | Required sources have been acquired and validated; recording has not started yet. |
| Recording | Red | The recorder is actively collecting the selected source mix. |
| Stopped / finalized | Green-ready styling | Recording has ended and the local result can be played or downloaded. |

If a required source fails before the countdown, the demo should return to a safe ready state. A permission prompt being displayed is not proof that permission was granted, and a granted permission is not proof that a usable audio track exists.

## Report a result

For a useful issue report, include:

- browser name and exact version;
- operating system and version;
- microphone/system-audio selections and selected display surface type (screen, window, or tab);
- whether the request was granted, denied, dismissed, or returned without audio;
- the exact page error/status text and any browser console error;
- whether retry and repeated start/stop worked.

Do not attach private recordings or sensitive system audio unless you deliberately intend to share that content.

---

[Audio Capture Lab](audio-capture-lab.html) · [FSM_UserIO API contract](../API_CONTRACTS.md) · [Repository issues](https://github.com/TrentBest/FSM_UserIO/issues)

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em>
</p>
