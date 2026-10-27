<!-- The Singularity Workshop moniker -->
<p align="center">
  <a href="https://github.com/TrentBest">
    <img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" />
  </a>
</p>

# API Contracts

This page documents the public contracts compiled from the current `development` source. The published NuGet package `0.1.0-alpha.1` remains the earlier `SemanticIntent`-only baseline; the new contracts below are not available to package consumers until a future release is explicitly authorized and published.

## Contract inventory

| Contract | Responsibility | Key invariant |
|---|---|---|
| `SemanticIntent` | Application-owned semantic meaning | Required non-whitespace name; optional `ulong` association |
| `InteractionCapability` / `InteractionMode` | Describe what a form can observe, express, or do | A capability is descriptive, not permission |
| `SemanticObservation` | Record a source-neutral observation | Timestamp is source-supplied; optional confidence is finite and within `[0, 1]` |
| `SemanticExpression` | Carry an expression or proposal before interpretation | Not itself an intent, authorization, or command |
| `InteractionContext` | Carry correlation and optional participant/session labels | Non-empty correlation ID; optional labels remain application-owned |
| `SemanticFeedback` / `FeedbackKind` | Carry non-authoritative feedback for an interaction | Non-empty correlation ID and message; kind does not prove execution |

All these types are immutable sealed records with value equality. Constructor validation rejects invalid values instead of silently normalizing them.

## SemanticIntent

```csharp
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
```

- `Name` is required and non-whitespace; a valid name is preserved exactly, including case and surrounding characters.
- `ProtocolId` is optional. Zero and `ulong.MaxValue` are valid values.
- Equality includes both `Name` and `ProtocolId`.
- The ID is an association only. UserIO does not allocate, resolve, namespace, authenticate, or authorize it.

## Capability and interaction mode

```csharp
public sealed record InteractionCapability(string Name, InteractionMode Mode);
public enum InteractionMode { Observe = 0, Express = 1, Act = 2 }
```

The name is application-defined and preserved exactly when valid. Undefined enum values are rejected. `Act` means the form describes itself as capable of an action; it does not grant permission or cause the action to execute. The enum describes the interaction direction, not a universal list of capabilities.

## Observation and expression

```csharp
public sealed record SemanticObservation(
    string Name,
    DateTimeOffset ObservedAt,
    string? Source = null,
    double? Confidence = null);

public sealed record SemanticExpression(
    string Name,
    DateTimeOffset ExpressedAt,
    string? Source = null);
```

Names are required and preserved. Timestamps are supplied by the producer and are not rewritten or validated against a global clock. An optional source label must be non-whitespace when supplied; it is metadata, not authenticated provenance. Observation confidence is optional and must be finite, between zero and one inclusive. An expression is deliberately not automatically converted into an intent: interpretation belongs to the host or adapter.

These contracts do not impose ordering, deduplication, replay protection, clock synchronization, source trust, or delivery guarantees.

## Interaction context

```csharp
public sealed record InteractionContext(
    Guid CorrelationId,
    string? ParticipantId = null,
    Guid? SessionId = null);
```

`CorrelationId` must not be `Guid.Empty`. Participant and session identifiers are optional application-owned labels; a session ID does not create, store, expire, or manage a session. The context does not prove who produced an interaction.

## Semantic feedback

```csharp
public sealed record SemanticFeedback(
    Guid CorrelationId,
    FeedbackKind Kind,
    string Message);

public enum FeedbackKind
{
    Informational = 0,
    Accepted = 1,
    Rejected = 2,
    ClarificationRequired = 3,
    Completed = 4,
    Failed = 5
}
```

The correlation ID must not be empty, the kind must be a defined enum value, and the message must be non-whitespace. Feedback is a statement made by its producer. UserIO does not verify its truth, authenticate the producer, define a universal result protocol, or infer that `Accepted` or `Completed` grants future authority.

## Responsibility boundary

UserIO carries descriptive semantic values. The host/application still owns interpretation, domain validation, authorization, policy, execution, side effects, and decisions about whether feedback is trustworthy. Adapters own native device events and source-specific conversion. GUI owns presentation; ProtocolAi owns vocabulary/identity policy; GrammarAi owns structural composition; FSM_COS owns runtime composition; Foundry owns authoritative Hub workflow execution and run state.

The core remains dependency-free and platform-neutral. It does not define transport, serialization, persistence, retries, ordering, replay, deduplication, cancellation, capability negotiation, or adapter lifecycle management.

## Verification and release status

Implementation: `SemanticIntent.cs` and `SemanticInteraction.cs`. Tests: `tests/FSM_UserIO.Tests/SemanticIntentTests.cs` and `tests/FSM_UserIO.Tests/SemanticInteractionTests.cs`. The build workflow verifies compilation, tests/coverage, package contents, and a clean consumer build. Only a successful run for the exact commit verifies that commit. Package version remains `0.1.0-alpha.1`; publication requires explicit owner authorization.

---

<p align="center">
  <em>The Singularity Workshop — edify, don't mystify.</em>
</p>
