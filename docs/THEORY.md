# FSM_UserIO Theory

## The missing boundary

GUI is the human-facing bridge between datum and presentation. But GUI consumes interaction without owning the physical source of that interaction.

That missing ownership boundary is **FSM_UserIO**.

The same semantic action can arrive from keyboard, mouse, touch, controller, hand tracking, gaze, voice, accessibility technology, automation, another software process, or an AI system. Those sources are observations or proposals. They are not the application's meaning.

## Observation is not intent

A physical observation might be:

~~~text
keyboard / controller / touch
            |
            v
       physical event
~~~

The semantic meaning might instead be:

~~~text
physical event
      |
      v
   "select"
~~~

The first belongs to an input/output adapter. The second belongs to an application-owned semantic vocabulary. FSM_UserIO is the boundary between them.

## UserIO is not GUI

GUI answers what is presented, where it is presented, how it is arranged, how it is labeled, and how a user can inspect or modify datum.

FSM_UserIO answers a different question:

> **What semantic interaction has been proposed or observed?**

Therefore:

~~~text
Input source
     |
     v
FSM_UserIO
     |
     v
semantic interaction
     |
     +----> GUI presentation / interaction
     |
     +----> other application surfaces
     |
     v
host policy
     |
     v
execution
~~~

GUI and UserIO are siblings at the semantic boundary.

## User is not device

A user is not reducible to a device. The same user can interact through keyboard, voice, gaze, VR controller, accessibility technology, or another software surface.

Conversely, a device can produce observations without a human being the immediate source.

That is why this package does not define a universal Keyboard, Mouse, or Controller hierarchy.

## Intent is not authority

A semantic intent says what was proposed or observed. It does not say whether the action is permitted, whether the target exists, whether the user is authorized, whether the application should execute it, or whether execution is safe.

Those are host/application policy decisions.

~~~text
semantic intent
      |
      v
policy
      |
      v
authorization / validation
      |
      v
execution
~~~

This keeps the semantic boundary reusable without turning it into an execution framework.

## ProtocolAi connection

ProtocolAi can provide deterministic identity for application-owned semantic symbols.

~~~text
human / device / AI
        |
        v
semantic intent
        |
        v
ProtocolAi identity
        |
        v
host policy
        |
        v
execution
~~~

This is a boundary of **deprobabilization**, not a claim that an LLM becomes deterministic.

An application can move meaning such as select, focus, navigate, inspect, or a domain-specific action into an explicit application-owned identity space. The identity can then be shared by different producers and consumers without granting those producers execution authority.

FSM_UserIO should not become a second ProtocolAi. Its role is to carry semantic interaction across the user-facing boundary.

## GrammarAi connection

GrammarAi can describe how protocol identities are structured into larger commands or exchanges.

~~~text
ProtocolAi = WHAT
GrammarAi  = HOW
FSM_UserIO  = semantic user interaction boundary
GUI         = human-facing presentation
Host        = POLICY + EXECUTION
~~~

None of those layers should silently acquire the authority of another.

## The minimum-behavior principle

Different GUI domains will overlap. Suppose Blazor and WPF both need a concept such as Sparkles, while another platform does not.

Do not force Sparkles into every GUI implementation. Extract the shared semantic concept into the smallest bridge that actually needs it.

~~~text
shared semantic concept
          |
          v
       bridge
      /         Blazor    WPF
~~~

Likewise, if several input domains share a semantic interaction concept, that concept belongs at the smallest boundary that actually shares it.

This follows the same mathematical idea used for GUI.Core:

~~~text
Core ≈ intersection of shared semantic capabilities
Bridge ≈ shared subset between applicable domains
Platform ≈ Core + bridges + native extension
~~~

## Why the API starts small

A new package is a discovery, not a reason to invent a hundred types.

The first alpha establishes only the smallest useful semantic artifact: SemanticIntent.

It can carry application-defined semantic identity by name and an optional deterministic ProtocolAi identity.

It does not carry keyboard codes, mouse coordinates, WPF events, Blazor callbacks, Unity input objects, GUI widgets, domain datum, authorization, or execution commands.

Those remain outside this core.

## Architectural invariant

> **FSM_UserIO translates or carries user interaction semantics; it does not own the user, the device, the GUI, the datum, or execution.**

That is the boundary this repository exists to protect.
