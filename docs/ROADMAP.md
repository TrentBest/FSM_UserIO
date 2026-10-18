# UserIO Domain Completion Roadmap

This document is the domain inventory and evidence ledger for FSM_UserIO. It separates the semantic domain the Workshop intends to support from the contracts actually implemented in Alpha 1. It is deliberately not a promise to put every concept in the core package.

For the package-level definition of done, evidence gates, and what is *not* required for completion, see [Domain Completion Criteria](DOMAIN_COMPLETION_CRITERIA.md).

## Domain statement

**FSM_UserIO is the platform-neutral semantic interaction boundary between a participating form's contextual capabilities and application-owned meaning.**

A form may be biological, mechanical, virtual, software-defined, remote, distributed, or AI-mediated. A device, sensor, UI event, protocol, or transport is a manifestation or carrier—not the definition of the domain.

The governing separation is:

`capability ≠ observation ≠ expression/proposal ≠ intent ≠ authority ≠ execution`

Only `SemanticIntent` is currently an implemented core domain contract. The remaining concepts below are domain areas and design questions unless marked implemented.

## Domain map

| Domain area | Meaning / ownership | Alpha 1 status | Evidence or next decision |
|---|---|---|---|
| Semantic intent | Application-owned semantic name, optionally associated with a deterministic protocol identity | **Implemented** | `SemanticIntent(string Name, ulong? ProtocolId = null)`; blank names rejected; value semantics covered by tests |
| Protocol identity association | Optional numeric identity that can be resolved by an application protocol | **Implemented as a value slot only** | UserIO does not allocate IDs, define a vocabulary, or depend on ProtocolAi |
| Participating form / actor | Entity or computational representation that originates or receives interaction | **Conceptual; not a core type** | Keep domain-specific actor models in their owning domain |
| Contextual capability | What a form can sense, express, or perform in a particular context | **Conceptual; no contract** | Decide whether a real consumer needs capability descriptors or whether hosts/adapters already own them |
| Source observation | Source-neutral account of something observed, sensed, or received | **Not implemented** | Define source, timestamp, value, confidence, provenance, ordering, and validity only when a consumer demonstrates the minimum needed fields |
| Expression / proposal | A form expresses or proposes something; it may be inferred, spoken, gestured, or emitted by software | **Not implemented** | Decide whether it differs semantically from observation and intent; do not collapse proposal into authorized command |
| Interaction payload | Structured data associated with an intent beyond its name and optional protocol ID | **Not implemented** | Identify schema ownership, serialization, size/version rules, and whether the payload belongs in UserIO or the host/domain |
| Interaction context / session | Correlation across a sequence of interactions or an active interaction | **Not implemented** | Determine who owns actor/session identifiers, lifecycle, cancellation, and expiration |
| Feedback / semantic output | Information returned to a form, including status, result, affordance, or rejection | **Not implemented** | Define only after a concrete producer and consumer establish what feedback means; presentation stays with the surface |
| Capability discovery / negotiation | Discovering what a peer or form can do and agreeing on compatible interaction | **Not implemented** | Likely belongs in a bridge or protocol unless a shared, source-neutral core contract is demonstrated |
| Policy / authorization / validation | Whether an interaction is allowed, valid, or appropriate | **Explicitly outside core** | Host/application owns the decision; capability or intent never grants permission |
| Execution and domain effects | Performing the requested action and changing application state | **Explicitly outside core** | Host/application owns execution, side effects, and domain meaning |
| Physical devices and native events | Keyboard, gaze tracker, microphone, controller, robot sensor, accessibility switch, and platform events | **Outside core** | Adapters translate manifestations; no device/framework dependency in UserIO |
| Transport and remote delivery | HTTP, message bus, sockets, RPC, network retries, and remote identity | **Outside core unless evidence changes the boundary** | Keep transport in adapters/hosts; define delivery semantics at the layer that owns them |
| Ordering, replay, deduplication, idempotency | How repeated, late, reordered, or replayed interactions are handled | **Unspecified** | Host/transport policy first; add a shared contract only if multiple consumers need the same guarantee |
| Trust, provenance, and integrity | Who produced an interaction and how its origin is trusted | **Unspecified; policy-sensitive** | Never treat a protocol ID or intent name as authentication or authorization |
| Errors and rejection | Invalid representation versus rejected application action | **Not implemented** | Separate contract-validation errors from host policy decisions if a shared result type becomes necessary |
| Adapter conformance | Observable rules adapters must satisfy to preserve semantic meaning | **Documentation-only design need** | Build a reusable conformance test suite only after observation/expression contracts stabilize |
| Accessibility and multimodal interaction | Multiple modalities that express the same semantic interaction | **Demonstration target, not core ontology** | Demonstrate equivalence at the semantic boundary without baking modality names into a universal enum |
| Runtime composition | Loading and composing runtime capabilities | **Outside UserIO** | FSM_COS owns runtime composition; UserIO must not depend upward on it |
| Semantic vocabulary and grammar | Deterministic symbol identity and structure between symbols | **Neighboring domains** | ProtocolAi owns protocol identity/vocabulary; GrammarAi owns structural composition; UserIO only carries the optional identity slot |

