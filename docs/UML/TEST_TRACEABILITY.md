<!-- The Singularity Workshop moniker -->
<p align="center"><a href="https://github.com/TrentBest"><img src="https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Visualize/development/docs/assets/moniker.svg" alt="The Singularity Workshop animated moniker" width="720" /></a></p>

# Requirement-to-Test Traceability

**Evidence type:** Human-authored mapping checked against the named test methods in the current `development` branch. This page is not generated coverage data and does not claim that every public declaration or requirement has been mapped.

**Repository:** `TrentBest/FSM_UserIO`  
**Branch reviewed:** `development`  
**Scope reviewed:** semantic intent, semantic interaction records, payload ownership, and source-neutral audio capture status.

## How to read this page

Each row connects an observable contract or invariant to a test method that exercises it. A test proves only the behavior asserted by that test; it does not prove production integration, OS behavior, authorization policy, or untested edge cases.

## Semantic intent

Source: [SemanticIntent.cs](../../SemanticIntent.cs)  
Tests: [SemanticIntentTests.cs](../../tests/FSM_UserIO.Tests/SemanticIntentTests.cs)

| Contract / requirement | Test evidence | What the test establishes |
|---|---|---|
| Application-owned intent name and optional protocol identity are preserved | `Intent_preserves_application_owned_identity` | The supplied name and protocol ID are retained. |
| Protocol identity is optional | `Intent_does_not_require_protocol_identity` | An intent can be constructed without a protocol ID. |
| Whitespace-only names are invalid, including additional Unicode whitespace forms | `Intent_rejects_additional_unicode_whitespace_names` | The listed control and Unicode whitespace values throw `ArgumentException`. |
| Equal intent values have equal hash codes | `Intent_equal_values_have_equal_hash_codes` | Record value equality and hash consistency for equal values. |
| Name comparison is case-sensitive | `Intent_name_comparison_is_case_sensitive` | Differently cased names are not equal. |

## Semantic interaction contracts

Source: [SemanticInteraction.cs](../../SemanticInteraction.cs)  
Tests: [SemanticInteractionTests.cs](../../tests/FSM_UserIO.Tests/SemanticInteractionTests.cs)

| Contract / requirement | Test evidence | What the test establishes |
|---|---|---|
| Capabilities preserve a name and a defined interaction mode | `Capability_preserves_name_and_defined_mode`, `Capability_rejects_invalid_name`, `Capability_rejects_undefined_mode` | Valid values are retained; blank names and undefined enum values are rejected. |
| Observation confidence is finite and within the inclusive range [0, 1] | `Observation_accepts_confidence_inclusive_range`, `Observation_rejects_invalid_confidence` | Boundary values are accepted; out-of-range, NaN, and infinities are rejected. |
| Observation preserves supplied timestamp/source/name values | `Observation_preserves_timestamp_and_source_and_does_not_normalize` | Timestamp and labels are retained without normalization. |
| Optional observation source, when supplied, cannot be blank | `Observation_rejects_blank_source_when_supplied` | Empty and whitespace source labels are rejected. |
| Observation and expression names are required | `Observation_rejects_invalid_name`, `Expression_rejects_invalid_name` | Null, empty, and whitespace-only names are rejected. |
| Expressions retain their source label and remain distinct from intent | `Expression_is_not_an_intent_and_keeps_its_source_label`, `Expression_rejects_blank_source_when_supplied` | The expression name/source are preserved and a supplied blank source is rejected. |
| Interaction context requires correlation identity while participant/session metadata is optional | `Context_allows_optional_participant_and_session_metadata`, `Context_defaults_optional_metadata_to_null`, `Context_rejects_empty_correlation_and_blank_participant` | Optional metadata behavior and invalid correlation/participant/session values are covered. |
| Feedback preserves correlation, kind, and message, while rejecting invalid values | `Feedback_preserves_kind_message_and_correlation`, `Feedback_rejects_invalid_correlation_kind_and_message` | Valid feedback fields are retained; empty correlation, undefined kind, and blank message are rejected. |
| Semantic contract records provide value equality and consistent hashes | `Semantic_contract_records_use_value_equality_and_consistent_hash_codes` | Equality/hash consistency is exercised for capability, observation, expression, context, and feedback records. |
| Payload input and returned data are isolated from caller mutation | `Payload_copies_input_and_exposes_copy_backed_data` | Mutating the constructor array or a copied array from `Data` does not mutate stored bytes. |
| Payload equality includes metadata and bytes | `Payload_uses_value_equality_for_metadata_and_bytes` | Identical metadata/bytes compare equal; differing bytes compare unequal. |
| Payload metadata is required, bytes cannot be null, and empty bytes are allowed | `Payload_rejects_missing_metadata_or_null_bytes_but_allows_empty_bytes` | Constructor validation and the empty-payload case are covered. |

