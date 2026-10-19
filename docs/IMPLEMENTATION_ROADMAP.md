# FSM_UserIO — Domain Definition and Implementation Roadmap

> **Status:** active audit. This document separates the domain already established by source and documentation from proposed contracts that still require evidence and design decisions.

## Release readiness

- **Current readiness: 65%.** The latest CI run passed build, tests, coverage upload, pack, documentation-path verification, clean-consumer installation, and artifact upload. The previously inspected alpha package contained the README, MIT license, seven Markdown guides, assembly, and XML documentation. `docs/API_CONTRACTS.md` was added after that artifact was produced, so the latest CI must verify the updated package includes it before package-content readiness can be checked off. Remaining work includes the full public-contract/dependency-boundary audit, documentation and version consistency review, and broader adapter/integration guidance.
- **100% means release-ready**, not published.
- Publication remains a separate action requiring explicit authorization. The workflow requires manual dispatch on `development` with `publish: true`; do not enable that input until the owner authorizes a release.

## 00 — Definition of done

- [x] Establish FSM_UserIO as a platform-neutral semantic interaction boundary.
- [x] Establish `SemanticIntent` as the current public contract.
- [x] Keep the core free of runtime package dependencies and platform-specific types.
- [x] Separate capability from authority, semantic intent from domain meaning, and validation from execution.
- [x] Document the intended relationship to ProtocolAi, GrammarAi, GUI, FSM_COS, and host/application policy.
- [x] Add `docs/API_CONTRACTS.md` describing the implemented `SemanticIntent` constructor, property, equality, and non-guarantees; keep it traceable to source and tests.
- [ ] Audit every public type/member against implementation, XML docs, tests, and package metadata.
- [ ] Mark every roadmap claim as implemented, missing, intentionally out of scope, or awaiting a decision.
- [ ] Run the complete release verification sequence and preserve evidence.
- [ ] Verify the packed package in a clean consumer project.
- [ ] Verify the documentation links, badges, branch references, and version claims.
- [ ] Maintain only `master` and `development` as long-lived branches.

## 01 — Domain contract: what UserIO owns

The current domain is the **semantic interaction crossing point** between a source/surface and an application host. It carries application-owned intent without owning the source, GUI, datum, policy, authorization, or execution.

### Established contract

- [x] `SemanticIntent.Name` is required and cannot be null, empty, or whitespace.
- [x] `SemanticIntent.ProtocolId` is optional.
- [x] The intent is a value-based record.
- [x] The protocol identity is numeric and does not require a ProtocolAi assembly reference.
- [x] The package does not define device, GUI, rendering, network, or execution APIs.

### Domain vocabulary to keep consistent

- **Actor/form:** the participating human, animal, robot, virtual entity, software process, remote operator, or AI-mediated process. UserIO does not define a universal actor hierarchy.
- **Capability:** what a form can sense, express, or perform in a context. Capability is not permission.
- **Observation:** a source-derived report about a state or event.
- **Expression/proposal:** a communicated expression or proposed intent; it is not automatically trusted or executed.
- **Semantic intent:** an application-owned interaction identity represented currently by `SemanticIntent`.
- **Protocol identity:** an optional stable numeric association supplied by the application; ProtocolAi owns protocol vocabulary/resolution when used.
- **Grammar/structure:** rules for arranging symbols; GrammarAi may describe structure but does not authorize or execute.
- **Policy/authorization/validation:** host/application responsibility.
- **Datum and domain meaning:** application/domain responsibility.
- **Execution:** host/runtime responsibility.
- **Manifestation:** platform-specific presentation, sensors, devices, or transport, owned by adapters and host packages.

## 02 — Boundary and dependency rules

- [x] No runtime dependency from FSM_UserIO to ProtocolAi or GrammarAi.
- [x] No runtime dependency on GUI, WPF, Blazor, Unity, device SDKs, or FSM_COS.
- [x] Keep application-owned meaning outside the package.
- [x] Keep capability distinct from authorization and execution.
- [ ] Verify project-file, package, and compiled dependency graphs match the documented boundary.
- [ ] Add a repeatable dependency-boundary check to CI if it can be implemented without brittle heuristics.
- [ ] Add an adapter guidance example showing native events translated into semantic interaction without leaking platform types into the core.
- [ ] Confirm any future cross-package examples remain consumer/integration tests rather than upward dependencies.

