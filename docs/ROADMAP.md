# Roadmap

## Established

- [x] Create a dedicated UserIO repository.
- [x] Define UserIO as the semantic boundary between observations and application interaction.
- [x] Keep physical input/output outside GUI.Core.
- [x] Keep GUI, datum, and execution outside UserIO ownership.
- [x] Establish an optional numeric identity slot for ProtocolAi integration.
- [x] Add build, test, package, and publication safety infrastructure.

## Next

- [ ] Define a source-agnostic observation contract.
- [ ] Define semantic interaction payloads without freezing a universal command vocabulary.
- [ ] Establish user/session context only where it has clear ownership.
- [ ] Add adapter conformance tests.
- [ ] Demonstrate a GUI adapter consuming UserIO semantics.
- [ ] Demonstrate a ProtocolAi adapter producing UserIO semantics.
- [ ] Explore GrammarAi composition without moving execution policy into UserIO.

## Explicitly not planned in Core

- physical keyboard/mouse/controller APIs;
- WPF, Blazor, Unity, or other GUI event types;
- device SDK dependencies;
- authorization policy;
- application datum;
- command execution;
- rendering.
