<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg" alt="The Singularity Workshop — animated workshop moniker" width="720" /></a></p>

# README Badge Audit

**Repository:** `TrentBest/FSM_UserIO`  
**Branch reviewed:** `development`  
**Review date:** 2026-10-29  
**Scope:** README badge order, destination, applicability, and branch alignment. Image-host availability was not independently confirmed by a live browser render.

## Findings

| Badge | Result | Action |
|---|---|---|
| GitHub Actions | Correct workflow; README now targets `development`, where active work is validated. | Retained and moved first. |
| NuGet version | Exact package ID; reflects the published feed, not development source. | Retained second. |
| NuGet downloads | Exact package ID and feed. | Retained third. |
| Codecov | Repository-specific coverage endpoint is configured in CI. The badge value itself was not independently rendered during this audit. | Retained fourth; visual health remains unverified. |
| Last commit | Previously targeted `master` while the README being edited was on `development`. | Corrected to `development`; retained fifth. |
| License | Points to the repository's `LICENSE.txt`. | Retained last. |
| GitHub issues | Extra badge outside the shared default set; no unique release-readiness evidence. | Removed from the compact row. |
| .NET 8 | Target framework is useful information but does not need a badge; the project file and onboarding explain the runtime requirement. | Removed from the compact row. |

## Limits

This is a source/configuration audit, not a guarantee that external badge image services are currently healthy. A transient or inaccessible image service should be reported as unverified rather than called broken. Recheck badge rendering when validating the release candidate.

---

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
