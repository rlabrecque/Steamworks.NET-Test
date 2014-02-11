using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamTest : MonoBehaviour {
	public enum EGUIState {
		SteamApps,
		SteamClient,
		SteamController,
		SteamFriends,
		SteamFriendsPg2,
		SteamHTTP,
		SteamRemoteStorage,
		SteamRemoteStoragePg2,
		SteamScreenshots,
		SteamUser,
		SteamUserStatsTest,
		SteamUserStatsTestPg2,
		SteamUtils,

		MAX_STATES
	}

	public EGUIState m_State { get; private set; }

	private bool m_bInitialized = false;
	private bool m_bControllerInitialized = false;

	public static SteamTest m_SteamTest = null;

	private SteamAppsTest AppsTest;
	private SteamClientTest ClientTest;
	private SteamControllerTest ControllerTest;
	private SteamFriendsTest FriendsTest;
	private SteamHTTPTest HTTPTest;
	private SteamRemoteStorageTest RemoteStorageTest;
	private SteamScreenshotsTest ScreenshotsTest;
	private SteamUserTest UserTest;
	private SteamUserStatsTest UserStatsTest;
	private SteamUtilsTest UtilsTest;

	SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;
	static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText) {
		Debug.LogWarning(pchDebugText);
	}

	void Awake() {
		// Only one instance of Steamworks at a time!
		if (m_SteamTest != null) {
			Destroy(gameObject);
			return;
		}

		if (!Packsize.Test()) {
			throw new System.Exception("Packsize is wrong! You are likely using a Linux/OSX build on Windows or vice versa.");
		}
		
		// Initialize SteamAPI, if this fails we bail out since we depend on Steam for everything.
		// You don't necessarily have to though if you write your code to check whether all the Steam
		// interfaces are NULL before using them and provide alternate paths when they are unavailable.
		//
		// This will also load the in-game steam overlay dll into your process.  That dll is normally
		// injected by steam when it launches games, but by calling this you cause it to always load,
		// even when not launched via steam.
		m_bInitialized = SteamAPI.Init();
		if (!m_bInitialized) {
			Debug.Log("SteamAPI_Init() failed");
			Application.Quit();
			return;
		}

		// Set up our callback to recieve warning messages from Steam.
		// You must launch with "-debug_steamapi" in the launch args to recieve warnings.
		SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
		SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);

		// We are going to use the controller interface, initialize it, which is a seperate step as it 
		// create a new thread in the game proc and we don't want to force that on games that don't
		// have native Steam controller implementations
		m_bControllerInitialized = SteamController.Init(Application.dataPath + "/controller.vdf");
		if (!m_bControllerInitialized) {
			Debug.LogWarning("Steam Controller Failed to Initialize");
		}
		
		// Register our Steam Callbacks
		AppsTest = gameObject.AddComponent<SteamAppsTest>();
		ClientTest = gameObject.AddComponent<SteamClientTest>();
		ControllerTest = gameObject.AddComponent<SteamControllerTest>();
		FriendsTest = gameObject.AddComponent<SteamFriendsTest>();
		HTTPTest = gameObject.AddComponent<SteamHTTPTest>();
		RemoteStorageTest = gameObject.AddComponent<SteamRemoteStorageTest>();
		ScreenshotsTest = gameObject.AddComponent<SteamScreenshotsTest>();
		UserTest = gameObject.AddComponent<SteamUserTest>();
		UserStatsTest = gameObject.AddComponent<SteamUserStatsTest>();
		UtilsTest = gameObject.AddComponent<SteamUtilsTest>();

		// We want our Steam Instance to persist across scenes.
		DontDestroyOnLoad(gameObject);
		m_SteamTest = this;
	}

	void OnEnable() {
		// These should only get called after an Assembly reload, You should probably never Disable the Steamworks Manager yourself.
		if (m_SteamTest == null) {
			m_SteamTest = this;
		}

		if (SteamAPIWarningMessageHook == null) {
			SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);
		}
	}

	void OnDestroy() {
		if (m_bControllerInitialized) {
			bool ret = SteamController.Shutdown();
			if (!ret) {
				Debug.LogWarning("SteamController.Shutdown() returned false");
			}
		}

		if (m_bInitialized) {
			SteamAPI.Shutdown();
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)) {
			++m_State;
			if (m_State == EGUIState.MAX_STATES)
				m_State = EGUIState.SteamApps;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			--m_State;
			if (m_State == (EGUIState)(-1))
				m_State = EGUIState.MAX_STATES - 1;
		}
	}

	void FixedUpdate() {
		if (!m_bInitialized) {
			return;
		}

		SteamAPI.RunCallbacks();
	}

	void OnGUI() {
		if (!m_bInitialized) {
			return;
		}

		GUILayout.Label("[" + ((int)m_State + 1) + " / " + (int)EGUIState.MAX_STATES + "] " + m_State.ToString());

		switch (m_State) {
			case EGUIState.SteamApps:
				AppsTest.RenderOnGUI();
				break;
			case EGUIState.SteamClient:
				ClientTest.RenderOnGUI();
				break;
			case EGUIState.SteamController:
				if (m_bControllerInitialized)
					ControllerTest.RenderOnGUI();
				break;
			case EGUIState.SteamFriends:
			case EGUIState.SteamFriendsPg2:
				FriendsTest.RenderOnGUI(m_State);
				break;
			case EGUIState.SteamHTTP:
				HTTPTest.RenderOnGUI();
				break;
			case EGUIState.SteamRemoteStorage:
			case EGUIState.SteamRemoteStoragePg2:
				RemoteStorageTest.RenderOnGUI(m_State);
				break;
			case EGUIState.SteamScreenshots:
				ScreenshotsTest.RenderOnGUI();
				break;
			case EGUIState.SteamUser:
				UserTest.RenderOnGUI();
				break;
			case EGUIState.SteamUserStatsTest:
			case EGUIState.SteamUserStatsTestPg2:
				UserStatsTest.RenderOnGUI(m_State);
				break;
			case EGUIState.SteamUtils:
				UtilsTest.RenderOnGUI();
				break;
		}
	}
}