**Authority boundary:** these tests validate data contracts. They do not establish that a capability grants permission, that an expression is authorized, or that feedback is an authoritative receipt. Those decisions remain with the integrating host.

## Audio capture source status

Source: [AudioCaptureSourceStatus.cs](../../AudioCaptureSourceStatus.cs)  
Tests: [AudioCaptureSourceStatusTests.cs](../../tests/FSM_UserIO.Tests/AudioCaptureSourceStatusTests.cs)

| Contract / requirement | Test evidence | What the test establishes |
|---|---|---|
| Microphone and system-audio statuses are independent source-tagged values | `Sources_are_independent_values` | Both source kinds retain their identity and default evidence value. |
| Successful acquisition follows the declared lifecycle without mutating prior values | `Successful_capture_lifecycle_is_valid_and_immutable` | NotRequested → RequestPending → Ready → Capturing → Ready is accepted and earlier records remain unchanged. |
| Failure states preserve source-neutral diagnostic metadata | `Failed_acquisition_preserves_source_neutral_diagnostics` | Denied, Unavailable, and Failed transitions preserve error/recovery text. |
| Cancellation is represented explicitly and can be retried | `Cancellation_is_a_real_state_and_can_be_retried` | Cancellation and subsequent request/retry transitions are accepted. |
| Host estimates are distinguishable from direct observations | `Host_estimate_is_not_misrepresented_as_direct_observation` | HostReportedEstimate remains explicit in the resulting value. |
| Partial success does not collapse microphone and system-audio outcomes | `Partial_success_keeps_microphone_and_system_audio_statuses_separate` | One source can be denied while the other is ready. |
| Invalid lifecycle edges are rejected | `Invalid_lifecycle_transitions_are_rejected` | Representative illegal state transitions throw `InvalidOperationException`. |
| Undefined source, state, and evidence enum values are rejected | `Undefined_source_state_and_evidence_are_rejected` | Constructor and transition validation reject undefined enum values. |
| Optional diagnostic strings cannot be whitespace-only | `Optional_error_metadata_rejects_whitespace` | Whitespace error codes and recovery guidance throw `ArgumentException`. |
| Permission revocation and source loss can terminate active capture | `Permission_revocation_and_source_loss_can_end_active_capture` | Capturing → Denied and Capturing → Unavailable transitions are supported. |

**Scope boundary:** these are tests of the source-neutral status value and transition contract. They do not verify Windows WASAPI capture, endpoint enumeration, hardware removal, timing, callback delivery, or real device permissions.

## Known traceability gaps

- No complete requirement catalogue has yet been mapped to these tests.
- The tables do not include every test project, sample, integration surface, or public declaration in the repository.
- The Windows loopback adapter's compilation evidence and hardware behavior are separate from the status-record tests. The documented hardware verification remains pending.
- No automated test-name extraction, coverage report, or requirement-coverage gate is claimed.
- A passing test suite would establish only the assertions exercised by its tests; it would not close the gaps above.

## Maintenance rule

When a contract changes, update the source-linked type page and this mapping in the same change. Add or revise a test whose name describes the behavior being protected. If a requirement is intentionally not tested, state why and identify the remaining verification method rather than implying coverage.

<!-- THE SINGULARITY WORKSHOP FOOTER -->
<p align="center"><img src="https://raw.githubusercontent.com/TrentBest/FSM_API/master/Documentation/Branding/TheSingularityWorkshop.png" alt="The Singularity Workshop avatar" width="200" /></p>
