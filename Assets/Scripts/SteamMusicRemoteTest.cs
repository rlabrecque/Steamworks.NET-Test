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
			bool ret = SteamMusicRemote.RegisterSteamMusicRemote("Steamworks.NET Test Remote");
			print("SteamMusicRemote.RegisterSteamMusicRemote(" + "\"Steamworks.NET Test Remote\"" + ") : " + ret);
		}

		if (GUILayout.Button("DeregisterSteamMusicRemote()")) {
			bool ret = SteamMusicRemote.DeregisterSteamMusicRemote();
			print("SteamMusicRemote.DeregisterSteamMusicRemote() : " + ret);
		}

		GUILayout.Label("BIsCurrentMusicRemote() : " + SteamMusicRemote.BIsCurrentMusicRemote());

		GUILayout.Label("BActivationSuccess(true) : " + SteamMusicRemote.BActivationSuccess(true));

		if (GUILayout.Button("SetDisplayName(\"Some Display Name\")")) {
			bool ret = SteamMusicRemote.SetDisplayName("Some Display Name");
			print("SteamMusicRemote.SetDisplayName(" + "\"Some Display Name\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetPNGIcon_64x64(null, 0)")) {
			// TODO
			bool ret = SteamMusicRemote.SetPNGIcon_64x64(null, 0);
			print("SteamMusicRemote.SetPNGIcon_64x64(" + null + ", " + 0 + ") : " + ret);
		}

		// Abilities for the user interface
		if (GUILayout.Button("EnablePlayPrevious(true)")) {
			bool ret = SteamMusicRemote.EnablePlayPrevious(true);
			print("SteamMusicRemote.EnablePlayPrevious(" + true + ") : " + ret);
		}

		if (GUILayout.Button("EnablePlayNext(true)")) {
			bool ret = SteamMusicRemote.EnablePlayNext(true);
			print("SteamMusicRemote.EnablePlayNext(" + true + ") : " + ret);
		}

		if (GUILayout.Button("EnableShuffled(true)")) {
			bool ret = SteamMusicRemote.EnableShuffled(true);
			print("SteamMusicRemote.EnableShuffled(" + true + ") : " + ret);
		}

		if (GUILayout.Button("EnableLooped(true)")) {
			bool ret = SteamMusicRemote.EnableLooped(true);
			print("SteamMusicRemote.EnableLooped(" + true + ") : " + ret);
		}

		if (GUILayout.Button("EnableQueue(true)")) {
			bool ret = SteamMusicRemote.EnableQueue(true);
			print("SteamMusicRemote.EnableQueue(" + true + ") : " + ret);
		}

		if (GUILayout.Button("EnablePlaylists(true)")) {
			bool ret = SteamMusicRemote.EnablePlaylists(true);
			print("SteamMusicRemote.EnablePlaylists(" + true + ") : " + ret);
		}

		// Status
		if (GUILayout.Button("UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused)")) {
			bool ret = SteamMusicRemote.UpdatePlaybackStatus(AudioPlayback_Status.AudioPlayback_Paused);
			print("SteamMusicRemote.UpdatePlaybackStatus(" + AudioPlayback_Status.AudioPlayback_Paused + ") : " + ret);
		}

		if (GUILayout.Button("UpdateShuffled(true)")) {
			bool ret = SteamMusicRemote.UpdateShuffled(true);
			print("SteamMusicRemote.UpdateShuffled(" + true + ") : " + ret);
		}

		if (GUILayout.Button("UpdateLooped(true)")) {
			bool ret = SteamMusicRemote.UpdateLooped(true);
			print("SteamMusicRemote.UpdateLooped(" + true + ") : " + ret);
		}

		if (GUILayout.Button("UpdateVolume(0.5f)")) {
			bool ret = SteamMusicRemote.UpdateVolume(0.5f);
			print("SteamMusicRemote.UpdateVolume(" + 0.5f + ") : " + ret);
		}

		// Current Entry
		if (GUILayout.Button("CurrentEntryWillChange()")) {
			bool ret = SteamMusicRemote.CurrentEntryWillChange();
			print("SteamMusicRemote.CurrentEntryWillChange() : " + ret);
		}

		if (GUILayout.Button("CurrentEntryIsAvailable(true)")) {
			bool ret = SteamMusicRemote.CurrentEntryIsAvailable(true);
			print("SteamMusicRemote.CurrentEntryIsAvailable(" + true + ") : " + ret);
		}

		if (GUILayout.Button("UpdateCurrentEntryText(\"Current Entry Text\")")) {
			bool ret = SteamMusicRemote.UpdateCurrentEntryText("Current Entry Text");
			print("SteamMusicRemote.UpdateCurrentEntryText(" + "\"Current Entry Text\"" + ") : " + ret);
		}

		if (GUILayout.Button("UpdateCurrentEntryElapsedSeconds(10)")) {
			bool ret = SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(10);
			print("SteamMusicRemote.UpdateCurrentEntryElapsedSeconds(" + 10 + ") : " + ret);
		}

		if (GUILayout.Button("UpdateCurrentEntryCoverArt(null, 0)")) {
			// TODO
			bool ret = SteamMusicRemote.UpdateCurrentEntryCoverArt(null, 0);
			print("SteamMusicRemote.UpdateCurrentEntryCoverArt(" + null + ", " + 0 + ") : " + ret);
		}

		if (GUILayout.Button("CurrentEntryDidChange()")) {
			bool ret = SteamMusicRemote.CurrentEntryDidChange();
			print("SteamMusicRemote.CurrentEntryDidChange() : " + ret);
		}

		// Queue
		if (GUILayout.Button("QueueWillChange()")) {
			bool ret = SteamMusicRemote.QueueWillChange();
			print("SteamMusicRemote.QueueWillChange() : " + ret);
		}

		if (GUILayout.Button("ResetQueueEntries()")) {
			bool ret = SteamMusicRemote.ResetQueueEntries();
			print("SteamMusicRemote.ResetQueueEntries() : " + ret);
		}

		if (GUILayout.Button("SetQueueEntry(0, 0, \"I don't know what I'm doing\")")) {
			bool ret = SteamMusicRemote.SetQueueEntry(0, 0, "I don't know what I'm doing");
			print("SteamMusicRemote.SetQueueEntry(" + 0 + ", " + 0 + ", " + "\"I don't know what I'm doing\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetCurrentQueueEntry(0)")) {
			bool ret = SteamMusicRemote.SetCurrentQueueEntry(0);
			print("SteamMusicRemote.SetCurrentQueueEntry(" + 0 + ") : " + ret);
		}

		if (GUILayout.Button("QueueDidChange()")) {
			bool ret = SteamMusicRemote.QueueDidChange();
			print("SteamMusicRemote.QueueDidChange() : " + ret);
		}

		// Playlist
		if (GUILayout.Button("PlaylistWillChange()")) {
			bool ret = SteamMusicRemote.PlaylistWillChange();
			print("SteamMusicRemote.PlaylistWillChange() : " + ret);
		}

		if (GUILayout.Button("ResetPlaylistEntries()")) {
			bool ret = SteamMusicRemote.ResetPlaylistEntries();
			print("SteamMusicRemote.ResetPlaylistEntries() : " + ret);
		}

		if (GUILayout.Button("SetPlaylistEntry(0, 0, \"I don't know what I'm doing\")")) {
			bool ret = SteamMusicRemote.SetPlaylistEntry(0, 0, "I don't know what I'm doing");
			print("SteamMusicRemote.SetPlaylistEntry(" + 0 + ", " + 0 + ", " + "\"I don't know what I'm doing\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetCurrentPlaylistEntry(0)")) {
			bool ret = SteamMusicRemote.SetCurrentPlaylistEntry(0);
			print("SteamMusicRemote.SetCurrentPlaylistEntry(" + 0 + ") : " + ret);
		}

		if (GUILayout.Button("PlaylistDidChange()")) {
			bool ret = SteamMusicRemote.PlaylistDidChange();
			print("SteamMusicRemote.PlaylistDidChange() : " + ret);
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