<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# InteractionCapability

**Evidence:** Source-derived from [InteractionCapability](../../../SemanticInteraction.cs) and named tests in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose and contract

Describes a named capability and its interaction mode. The name is required and cannot be whitespace; the original value is retained without trimming. Mode must be a defined InteractionMode. The sealed record has value equality. Crucially, describing a capability does not grant permission, authenticate a caller, or authorize execution. Tests: Capability_preserves_name_and_defined_mode, Capability_rejects_invalid_name, Capability_rejects_undefined_mode.

## Evidence and limits

The linked tests are evidence for the specific behaviors named above, not proof of host policy, trust, or runtime behavior beyond their assertions. See [API contracts](../../API_CONTRACTS.md), [interaction lifecycle](../../INTERACTION_LIFECYCLE.md), and [UML inventory](../README.md) for the broader semantic boundary and remaining diagram coverage.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
