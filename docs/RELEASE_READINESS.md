<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# FSM_UserIO Release Readiness Checklist

> **Purpose:** This is the living, repository-level release gate for FSM_UserIO. It defines the evidence required before the repository may be presented to Trent as release-ready. A completed checklist means **ready for final owner approval**; it does not authorize a merge, tag, deployment, or NuGet publication.

## Release decision and status

- **Active source of truth:** `development`.
- **Current published baseline:** `0.1.0-alpha.1` (the published package does not contain the expanded development API).
- **Candidate version:** decide explicitly after the API and compatibility review; never infer it from the current project version.
- **Readiness state:** NOT RELEASE-READY. Core CI has passed on recent development commits, but the exact current head must be checked after the latest evidence/workflow changes; release-critical API, compatibility, package, adapter, and owner-approval audits remain open.
- **Final authority:** Trent Best.
- **Publication state:** hard-disabled in GitHub Actions with a final `&& false` condition.

Do not use a percentage as a substitute for evidence. Every release-critical item must be checked with a link, command output, artifact, test, or explicit owner decision. Re-open any evidence invalidated by a later commit.

## 1. Product boundary and scope

- [ ] Confirm the release scope: identify every public type/member intended for this package version.
- [ ] Confirm the release is a coherent semantic-interaction contract, not an accidental bundle of unrelated audio, GUI, device, or platform functionality.
- [ ] Confirm the core ownership invariant remains true: UserIO carries source-neutral semantic interaction; it does not own actors, devices, presentation, domain data, policy, authorization, transport, or execution.
- [ ] Confirm capability is descriptive, not authority; observation is not acceptance; intent is not execution; feedback is not proof of authorization or completion.
- [ ] Confirm each contract has a real consumer/use case and is not speculative ontology.
- [ ] Resolve or explicitly defer every open decision in the implementation roadmap (including provenance/trust, result semantics, versioning/compatibility, and adapter conformance).
- [ ] Confirm deferred questions are documented as non-guarantees rather than implied behavior.
- [ ] Confirm no breaking or scope-expanding API change has slipped into the candidate without review.

## 2. Public API and implementation audit

Audit **every** public type, constructor, property, enum, and method against source, XML documentation, tests, README, API contract documentation, samples, and package contents.

- [x] Verify `SemanticIntent` validation, exact name preservation, optional `ProtocolId`, equality/hash behavior, null/blank handling, and numeric boundary behavior. Direct edge tests were added and passed in exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823).
- [ ] Verify `SemanticInteraction` composition and immutability/value semantics.
- [ ] Verify `InteractionCapability` and `InteractionMode` semantics and ensure no universal capability ontology is implied.
- [ ] Verify `SemanticObservation` timestamps/source metadata are descriptive and do not imply authenticated provenance, ordering, or trust.
- [ ] Verify `SemanticExpression` semantics remain distinct from observation and do not imply acceptance.
- [ ] Verify `SemanticPayload` is opaque; document that schema interpretation, serialization, size limits, and validation are not silently promised.
- [ ] Verify `InteractionContext` correlation/participant/session labels do not imply identity authentication or lifecycle ownership.
- [ ] Verify `SemanticFeedback` is descriptive/producer-reported and does not prove execution, success, or authorization.
- [ ] Verify `AudioCaptureSourceStatus` is in scope for the core package; if it is adapter/demo-specific, move it to the appropriate boundary before release.
- [ ] Verify nullability annotations and argument exception types/parameter names are intentional and tested.
- [ ] Verify public XML documentation is complete and accurate; no warning suppression hides missing contract documentation.
- [ ] Verify no accidental public API, internal implementation detail, or sample-only type is included.
- [ ] Produce and review a public API diff against the currently published `0.1.0-alpha.1` baseline.
- [ ] Decide whether the candidate is a prerelease or stable release and whether its version communicates compatibility honestly.

## 3. Architecture and dependency boundaries

