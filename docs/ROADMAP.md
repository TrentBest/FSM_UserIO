# Roadmap

## Alpha 1 — boundary established

- [x] Create a dedicated UserIO repository.
- [x] Define UserIO as the semantic interaction boundary.
- [x] Replace the human/device-centric mental model with actor/form + capability.
- [x] Distinguish capability, observation/expression, semantic interaction, authority, and execution.
- [x] Keep physical input/output outside GUI.Core and FSM_UserIO.
- [x] Keep GUI, datum, policy, authorization, rendering, and execution outside UserIO ownership.
- [x] Establish an optional numeric identity slot for ProtocolAi association.
- [x] Keep FSM_UserIO free of runtime package dependencies.
- [x] Add build, test, coverage, package, and publication-safety infrastructure.
- [x] Document platform independence explicitly, including Unity, WPF, and Blazor boundaries.
- [x] Document the end-to-end interaction lifecycle and explicitly distinguish conceptual stages from implemented API types in [Interaction Lifecycle](INTERACTION_LIFECYCLE.md).

## Alpha 1 — release-readiness work

- [ ] Complete the full documentation review against the actor/capability model; cross-document alignment, image/footer standard, and link/content review remain in progress.
- [x] Maintain meaningful unit-test coverage for the implemented `SemanticIntent` behavior.
- [x] Inspect the Alpha 1 package contents, XML documentation, README, license, version metadata, and declared NuGet dependencies; artifact identity and SHA-256 are recorded in the [implementation audit](IMPLEMENTATION_ROADMAP.md) from [CI run #139](https://github.com/TrentBest/FSM_UserIO/actions/runs/39320861310).
- [x] Verify CI build, test, coverage, pack, clean-consumer build, and artifact generation; [run #143](https://github.com/TrentBest/FSM_UserIO/actions/runs/39321870262) passed on the exact action-pinning commit.
- [ ] Independently inspect the compiled assembly's reference graph and verify repository merge-protection settings.
- [ ] Verify NuGet Trusted Publishing configuration before an authorized release.
- [ ] Publish only through the Workshop hard-gated release procedure after explicit authorization.

## Next architectural questions

These are questions, not commitments to APIs:

- [ ] What is the smallest source-agnostic observation contract?
- [ ] Does an expression/proposal need a distinct representation from an observation?
- [ ] What is the smallest semantic interaction payload beyond `SemanticIntent`?
- [ ] Does capability negotiation belong in UserIO or in an adjacent bridge?
- [ ] When is actor/session context necessary, and who owns it?
- [ ] What adapter conformance contract can be proven across multiple consumers?
- [ ] Can a GUI consumer and ProtocolAi producer share the same semantic interaction without coupling the packages?
- [ ] What does a useful non-human example look like without turning the core into an organism ontology?

## Demonstrations to seek

The architecture should eventually be demonstrated by multiple forms rather than by one favored platform:

- [ ] Human → platform adapter → UserIO.
- [ ] Accessibility surface → UserIO.
- [ ] Software process → UserIO.
- [ ] AI proposal → ProtocolAi → UserIO.
- [ ] Robot or simulated agent → UserIO.
- [ ] GUI consumes the same semantic interaction vocabulary.
- [ ] A host applies policy and can reject an otherwise valid interaction.

A Unity demonstration may be useful, but it is only one demonstration. It must not become the definition of the package.

## Explicitly not planned in Core

- physical keyboard/mouse/controller APIs;
- WPF, Blazor, Unity, or other GUI event types;
- device SDK dependencies;
- a universal human-input enum;
- an organism capability ontology;
- authorization policy;
- application datum;
- command execution;
- rendering;
- network transport;
- runtime composition.

## Governing principle

> **Define the smallest semantic boundary that can serve the largest computational design space.**

Do not optimize the core for the first platform that happens to consume it.
