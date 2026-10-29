<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# SemanticValueValidation

**Evidence:** Source-derived from the internal helper in [SemanticInteraction.cs](../../../SemanticInteraction.cs), with behavioral coverage in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose

This internal helper centralizes constructor validation shared by the semantic interaction contracts. It is an implementation detail, not a public extension point or a security boundary.

## Operations

- **Required** rejects null, empty, or whitespace-only strings with ArgumentException; otherwise it returns the original value without trimming.
- **Optional** accepts null, otherwise applies Required.
- **OptionalIdentifier** accepts null and rejects Guid.Empty.
- **Confidence** accepts null or a finite double in the inclusive range 0 through 1; otherwise it throws ArgumentOutOfRangeException.
- **Defined<TEnum>** accepts only enum values recognized by Enum.IsDefined and rejects undefined values with ArgumentOutOfRangeException.

## Complexity and limits

String and enum validation use constant-time checks relative to the value's stored representation for ordinary inputs; this page makes no measured latency or allocation claim. Confidence validation rejects NaN and both infinities through double.IsFinite. These checks validate shape and range, not identity, trust, authorization, semantic correctness, or policy.

## Evidence and limits

Tests exercise these validations through public contract constructors. The helper is internal and its implementation may change without a public API compatibility guarantee. No separate helper-specific test class is currently claimed.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
