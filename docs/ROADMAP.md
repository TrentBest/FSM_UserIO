<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# UserIO Domain Completion Roadmap

This document is the domain inventory and evidence ledger for FSM_UserIO. It separates the semantic domain the Workshop intends to support from the contracts actually implemented in Alpha 1. It is deliberately not a promise to put every concept in the core package.

For the package-level definition of done, evidence gates, and what is *not* required for completion, see [Domain Completion Criteria](DOMAIN_COMPLETION_CRITERIA.md).

## Domain statement

**FSM_UserIO is the platform-neutral semantic interaction boundary between a participating form's contextual capabilities and application-owned meaning.**

A form may be biological, mechanical, virtual, software-defined, remote, distributed, or AI-mediated. A device, sensor, UI event, protocol, or transport is a manifestation or carrier—not the definition of the domain.

The governing separation is:

`capability ≠ observation ≠ expression/proposal ≠ intent ≠ authority ≠ execution`

The `development` source implements the foundational source-neutral contracts listed below. The published Alpha 1 package remains `SemanticIntent`-only until a separately authorized release.

## Domain map

| Domain area | Meaning / ownership | Current source status | Evidence or next decision |
|---|---|---|---|
| Semantic intent | Application-owned semantic name, optionally associated with a numeric identity | **Implemented** | `SemanticIntent(string Name, ulong? ProtocolId = null)`; blank names rejected; value semantics covered by tests |
| Protocol identity association | Optional numeric identity that can be resolved by an application protocol | **Implemented as a value slot only** | UserIO does not allocate IDs, define a vocabulary, or depend on ProtocolAi |
| Participating form / actor | Entity or computational representation that originates or receives interaction | **Descriptive only; no universal actor type** | Keep domain-specific actor models in their owning domain |
| Contextual capability | What a form can sense, express, or perform in a particular context | **Implemented as `InteractionCapability` + `InteractionMode`** | Descriptive only; no permission or enforcement |
| Source observation | Source-neutral account of something observed, sensed, or received | **Implemented as `SemanticObservation`** | Name, producer-supplied timestamp, optional source label, optional confidence in `[0,1]`; no ordering or trust guarantee |
| Expression / proposal | A form expresses or proposes something before interpretation | **Implemented as `SemanticExpression`** | Kept distinct from `SemanticIntent`; no automatic interpretation or authorization |
| Interaction payload | Opaque bytes with application-defined content type and schema labels | **Implemented as `SemanticPayload`** | Defensive copies and byte-wise value equality; serialization and schema validation remain caller-owned |
| Interaction context / session | Correlation across interactions with optional participant/session labels | **Implemented as `InteractionContext`** | Carries IDs only; does not create or manage session lifecycle |
| Feedback / semantic output | Non-authoritative status or explanatory feedback | **Implemented as `SemanticFeedback` + `FeedbackKind`** | Feedback is producer-reported and not independently verified by UserIO |
| Capability discovery / negotiation | Discovering what a peer or form can do and agreeing on compatible interaction | **Not implemented** | Likely belongs in a bridge or protocol unless a shared, source-neutral core contract is demonstrated |
| Policy / authorization / validation | Whether an interaction is allowed, valid, or appropriate | **Explicitly outside core** | Host/application owns the decision; capability or intent never grants permission |
| Execution and domain effects | Performing the requested action and changing application state | **Explicitly outside core** | Host/application owns execution, side effects, and domain meaning |
| Physical devices and native events | Keyboard, gaze tracker, microphone, controller, robot sensor, accessibility switch, and platform events | **Outside core** | Adapters translate manifestations; no device/framework dependency in UserIO |
| Transport and remote delivery | HTTP, message bus, sockets, RPC, network retries, and remote identity | **Outside core unless evidence changes the boundary** | Keep transport in adapters/hosts; define delivery semantics at the layer that owns them |
| Ordering, replay, deduplication, idempotency | How repeated, late, reordered, or replayed interactions are handled | **Unspecified** | Host/transport policy first; add a shared contract only if multiple consumers need the same guarantee |
| Trust, provenance, and integrity | Who produced an interaction and how its origin is trusted | **Unspecified; policy-sensitive** | Never treat a protocol ID or intent name as authentication or authorization |
| Errors and rejection | Constructor-invalid values versus host-reported rejection/failure | **Partially represented** | Constructors reject invalid values; `SemanticFeedback` carries reported rejection/failure, but UserIO does not define a universal error/result protocol |
| Adapter conformance | Observable rules adapters must satisfy to preserve semantic meaning | **Documentation-only design need** | Build a reusable conformance test suite only after observation/expression contracts stabilize |
| Accessibility and multimodal interaction | Multiple modalities that express the same semantic interaction | **Demonstration target, not core ontology** | Demonstrate equivalence at the semantic boundary without baking modality names into a universal enum |
| Runtime composition | Loading and composing runtime capabilities | **Outside UserIO** | FSM_COS owns runtime composition; UserIO must not depend upward on it |
| Semantic vocabulary and grammar | Deterministic symbol identity and structure between symbols | **Neighboring domains** | ProtocolAi owns protocol identity/vocabulary; GrammarAi owns structural composition; UserIO only carries the optional identity slot |

## Lifecycle and ownership

The intended conceptual lifecycle is:

