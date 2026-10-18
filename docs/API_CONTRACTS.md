# UserIO API Contract: Alpha 1

This page documents the public core contract that exists today. It is intentionally narrower than the complete UserIO domain.

## `SemanticIntent`

Namespace: `TheSingularityWorkshop.FSM_UserIO`

```csharp
public sealed record SemanticIntent(string Name, ulong? ProtocolId = null);
```

### Fields and behavior

| Member | Contract |
|---|---|
| `Name` | Required application-defined semantic name. Null, empty, and whitespace-only names throw `ArgumentException`. A nonblank value is preserved as supplied; the constructor does not normalize or trim it. |
| `ProtocolId` | Optional `ulong` value. Null means no numeric protocol identity is associated with this value. UserIO does not allocate, resolve, validate against a vocabulary, or authenticate this ID. |
| Record equality | The type is a C# record and therefore uses value-based equality semantics for its record members. |

### Example

```csharp
var openSettings = new SemanticIntent("open-settings");
var identified = new SemanticIntent("open-settings", protocolId: 42UL);
```

The first value has no numeric identity. The second carries an application-supplied identity association. Neither value grants permission or executes an operation.

### Test traceability

The current test suite demonstrates the implemented Alpha 1 contract:

| Contract behavior | Test |
|---|---|
| Application-owned name and optional numeric identity are preserved | `Intent_preserves_application_owned_identity` |
| Numeric identity is optional | `Intent_does_not_require_protocol_identity` |
| Empty and whitespace-only names are rejected | `Intent_requires_a_non_blank_name` |
| Equal member values produce equal records | `Intent_is_value_based` |

These tests cover the present value object; they do not demonstrate adapter interoperability, host authorization, execution, delivery guarantees, or any deferred domain contract.

### Non-guarantees

`SemanticIntent` does not prove that:

- the name exists in an application vocabulary;
- the numeric identity is registered, unique, trusted, or valid for a particular host;
- the originating form possesses the relevant capability;
- the intent is authorized or safe;
- a payload, session, observation, source, timestamp, or provenance record exists;
- the requested operation was performed.

Those questions belong to the vocabulary owner, adapter, host, policy, or application domain as appropriate.

## Contract evolution rule

Do not expand this type merely because a future interaction protocol might need more fields. First identify a real consumer, the owning layer, compatibility requirements, and a testable invariant. Observation, expression/proposal, payload, session, feedback, and delivery semantics are not Alpha 1 API contracts; see the [domain completion roadmap](ROADMAP.md).
