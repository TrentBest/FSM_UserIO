<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# FeedbackKind

**Evidence:** Source-derived from [FeedbackKind](../../../SemanticInteraction.cs) and named tests in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose and contract

Classifies SemanticFeedback: Informational (0) conveys information without asserting acceptance/execution; Accepted (1) means the host reports acceptance for further handling; Rejected (2) means host-reported rejection; ClarificationRequired (3) requests clarification; Completed (4) reports completion of an application-defined operation; Failed (5) reports an application-defined failure. These are not authorization mechanisms, execution receipts, or a standardized workflow. Undefined numeric values are rejected by SemanticFeedback.

## Evidence and limits

The linked tests are evidence for the specific behaviors named above, not proof of host policy, trust, or runtime behavior beyond their assertions. See [API contracts](../../API_CONTRACTS.md), [interaction lifecycle](../../INTERACTION_LIFECYCLE.md), and [UML inventory](../README.md) for the broader semantic boundary and remaining diagram coverage.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
