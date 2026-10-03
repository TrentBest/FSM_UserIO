# TheSingularityWorkshop.FSM_UserIO

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/FSM_UserIO/build.yml?branch=main&style=flat-square&logo=github)](https://github.com/TrentBest/FSM_UserIO/actions)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/FSM_UserIO/main)](https://github.com/TrentBest/FSM_UserIO/commits/main)

**The Singularity Workshop UserIO boundary: semantic human interaction without owning the user, the device, the GUI, or execution.**

FSM_UserIO is the platform-neutral boundary between physical or software-originated observations and application-owned semantic interaction.

It exists because **GUI uses input, but GUI should not own input**.

```text
physical / software source
        |
        v
     FSM_UserIO
        |
   semantic intent
        |
        +---------> ProtocolAi identity
        |
        v
       GUI
        |
        v
 domain / application request
        |
        v
 host policy + execution
```

## Why this package exists

A keyboard key, mouse movement, controller button, hand gesture, gaze event, voice utterance, accessibility action, automation signal, or AI proposal is not itself the application's meaning.

FSM_UserIO provides the boundary at which an observation can become a **semantic interaction** without forcing that interaction to belong to a particular GUI technology or physical device.

That distinction lets the Workshop keep these concerns separate:

| Concern | Owner |
|---|---|
| Physical device observation | input/output adapter |
| User/session identity | user/application boundary |
| Semantic interaction | FSM_UserIO |
| Deterministic semantic identity | ProtocolAi |
| Structural command composition | GrammarAi |
| Human-facing presentation | GUI |
| Domain datum/state | application/domain |
| Policy and execution | host |
| Runtime composition | FSM_COS |

## The core idea

The package is intentionally **not** a keyboard abstraction, mouse library, GUI event system, or device framework.

It is the semantic middle.

```text
device / voice / gaze / automation / AI
                  |
                  v
        observation / proposal
                  |
                  v
             FSM_UserIO
                  |
                  v
          semantic interaction
                  |
          +-------+-------+
          |               |
          v               v
      ProtocolAi         GUI
       identity       presentation
          |               |
          +-------+-------+
                  |
                  v
          host policy / execution
```

The same semantic interaction vocabulary can therefore be approached by a human through a GUI or proposed by an AI through ProtocolAi, while neither mechanism receives authority merely by producing an intent.

## Architecture

FSM_UserIO sits beside GUI rather than underneath it:

```text
                 Domain / Application
                         ^
                         |
                 Host policy/execution
                         ^
                         |
       +-----------------+-----------------+
       |                                   |
     GUI                             FSM_UserIO
       |                                   |
       |                         +---------+---------+
       |                         |                   |
       v                         v                   v
  presentation             physical adapters      AI proposal
                              / devices          via ProtocolAi
```

The exact concrete contracts are deliberately being established incrementally. The first responsibility of this repository is to make the **ownership boundary** explicit and testable before freezing a large API.

## Theory

Start with [docs/THEORY.md](docs/THEORY.md).

The theory defines:

- observation versus semantic intent;
- the user/device/application ownership boundary;
- why GUI consumes interaction but does not own physical input;
- how ProtocolAi can provide deterministic identity for semantic interactions;
- how GrammarAi can describe structure without granting execution authority;
- why user interaction must remain policy-neutral;
- how shared interaction concepts can remain platform-neutral while physical adapters specialize below the boundary.

See also [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

## Design invariants

1. **FSM_UserIO does not own GUI.**
2. **FSM_UserIO does not own physical devices.**
3. **FSM_UserIO does not own application datum.**
4. **An interaction is not execution.**
5. **Protocol identity is not authority.**
6. **Grammar describes structure; it does not execute it.**
7. **Hosts retain policy and execution authority.**
8. **Concrete device APIs stay outside the semantic core.**
9. **Platform-specific GUI behavior stays outside the semantic core.**
10. **The smallest useful semantic contract is preferred over a universal input enum.**

## Relationship to ProtocolAi

ProtocolAi gives application-owned semantic symbols deterministic identity.

That creates an important possibility:

```text
human action
     |
     v
semantic intent
     |
     v
ProtocolAi identity
     |
     +---- GUI can present it
     |
     +---- AI can propose it
     |
     v
host policy
     |
     v
execution
```

This does **not** make an LLM deterministic. It moves application-owned meaning into an explicit address space that a host can validate before execution.

See [TheSingularityWorkshop.ProtocolAi](https://github.com/TrentBest/TheSingularityWorkshop.ProtocolAi).

## Relationship to GUI

GUI is the human-facing bridge between datum and non-datum presentation.

FSM_UserIO supplies the semantic interaction boundary that GUI can consume without becoming responsible for keyboard, mouse, touch, controller, gaze, voice, or other physical/software sources.

See [TheSingularityWorkshop.GUI](https://github.com/TrentBest/TheSingularityWorkshop.GUI).

## Status

This repository is a **new architectural boundary**, not a claim that the final interaction API has already been designed.

The initial work establishes theory, ownership, tests, documentation, packaging, and CI before committing the Workshop to a large concrete interaction vocabulary.

## NuGet publication

Publication is intentionally disabled by default.

The Workshop-wide release safety rule is that NuGet publication jobs end with an explicit `&& false` gate. Build, test, coverage, and package creation remain useful while publication stays off.

The release procedure is documented in the Workshop's [NuGet Publication Runbook](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/docs/RELEASING.md).

## License

MIT. See [LICENSE.txt](LICENSE.txt).

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em>
</p>
