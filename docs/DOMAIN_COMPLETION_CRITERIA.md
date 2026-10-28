<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# Domain Completion Criteria

This document defines what it means to finish FSM_UserIO as a package. “Complete” means the semantic boundary is coherent, useful, tested, and deliberately bounded—not that every possible interaction concept has become a public type.

## Completion target

**FSM_UserIO is complete when a consumer can carry an application-owned semantic intent across a source-independent boundary, understand exactly what the value guarantees, and retain full ownership of interpretation, policy, and effects.**

The published Alpha 1 package implements the original core value; current `development` now implements the foundational interaction contracts described in the API reference. The original value is:

```csharp
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
```

The remaining work is now contract verification, consumer examples, traceability, and release readiness. Further extensions still require explicit ownership and invariant review; UserIO does not absorb platform adapters, transport, trust enforcement, authorization, or execution.

## What must be true to call the package complete

### 1. Domain and boundary are closed

- [ ] State the domain in one sentence and use it consistently across README, Theory, Architecture, API contracts, and package metadata.
- [ ] Define the semantic lifecycle and distinguish capability, observation, expression/proposal, intent, authority, and execution.
- [ ] For every neighboring concern, identify its owner and explain why it is not a UserIO core responsibility.
- [ ] Label every domain area as implemented, documentation-only, deferred pending evidence, or explicitly out of scope.
- [ ] Ensure the domain map and all diagrams visually distinguish shipped contracts from conceptual flows.

### 2. The shipped contract is exact

- [ ] Public API reference matches the compiled assembly and package XML documentation.
- [ ] Constructor behavior, null/blank handling, name preservation, optional protocol identity, and value equality are tested and documented.
- [ ] Tests cover the actual public guarantees, including negative cases; docs do not claim tests that do not exist.
- [ ] Package consumers can use the contract without a dependency on UI frameworks, devices, ProtocolAi, GrammarAi, FSM_COS, or host implementations.
- [ ] API compatibility and package versioning rules are documented before any breaking contract is introduced.

### 3. A real consumer path proves usefulness

- [ ] The minimal sample compiles against the published package version, not a private implementation detail.
- [ ] At least one host-side example demonstrates that an intent can be accepted or rejected without UserIO authorizing or executing it.
- [ ] A concrete source-to-semantic mapping is clearly labeled as adapter/application code, not falsely presented as a UserIO API.
- [ ] Before declaring cross-modal generality proven, demonstrate that distinct source types can map to the same semantic intent without source-specific types leaking into core.
- [ ] If multiple independent consumers exist, record their shared requirements and test the smallest intersection rather than generalizing from one example.

### 4. Ownership and interoperability are unambiguous

- [ ] Protocol identity remains a value association; vocabulary registration, symbol resolution, and deterministic identity policy belong to ProtocolAi or the application protocol owner.
- [ ] Grammar and structural composition remain GrammarAi concerns.
- [ ] Presentation remains with GUI/surfaces; native event and device details remain in adapters.
- [ ] Host/application owns domain meaning, validation, authorization, and execution.
- [ ] Transport, persistence, retries, ordering, replay, deduplication, cancellation, trust, and delivery guarantees have explicit owners; UserIO does not imply guarantees it cannot enforce.
- [ ] Any future bridge or adapter package is justified by a concrete shared contract and has its own conformance tests.

### 5. Release evidence is complete

- [ ] CI passes on the exact proposed PR head.
- [ ] Tests and coverage reporting complete successfully.
- [ ] NuGet package creation succeeds; inspect the produced package for the expected assembly, XML docs, target framework, metadata, and absence of unintended files.
- [ ] README sample, package version, repository metadata, license, and release workflow agree with the published artifact.
- [ ] Publication remains a separate, explicitly authorized action; a successful pack/upload job is not a NuGet publication.
- [ ] Draft review confirms no accidental runtime/API changes if the change is intended to be documentation-only.

## What is not required for completion

The following are not automatic blockers and must not be implemented merely to make a domain inventory look exhaustive:

- universal actor, user, animal, robot, or AI-agent class hierarchies;
- universal capability descriptors or capability negotiation;
- additional source observation/expression/proposal contracts beyond the value carriers already implemented in `development`, unless a demonstrated shared consumer contract justifies the extension;
- additional payload/session/feedback value types beyond the existing carriers, or stronger session-lifecycle, result, or error guarantees, without a stable owner and testable lifecycle;
- device-specific APIs, GUI widgets, native event types, transport clients, or runtime composition;
- authentication, authorization, execution, or delivery guarantees that belong to hosts, adapters, or transport layers.

These may become legitimate future extensions. They are not implicit promises of the current package.

## Evidence gates for deferred concepts

A deferred concept can be promoted to a proposed API only when all five are recorded:

1. **Consumer:** at least one concrete use case, preferably two independent consumers for a shared abstraction.
2. **Meaning:** a precise definition that does not collapse adjacent concepts.
3. **Owner:** an explicit reason the contract belongs in UserIO rather than an adapter, bridge, host, protocol, or domain package.
4. **Invariant:** observable behavior that can be tested without a physical device or production host.
5. **Evolution:** compatibility, optionality, lifecycle, and failure semantics are understood well enough to support a public contract.

If any gate is missing, retain the concept in the domain inventory as an open question.

## Current assessment

The foundational source-neutral contracts are implemented on `development`, and the API reference records their invariants and non-ownership boundaries. The remaining path to completion is to verify the exact branch head, reconcile all docs and diagrams, run CI on the final source, inspect the packed artifact, and prove the contracts in a compiled host-side consumer sample.

The domain is not “unfinished” merely because deferred concepts have no types. It is unfinished only where the boundary, its evidence, or its release gates remain unresolved.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
