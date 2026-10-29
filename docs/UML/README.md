<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# UML and Source-Documentation Inventory

This is the initial source-backed inventory for FSM_UserIO, aligned with the Workshop's [Documentation Standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/DOCUMENTATION_STANDARD.md) and its [UML view specifications](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/tree/master/docs/UML/Standard).

**Evidence status:** source-checked inventory; no repository-wide Roslyn UML generator or stale-diagram gate is claimed here. The entries below are a coverage plan and scope declaration, not evidence that diagrams have already been generated.

## Scope

- Repository: `TrentBest/FSM_UserIO`
- Branch/revision checked: `development` at `8f8dfaa1b18f2b6ffaca0e684bd8678e7c5ec038`
- Scope: tracked C# declarations, projects, tests, samples, public package boundary, and the current documented integration surfaces.
- Evidence methods: source/file inventory and existing tests/docs; no reflection scan or runtime trace was performed for this inventory.
- Core source declarations currently visible: `SemanticIntent`, `SemanticInteraction`, `AudioCaptureSourceStatus`, `WindowsSystemAudioLoopbackCapture`, and `WindowsAudioOutputEndpoint`. Tests and samples are consumers/evidence, not automatically public package contracts.

## Required-view inventory

| View family | Status | Scope and reason |
|---|---|---|
| Class/type pages | **Required — gap** | Create a source-linked page for every in-scope declared type, including nested and generic declarations; current docs explain concepts/contracts but do not yet provide complete per-type companions. |
| Target-centered class neighborhoods | **Required — gap** | Consumers above, focal type centered, dependencies below, package/project references to the right; no complete neighborhood set is currently claimed. |
| Sequence diagrams and complexity | **Required — manual/source-derived, gap** | Prioritize intent validation/host decision and Windows loopback capture/disposal. Record source traceability and complexity only where meaningful; no numeric runtime bounds without measurement. |
| Activity/control flow | **Required where nontrivial — gap** | Cover validation, capture start/stop/disposal, and failure paths when source complexity warrants a diagram. |
| State/lifecycle | **Required where stateful — gap** | Windows capture lifecycle and source status transitions are candidates; mark stateless contracts not applicable individually. |
| Use cases | **Required — gap** | Add one goal-oriented file per use case with actor, trigger, success, failure alternatives, and evidence. |
| Architecture/components | **Required — partial** | Existing [Architecture](../ARCHITECTURE.md) and [Ownership Matrix](../OWNERSHIP_MATRIX.md) provide narrative; add a bounded component view and verify direct/transitive dependency claims. |
| Deployment/runtime topology | **Not applicable to core library; conditional for Windows integration** | Core semantic contracts have no deployment topology. Windows loopback docs should describe the actual OS/runtime boundary without implying a hosted service or network topology. |
| Data models/flow/lineage | **Required — partial** | Existing [API contracts](../API_CONTRACTS.md), [Interaction lifecycle](../INTERACTION_LIFECYCLE.md), and [Audio capture contract](../AUDIO_CAPTURE_CONTRACT.md) describe important boundaries; add source-linked data-flow views and ownership/mutability details. |
| MicroBundles/manifests | **Not applicable** | FSM_UserIO does not currently define or compose MicroBundles or manifests. Reassess if this changes. |
| Requirement/test traceability | **Required — partial** | Tests exist for semantic intent, semantic interaction, and audio source status; map each public contract/invariant to test names and evidence. |
| Decision/constraint records | **Required — partial** | [Theory](../THEORY.md), [Architecture](../ARCHITECTURE.md), and [Domain completion criteria](../DOMAIN_COMPLETION_CRITERIA.md) describe constraints, but a linked invariant/decision index is still needed. |

## Evidence and completion rules

- Label each artifact **Source-derived**, **Reflection-derived**, **Runtime-captured**, **Human-authored**, or **Conceptual**; mixed artifacts identify which statements use which evidence.
- Record target revision, analysis scope, tool/version if generated, unresolved relationships, and regeneration/check instructions.
- Do not treat source structure as a runtime trace, reflection as a complete AST, a static SVG as interactive, or a green CI run as proof of documentation coverage.
- Complexity claims must identify input-size variables, dominant operations, allocations/copies, and defensible average/worst-case bounds. Use **unknown / requires measurement** when evidence is insufficient.
- A future documentation gate must separately report declaration, relationship, use-case, and freshness coverage. This initial inventory is not that gate.

## Next coverage order

1. Complete per-type pages for the public semantic contracts and audio contracts.
2. Add source-linked class neighborhoods and key interaction/capture sequences.
3. Add use-case files and requirement-to-test traceability.
4. Validate local links and branding, then add deterministic coverage/freshness checks only when their actual scope can be verified.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
