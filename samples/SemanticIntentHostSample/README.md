<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# SemanticIntent Host Sample

This runnable .NET 8 console sample uses the **published** `TheSingularityWorkshop.FSM_UserIO 0.1.0-alpha.1` package. It demonstrates a human-facing keyboard source, application-owned mapping to `SemanticIntent`, and a separate host policy decision.

## Run it

From the repository root:

```powershell
dotnet run --project samples/SemanticIntentHostSample/SemanticIntentHostSample.csproj
```

- Press **S** to propose `open-settings`. The host accepts it.
- Press **D** to propose `delete-account`. The host denies it.
- Press **Q** to quit.

The key-to-intent mapping and `HostAccepts` function are local sample code, not public FSM_UserIO APIs. The example intentionally demonstrates that a well-formed intent does not grant permission or execute an effect.

## What this proves—and does not prove

The sample compiles against the published Alpha 1 package and demonstrates one concrete keyboard-to-semantic mapping plus a host-owned negative path. It is not a reusable platform adapter, authorization framework, accessibility integration, or cross-modal conformance test.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
