<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# Consumer / Producer Ownership Matrix

This matrix names the current architectural owner of each concern and links to that repository's authoritative entry point. It is a responsibility map, not a claim that every pair of packages already has a runtime integration.

| Concern | Producer / consumer responsibility | Owner | UserIO relationship |
|---|---|---|---|
| Semantic intent value | Preserve an application-owned semantic name and optional numeric identity association | [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | Implemented as `SemanticIntent` in source; Alpha 1 package is older |
| Source-neutral interaction values | Describe capability, observation, expression, opaque payload, correlation context, and feedback | [FSM_UserIO](https://github.com/TrentBest/FSM_UserIO) | Implemented on `development`; see [API Contracts](API_CONTRACTS.md) |
| Deterministic symbol identity / vocabulary | Define and resolve application-owned symbols in a deterministic address space | [ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi) | Neighboring domain; UserIO's `ProtocolId` is only an optional value slot and does not resolve or allocate IDs |
| Grammar / structural relationships | Define how protocol identities can be connected into grammar structure | [GrammarAi](https://github.com/TrentBest/TheSingularityWorkshop.GrammarAi) | Neighboring domain; UserIO does not define grammar or execution |
| Semantic GUI tree and presentation manifestations | Describe GUI content neutrally and manifest it through appropriate presentation targets | [Workshop GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) | Potential consumer/producer of semantic interactions; no direct integration is asserted here |
| Runtime composition | Resolve and compose runtime capabilities into a host handoff | [FSM_COS](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS) | Composition boundary only; FSM_UserIO must not depend upward on FSM_COS |
| Hub workflow execution | Own workflow orchestration, execution lifecycle, and run state | Foundry / Hub host | Authoritative execution pipeline; UserIO supplies interaction primitives and must not duplicate workflow scheduling or own run state |
| Hub presentation | Present the Hub when a human-facing surface is selected | [Workshop GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI) or another selected surface | Optional manifestation; the Hub and UserIO must remain usable without GUI |
| Native device/framework events | Convert source-specific events into application-owned semantic representations | A concrete platform adapter or application | No shared adapter contract is implemented in Alpha 1 |
| Domain data and meaning | Interpret what an intent means in the application | Host / application domain | Outside UserIO |
| Validation, policy, authorization, and effects | Accept, reject, clarify, authorize, and execute a requested operation | Host / application | Outside UserIO; an intent carries no authority |
| Transport, delivery, replay, and retry | Move interactions between processes and define delivery guarantees | Transport / host adapter | Outside UserIO unless future consumers prove a shared source-neutral contract |

## Dependency direction

The intended direction is:

```text
native source / surface
        |
        v
application-owned adapter
        |
        v
   FSM_UserIO
        |
        v
host policy and domain
        |
        v
application effects

ProtocolAi: symbol identity
GrammarAi: structure between symbols
GUI: presentation / manifestations
FSM_COS: runtime composition
Foundry: authoritative Hub workflow execution and run state
```

The lines in this diagram describe responsibility and possible information flow, not mandatory direct project references. Foundry owns the Hub workflow lifecycle; UserIO does not duplicate it, and GUI is optional. In particular, FSM_UserIO currently has no runtime package dependencies.

## Evidence discipline

- A repository link establishes the location of an owning package, not that an integration adapter already exists.
- The integration walkthrough demonstrates the published Alpha 1 `SemanticIntent` path and identifies the expanded `development` contracts separately.
- The foundational observation, payload, context, and feedback value carriers exist on `development`; production adapter conformance, trusted provenance, session lifecycle, payload interpretation, delivery guarantees, and universal result semantics remain future work until concrete consumers establish their requirements.


---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://www.patreon.com/TheSingularityWorkshop">Support the Workshop</a>
  <br />
  <img src="assets/Avatar.jpg" alt="The Singularity Workshop avatar" width="96" />
</p>
