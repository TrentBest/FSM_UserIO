# Development

## Build

The repository uses the `.slnx` solution format.

```bash
dotnet restore TheSingularityWorkshop.FSM_UserIO.slnx
dotnet build TheSingularityWorkshop.FSM_UserIO.slnx --configuration Release --no-restore
```

## Test

```bash
dotnet test TheSingularityWorkshop.FSM_UserIO.slnx --configuration Release --no-build
```

## Package

```bash
dotnet pack TheSingularityWorkshop.FSM_UserIO.csproj --configuration Release --no-build --output ./artifacts
```

## Coverage

Coverage is part of the normal CI verification path.

The objective is **meaningful coverage**, not a percentage manufactured by testing implementation trivia.

Every public behavior added to the package should have a test that demonstrates its intended contract.

When a branch exists because it protects a meaningful invariant, test that invariant directly.

## Design rule

Do not add a concrete device API merely because it is convenient.

Before adding a contract, answer:

1. What semantic meaning does it own?
2. Is that meaning about an actor/form, a capability, an observation, an expression, an intent, authority, or execution?
3. Which boundary is responsible for that meaning?
4. Is the behavior shared by multiple domains?
5. Could a platform adapter implement it without leaking platform types upward?
6. Can the contract be tested without a physical device?
7. Does the contract belong in the core, or is it actually a bridge/adapter/host concern?

If those answers are unclear, document the boundary before expanding the API.

## Platform independence test

When reviewing a proposed core API, mentally remove the first implementation platform.

If the type stops making sense without WPF, Blazor, Unity, a game engine, a keyboard, a mouse, or a human operator, it probably belongs below the UserIO boundary.

The inverse test also matters:

> If several unrelated forms and computational environments can use the semantic contract unchanged, that is evidence the concept belongs at the boundary.

## Capability discipline

Do not turn capability into a universal catalog.

A capability should be introduced only when the package has a demonstrated need to represent it.

Examples such as speech, grasping, gaze, biting, locomotion, network calls, or tool use are useful for testing the abstraction. They are not reasons to add one type per capability.

## Dependency discipline

FSM_UserIO should remain dependency-light.

In particular:

- ProtocolAi association must not require a ProtocolAi runtime dependency;
- GUI consumption must not require GUI as a runtime dependency;
- WPF must not enter the core;
- Blazor must not enter the core;
- Unity must not enter the core;
- FSM_COS must not become a dependency of UserIO merely because a host later composes it.

Adapters and hosts may reference the core.

The core should not reach upward into those hosts.

## Pull request expectations

A change is ready for review when:

- ownership is documented;
- the smallest useful API is implemented;
- XML documentation is complete;
- tests cover implemented behavior;
- build and test are clean;
- package contents are intentional;
- README and theory agree with the code;
- no platform-specific dependency has leaked into the core.

## Release discipline

NuGet publication is hard-gated.

The normal development state leaves the publication condition ending in `&& false`.

An authorized release temporarily enables publication, verifies the resulting package, and immediately restores the hard gate.

See the Workshop [NuGet Publication Runbook](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/docs/RELEASING.md).

## Architectural invariant

> **FSM_UserIO carries semantic interaction across actor/form capabilities; it does not own the actor, the device, the GUI, the datum, policy, or execution.**
