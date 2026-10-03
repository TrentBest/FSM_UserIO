# Architecture

## Boundary statement

**FSM_UserIO is the platform-neutral semantic interaction boundary between an actor/form's capabilities and application-owned meaning.**

It is not a device framework, GUI framework, rendering framework, authorization framework, or execution framework.

The core should remain usable by any computational environment capable of producing or consuming semantic interaction.

## Ownership

| Boundary | Responsibility |
|---|---|
| Actor / form | Possesses or is granted capabilities in a particular context |
| Capability layer | Describes what the form can sense, express, or perform |
| Source / surface adapter | Converts a physical or software manifestation into an application-facing observation/expression |
| FSM_UserIO | Carries semantic interaction |
| ProtocolAi | Gives application-owned symbols deterministic identity |
| GrammarAi | Describes structure between protocol identities |
| GUI | Presents and mediates human-facing interaction with datum |
| Domain | Owns datum and domain meaning |
| Host | Applies policy, authorization, validation, and execution |
| FSM_COS | Composes runtime capabilities |
| Platform / renderer | Provides native manifestation |

## The architectural flow

```text
                    ACTOR / FORM
                         |
                    capabilities
                         |
             +-----------+-----------+
             |           |           |
          sensing     expression   action
             |           |           |
             +-----------+-----------+
                         |
              source / surface adapter
                         |
                         v
                  FSM_UserIO
                         |
                 semantic interaction
                         |
          +--------------+--------------+
          |              |              |
          v              v              v
      ProtocolAi        GUI       other surfaces
          |              |
          +--------------+--------------+
                         |
                         v
                 host / application
                         |
                policy + execution
                         |
                         v
                    computation
                         |
                         v
                     FSM_COS
```

The arrows describe responsibility and information flow. They do not imply direct package references.

## Dependency direction

The semantic architecture can be represented as:

```text
platform / source adapters
          |
          v
     FSM_UserIO
          |
          +----> ProtocolAi association
          |
          +----> GUI consumption
          |
          +----> other application surfaces
          |
          v
       host policy
          |
          v
       execution
```

FSM_UserIO currently has **no runtime package dependencies**.

That is intentional.

ProtocolAi, GUI, and FSM_COS may consume or associate with UserIO concepts without UserIO depending upward on them.

## Actor/form

An actor/form is the participating entity or computational representation at the interaction boundary.

It may be:

- biological;
- mechanical;
- virtual;
- software-defined;
- remote;
- distributed;
- AI-mediated.

FSM_UserIO does not define a universal actor hierarchy.

The package is concerned with the interaction semantics that cross the boundary, not with defining every possible actor.

## Capability

A capability describes a possible sensing, expression, or action of a form in a given application context.

Capability is intentionally contextual.

A VR avatar might have a capability that a different experience does not grant.

A robot's available actuator is not automatically an application-authorized action.

An AI agent's available tool is not automatically an authorized execution path.

Therefore capability must remain distinct from policy.

## Observation and expression

A source adapter may translate a physical or software manifestation into a source-neutral representation.

Examples include:

```text
keyboard key       -> observation
gaze fixation      -> observation
voice recognition  -> expression/proposal
robot sensor       -> observation
AI model proposal  -> semantic proposal
software message   -> observation/proposal
```

These examples illustrate the boundary. They do not define Alpha 1 API types.

## Semantic interaction

FSM_UserIO is the semantic middle.

Its responsibility is to preserve application-owned interaction meaning without importing the implementation details of the source.

The first alpha artifact is:

```csharp
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
```

The optional numeric identity allows an application to associate the intent with a deterministic protocol symbol without requiring a ProtocolAi package reference.

## Platform adapters

WPF, Blazor, Unity, and other platforms are not UserIO implementations merely because they expose input events.

A platform adapter may eventually exist where a real consumer needs one:

```text
WPF / Blazor / Unity / other platform
                |
          native events
                |
                v
       platform adapter
                |
                v
           FSM_UserIO
```

The adapter owns platform types.

FSM_UserIO owns semantic interaction.

This keeps the core independent of game engines and desktop/web UI frameworks.

### Do we need WPF_IO or Blazor_IO?

Not automatically.

A package should be created when a concrete shared contract exists, not because a platform name can be appended to `IO`.

There are at least three legitimate outcomes:

1. a platform adapter is useful and deserves its own package;
2. several platforms share a capability, so a capability bridge is more appropriate;
3. no reusable package boundary exists yet, so the application keeps the adapter local.

The architecture deliberately leaves that choice open until evidence exists.

## Capability bridges

If multiple platforms or forms share a semantic capability, the shared behavior should live at the smallest useful bridge.

```text
              FSM_UserIO
                   |
          +--------+--------+
          |                 |
     shared capability   platform-specific
          |                 |
       bridge        native extension
       /   \
    WPF     Blazor
```

This prevents the core from accumulating unrelated platform assumptions.

## Authority boundary

The following are separate concepts:

```text
capability != observation != intent != authority != execution
```

A form may be capable of an action.

A source may report an observation.

FSM_UserIO may carry an intent.

The host may reject it.

Only the host/application execution boundary decides what actually happens.

## Explicit non-ownership

FSM_UserIO does not own:

- the actor/form;
- physical devices;
- device discovery;
- WPF APIs;
- Blazor APIs;
- Unity APIs;
- rendering;
- GUI widgets;
- application datum;
- authorization;
- policy;
- command execution;
- network transport;
- runtime composition.

If a future contract appears to require one of these, the ownership boundary must be revisited before the type is added.

## Alpha design discipline

Before adding a contract, demonstrate:

1. a real semantic boundary that needs it;
2. at least one consumer;
3. an ownership statement;
4. platform-neutral semantics;
5. tests that can run without the physical source;
6. a reason the concept belongs in the core rather than a bridge or host.

This is how the package can remain small while supporting a very large computational design space.