- [ ] Build the dependency/ownership graph from the actual compiled assembly, not only from project-file declarations or the NuGet nuspec.
- [ ] Confirm no runtime dependency on ProtocolAi, GrammarAi, GUI, FSM_COS, Foundry, WPF, Blazor, Unity, browser APIs, audio SDKs, or device SDKs unless explicitly approved as part of the release scope.
- [ ] Confirm the compiled assembly references only expected framework/runtime assemblies.
- [ ] Confirm optional protocol identity remains a carrier value and does not require ProtocolAi at runtime.
- [ ] Confirm no transport, event bus, queue, scheduler, workflow lifecycle, or command execution behavior has entered the core accidentally.
- [ ] Confirm adapters own native/platform types and translate them at the boundary.
- [ ] Confirm consumer integrations are tested in consumer/host repositories rather than introducing upward dependencies into UserIO.
- [ ] Confirm no secrets, credentials, tokens, machine paths, or environment-specific configuration are committed.

## 4. Tests and behavioral evidence

- [ ] Run the complete test suite from a clean checkout at the exact candidate commit.
- [ ] Verify every public invariant has direct tests, including invalid inputs and meaningful edge cases.
- [ ] Verify equality/hash consistency and immutability where promised.
- [ ] Verify negative paths: malformed input is rejected; well-formed but unauthorized intent can still be denied by the host.
- [ ] Verify tests do not rely on timing, external services, a physical device, microphone permission, or a developer workstation unless explicitly isolated as manual integration tests.
- [ ] Verify test names and assertions describe contract behavior rather than implementation details.
- [ ] Review line/branch coverage for meaningful gaps; do not add tests solely to inflate a percentage.
- [ ] Preserve TRX and Cobertura outputs and inspect failures/warnings rather than relying only on a green badge.
- [ ] Verify the published-package host sample still demonstrates acceptance and denial using the actual published Alpha 1 package, and label it accurately as an Alpha 1 sample.
- [ ] Add/verify a separate development-source consumer test for the expanded API so the two API surfaces are not conflated.

## 5. Audio and platform-adapter scope (if included)

- [ ] Decide explicitly whether audio capture is part of this package's release scope or only a demonstration of an external adapter.
- [ ] Keep browser capture APIs, permission prompts, audio streams, recording, playback, and platform lifecycle outside the platform-neutral core.
- [ ] Verify the Audio Capture Lab states and transitions against the source contract, including permission denied, cancellation, unavailable source, missing tracks, source loss, retry, and repeated sessions.
- [ ] Review Windows adapter callback/stop/disposal behavior, including unexpected `RecordingStopped`, concurrent `Start`/`Stop`/`Dispose`, event-handler exceptions, and ownership/mutability of shared buffer payloads; add deterministic adapter tests where feasible.
- [ ] Verify privacy behavior: no capture without an explicit user action/permission; no hidden persistence or transmission; clear recording/stop state.
- [ ] Verify supported browser/OS combinations and document known limitations.
- [ ] Run the manual browser/device matrix on real supported environments; CI cannot prove real microphone/system-audio behavior.
- [ ] Confirm samples and demos are clearly marked as samples/prototypes and do not imply the core package provides platform capture.
- [ ] Verify the Windows loopback sample's target framework, dependencies, permissions, and manual verification instructions independently from the core package.

## 6. Documentation, examples, and Workshop standards

- [ ] Reconcile README, API contracts, architecture, theory, lifecycle, roadmap, domain completion criteria, Alpha readiness docs, and implementation roadmap with the exact source being released.
- [ ] Clearly distinguish the published Alpha 1 API from the expanded `development` API until a new package is authorized and published.
- [ ] Ensure all examples compile against the API version they claim to demonstrate.
- [ ] Label pseudocode, conceptual diagrams, manual tests, and verified executable examples distinctly.
- [ ] Audit all local Markdown links and packaged documentation paths.
- [ ] Audit external links, badge targets/branch names, image URLs, SVG rendering, alt text, and README navigation.
- [ ] Verify Workshop branding/moniker/footer requirements on all relevant Markdown documents.
- [ ] Verify the canonical README has the expected five-section structure and current badges.
- [ ] Verify the package README, license, API documentation, diagrams, and any intentionally distributed demos are present at their intended package paths.
- [ ] Ensure no stale version numbers, dates, CI run IDs, branch names, or claims of current verification remain.
- [ ] Ensure documentation makes no unsupported claims about security, authentication, trust, ordering, serialization, compatibility, or platform support.

