# FSM_UserIO Theory

## The missing boundary

FSM_UserIO exists at a boundary that is easy to misidentify.

It is tempting to define "input/output" from the perspective of a human using a device:

```text
keyboard -> application
mouse    -> application
controller -> application
```

That is too narrow.

The more fundamental model is:

```text
actor / form
     |
     +-- capabilities
     |
     +-- sensing
     +-- expression
     +-- action
     |
     v
observation / expression
     |
     v
semantic interaction
     |
     v
application policy
     |
     v
execution
```

A human is one possible actor/form. A dog, robot, VR avatar, accessibility system, AI agent, remote operator, or software process can also participate.

The common boundary is therefore **capability and semantic interaction**, not human anatomy and not a particular device.

## Form is not device

An actor/form describes the participating entity or computational representation.

A device is only one possible mechanism through which a capability is manifested.

For example:

- a human can speak through a microphone;
- the same human can express a semantic action through gaze;
- a dog can bark, bite, walk, smell, or urinate;
- a robot can sense and manipulate;
- an AI agent can propose a semantic operation;
- a software process can emit a message or invoke an API.

FSM_UserIO does not need to know that a dog has teeth or that a human has thumbs. Those examples demonstrate why the abstraction must remain above the physical implementation.

## Capability is not a device list

A capability answers a question such as:

> **What can this form legitimately sense, express, or perform in this application context?**

This is broader than "what input devices are attached?"

Capability may be:

- biological;
- mechanical;
- virtual;
- software-defined;
- application-granted;
- mediated by accessibility technology;
- mediated by another system.

The package should not turn these possibilities into a universal ontology.

The purpose of the abstraction is to keep the semantic boundary independent of the particular form.

## Capability is not authority

This distinction is essential.

```text
CAPABILITY
    |
    | what can be sensed, expressed, or performed
    v
OBSERVATION / EXPRESSION
    |
    | what occurred or was proposed
    v
SEMANTIC INTERACTION
    |
    | what the application says it means
    v
POLICY
    |
    | what is permitted, valid, and appropriate
    v
EXECUTION
```

Capability does not imply permission.

A capability can exist without authorization.

An observation can exist without being accepted.

A semantic intent can exist without being executable.

Only the host/application boundary decides whether execution occurs.

## Observation is not intent

A physical or software observation is not automatically semantic meaning.

For example:

```text
button pressed
     |
     v
source observation
     |
     v
"select"
```

The first is source-specific.

The second is application meaning.

The same semantic interaction could be produced by:

- a keyboard;
- a hand gesture;
- gaze;
- voice;
- accessibility input;
- another program;
- an AI proposal.

FSM_UserIO protects the semantic middle from being defined by any one source.

## Expression is broader than input

The term "input" can imply a one-way model in which the computer receives information from a human.

That is not sufficient.

A form can express a semantic action through many modalities, and an application can also expose meaningful output or feedback.

For Alpha 1, the package does not yet freeze an observation/output object model. The theory establishes the direction without prematurely inventing contracts.

## User is not human

"User" in the package name should be read functionally, not anatomically.

The interacting party may be:

- a human;
- an animal;
- a robot;
- a virtual avatar;
- an AI agent;
- an accessibility system;
- a software process;
- a remote operator;
- another computational environment.

The package does not need a `HumanUser`, `DogUser`, `RobotUser`, or `AIUser` hierarchy.

Those would encode examples instead of defining the boundary.

## UserIO is not GUI

GUI is the human-facing bridge between datum and non-datum presentation.

FSM_UserIO answers a different question:

> **What semantic interaction has been observed, expressed, or proposed across an interaction boundary?**

Therefore GUI and UserIO are complementary:

```text
                 Domain datum
                      |
                      v
                    GUI
                      ^
                      |
               semantic interaction
                      ^
                      |
                 FSM_UserIO
                      ^
                      |
             source / surface adapter
                      ^
                      |
                 actor / form
```

GUI does not need to own physical input.

FSM_UserIO does not need to own presentation.

## ProtocolAi connection

ProtocolAi can give application-owned semantic symbols deterministic identity.

```text
source / actor
     |
     v
semantic interaction
     |
     v
ProtocolAi identity
     |
     v
host policy
     |
     v
execution
```

This is a form of **deprobabilization**: application-owned meaning can move from an open-ended representation into an explicit deterministic address space.

That does not make an LLM deterministic.

FSM_UserIO should not become a second ProtocolAi. It carries interaction semantics; ProtocolAi owns deterministic symbol identity.

## GrammarAi connection

GrammarAi describes how protocol identities may be structurally connected.

```text
ProtocolAi = WHAT
GrammarAi  = HOW
FSM_UserIO  = interaction boundary
GUI         = human-facing presentation
Host        = POLICY + EXECUTION
```

Grammar does not become execution merely because it describes executable-looking structure.

## Minimum-intersection principle

The Workshop repeatedly uses the idea that a shared core should contain the intersection of capabilities actually shared by its consumers.

```text
Core       ≈ intersection of shared semantic capabilities
Bridge     ≈ shared subset between applicable domains
Platform   ≈ Core + bridges + native extension
```

This means:

- do not put WPF events in FSM_UserIO;
- do not put Blazor callbacks in FSM_UserIO;
- do not put platform-specific host input objects in FSM_UserIO;
- do not put every imaginable animal capability in FSM_UserIO;
- do not create a bridge until multiple consumers demonstrate the shared boundary.

If WPF and Blazor eventually share a semantic capability, a bridge can represent that shared subset.

If platform-specific host and another environment share a capability, the same principle applies.

The platform is an adapter. The semantic boundary remains platform-neutral.

## Why Alpha 1 is intentionally tiny

A new repository is a discovery of a boundary, not permission to invent an ontology.

Alpha 1 contains only `SemanticIntent`.

That type establishes a useful fact:

```text
application-owned semantic identity
             |
             v
        SemanticIntent
             |
             +--> optional deterministic identity
             |
             +--> no execution authority
```

It deliberately does not define:

- keyboard codes;
- mouse coordinates;
- WPF events;
- Blazor callbacks;
- platform-specific host input objects;
- controller SDKs;
- device discovery;
- GUI widgets;
- application datum;
- authorization;
- execution.

Those remain outside the core until a real consumer demonstrates a need.

## Architectural invariant

> **FSM_UserIO carries semantic interaction across actor/form capabilities; it does not own the actor, the device, the GUI, the datum, policy, or execution.**

That is the boundary this repository exists to protect.

## The larger vision

The intended ceiling is not a particular engine.

The intended ceiling is the capability of the participating form and the computation available to the system.

```text
biological form
     |
mechanical form
     |
virtual form
     |
software form
     |
distributed form
     |
     v
semantic interaction
     |
     v
computation
```

The package is therefore deliberately agnostic about whether the eventual consumer is a desktop application, web application, game engine, immersive environment, robot, accessibility system, AI agent, server, or distributed computation.

The implementation remains small because the vision is broad enough that premature specialization would be an architectural mistake.
