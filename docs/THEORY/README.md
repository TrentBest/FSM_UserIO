<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# FSM_UserIO Theory — Learning Path

This is the progressive learning path for the semantic-interaction domain. It starts with the problem in ordinary language, builds the conceptual model, and then connects each concept to the contracts that actually exist in FSM_UserIO.

The existing [theory overview](../THEORY.md) remains the broad conceptual map. These chapters teach the ideas in sequence and link back to the implementation, tests, API contracts, and known limits.

## How to use the chapters

Each chapter follows the same pattern:

1. **Intuition** — the real-world problem before introducing terminology.
2. **Theory** — the concepts, relationships, assumptions, and counterexamples.
3. **Model** — a diagram or worked flow when it makes the relationship clearer.
4. **Implementation** — the exact development-source contracts that represent the concept.
5. **Evidence and limits** — named tests and explicit non-guarantees.
6. **Apply it** — a small scenario or review questions.

The chapter's implementation section describes the current `development` source unless it explicitly says otherwise. The published NuGet package `0.1.0-alpha.1` remains the earlier `SemanticIntent`-only baseline. Development-source contracts are not package-consumer APIs until a later version is separately reviewed, authorized, and published.

## Chapters

| Chapter | Subject | Status |
|---|---|---|
| [01 — What Is Semantic Interaction?](01-SEMANTIC-INTERACTION.md) | Why source events and application meaning are different things | **Available** |
| [02 — Actors, Forms, and Contextual Capabilities](02-ACTORS-FORMS-CAPABILITIES.md) | The interacting form, what it can do, and why devices are only manifestations | **Available** |
| 03 — Observation, Expression, Interpretation, and Intent | The stages between a source event and application-owned meaning | Planned |
| 04 — Capability Is Not Authority | Host policy, validation, authorization, and execution ownership | Planned |
| 05 — Protocol Identity, Grammar, and Application Meaning | How ProtocolAi and GrammarAi relate without taking ownership of interaction | Planned |
| 06 — Input and Output Are Not Necessarily Symmetric | Feedback, payloads, correlation, and the limits of reported outcomes | Planned |
| 07 — Platform Adapters and the Minimum-Intersection Principle | How native platforms meet a stable semantic boundary | Planned |
| 08 — Trace an Interaction from Source to Host Decision | A source-grounded end-to-end walkthrough with tests and explicit limits | Planned |

## Source of truth

- [API contracts](../API_CONTRACTS.md) — exact constructor, validation, equality, and non-guarantee behavior.
- [Interaction lifecycle](../INTERACTION_LIFECYCLE.md) — conceptual lifecycle and ownership boundaries.
- [Architecture](../ARCHITECTURE.md) — package responsibilities and dependency direction.
- [Requirement-to-test traceability](../UML/TEST_TRACEABILITY.md) — named test evidence for reviewed contracts.
- [Runnable Alpha 1 consumer sample](../../samples/SemanticIntentHostSample/README.md) — the API available from the published package today.

## Learning rule

Do not infer a public API from a conceptual diagram. A concept is implemented only when a corresponding source contract exists; a behavior is verified only to the extent that a named test or an explicitly described manual verification establishes it.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