## Lifecycle and ownership

The intended conceptual lifecycle is:

1. A form has contextual capabilities, established by its environment and application.
2. A source or surface adapter observes an event or receives an expression/proposal.
3. The adapter translates the source-specific manifestation into a source-neutral representation when such a contract exists.
4. A semantic interaction is represented. In Alpha 1, the implemented representation is `SemanticIntent`.
5. The host interprets the intent and performs validation, authorization, and policy checks.
6. The host may reject, request clarification, provide feedback, or execute a domain action.
7. Any presentation, transport, persistence, or runtime composition is handled by its owning layer.

Steps 1–3 and 5–7 are architectural responsibilities, not APIs supplied by Alpha 1. The only currently implemented UserIO step is the semantic-intent value in step 4.

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
- [x] Maintain a traceability table from each public contract to tests and package documentation. Alpha 1 currently has one public contract; see [API contracts](API_CONTRACTS.md).
- [x] Add an implementation-grounded domain map that marks implemented, conceptual/deferred, and neighboring ownership distinctly; keep it synchronized as the API changes.
- [x] Define package-level completion criteria so deferred concepts are not mistaken for required implementation: [Domain Completion Criteria](DOMAIN_COMPLETION_CRITERIA.md).

### Contract decisions that need evidence before implementation

- [ ] Observation contract: is a common representation needed across more than one independent consumer?
- [ ] Expression/proposal contract: does it differ from observation and from `SemanticIntent` in a way consumers must preserve?
- [ ] Payload: is `Name + ProtocolId` insufficient for a real scenario, and who owns payload schemas?
- [ ] Context/session: which lifecycle/correlation facts are genuinely shared rather than host-specific?
- [ ] Feedback/result: is a shared semantic result needed, distinct from GUI presentation and application errors?
- [ ] Provenance/trust: what is identity metadata versus authentication or policy?
- [ ] Delivery semantics: which layer owns ordering, replay, retries, cancellation, and deduplication?
- [ ] Adapter conformance: what observable invariants can be tested without a physical device?
- [ ] Capability discovery: can an existing host/bridge own it without expanding the semantic core?
- [ ] Cross-consumer proof: which minimal scenario demonstrates the same semantic intent produced by a human-facing adapter, a software process, and an AI proposal?

These are questions, not approved API additions. A question should become a contract only when a real consumer, ownership boundary, compatibility need, and testable invariant are identified.

## Release and completion gates

- [ ] Source, tests, docs, package metadata, and README agree on the exact API.
- [ ] Build, tests, coverage, package creation, XML docs, and package-content inspection pass.
- [ ] README uses the Workshop 00–05 reader journey and links to focused domain documentation.
- [ ] Domain map differentiates implemented contracts from conceptual and deferred areas.
- [ ] No runtime dependencies or platform-specific types have leaked into core.
- [ ] NuGet publication remains disabled unless explicitly authorized through the Workshop release process.

## Current conclusion

Alpha 1 establishes a deliberately small semantic carrier, not a complete interaction protocol. The domain is broader than the current code. The next step is to document and prove the boundaries above—not to turn every row into a public type.
