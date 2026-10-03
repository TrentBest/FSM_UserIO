# Architecture

## Ownership

| Boundary | Responsibility |
|---|---|
| Physical adapter | Observe a device or external signal |
| FSM_UserIO | Carry semantic interaction |
| ProtocolAi | Give application-owned symbols deterministic identity |
| GrammarAi | Describe structure between protocol identities |
| GUI | Present and mediate human interaction with datum |
| Domain | Own datum and domain meaning |
| Host | Apply policy and execute |
| FSM_COS | Compose runtime capabilities |

## Dependency direction

~~~text
physical/software adapters
          |
          v
     FSM_UserIO
          |
     +----+----+
     |         |
     v         v
 ProtocolAi   GUI
     |         |
     +----+----+
          |
          v
        Host
          |
          v
      execution
~~~

This is a semantic relationship, not a requirement that every package directly reference every other package. In particular, FSM_UserIO does not require GUI to function.

## Current implementation

Alpha 1 intentionally contains a very small contract:

~~~csharp
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
~~~

The optional numeric identity allows an application to associate an intent with a deterministic protocol symbol without making ProtocolAi a runtime dependency of this package.

That is deliberate: the UserIO boundary should remain useful even when an application does not use ProtocolAi.

## What is outside the package

The following belong in adapters, bridges, or hosts:

- physical keyboard and mouse APIs;
- controller SDKs;
- touch and pointer event types;
- speech-recognition provider APIs;
- hand/gaze tracking SDKs;
- WPF and Blazor event types;
- Unity input types;
- user authorization;
- application datum;
- command execution;
- rendering.

## Future boundaries

The package may eventually need contracts for observations, interaction targets, input/output capability negotiation, user/session context, semantic interaction payloads, protocol registration, and adapter conformance.

Those are intentionally future work until their ownership can be demonstrated with tests and real consumers.
