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
- [x] Document platform independence explicitly, including platform boundaries such as WPF and Blazor.

## Alpha 1 — release-readiness work

- [ ] Complete documentation review against the actor/capability model.
- [ ] Maintain meaningful unit-test coverage for every implemented behavior.
- [ ] Verify package contents, XML documentation, README, license, and version metadata.
- [ ] Verify CI build, test, coverage, pack, and artifact generation.
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

A platform-specific demonstration may be useful, but it is only one demonstration. It must not become the definition of the package.

## Explicitly not planned in Core

- physical keyboard/mouse/controller APIs;
- WPF, Blazor, or other platform-specific GUI event types;
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