## 03 — SemanticIntent contract and tests

- [x] Preserve the supplied name.
- [x] Reject blank names.
- [x] Support an omitted protocol identity.
- [x] Support an explicit protocol identity.
- [x] Verify value equality.
- [x] Test null-name behavior explicitly (including parameter name and exception contract).
- [x] Test preservation of meaningful leading/trailing whitespace; names are preserved exactly and are not silently normalized.
- [x] Test equality for same name/different protocol IDs and different names/same ID.
- [ ] Document whether `ProtocolId = 0` is valid; do not assume it is reserved without a domain decision.
- [ ] Document that equal numeric IDs from different protocol namespaces may be ambiguous unless the application supplies scope elsewhere.
- [ ] Confirm record equality and serialization behavior are the intended public contract.
- [ ] Ensure tests protect semantics rather than inflate coverage.

## 04 — Domain questions: decide before expanding the API

These are open decisions, not promises to add types.

- [ ] **Observation vs. expression:** Does the package need separate contracts, or is the current intent carrier enough until a real consumer proves otherwise?
- [ ] **Payload:** Is a name plus optional protocol identity sufficient? If payloads are needed, decide whether they belong in ProtocolAi, FSM_Serialization, or an application-specific envelope rather than duplicating ownership.
- [ ] **Source and actor identity:** Does UserIO need actor/source/session identity, or should adapters/hosts carry that context?
- [ ] **Capability description:** Is there a reusable shared capability contract demonstrated by multiple consumers? Do not create a universal enum or ontology merely to enumerate examples.
- [ ] **Result/response:** Does output need a semantic response contract, or is presentation correctly owned by GUI/host APIs?
- [ ] **Time and ordering:** If observations need timestamps, sequence numbers, correlation, or causality, define who owns those semantics before adding fields.
- [ ] **Trust and provenance:** If proposals need source attribution or confidence, define the trust model outside implicit booleans and do not treat provenance as authorization.
- [ ] **Versioning:** Decide how contract evolution will be versioned before promising serialized/wire compatibility.
- [ ] **Nullability and validation:** Define which invariants belong in constructors versus host-level validation.
- [ ] **Adapter conformance:** Determine whether more than one real adapter can share a small, testable conformance contract.

## 05 — ProtocolAi and GrammarAi integration

- [x] Document UserIO as the carrier of semantic intent and ProtocolAi as the owner of deterministic symbol identity.
- [x] Document GrammarAi as structural composition, not semantic authority.
- [x] Preserve package independence: optional numeric identity does not require a package reference.
- [ ] Add a small integration test in a consumer repository proving a host-owned protocol symbol can be associated with a `SemanticIntent`.
- [ ] Test that a missing or mismatched protocol identity is rejected or handled by host policy where appropriate; do not make UserIO resolve protocols itself.
- [ ] Test that grammar admission does not imply authorization.
- [ ] Keep the cross-repository usage map accurate about direct package references, carrier boundaries, composition examples, and candidate integrations.
- [ ] Link this roadmap from ProtocolAi's consumer guide and keep reciprocal links current.
- [ ] Do not add ProtocolAi/GrammarAi references to FSM_UserIO's runtime dependencies merely to make the relationship more discoverable.

## 06 — Consumer and adapter demonstrations

Prioritize real, small demonstrations. Mark each one implemented only when source and tests prove it.

- [x] AnyApp carries `SemanticIntent` through its host-owned semantic exchange and manifest surface (consumer evidence must remain linked from the ecosystem map).
- [x] WebPage has a composition-level test for ProtocolAi/GrammarAi through FSM_COS; this is not, by itself, proof of a direct package reference in the main application project.
- [ ] Human-facing adapter example: native input becomes a semantic intent; host policy independently decides what to do.
- [ ] Accessibility example: alternate input produces the same semantic contract without device assumptions in core.
- [ ] Software-process example: a service/event produces a semantic interaction without pretending to be a human.
- [ ] AI-proposal example: untrusted proposed intent is parsed/resolved, structurally checked if applicable, then authorized by the host.
- [ ] Simulated robot/agent example: capability is contextual and never implies permission.
- [ ] Negative-path example: a well-formed, grammar-admitted intent is still denied by host policy.
- [ ] Keep examples concise and executable; distinguish verified code from conceptual diagrams.

## 07 — Documentation and living examples

