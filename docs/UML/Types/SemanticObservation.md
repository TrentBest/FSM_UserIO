<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# SemanticObservation

**Evidence:** Source-derived from [SemanticObservation](../../../SemanticInteraction.cs) and named tests in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose and contract

Represents a source-neutral observation from a participating form or adapter, not proof that an event happened or that a source is trusted. Name is required and preserved without trimming. ObservedAt retains the supplied DateTimeOffset. Source is optional and, when present, cannot be whitespace; it is a label, not authenticated identity. Confidence is optional and must be finite within [0, 1]. Tests cover boundaries, NaN/infinities/out-of-range confidence, timestamp/source preservation, and invalid names/source labels. Clock accuracy, source authentication, and confidence calibration are outside this contract.

## Evidence and limits

The linked tests are evidence for the specific behaviors named above, not proof of host policy, trust, or runtime behavior beyond their assertions. See [API contracts](../../API_CONTRACTS.md), [interaction lifecycle](../../INTERACTION_LIFECYCLE.md), and [UML inventory](../README.md) for the broader semantic boundary and remaining diagram coverage.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
