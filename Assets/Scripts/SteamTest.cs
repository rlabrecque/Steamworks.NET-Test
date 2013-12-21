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

	// serialize this across script reloads somehow?
	private static SteamTest m_SteamTest = null;

	private SteamAppsTest AppsTest;
	private SteamClientTest ClientTest;
	private SteamFriendsTest FriendsTest;
	private SteamUserTest UserTest;
	private SteamUtilsTest UtilsTest;

	void Awake() {
		// Only one instance of Steamworks at a time!
		if (m_SteamTest != null) {
			Destroy(gameObject);
			return;
		}
		
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
		AppsTest = gameObject.AddComponent<SteamAppsTest>();
		ClientTest = gameObject.AddComponent<SteamClientTest>();
		FriendsTest = gameObject.AddComponent<SteamFriendsTest>();
		UserTest = gameObject.AddComponent<SteamUserTest>();
		UtilsTest = gameObject.AddComponent<SteamUtilsTest>();

		// We want our Steam Instance to persist across scenes.
		DontDestroyOnLoad(gameObject);
		m_SteamTest = this;
	}

	void OnDestroy() {
		if (m_bInitialized) {
			SteamAPI.Shutdown();
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)) {
			++state;
			if (state == EGUIState.MAX_STATES)
				state = EGUIState.SteamApps;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			--state;
			if (state == (EGUIState)(-1))
				state = EGUIState.MAX_STATES - 1;
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

		GUILayout.Label("[" + ((int)state + 1) + " / " + (int)EGUIState.MAX_STATES + "] " + state.ToString());

		switch (state) {
			case EGUIState.SteamApps:
				AppsTest.RenderOnGUI();
				break;
			case EGUIState.SteamClient:
				ClientTest.RenderOnGUI();
				break;
			case EGUIState.SteamFriends:
				FriendsTest.RenderOnGUI();
				break;
			case EGUIState.SteamUser:
				UserTest.RenderOnGUI();
				break;
			case EGUIState.SteamUtils:
				UtilsTest.RenderOnGUI();
				break;
		}
	}
}
