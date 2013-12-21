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
		SteamUserTest.RegisterCallbacks();
		SteamUtilsTest.RegisterCallbacks();
	}

	void OnDestroy() {
		if (m_bInitialized) {
			SteamAPI.Shutdown();
		}
	}

	void Update() {
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
				SteamUserTest.RenderOnGUI();
				break;
			case EGUIState.SteamUtils:
				SteamUtilsTest.RenderOnGUI();
				break;
		}
	}
}
