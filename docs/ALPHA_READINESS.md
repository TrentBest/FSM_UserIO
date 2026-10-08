# Alpha Release Readiness

## Purpose

FSM_UserIO Alpha 1 establishes a new Workshop boundary. The release is successful only if a reader can understand the boundary without importing assumptions from platform-specific host, WPF, Blazor, game engines, or the human keyboard-and-mouse model.

The package must communicate a broader vision:

> **Semantic interaction is bounded by the capabilities of the participating form and by computation—not by a favored device, GUI framework, or game engine.**

## Alpha 1 contract

The implementation contains one semantic artifact:

```csharp
SemanticIntent(string Name, ulong? ProtocolId = null)
```

That is enough for Alpha 1 because the purpose of this release is to establish the boundary, not to freeze the entire future interaction model.

## Documentation gate

Before release, confirm:

- README explains the computation-first vision.
- Theory explains actor/form, capability, observation/expression, intent, authority, and execution.
- Architecture identifies ownership and dependency direction.
- Roadmap distinguishes established facts from future questions.
- Development guidance prevents platform leakage.
- Package metadata describes semantic interaction rather than human/device input.
- Documentation explicitly states that platform-specific host, WPF, and Blazor are possible adapters, not the definition of UserIO.

## Code gate

Confirm:

- public behavior has meaningful tests;
- XML documentation is generated without warnings;
- nullable analysis is clean;
- warnings are treated as errors;
- the package has no runtime dependencies;
- no platform-specific assembly is referenced;
- package contents contain the README, license, and documentation.

## Coverage gate

Coverage should be high because the Alpha 1 implementation is intentionally small.

Do not add meaningless tests solely to increase a percentage. Every test should protect a semantic or behavioral invariant.

For the current contract, tests should cover:

- application-owned name preservation;
- optional ProtocolAi identity;
- rejection of blank names;
- value semantics.

## CI gate

The release candidate should pass:

1. restore;
2. build;
3. test with coverage;
4. coverage artifact generation;
5. pack;
6. package artifact upload.

NuGet publication remains disabled until an explicit release decision.

## Publication gate

The repository follows the Workshop-wide hard gate:

```yaml
if: ${{ ... && false }}
```

The release process temporarily changes only the final `false` to `true` when publication is explicitly authorized, then restores the hard gate immediately after verification.

See the Workshop NuGet Publication Runbook for the full procedure.

## What Alpha 1 must not imply

Alpha 1 must not imply that:

- UserIO is a human-only abstraction;
- input means keyboard/controller input;
- output means a GUI;
- platform-specific host defines the interaction model;
- WPF or Blazor define the interaction model;
- capability equals authority;
- intent equals execution;
- ProtocolAi is required;
- a universal capability ontology already exists.

## Exit condition

Alpha 1 is ready when the documentation, implementation, tests, package metadata, and CI all tell the same story.

The story is intentionally bigger than the current API:

```text
many forms
    |
many capabilities
    |
many manifestations
    |
semantic interaction
    |
application policy
    |
computation
```

The API can grow later.

The boundary must be right first.
