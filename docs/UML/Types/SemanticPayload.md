<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# SemanticPayload

**Evidence:** Source-derived from [SemanticPayload](../../../SemanticInteraction.cs) and named tests in [SemanticInteractionTests.cs](../../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs). Checked against current development source; manually authored, not generator output.

## Purpose and contract

Carries opaque application-owned bytes with application-defined media type and schema labels. MediaType and Schema must be non-whitespace. A null byte array is rejected; empty bytes are allowed. Input bytes are copied at construction and Data returns a copy-backed ReadOnlyMemory<byte>, so callers cannot mutate stored bytes through the returned view. Equality compares labels ordinally and bytes by content; hashing traverses the same fields/bytes. Construction, Data access, equality, and hashing are O(n) in payload length; Data allocates a fresh array per access. No size limit, serializer, schema registry, or deserialization policy is prescribed. Tests cover copying, equality, metadata validation, null rejection, and empty payloads.

## Evidence and limits

The linked tests are evidence for the specific behaviors named above, not proof of host policy, trust, or runtime behavior beyond their assertions. See [API contracts](../../API_CONTRACTS.md), [interaction lifecycle](../../INTERACTION_LIFECYCLE.md), and [UML inventory](../README.md) for the broader semantic boundary and remaining diagram coverage.


<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
