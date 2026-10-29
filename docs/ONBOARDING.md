<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# FSM_UserIO Onboarding

**Goal:** understand the semantic boundary, run a working consumer, and know which contracts are actually available in the published package.

This guide follows the Workshop's [shared documentation standard](https://github.com/TrentBest/TheSingularityWorkshop.FSM_COS/blob/master/DOCUMENTATION_STANDARD.md). FSM_UserIO remains authoritative for its own API, package status, and boundaries.

## 1. Understand the boundary

FSM_UserIO carries source-neutral semantic interaction. It does not own a physical device, GUI framework, application authorization policy, or execution.

`Capability ≠ observation ≠ expression/proposal ≠ intent ≠ authority ≠ execution`

A keyboard event, microphone observation, accessibility action, software message, or AI proposal may be mapped to application-owned meaning by an adapter or host. The host decides whether that meaning is permitted and what happens next.

## 2. Install the published package

The currently published package is **`TheSingularityWorkshop.FSM_UserIO 0.1.0-alpha.1`**. That release exposes `SemanticIntent`; the additional contracts documented on the development branch are not available from that published package yet.

Create a .NET 8 console application and install the published version:

```powershell
dotnet new console --framework net8.0
cd <your-project-directory>
dotnet add package TheSingularityWorkshop.FSM_UserIO --version 0.1.0-alpha.1
```

Replace `Program.cs` with:

```csharp
using TheSingularityWorkshop.FSM_UserIO;

var intent = new SemanticIntent("open-settings");

Console.WriteLine(intent.Name);
Console.WriteLine(intent.ProtocolId is null ? "No protocol identity" : intent.ProtocolId);
```

Run it:

```powershell
dotnet run
```

Expected output:

```text
open-settings
No protocol identity
```

`Name` is required and cannot be null, empty, or whitespace. `ProtocolId` is an optional association value; it does not authenticate a sender, resolve a protocol namespace, grant permission, or execute an action.

## 3. See the host-owned decision

Run the repository's consumer sample from the repository root:

```powershell
dotnet run --project samples/SemanticIntentHostSample/SemanticIntentHostSample.csproj --configuration Release -- --self-test
```

Expected output:

```text
Host accepted: open-settings
Host denied: delete-account
```

This sample uses the published Alpha 1 package. Its key-to-intent mapping and host policy are application code, not APIs supplied by FSM_UserIO. The important proof is that a valid semantic intent can still be denied by the host.

## 4. Choose the next path

- [API contracts](API_CONTRACTS.md) — constructor invariants, equality, payload and context contracts, and non-guarantees.
- [Architecture](ARCHITECTURE.md) — ownership boundaries and dependency direction.
- [Theory](THEORY.md) — why capability, observation, expression, intent, authority, and execution stay distinct.
- [Integration walkthrough](INTEGRATION_WALKTHROUGH.md) — adapter-to-intent-to-host decision.
- [Gallery](GALLERY.md) — diagrams and evidence, with conceptual visuals distinguished from verified behavior.
- [Audio capture source contract](AUDIO_CAPTURE_CONTRACT.md) — source-neutral audio lifecycle; this contract does not itself acquire audio.
- [Windows system-audio loopback decision](WINDOWS_SYSTEM_AUDIO_LOOPBACK.md) and [Windows loopback sample](../samples/WindowsLoopbackSample/README.md) — optional platform adapter and its manual verification requirements.
- [Development](DEVELOPMENT.md) — build, test, package, and contribution rules.
- [Alpha readiness](ALPHA_READINESS.md) and [Domain completion criteria](DOMAIN_COMPLETION_CRITERIA.md) — release evidence and completion criteria.

## 5. Keep package status straight

| Evidence target | What it means |
|---|---|
| Published `0.1.0-alpha.1` | Consumer-visible package currently available on NuGet; `SemanticIntent` is the published baseline. |
| `development` source | Contains additional contracts and samples that require their own release review and verification. |
| Build/test/pack success | Evidence for the checks actually run; not authorization to publish. |
| Conceptual diagram | Explains an intended boundary; does not prove runtime behavior. |

Do not rely on a development-only type from a consumer project that references the published Alpha 1 package. A future package version must be reviewed and explicitly authorized before publication.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
