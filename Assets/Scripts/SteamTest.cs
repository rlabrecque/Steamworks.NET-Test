using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamTest : MonoBehaviour {
	enum EGUIState {
		SteamApps,
		SteamClient,
		SteamFriends,
		SteamUser,
		SteamUtils,

		MAX_STATES
	}
	EGUIState state = EGUIState.SteamApps;

	private bool m_bInitialized = false;

	void Awake() {
		DontDestroyOnLoad(gameObject); // We want our Steam Instance to persist across scenes.

		if (SteamAPI.RestartAppIfNecessary(Constants.k_uAppIdInvalid)) {
			// If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the 
			// local Steam client and also launches this game again.

			// Once you get a public Steam AppID assigned for this game, you need to replace k_uAppIdInvalid with it and
			// removed steam_appid.txt from the game depot.

			Application.Quit();
			return;
		}

		// Initialize SteamAPI, if this fails we bail out since we depend on Steam for everything.
		// You don't necessarily have to though if you write your code to check whether all the Steam
		// interfaces are NULL before using them and provide alternate paths when they are unavailable.
		//
		// This will also load the in-game steam overlay dll into your process.  That dll is normally
		// injected by steam when it launches games, but by calling this you cause it to always load,
		// even when not launched via steam.
		m_bInitialized = SteamAPI.InitSafe();
		if (!m_bInitialized) {
			Debug.Log("SteamAPI_Init() failed");
			return;
		}

		//todo: Check SteamUser.BLoggedOn(); ?

		// Register our Steam Callbacks
		SteamAppsTest.RegisterCallbacks();
		SteamClientTest.RegisterCallbacks();
		SteamFriendsTest.RegisterCallbacks();
		SteamUtilsTest.RegisterCallbacks();
	}

	void OnDestroy() {
		if (m_bInitialized) {
			SteamAPI.Shutdown();
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			++state;
			if (state == EGUIState.MAX_STATES)
				state = EGUIState.SteamApps;
		}
	}

	void FixedUpdate() {
		if (!m_bInitialized) {
			return;
		}

		CallbackDispatcher.RunCallbacks();
	}

	void OnGUI() {
		if (!m_bInitialized) {
			return;
		}

		GUILayout.Label(state.ToString());

		switch (state) {
			case EGUIState.SteamApps:
				SteamAppsTest.RenderOnGUI();
				break;
			case EGUIState.SteamClient:
				SteamClientTest.RenderOnGUI();
				break;
			case EGUIState.SteamFriends:
				SteamFriendsTest.RenderOnGUI();
				break;
			case EGUIState.SteamUser:
				GUISteamUser();
				break;
			case EGUIState.SteamUtils:
				SteamUtilsTest.RenderOnGUI();
				break;
		}
	}
	
	void GUISteamUser() {
		GUILayout.Label("SteamUser.GetHSteamUser : " + SteamUser.GetHSteamUser());
		GUILayout.Label("SteamUser.BLoggedOn : " + SteamUser.BLoggedOn());
		GUILayout.Label("SteamUser.GetSteamID : " + SteamUser.GetSteamID());
		//GUILayout.Label("SteamUser.InitiateGameConnection : " + SteamUser.InitiateGameConnection()); // N/A
		//GUILayout.Label("SteamUser.TerminateGameConnection : " + SteamUser.TerminateGameConnection()); // N/A
		//GUILayout.Label("SteamUser.TrackAppUsageEvent : " + SteamUser.TrackAppUsageEvent()); // N/A
		//GUILayout.Label("SteamUser.GetUserDataFolder : " + SteamUser.GetUserDataFolder()); // Todo
		//GUILayout.Label("SteamUser.StartVoiceRecording : " + SteamUser.StartVoiceRecording()); // N/A
		//GUILayout.Label("SteamUser.StopVoiceRecording : " + SteamUser.StopVoiceRecording()); // N/A
		//GUILayout.Label("SteamUser.GetAvailableVoice : " + SteamUser.GetAvailableVoice()); // N/A
		//GUILayout.Label("SteamUser.GetVoice : " + SteamUser.GetVoice()); // N/A
		//GUILayout.Label("SteamUser.DecompressVoice : " + SteamUser.DecompressVoice()); // N/A
		//GUILayout.Label("SteamUser.GetVoiceOptimalSampleRate : " + SteamUser.GetVoiceOptimalSampleRate()); // N/A
		//GUILayout.Label("SteamUser.GetAuthSessionTicket : " + SteamUser.GetAuthSessionTicket()); // N/A
		//GUILayout.Label("SteamUser.BeginAuthSession : " + SteamUser.BeginAuthSession()); // N/A
		//GUILayout.Label("SteamUser.EndAuthSession : " + SteamUser.EndAuthSession()); // N/A
		//GUILayout.Label("SteamUser.CancelAuthTicket : " + SteamUser.CancelAuthTicket()); // N/A
		GUILayout.Label("SteamUser.UserHasLicenseForApp : " + SteamUser.UserHasLicenseForApp(SteamUser.GetSteamID(), 480));
		GUILayout.Label("SteamUser.BIsBehindNAT : " + SteamUser.BIsBehindNAT());
		//GUILayout.Label("SteamUser.AdvertiseGame : " + SteamUser.AdvertiseGame()); // N/A
		//GUILayout.Label("SteamUser.RequestEncryptedAppTicket : " + SteamUser.RequestEncryptedAppTicket()); // N/A
		//GUILayout.Label("SteamUser.GetEncryptedAppTicket : " + SteamUser.GetEncryptedAppTicket()); // N/A
		GUILayout.Label("SteamUser.GetGameBadgeLevel : " + SteamUser.GetGameBadgeLevel(1, false));
		GUILayout.Label("SteamUser.GetPlayerSteamLevel : " + SteamUser.GetPlayerSteamLevel());
#if _PS3
		//GUILayout.Label("SteamUser.LogOn : " + SteamUser.LogOn()); // N/A
		//GUILayout.Label("SteamUser.LogOnAndLinkSteamAccountToPSN : " + SteamUser.LogOnAndLinkSteamAccountToPSN()); // N/A
		//GUILayout.Label("SteamUser.LogOnAndCreateNewSteamAccountIfNeeded : " + SteamUser.LogOnAndCreateNewSteamAccountIfNeeded()); // N/A
		//GUILayout.Label("SteamUser.GetConsoleSteamID : " + SteamUser.GetConsoleSteamID());
#endif
	}
}
