<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# InteractionMode

**Evidence:** Source-derived from [InteractionMode](../../../SemanticInteraction.cs) and named tests in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose and contract

Classifies the kind of capability represented by InteractionCapability. Observe (0) means receive, sense, or observe information; Express (1) means express or propose information; Act (2) describes a capability to perform an action. Act is not authorization and does not imply the action is trusted, permitted, queued, or executed. The capability tests exercise defined values and rejection of an undefined numeric enum.

## Evidence and limits

The linked tests are evidence for the specific behaviors named above, not proof of host policy, trust, or runtime behavior beyond their assertions. See [API contracts](../../API_CONTRACTS.md), [interaction lifecycle](../../INTERACTION_LIFECYCLE.md), and [UML inventory](../README.md) for the broader semantic boundary and remaining diagram coverage.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
