<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# FSM_UserIO — Domain Definition and Implementation Roadmap

> **Status:** active audit. This document separates the domain already established by source and documentation from proposed contracts that still require evidence and design decisions.

## Release readiness

- **Current readiness: not yet re-scored.** The historical 77% figure and older CI references are not a current release assessment. The latest completed verification before this audit was [run #190](https://github.com/TrentBest/FSM_UserIO/actions/runs/40534441661) on commit `d357a174825e1eaf658c55ae34fd616f07ef00d6`; later ownership-documentation commits were verified by [run #40541607758](https://github.com/TrentBest/FSM_UserIO/actions/runs/40541607758) on `177a0ca83aa1542f52cfd1343a7af00c3a2ac4df`. The expanded edge-case test commit `3f2351895b8d3cd0a06098747f3b3d51b379f003` is awaiting CI at the time of this edit. The live branch list was checked on 2026-10-27 and contains only `master` and `development`; no topic branches remain. A prior local Markdown-link audit found no unresolved relative links, but external URL reachability, badge rendering, and the complete Workshop documentation-standard pass remain unverified. The full compiled reference graph still needs independent review, and browser-level audio behavior still needs manual verification. Do not treat this roadmap as a percentage-based release approval until the remaining evidence is re-evaluated against the current candidate.
- **100% means release-ready**, not published.
- Publication remains a separate action requiring explicit authorization. The workflow requires manual dispatch on `development` with `publish: true`; do not enable that input until the owner authorizes a release.

## Security and supply-chain audit

- [x] Pin all seven workflow action uses to full commit SHAs with upstream version comments; the final previously mutable `actions/upload-artifact@v7` reference was pinned in commit `4cfff96db4fadb2d0fbc7cddafcd7adbdb0b1fc7`. Exact-commit CI verification passed in [run #143](https://github.com/TrentBest/FSM_UserIO/actions/runs/39321870262).
- [x] Reconcile branch inventory with Sith policy: the live GitHub branch list on 2026-10-27 contains only `master` and `development`. Branch protection/ruleset enforcement is a separate governance question and has not been established by this branch inventory.
- [ ] Inspect the compiled assembly's referenced assemblies independently of the NuGet `.nuspec`; the manifest declares no runtime package dependencies, but that alone does not prove the binary has no unexpected references.
- [x] Verify the fully action-pinned workflow passes on the exact commit; [run #143](https://github.com/TrentBest/FSM_UserIO/actions/runs/39321870262) passed for commit `4cfff96db4fadb2d0fbc7cddafcd7adbdb0b1fc7`.
- [x] Keep default workflow permissions read-only at workflow scope and grant `id-token: write` only to the opt-in NuGet publish job.
- [x] Confirm the publish job is gated on manual dispatch, explicit `publish: true`, `development` ref, and successful completion of the verification job. The gate is not publication authorization.

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
- [x] Verify the packed package in a clean consumer project (run #139, exact commit above).
- [x] Audit relative Markdown links in all 12 Markdown files; no unresolved local paths were found on the current development tree. External URL reachability, badge rendering, branch references, and version claims still require separate verification.
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
- [ ] Independently review the project-file, package metadata, and complete compiled assembly reference list to confirm they match the documented boundary.
- [x] Add a repeatable test that inspects the compiled core assembly's references and rejects known upstream-product and platform-specific assembly prefixes; CI run #183 passed. This is a guardrail, not a complete inventory of every reference.
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
- [x] Document that `ProtocolId = 0` is accepted and is not reserved by the current contract; tests also cover `0` and `ulong.MaxValue`.
- [ ] Document that equal numeric IDs from different protocol namespaces may be ambiguous unless the application supplies scope elsewhere.
- [x] Confirm record value equality over `Name` and `ProtocolId` is documented and tested, including equal-value hash-code consistency. Serialization behavior is not promised by the current contract and remains out of scope until a consumer requires it.
- [x] Expand edge-case tests for protocol-ID boundaries, explicit null identity, additional Unicode whitespace, case-sensitive names, and hash-code consistency. CI evidence for the expanded test set is pending.

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
- [x] Inspect the exact packed package's README, license, intended documentation files, assembly presence, XML docs, metadata, target framework, and NuGet dependency metadata; artifact identity and SHA-256 are recorded above for run #139. The independent compiled assembly reference graph remains a separate open check.
- [ ] Independently inspect the packed package's dependency metadata and assembly references to verify no platform-specific assembly or unintended runtime dependency is included.
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
| 2026-10-19 | Repaired the malformed package-verification workflow and required the README, license, and API contract file at their exact package paths. The clean-consumer build and all configured checks passed on the exact commit; publish job skipped. | [CI run #133](https://github.com/TrentBest/FSM_UserIO/actions/runs/39320150039) · [Verified commit](https://github.com/TrentBest/FSM_UserIO/commit/ec44a5c517d3e5412541f402880729cf1bd3330f) | 70% |

> **Governing principle:** Define the smallest semantic boundary that serves the largest computational design space. Expand the API only when a real consumer and a test can prove the need.

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em>
</p>