## 7. Build, CI, and supply-chain verification

- [ ] Verify the documented SDK/target framework is intentional and matches the package's supported consumer matrix.
- [x] Restore and build the solution in Release configuration from a clean checkout; exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823) passed.
- [x] Run tests with coverage and verify the coverage report is actually generated; exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823) passed.
- [x] Verify Codecov upload behavior and required secret handling; the exact-head CI upload succeeded in run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823). Secret-value exposure still requires log review.
- [x] Verify the branding/documentation validation script passes; exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823) passed.
- [x] Verify the published-package host sample builds and its deterministic self-test produces exactly the expected accept/deny results; exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823) passed.
- [ ] Pack the exact candidate and inspect the resulting `.nupkg`, `.nuspec`, README, license, XML docs, assembly, dependency metadata, and all required documentation/demo entries.
- [ ] Install the exact package artifact into a fresh consumer project with no project reference to the source tree, then compile and run a minimal use case.
- [ ] Verify CI action references are pinned to full commit SHAs and permissions follow least privilege.
- [x] Verify the core workflow triggers only on the intended `master` and `development` branches.
- [x] Verify the Windows adapter workflow also triggers when its consumed core contracts (`AudioCaptureSourceStatus.cs`, `SemanticInteraction.cs`) or `global.json` change; [Windows adapter run #44206484790](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206484790) passed after the path-filter correction.
- [x] Verify the publish job remains impossible to run while the hard `&& false` gate is present; `publish_nuget` was skipped in exact-head CI run [#44206746823](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746823).
- [ ] Record the exact release-candidate commit SHA, workflow run URL, package filename, package version, and SHA-256.
- [x] Inspect a current CI package artifact (not a release candidate): [run #44206906945](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206906945) produced `TheSingularityWorkshop.FSM_UserIO.0.1.0-ci.473.nupkg` for commit `91a7231bdd1b906bf3fcb2703d4ae750aa986605`; SHA-256 `541d2846873b84b105d03b049a205b24a91a0fa4878e5edd3dec05c39b5a58c8`. The nuspec targets `net10.0` with an empty runtime dependency group; the archive contains the DLL/XML docs, README, license, 51 documentation entries, and all required package paths. Independent CLR assembly-reference enumeration remains open.
- [ ] Re-run the complete verification after the final source/documentation change; evidence from an earlier SHA is not evidence for the candidate head.

## 8. Repository and release-branch hygiene

- [x] Confirm the only long-lived branches are exactly `master` and `development`; [Sith Policy run #44206746847](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206746847) reports COMPLIANT with `branch_count: 2`.
- [ ] Confirm all intended work is committed to `development` and its working tree/candidate state is understood.
- [ ] Review the full diff from the last released state; explain every file addition, modification, and deletion.
- [ ] Confirm `master` remains the last released/NuGet-aligned state until the owner approves the release flow.
- [ ] Prepare the release candidate so that, after approval, `master` can be aligned with the verified `development` state using the Workshop release procedure.
- [ ] Reconcile the known branch divergence before release alignment: the branches share common ancestor `9752b3e4375bdab52fedf639b2257f0917e8ebce`, and 39 commits exist on `master` but not `development`. Those master-only commits include Alpha 1 publication history and later publication-gate/Sith changes. The branches are diverged, so fast-forward is not possible. Review and preserve that history; do not force-reset either branch.
- [ ] Re-run build/test/package verification against the exact release-aligned state as required by the Workshop release process.
- [ ] Confirm Sith policy reports exactly two branches and the live badge/evidence is current.
- [ ] Do not delete branches, merge, tag, or alter release refs without the owner's explicit authorization.

## 9. Final owner-approval packet

Before asking for approval, provide Trent with:

- [ ] Candidate version and whether it is prerelease/stable.
- [ ] Exact source commit SHA and branch.
- [ ] Concise summary of public API additions/changes and compatibility implications.
- [ ] Test/build/coverage results with exact workflow run links.
- [ ] Package inspection results, clean-consumer result, and package SHA-256.
- [ ] Dependency/assembly-reference audit results.
- [ ] Documentation/branding/link audit results.
- [ ] Audio/platform manual-test results or an explicit statement that those features are excluded/deferred.
- [ ] Known limitations, unresolved decisions, and accepted risks.
- [ ] Confirmation that the publish job is still hard-disabled.
- [ ] Explicit statement: **No merge, tag, release, or NuGet publication has occurred.**

## 10. Release-ready decision gate

The repository may be presented as **READY FOR TRENT'S FINAL APPROVAL** only when:

1. Every release-critical item above is complete or explicitly accepted/deferred by Trent.
2. The public API, tests, docs, package metadata, and package contents describe the same release.
3. The exact candidate commit passes all required CI and clean-consumer checks.
4. Dependency boundaries and platform separation are independently verified.
5. The candidate diff and release impact are understood.
6. The owner-approval packet is complete.
7. Publication remains hard-disabled.

After this gate, stop and request Trent's explicit decision. Approval to call the repository ready is **not** approval to merge, tag, release, or publish. NuGet publication requires a separate explicit authorization for the specific package version.

## 11. Focused audit findings — 2026-11-19

The following findings were confirmed by direct source and test inspection on `development` at branch head `61372d852665913a623f071e4a98fdfd4353c74e`. They are release gates, not assumptions about behavior.

### Core semantic contracts

- [x] `SemanticInteraction` requires non-null intent/context, keeps optional observation/expression/payload distinct, and has record value semantics; direct composition, null-guard, and equality tests exist.
- [x] `InteractionCapability`, `SemanticObservation`, `SemanticExpression`, `InteractionContext`, and `SemanticFeedback` validate required values, defined enum values, optional labels, identifiers, and confidence range. Tests cover the principal valid/invalid paths.
- [x] `SemanticPayload` defensively copies constructor bytes and returns a copy-backed `ReadOnlyMemory<byte>`; equality compares labels and bytes. Tests verify input/output mutation isolation and byte-level equality.
- [ ] Finish public API review against the published Alpha 1 package/API surface; source and current tests alone do not establish compatibility.
- [ ] Add explicit tests for all promised hash/equality edge cases where the public contract relies on them, including payload hash consistency and distinctions across each record's fields.

### Windows WASAPI loopback adapter

Direct inspection of `src/FSM_UserIO.WindowsAudio/WindowsSystemAudioLoopbackCapture.cs` found:

- [ ] **Unexpected stop state can become stale.** `OnRecordingStopped` signals the stop task, but only transitions status when `StoppedEventArgs.Exception` is non-null. A normal stop initiated outside this instance can leave `Status.State == Capturing` even though capture has ended.
- [ ] **Lifecycle operations are not synchronized.** `Start`, `Stop`, and `Dispose` mutate `_capture`, `_selectedDevice`, `_status`, and `_disposed` without a concurrency protocol. Concurrent calls can race resource release and callback delivery. Add deterministic concurrency tests and define whether calls are serialized or rejected.
- [ ] **User event handlers execute inline.** `AudioDataAvailable` and `StatusChanged` subscribers are invoked directly on adapter/native callback paths. A subscriber exception can escape the callback and interfere with NAudio's callback thread or a lifecycle operation. Define exception isolation/dispatch semantics and test throwing subscribers.
- [x] **Audio buffer ownership is explicit.** The adapter copies the NAudio buffer before raising `AudioDataAvailable`; the consumer owns the event payload. The payload is a mutable `byte[]`, so the contract must continue to state that each event consumer may mutate its own copy and must not expect a read-only view.
- [ ] Add a dedicated Windows adapter test project with an injectable capture/device seam so lifecycle and callback behavior can be tested without physical audio hardware.
- [ ] Perform real-device Windows verification for endpoint enumeration, selected/default endpoint capture, endpoint removal, and stop/disposal behavior. CI compilation is not device evidence.

### Repository and exact-head controls

- [x] Live branch inventory contains exactly `development` and `master`.
- [ ] Both branches currently report `protected: false` and disabled required status checks. Decide whether repository rulesets/branch protection should enforce the release workflow; do not treat a two-branch count as equivalent to protected release controls.
- [x] Pin the Sith Policy workflow's checkout action to the same full v7 commit SHA used by the build workflows.
- [x] Make Sith policy JSON stable across no-op evaluations: persist the branch inventory and policy result, while keeping per-run timestamps and URLs in the workflow summary. This prevents a bot-only commit on every evaluation when the branch policy has not changed.
- [x] Core CI run [#44207156682](https://github.com/TrentBest/FSM_UserIO/actions/runs/44207156682) passed all build/test/coverage/Codecov/sample/package/clean-consumer steps on `c2bdf671bb87c03c3cb00de7b82a7d23f2236374`; `publish_nuget` was skipped. Follow-up run [#44655093475](https://github.com/TrentBest/FSM_UserIO/actions/runs/44655093475) also passed all build/test/coverage/Codecov/sample/package/clean-consumer steps on `1776be5a79b04e8e5d1a66d874ca55ceadef32dd`.
- [ ] Review and resolve the confirmed adapter lifecycle findings before including Windows loopback in a release. Alternatively, explicitly defer the adapter from the release scope and keep its status separate from core package readiness.
- [ ] Exact-head CI is queued/running for the current source after the Sith workflow stabilization; verify the final branch SHA and all jobs before marking current-head validation complete.

## Progress log

| Date | Candidate SHA | Work / evidence | Status |
|---|---|---|---|
| 2026-11-20 | `1776be5a79b04e8e5d1a66d874ca55ceadef32dd` | Completed direct semantic-contract and Windows adapter source/test review; recorded three concrete adapter lifecycle risks (stale state after unexpected normal stop, unsynchronized lifecycle calls, inline subscriber exceptions) and explicit hardware-test requirements. Pinned the Sith workflow checkout action and stabilized its JSON output to avoid no-op bot commits. Core CI [#44655093475](https://github.com/TrentBest/FSM_UserIO/actions/runs/44655093475) passed all steps, including clean-consumer package build; a later exact-head run is required after the final Sith evidence/workflow commits. Branch inventory remains exactly `master` + `development`; both refs are currently unprotected. No release, merge, tag, or NuGet publication occurred. | NOT RELEASE-READY |
| 2026-11-15 | TBD | Initial comprehensive release gate created on `development`; current exact-head evidence still to be collected. | NOT YET VERIFIED |
| 2026-11-17 | `3bcb523201f429192fca63d3a59b5b38e8f5b41e` | Source audit confirmed that the expanded semantic contracts are grouped in the explicitly compiled root-level `SemanticInteraction.cs`; the project compile list is intentional, not a missing-`src/` defect. The audit found missing direct `SemanticIntent` tests for null/blank names, exact whitespace preservation, protocol-ID boundaries, and equality distinctions; those tests were added. Exact-head CI and the remaining API/docs/package audits are still pending. | NOT YET VERIFIED |
| 2026-11-17 | `91a7231bdd1b906bf3fcb2703d4ae750aa986605` | Completed the first source/API and framework-guidance audit. Added direct `SemanticIntent` edge tests and synchronized UML traceability. Updated active .NET guidance to .NET 10; historical Alpha 2 .NET 8 artifact references are explicitly labeled historical. Exact-head [Core CI run #44206906945](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206906945) passed Release build, tests/coverage, Codecov upload, host sample, package-path verification, and clean-consumer build; `publish_nuget` was skipped. The CI package artifact is `0.1.0-ci.473`, SHA-256 `541d2846873b84b105d03b049a205b24a91a0fa4878e5edd3dec05c39b5a58c8`; its nuspec targets `net10.0` and declares no runtime package dependencies. [Sith Policy run #44206906979](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206906979) confirms exactly two branches. [Windows adapter CI run #44206484790](https://github.com/TrentBest/FSM_UserIO/actions/runs/44206484790) passed after its path filters were expanded to include core contracts and `global.json`. Current master/development history remains diverged; 39 master-only commits include the Alpha 1 release history and must be preserved during reconciliation. Full public API/compatibility review, independent CLR assembly-reference enumeration, Windows hardware verification, branch reconciliation, and owner decisions remain open. | NOT RELEASE-READY |

---

**Governing principle:** define the smallest semantic boundary that serves the largest computational design space; release only what the implementation, tests, package, and documentation can prove.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>

