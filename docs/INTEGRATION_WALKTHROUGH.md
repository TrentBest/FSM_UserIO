<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# Integration Walkthrough: Adapter → Intent → Host Decision

The first example uses the published Alpha 1 contract and application-local functions. It deliberately does **not** invent a public UserIO adapter, dispatcher, authorization service, or execution API. The expanded source contracts available on `development` are summarized separately below; they are not available from the published Alpha 1 package.

## What the Alpha 1 sample demonstrates

1. Source-specific input is translated by application-owned adapter code.
2. The adapter creates the real `SemanticIntent` value.
3. The host independently accepts or rejects the intent.
4. UserIO neither grants authority nor performs the effect.

## Example

Create a .NET 8 console application and install the published Alpha 1 package:

```powershell
dotnet new console --framework net8.0
dotnet add package TheSingularityWorkshop.FSM_UserIO --version 0.1.0-alpha.1
```

Replace `Program.cs` with the following:

```csharp
using TheSingularityWorkshop.FSM_UserIO;

// These strings stand in for source-specific events. The mapping is application code,
// not a public FSM_UserIO adapter API.
var sourceEvents = new[] { "settings-button-clicked", "delete-button-clicked" };

foreach (var sourceEvent in sourceEvents)
{
    var intent = TranslateSourceEvent(sourceEvent);

    if (intent is null)
    {
        Console.WriteLine($"Ignored unsupported source event: {sourceEvent}");
        continue;
    }

    if (!HostAccepts(intent))
    {
        Console.WriteLine($"Host rejected: {intent.Name}");
        continue;
    }

    // The effect is deliberately owned by the host/application.
    Console.WriteLine($"Host accepted: {intent.Name}");
}

static SemanticIntent? TranslateSourceEvent(string sourceEvent) =>
    sourceEvent switch
    {
        "settings-button-clicked" => new SemanticIntent("open-settings"),
        "delete-button-clicked" => new SemanticIntent("delete-account"),
        _ => null
    };

static bool HostAccepts(SemanticIntent intent)
{
    // Illustrative host policy. Real applications must implement their own
    // authorization and domain validation; the intent name grants no permission.
    return intent.Name == "open-settings";
}
```

Expected output:

```text
Host accepted: open-settings
Host rejected: delete-account
```

## Expanded source-contract example (development only)

This example shows how source-neutral values can travel together without moving policy into UserIO. It requires the current repository source; it will not compile against the published Alpha 1 package.

```csharp
using System.Text;
using TheSingularityWorkshop.FSM_UserIO;

var capability = new InteractionCapability("navigation.open", InteractionMode.Act);
var observation = new SemanticObservation(
    "button-activated",
    DateTimeOffset.UtcNow,
    Source: "desktop-adapter",
    Confidence: 1.0);
var expression = new SemanticExpression(
    "open-settings",
    DateTimeOffset.UtcNow,
    Source: "hotkey-adapter");
var intent = new SemanticIntent("open-settings");
var payload = new SemanticPayload(
    "application/json",
    "settings.v1",
    Encoding.UTF8.GetBytes("{\"section\":\"display\"}"));
var context = new InteractionContext(Guid.NewGuid(), ParticipantId: "operator-1");
var feedback = new SemanticFeedback(
    context.CorrelationId,
    FeedbackKind.Informational,
    "Received; host policy has not yet been evaluated.");

// These values describe an interaction. They do not authorize or execute it.
Console.WriteLine($"{capability.Name}: {capability.Mode}");
Console.WriteLine($"{observation.Name} at {observation.ObservedAt:O}");
Console.WriteLine($"{expression.Name} -> {intent.Name}");
Console.WriteLine($"Payload bytes: {payload.Data.Length}; correlation: {context.CorrelationId}");
Console.WriteLine(feedback.Message);
```

The adapter supplies source labels and timestamps. The host decides how to interpret the expression, validate the payload schema, authorize the intent, and execute any effect. Feedback is only a statement by its producer; UserIO does not verify that statement.

## Reading the boundary correctly

| Step | Owner | What the current source provides |
|---|---|---|
| Receive a native or UI event | Source / application adapter | Nothing; source events stay outside the core |
| Translate source event to semantic meaning | Application adapter | The adapter can construct `SemanticIntent` |
| Carry semantic values | FSM_UserIO | `SemanticIntent`, `SemanticObservation`, `SemanticExpression`, `SemanticPayload`, `InteractionContext`, and `SemanticFeedback` |
| Decide whether the request is permitted | Host / application | Nothing; UserIO does not authorize |
| Perform the effect | Host / domain | Nothing; UserIO does not execute |

The sample is a **consumer walkthrough**, not evidence of an existing reusable adapter integration. The functions `TranslateSourceEvent` and `HostAccepts` are local example code, not exported package APIs.

## What this does not settle

The source-neutral values do not define a universal actor hierarchy, trusted provenance, global ordering, replay protection, cancellation, retries, delivery guarantees, schema registry, capability negotiation, authorization, or execution. Those remain owned by the host, adapter, or transport that can enforce the relevant guarantees.

See the [domain completion roadmap](ROADMAP.md), [ownership matrix](OWNERSHIP_MATRIX.md), [API contracts](API_CONTRACTS.md), and [architecture](ARCHITECTURE.md).

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