1. A form has contextual capabilities, established by its environment and application.
2. A source or surface adapter observes an event or receives an expression/proposal.
3. The adapter translates the source-specific manifestation into a source-neutral representation when such a contract exists.
4. The semantic interaction boundary carries an intent, with optional source-neutral observation, expression, payload, correlation context, and feedback values.
5. The host interprets the intent and performs validation, authorization, and policy checks.
6. The host may reject, request clarification, provide feedback, or execute a domain action.
7. Any presentation, transport, persistence, or runtime composition is handled by its owning layer.

Native event acquisition, semantic interpretation, policy, authorization, execution, transport, and runtime composition remain owned by adapters and hosts. The core contracts provide source-neutral values without taking over those responsibilities.

## Dependency and non-ownership rules

- Core has no runtime package dependencies.
- UserIO does not depend on GUI, ProtocolAi, GrammarAi, FSM_COS, a game engine, or a platform framework.
- Source/surface adapters may depend on UserIO; the reverse dependency is not permitted.
- ProtocolAi owns deterministic vocabulary identity. `ProtocolId` is an optional association field, not an ID allocator or protocol implementation.
- GrammarAi owns grammar/structure, not execution.
- GUI owns human-facing presentation, not physical input ownership or semantic policy.
- Host/application owns datum, interpretation, validation, authorization, and execution.
- FSM_COS owns runtime composition, not the definition of semantic interaction.
- A capability is not authority; an intent is not a command execution.

## What remains before the domain is fully documented

### Documentation work

- [x] Reconcile README, Theory, Architecture, Development, Alpha Readiness, and this roadmap on the implemented-versus-planned distinction; the integration example and ownership matrix are explicitly identified as documentation/examples, not shipped APIs.
- [x] Add a short API contract reference for `SemanticIntent`: constructor behavior, blank-name rejection, name preservation, optional `ProtocolId`, equality/value semantics, and explicit non-guarantees.
- [x] Add a small integration walkthrough showing application-local adapter code producing an intent and a host rejecting or accepting it without UserIO performing execution: [Integration Walkthrough](INTEGRATION_WALKTHROUGH.md).
- [x] Add a consumer/producer matrix mapping domain concepts to owning Workshop repositories and their READMEs: [Consumer / Producer Ownership Matrix](OWNERSHIP_MATRIX.md).
- [x] Maintain a traceability table from each public contract to tests and package documentation. The published Alpha 1 package has one public contract; the expanded source contract set is listed in [API contracts](API_CONTRACTS.md).
- [x] Add an implementation-grounded domain map that marks implemented, conceptual/deferred, and neighboring ownership distinctly; keep it synchronized as the API changes.
- [x] Define package-level completion criteria so deferred concepts are not mistaken for required implementation: [Domain Completion Criteria](DOMAIN_COMPLETION_CRITERIA.md).

### Contract decisions that need evidence before implementation

- [x] Observation contract: `SemanticObservation` carries a source-neutral name, producer timestamp, optional source label, and bounded confidence; provenance trust and ordering remain external.
- [x] Expression/proposal contract: `SemanticExpression` preserves an expression before host interpretation; it is distinct from `SemanticObservation` and `SemanticIntent`.
- [x] Payload carrier: `SemanticPayload` transports opaque bytes with caller-defined content/schema labels; serialization and schema validation remain application-owned.
- [x] Correlation context: `InteractionContext` carries a non-empty correlation ID and optional participant/session IDs; session lifecycle remains host-owned.
- [x] Feedback carrier: `SemanticFeedback` reports correlated status without validating its truth or replacing application error/result protocols.
- [ ] Provenance/trust: what is identity metadata versus authentication or policy?
- [ ] Delivery semantics: which layer owns ordering, replay, retries, cancellation, and deduplication?
- [ ] Adapter conformance: what observable invariants can be tested without a physical device?
- [ ] Capability discovery: can an existing host/bridge own it without expanding the semantic core?
- [ ] Cross-consumer proof: which minimal scenario demonstrates the same semantic intent produced by a human-facing adapter, a software process, and an AI proposal?

The implemented contracts intentionally stop at descriptive values. Remaining open questions concern enforcement or lifecycle guarantees that belong to hosts, adapters, or transport unless a concrete shared requirement proves otherwise.

## Release and completion gates

- [ ] Source, tests, docs, package metadata, and README agree on the exact API.
- [ ] Build, tests, coverage, package creation, XML docs, and package-content inspection pass.
- [ ] README uses the Workshop 00–05 reader journey and links to focused domain documentation.
- [x] Domain map differentiates implemented contracts from conceptual and deferred areas.
- [ ] No runtime dependencies or platform-specific types have leaked into core.
- [ ] NuGet publication remains disabled unless explicitly authorized through the Workshop release process.

## Current conclusion

The current development source establishes a foundational set of source-neutral values, not a complete interaction protocol. The next step is exact-head verification, a compiled consumer example, and evidence that distinct sources can map to the same semantic meaning. Trusted provenance, delivery semantics, adapter conformance, negotiation, authorization, and execution remain deliberately outside the core.


---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://www.patreon.com/TheSingularityWorkshop">Support the Workshop</a>
  <br />
  <img src="assets/Avatar.jpg" alt="The Singularity Workshop avatar" width="96" />
</p>
