<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# API Contract

## Status

This document describes the currently implemented public API. It is not a promise that the broader semantic-interaction domain has been implemented.

## Current public type

```csharp
namespace TheSingularityWorkshop.FSM_UserIO;

public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
```

The package currently exposes one domain type: `SemanticIntent`.

## Constructor and properties

| Member | Contract |
|---|---|
| `SemanticIntent(string Name, ulong? ProtocolId = null)` | Creates an application-owned semantic intent. |
| `Name` | Required, non-null, non-empty, and non-whitespace. A valid value is preserved exactly as supplied; it is not trimmed or normalized. |
| `ProtocolId` | Optional unsigned 64-bit numeric association. When omitted, the value is `null`. |

### Invalid names

A `null`, empty, or whitespace-only `Name` causes `ArgumentException` with parameter name `Name`. The contract does not silently repair invalid input.

### Identity and equality

`SemanticIntent` is a sealed record, so equality is value-based and includes the record's instance properties, `Name` and `ProtocolId`. Equal names with different protocol IDs are not equal; different names with the same protocol ID are not equal.

The core does not reserve `ProtocolId = 0`; it accepts any `ulong` value. Whether an ID has meaning, is unique, or is valid in a particular vocabulary is owned by the application/protocol that supplies it. Numeric IDs from different protocol namespaces may collide; this type does not carry namespace scope.

## Explicit non-guarantees

FSM_UserIO does not:

- allocate, resolve, or authenticate protocol identities;
- define a universal vocabulary or capability ontology;
- infer an intent from a device event;
- authorize, validate, or execute an intent;
- define actor, observation, expression, payload, session, transport, persistence, or feedback types;
- reference ProtocolAi, GrammarAi, GUI, FSM_COS, WPF, Blazor, Unity, or device SDK assemblies at runtime.

An optional `ProtocolId` is an association value, not proof of producer identity or permission.

## Verification

The implementation is in `SemanticIntent.cs`. The behavior is exercised by `tests/FSM_UserIO.Tests/SemanticIntentTests.cs`. CI is the verification authority for a particular commit; this document does not substitute for a passing run on a release candidate.

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em>
</p>