- [x] README introduces the boundary and links to theory, architecture, development, roadmap, and readiness documents.
- [x] Architecture explains ownership and dependency direction.
- [x] Theory explains the capability-first model and the separation from authority.
- [x] Roadmap records staged evolution and non-goals.
- [ ] Review all docs against actual public source and package contents.
- [ ] Ensure diagrams illustrate contracts that exist today and label proposed layers clearly.
- [ ] Add a minimal compile-tested example around the actual `SemanticIntent` API.
- [ ] Add a living integration example that uses real ProtocolAi/GrammarAi objects in a consumer project rather than simulating their behavior in a diagram.
- [ ] Avoid giant catalogues of hypothetical actors/capabilities becoming accidental normative ontology.
- [ ] Ensure the Alpha 1 / Alpha 2 readiness docs distinguish current source facts from future plans.
- [ ] Check all links, SVGs, alt text, badge branch names, and documentation navigation.

## 08 — CI, package, and release gate

- [x] Verify restore and Release build from a clean checkout (latest CI: [39276329767](https://github.com/TrentBest/FSM_UserIO/actions/runs/39276329767)).
- [x] Verify tests pass and coverage output is produced.
- [x] Verify Codecov integration and test/coverage artifact upload are functioning as configured.
- [x] Verify package creation succeeds and required documentation paths are present in the `.nupkg`.
- [x] Verify the packed package's README, license, documentation, assembly, XML docs, metadata, target framework, and dependencies (package inspection recorded in the readiness summary above).
- [x] Verify no platform-specific assembly or unintended runtime dependency is packed.
- [x] Install the packed package into a clean consumer project and compile it.
- [x] Verify the workflow triggers only on `master` and `development` for pushes and pull requests.
- [x] Verify the publication job requires manual workflow dispatch, `publish: true`, `development`, and successful build/test/package verification.
- [x] Keep publication untriggered until the user explicitly authorizes a release; the latest publish job was skipped as intended.
- [x] Record the exact verification run, commit, and remaining limitations; the full domain, dependency-boundary, docs/version consistency, and adapter/integration audits remain open.
- [ ] Set readiness to 100% only after all release-critical checks have evidence.

## 09 — Explicit non-goals for the core

- Physical keyboard, mouse, controller, sensor, actuator, or device APIs.
- WPF, Blazor, Unity, browser, or game-engine event types.
- GUI widgets, rendering, audio, or presentation.
- A universal actor hierarchy or capability ontology.
- Authentication, authorization, safety policy, or trust decisions.
- Domain data ownership, command execution, or application state transitions.
- Network transport, provider SDKs, model inference, prompt management, or secrets.
- A mandatory dependency on ProtocolAi, GrammarAi, FSM_COS, or GUI.
- New abstractions without a real consumer and a testable boundary.

## Recommended work order

1. Complete source/project/CI/package audit and run verification commands.
2. Align tests and XML docs with the existing `SemanticIntent` contract.
3. Verify dependency boundaries and package contents.
4. Add one real cross-package integration example and a policy-denial test in the consumer/host layer.
5. Resolve only the open domain questions demanded by real consumer evidence.
6. Polish docs and living examples, then reassess release readiness.

## Progress log

| Date | Change | Evidence | Readiness |
|---|---|---|---|
| 2026-10-19 | Expanded `SemanticIntent` tests to cover null-name exception contract, exact whitespace preservation, and equality differences across names and protocol IDs. | [SemanticIntent tests](../tests/FSM_UserIO.Tests/SemanticIntentTests.cs) · [Earlier verification](https://github.com/TrentBest/FSM_UserIO/actions/runs/39185428593) | 38% |
| 2026-10-19 | Corrected the README last-commit badge, replaced the stale disabled-publish instructions with the explicit manual opt-in gate, and aligned this roadmap and alpha-readiness guidance. Latest workflow passed restore, Release build, tests, coverage/Codecov, pack, documentation-path verification, clean-consumer build, and artifact uploads. Publish job was skipped as intended; no package was published. | [Latest CI run](https://github.com/TrentBest/FSM_UserIO/actions/runs/39276329767) · [Publish-gate CI](https://github.com/TrentBest/FSM_UserIO/actions/runs/39276234348) · [Workflow](../.github/workflows/build.yml) | 65% |

> **Governing principle:** Define the smallest semantic boundary that serves the largest computational design space. Expand the API only when a real consumer and a test can prove the need.
