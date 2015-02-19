using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamMusicRemoteTest : MonoBehaviour {
	protected Callback<MusicPlayerRemoteWillActivate_t> m_MusicPlayerRemoteWillActivate;
	protected Callback<MusicPlayerRemoteWillDeactivate_t> m_MusicPlayerRemoteWillDeactivate;
	protected Callback<MusicPlayerRemoteToFront_t> m_MusicPlayerRemoteToFront;
	protected Callback<MusicPlayerWillQuit_t> m_MusicPlayerWillQuit;
	protected Callback<MusicPlayerWantsPlay_t> m_MusicPlayerWantsPlay;
	protected Callback<MusicPlayerWantsPause_t> m_MusicPlayerWantsPause;
	protected Callback<MusicPlayerWantsPlayPrevious_t> m_MusicPlayerWantsPlayPrevious;
	protected Callback<MusicPlayerWantsPlayNext_t> m_MusicPlayerWantsPlayNext;
	protected Callback<MusicPlayerWantsShuffled_t> m_MusicPlayerWantsShuffled;
	protected Callback<MusicPlayerWantsLooped_t> m_MusicPlayerWantsLooped;
	protected Callback<MusicPlayerWantsVolume_t> m_MusicPlayerWantsVolume;
	protected Callback<MusicPlayerSelectsQueueEntry_t> m_MusicPlayerSelectsQueueEntry;
	protected Callback<MusicPlayerSelectsPlaylistEntry_t> m_MusicPlayerSelectsPlaylistEntry;
	protected Callback<MusicPlayerWantsPlayingRepeatStatus_t> m_MusicPlayerWantsPlayingRepeatStatus;

	public void OnEnable() {
		m_MusicPlayerRemoteWillActivate = Callback<MusicPlayerRemoteWillActivate_t>.Create(OnMusicPlayerRemoteWillActivate);
		m_MusicPlayerRemoteWillDeactivate = Callback<MusicPlayerRemoteWillDeactivate_t>.Create(OnMusicPlayerRemoteWillDeactivate);
		m_MusicPlayerRemoteToFront = Callback<MusicPlayerRemoteToFront_t>.Create(OnMusicPlayerRemoteToFront);
		m_MusicPlayerWillQuit = Callback<MusicPlayerWillQuit_t>.Create(OnMusicPlayerWillQuit);
		m_MusicPlayerWantsPlay = Callback<MusicPlayerWantsPlay_t>.Create(OnMusicPlayerWantsPlay);
		m_MusicPlayerWantsPause = Callback<MusicPlayerWantsPause_t>.Create(OnMusicPlayerWantsPause);
		m_MusicPlayerWantsPlayPrevious = Callback<MusicPlayerWantsPlayPrevious_t>.Create(OnMusicPlayerWantsPlayPrevious);
		m_MusicPlayerWantsPlayNext = Callback<MusicPlayerWantsPlayNext_t>.Create(OnMusicPlayerWantsPlayNext);
		m_MusicPlayerWantsShuffled = Callback<MusicPlayerWantsShuffled_t>.Create(OnMusicPlayerWantsShuffled);
		m_MusicPlayerWantsLooped = Callback<MusicPlayerWantsLooped_t>.Create(OnMusicPlayerWantsLooped);
		m_MusicPlayerWantsVolume = Callback<MusicPlayerWantsVolume_t>.Create(OnMusicPlayerWantsVolume);
		m_MusicPlayerSelectsQueueEntry = Callback<MusicPlayerSelectsQueueEntry_t>.Create(OnMusicPlayerSelectsQueueEntry);
		m_MusicPlayerSelectsPlaylistEntry = Callback<MusicPlayerSelectsPlaylistEntry_t>.Create(OnMusicPlayerSelectsPlaylistEntry);
		m_MusicPlayerWantsPlayingRepeatStatus = Callback<MusicPlayerWantsPlayingRepeatStatus_t>.Create(OnMusicPlayerWantsPlayingRepeatStatus);
	}

	public void RenderOnGUI() {
		// Service Definition
		if (GUILayout.Button("RegisterSteamMusicRemote(\"Steamworks.NET Test Remote\")")) {
			print("SteamMusicRemote.RegisterSteamMusicRemote(\"Steamworks.NET Test Remote\") : " + SteamMusicRemote.RegisterSteamMusicRemote("Steamworks.NET Test Remote"));
		}

		if (GUILayout.Button("DeregisterSteamMusicRemote()")) {
			print("SteamMusicRemote.DeregisterSteamMusicRemote() : " + SteamMusicRemote.DeregisterSteamMusicRemote());
		}

		GUILayout.Label("SteamMusicRemote.BIsCurrentMusicRemote() : " + SteamMusicRemote.BIsCurrentMusicRemote());
		GUILayout.Label("SteamMusicRemote.BActivationSuccess(true) : " + SteamMusicRemote.BActivationSuccess(true));

		if (GUILayout.Button("SetDisplayName(\"Some Display Name\")")) {
			print("SteamMusicRemote.SetDisplayName(\"Some Display Name\") : " + SteamMusicRemote.SetDisplayName("Some Display Name"));
		}

		if (GUILayout.Button("SetPNGIcon_64x64(TODO)")) {
			print("SteamMusicRemote.SetPNGIcon_64x64(TODO) : " + SteamMusicRemote.SetPNGIcon_64x64(null, 0)); // TODO
		}

		// Abilities for the user interface
		if (GUILayout.Button("EnablePlayPrevious(true)")) {
			print("SteamMusicRemote.EnablePlayPrevious(true) : " + SteamMusicRemote.EnablePlayPrevious(true));
		}

		if (GUILayout.Button("EnablePlayNext(true)")) {
			print("SteamMusicRemote.EnablePlayNext(true) : " + SteamMusicRemote.EnablePlayNext(true));
		}

		if (GUILayout.Button("EnableShuffled(true)")) {
			print("SteamMusicRemote.EnableShuffled(true) : " + SteamMusicRemote.EnableShuffled(true));
		}

		if (GUILayout.Button("EnableLooped(true)")) {
			print("SteamMusicRemote.EnableLooped(true) : " + SteamMusicRemote.EnableLooped(true));
		}

		if (GUILayout.Button("EnableQueue(true)")) {
			print("SteamMusicRemote.EnableQueue(true) : " + SteamMusicRemote.EnableQueue(true));
		}

		if (GUILayout.Button("EnablePlaylists(true)")) {
			print("SteamMusicRemote.EnablePlaylists(true) : " + SteamMusicRemote.EnablePlaylists(true));
		}

		// Status
		if (GUILayout.Button("UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused)")) {
			print("SteamMusicRemote.UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused) : " + SteamMusicRemote.UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused));
		}

		if (GUILayout.Button("UpdateShuffled(true)")) {
			print("SteamMusicRemote.UpdateShuffled(true) : " + SteamMusicRemote.UpdateShuffled(true));
		}

		if (GUILayout.Button("UpdateLooped(true)")) {
			print("SteamMusicRemote.UpdateLooped(true) : " + SteamMusicRemote.UpdateLooped(true));
		}

		if (GUILayout.Button("UpdateVolume(0.5f)")) {
			print("SteamMusicRemote.UpdateVolume(0.5f) : " + SteamMusicRemote.UpdateVolume(0.5f));
		}

		// Current Entry
		if (GUILayout.Button("CurrentEntryWillChange()")) {
			print("SteamMusicRemote.CurrentEntryWillChange() : " + SteamMusicRemote.CurrentEntryWillChange());
		}

		if (GUILayout.Button("CurrentEntryIsAvailable(true)")) {
			print("SteamMusicRemote.CurrentEntryIsAvailable(true) : " + SteamMusicRemote.CurrentEntryIsAvailable(true));
		}

		if (GUILayout.Button("UpdateCurrentEntryText(\"Current Entry Text\")")) {
			print("SteamMusicRemote.UpdateCurrentEntryText(\"Current Entry Text\") : " + SteamMusicRemote.UpdateCurrentEntryText("Current Entry Text"));
		}

		if (GUILayout.Button("UpdateCurrentEntryElapsedSeconds(10)")) {
			print("SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(10) : " + SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(10));
		}

		if (GUILayout.Button("UpdateCurrentEntryCoverArt(TODO)")) {
			print("SteamMusicRemote.UpdateCurrentEntryCoverArt(TODO) : " + SteamMusicRemote.UpdateCurrentEntryCoverArt(null, 0)); // TODO
		}

		if (GUILayout.Button("CurrentEntryDidChange()")) {
			print("SteamMusicRemote.CurrentEntryDidChange() : " + SteamMusicRemote.CurrentEntryDidChange());
		}

		// Queue
		if (GUILayout.Button("QueueWillChange()")) {
			print("SteamMusicRemote.QueueWillChange() : " + SteamMusicRemote.QueueWillChange());
		}

		if (GUILayout.Button("ResetQueueEntries()")) {
			print("SteamMusicRemote.ResetQueueEntries() : " + SteamMusicRemote.ResetQueueEntries());
		}

		if (GUILayout.Button("SetQueueEntry(\"I don't know what I'm doing\")")) {
			print("SteamMusicRemote.SetQueueEntry(\"I don't know what I'm doing\") : " + SteamMusicRemote.SetQueueEntry(0, 0, "I don't know what I'm doing"));
		}

		if (GUILayout.Button("SetCurrentQueueEntry(0)")) {
			print("SteamMusicRemote.SetCurrentQueueEntry(0) : " + SteamMusicRemote.SetCurrentQueueEntry(0));
		}

		if (GUILayout.Button("QueueDidChange()")) {
			print("SteamMusicRemote.QueueDidChange() : " + SteamMusicRemote.QueueDidChange());
		}

		// Playlist
		if (GUILayout.Button("PlaylistWillChange()")) {
			print("SteamMusicRemote.PlaylistWillChange() : " + SteamMusicRemote.PlaylistWillChange());
		}

		if (GUILayout.Button("ResetPlaylistEntries()")) {
			print("SteamMusicRemote.ResetPlaylistEntries() : " + SteamMusicRemote.ResetPlaylistEntries());
		}

		if (GUILayout.Button("SetPlaylistEntry(0, 0, \"I don't know what I'm doing\")")) {
			print("SteamMusicRemote.SetPlaylistEntry(0, 0, \"I don't know what I'm doing\") : " + SteamMusicRemote.SetPlaylistEntry(0, 0, "I don't know what I'm doing"));
		}

		if (GUILayout.Button("SetCurrentPlaylistEntry(0)")) {
			print("SteamMusicRemote.SetCurrentPlaylistEntry(0) : " + SteamMusicRemote.SetCurrentPlaylistEntry(0));
		}

		if (GUILayout.Button("PlaylistDidChange()")) {
			print("SteamMusicRemote.PlaylistDidChange() : " + SteamMusicRemote.PlaylistDidChange());
		}
	}

	void OnMusicPlayerRemoteWillActivate(MusicPlayerRemoteWillActivate_t pCallback) {
		Debug.Log("[" + MusicPlayerRemoteWillActivate_t.k_iCallback + " - MusicPlayerRemoteWillActivate]");
	}

	void OnMusicPlayerRemoteWillDeactivate(MusicPlayerRemoteWillDeactivate_t pCallback) {
		Debug.Log("[" + MusicPlayerRemoteWillDeactivate_t.k_iCallback + " - MusicPlayerRemoteWillDeactivate]");
	}

	void OnMusicPlayerRemoteToFront(MusicPlayerRemoteToFront_t pCallback) {
		Debug.Log("[" + MusicPlayerRemoteToFront_t.k_iCallback + " - MusicPlayerRemoteToFront]");
	}

	void OnMusicPlayerWillQuit(MusicPlayerWillQuit_t pCallback) {
		Debug.Log("[" + MusicPlayerWillQuit_t.k_iCallback + " - MusicPlayerWillQuit]");
	}

	void OnMusicPlayerWantsPlay(MusicPlayerWantsPlay_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsPlay_t.k_iCallback + " - MusicPlayerWantsPlay]");
	}

	void OnMusicPlayerWantsPause(MusicPlayerWantsPause_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsPause_t.k_iCallback + " - MusicPlayerWantsPause]");
	}

	void OnMusicPlayerWantsPlayPrevious(MusicPlayerWantsPlayPrevious_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsPlayPrevious_t.k_iCallback + " - MusicPlayerWantsPlayPrevious]");
	}

	void OnMusicPlayerWantsPlayNext(MusicPlayerWantsPlayNext_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsPlayNext_t.k_iCallback + " - MusicPlayerWantsPlayNext]");
	}

	void OnMusicPlayerWantsShuffled(MusicPlayerWantsShuffled_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsShuffled_t.k_iCallback + " - MusicPlayerWantsShuffled] - " + pCallback.m_bShuffled);
	}

	void OnMusicPlayerWantsLooped(MusicPlayerWantsLooped_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsLooped_t.k_iCallback + " - MusicPlayerWantsLooped] - " + pCallback.m_bLooped);
	}

	void OnMusicPlayerWantsVolume(MusicPlayerWantsVolume_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsVolume_t.k_iCallback + " - MusicPlayerWantsVolume] - " + pCallback.m_flNewVolume);
	}

	void OnMusicPlayerSelectsQueueEntry(MusicPlayerSelectsQueueEntry_t pCallback) {
		Debug.Log("[" + MusicPlayerSelectsQueueEntry_t.k_iCallback + " - MusicPlayerSelectsQueueEntry] - " + pCallback.nID);
	}

	void OnMusicPlayerSelectsPlaylistEntry(MusicPlayerSelectsPlaylistEntry_t pCallback) {
		Debug.Log("[" + MusicPlayerSelectsPlaylistEntry_t.k_iCallback + " - MusicPlayerSelectsPlaylistEntry] - " + pCallback.nID);
	}

	void OnMusicPlayerWantsPlayingRepeatStatus(MusicPlayerWantsPlayingRepeatStatus_t pCallback) {
		Debug.Log("[" + MusicPlayerWantsPlayingRepeatStatus_t.k_iCallback + " - MusicPlayerWantsPlayingRepeatStatus] - " + pCallback.m_nPlayingRepeatStatus);
	}
}
