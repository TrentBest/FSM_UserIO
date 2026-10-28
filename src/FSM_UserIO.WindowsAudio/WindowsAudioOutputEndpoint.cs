namespace TheSingularityWorkshop.FSM_UserIO.WindowsAudio;

/// <summary>Platform-neutral metadata for a selectable active Windows audio output endpoint.</summary>
/// <param name="Id">Opaque endpoint identifier understood by the Windows audio adapter.</param>
/// <param name="FriendlyName">Display name reported by Windows.</param>
/// <param name="IsDefault">Whether Windows reports this as the default multimedia render endpoint.</param>
public sealed record WindowsAudioOutputEndpoint(string Id, string FriendlyName, bool IsDefault);
