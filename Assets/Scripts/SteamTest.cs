using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamTest : MonoBehaviour {
	public enum EGUIState {
		SteamAppList,
		SteamApps,
		SteamClient,
		SteamController,
		SteamFriends,
		SteamFriendsPg2,
		SteamHTMLSurface,
		SteamHTTP,
		SteamInventory,
		SteamMatchmaking,
		SteamMatchmakingServers,
		SteamMusic,
		SteamMusicRemote,
		SteamNetworking,
		SteamRemoteStorage,
		SteamRemoteStoragePg2,
		SteamScreenshots,
		SteamUGC,
		SteamUnifiedMessages,
		SteamUser,
		SteamUserStatsTest,
		SteamUserStatsTestPg2,
		SteamUtils,
		SteamVideo,

		MAX_STATES
	}

	public EGUIState m_State { get; private set; }

	private bool m_bInitialized = false;
	private bool m_bControllerInitialized = false;

	private static SteamTest m_SteamTest = null;

	private SteamAppListTest AppListTest;
	private SteamAppsTest AppsTest;
	private SteamClientTest ClientTest;
	private SteamControllerTest ControllerTest;
	private SteamFriendsTest FriendsTest;
	private SteamHTMLSurfaceTest HTMLSurfaceTest;
	private SteamHTTPTest HTTPTest;
	private SteamInventoryTest InventoryTest;
	private SteamMatchmakingTest MatchmakingTest;
	private SteamMatchmakingServersTest MatchmakingServersTest;
	private SteamMusicTest MusicTest;
	private SteamMusicRemoteTest MusicRemoteTest;
	private SteamNetworkingTest NetworkingTest;
	private SteamRemoteStorageTest RemoteStorageTest;
	private SteamScreenshotsTest ScreenshotsTest;
	private SteamUGCTest UGCTest;
	private SteamUnifiedMessagesTest UnifiedMessagesTest;
	private SteamUserTest UserTest;
	private SteamUserStatsTest UserStatsTest;
	private SteamUtilsTest UtilsTest;
	private SteamVideoTest VideoTest;

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
		m_SteamTest = this;

		// We want our Steam Instance to persist across scenes.
		DontDestroyOnLoad(gameObject);

		if (!Packsize.Test()) {
			throw new System.Exception("Packsize is wrong! You are likely using a Linux/OSX build on Windows or vice versa.");
		}

		if (!DllCheck.Test()) {
			throw new System.Exception("DllCheck returned false.");
		}
		
		try {
			m_bInitialized = SteamAPI.Init();
		}
		catch (System.DllNotFoundException e) { // We catch this exception here, as it will be the first occurence of it.
			Debug.LogError("[Steamworks] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e, this);

			Application.Quit();
			return;
		}

		if (!m_bInitialized) {
			Debug.LogError("SteamAPI_Init() failed", this);
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
		AppListTest = gameObject.AddComponent<SteamAppListTest>();
		AppsTest = gameObject.AddComponent<SteamAppsTest>();
		ClientTest = gameObject.AddComponent<SteamClientTest>();
		ControllerTest = gameObject.AddComponent<SteamControllerTest>();
		FriendsTest = gameObject.AddComponent<SteamFriendsTest>();
		HTMLSurfaceTest = gameObject.AddComponent<SteamHTMLSurfaceTest>();
		HTTPTest = gameObject.AddComponent<SteamHTTPTest>();
		InventoryTest = gameObject.AddComponent<SteamInventoryTest>();
		MatchmakingTest = gameObject.AddComponent<SteamMatchmakingTest>();
		MatchmakingServersTest = gameObject.AddComponent<SteamMatchmakingServersTest>();
		MusicTest = gameObject.AddComponent<SteamMusicTest>();
		MusicRemoteTest = gameObject.AddComponent<SteamMusicRemoteTest>();
		NetworkingTest = gameObject.AddComponent<SteamNetworkingTest>();
		RemoteStorageTest = gameObject.AddComponent<SteamRemoteStorageTest>();
		ScreenshotsTest = gameObject.AddComponent<SteamScreenshotsTest>();
		UGCTest = gameObject.AddComponent<SteamUGCTest>();
		UnifiedMessagesTest = gameObject.AddComponent<SteamUnifiedMessagesTest>();
		UserTest = gameObject.AddComponent<SteamUserTest>();
		UserStatsTest = gameObject.AddComponent<SteamUserStatsTest>();
		UtilsTest = gameObject.AddComponent<SteamUtilsTest>();
		VideoTest = gameObject.AddComponent<SteamVideoTest>();
	}

	void OnEnable() {
		// This should only get called after an Assembly reload, You should never disable the Steamworks Manager yourself.
		if (m_SteamTest == null) {
			m_SteamTest = this;
		}

		if (!m_bInitialized) {
			return;
		}

		if (SteamAPIWarningMessageHook == null) {
			SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);
		}
	}

	void OnDestroy() {
		if (!m_bInitialized) {
			return;
		}

		if (m_bControllerInitialized) {
			bool ret = SteamController.Shutdown();
			if (!ret) {
				Debug.LogWarning("SteamController.Shutdown() returned false");
			}
		}

		SteamAPI.Shutdown();
	}

	void Update() {
		if (!m_bInitialized) {
			return;
		}

		SteamAPI.RunCallbacks();

		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)) {
			++m_State;
			if (m_State == EGUIState.MAX_STATES)
				m_State = (EGUIState)0;
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			--m_State;
			if (m_State == (EGUIState)(-1))
				m_State = EGUIState.MAX_STATES - 1;
		}
	}

	void OnGUI() {
		if (!m_bInitialized) {
			GUILayout.Label("Steamworks is not Initialized");
			return;
		}

		GUILayout.Label("[" + ((int)m_State + 1) + " / " + (int)EGUIState.MAX_STATES + "] " + m_State.ToString());

		switch (m_State) {
			case EGUIState.SteamAppList:
				AppListTest.RenderOnGUI();
				break;
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
			case EGUIState.SteamHTMLSurface:
				HTMLSurfaceTest.RenderOnGUI();
				break;
			case EGUIState.SteamHTTP:
				HTTPTest.RenderOnGUI();
				break;
			case EGUIState.SteamInventory:
				InventoryTest.RenderOnGUI();
				break;
			case EGUIState.SteamMatchmaking:
				MatchmakingTest.RenderOnGUI();
				break;
			case EGUIState.SteamMatchmakingServers:
				MatchmakingServersTest.RenderOnGUI();
				break;
			case EGUIState.SteamMusic:
				MusicTest.RenderOnGUI();
				break;
			case EGUIState.SteamMusicRemote:
				MusicRemoteTest.RenderOnGUI();
				break;
			case EGUIState.SteamNetworking:
				NetworkingTest.RenderOnGUI();
				break;
			case EGUIState.SteamRemoteStorage:
			case EGUIState.SteamRemoteStoragePg2:
				RemoteStorageTest.RenderOnGUI(m_State);
				break;
			case EGUIState.SteamScreenshots:
				ScreenshotsTest.RenderOnGUI();
				break;
			case EGUIState.SteamUGC:
				UGCTest.RenderOnGUI();
				break;
			case EGUIState.SteamUnifiedMessages:
				UnifiedMessagesTest.RenderOnGUI();
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
			case EGUIState.SteamVideo:
				VideoTest.RenderOnGUI();
				break;
		}
	}
}
