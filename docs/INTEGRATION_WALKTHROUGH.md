# Integration Walkthrough: Adapter → Intent → Host Decision

This walkthrough uses the published Alpha 1 contract and application-local functions. It deliberately does **not** invent a public UserIO adapter, observation, dispatcher, authorization service, or execution API.

## What this demonstrates

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

## Reading the boundary correctly

| Step | Owner | What Alpha 1 provides |
|---|---|---|
| Receive a native or UI event | Source / application adapter | Nothing; source events stay outside the core |
| Translate source event to semantic meaning | Application adapter | The adapter can construct `SemanticIntent` |
| Carry the intent value | FSM_UserIO | The implemented `SemanticIntent` type |
| Decide whether the request is permitted | Host / application | Nothing; UserIO does not authorize |
| Perform the effect | Host / domain | Nothing; UserIO does not execute |

The sample is a **consumer walkthrough**, not evidence of an existing reusable adapter integration. The functions `TranslateSourceEvent` and `HostAccepts` are local example code, not exported package APIs.

## What this does not settle

This minimal example does not define a shared observation or expression model, payload schema, actor/session identity, provenance, ordering, cancellation, retries, delivery guarantees, or feedback contract. Those remain open until a real producer and consumer establish the minimum shared semantics.

See the [domain completion roadmap](ROADMAP.md), [ownership matrix](OWNERSHIP_MATRIX.md), [API contracts](API_CONTRACTS.md), and [architecture](ARCHITECTURE.md).
