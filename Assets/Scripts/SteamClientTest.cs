using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamClientTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private HSteamPipe m_Pipe;
	private HSteamUser m_GlobalUser;
	private HSteamPipe m_LocalPipe;
	private HSteamUser m_LocalUser;

	public void OnEnable() {
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Pipe: " + m_Pipe);
		GUILayout.Label("m_GlobalUser: " + m_GlobalUser);
		GUILayout.Label("m_LocalPipe: " + m_LocalPipe);
		GUILayout.Label("m_LocalUser: " + m_LocalUser);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		GUILayout.Label("DON'T TOUCH THESE IF YOU DO NOT KNOW WHAT THEY DO, YOU COULD CRASH YOUR STEAM CLIENT");

		if (GUILayout.Button("CreateSteamPipe()")) {
			m_Pipe = SteamClient.CreateSteamPipe();
			print("SteamClient.CreateSteamPipe() : " + m_Pipe);
		}

		if (GUILayout.Button("BReleaseSteamPipe(m_Pipe)")) {
			bool ret = SteamClient.BReleaseSteamPipe(m_Pipe);
			print("SteamClient.BReleaseSteamPipe(" + m_Pipe + ") : " + ret);
		}

		if (GUILayout.Button("ConnectToGlobalUser(m_Pipe)")) {
			m_GlobalUser = SteamClient.ConnectToGlobalUser(m_Pipe);
			print("SteamClient.ConnectToGlobalUser(" + m_Pipe + ") : " + m_GlobalUser);
		}

		if (GUILayout.Button("CreateLocalUser(out m_LocalPipe, EAccountType.k_EAccountTypeGameServer)")) {
			m_LocalUser = SteamClient.CreateLocalUser(out m_LocalPipe, EAccountType.k_EAccountTypeGameServer);
			print("SteamClient.CreateLocalUser(" + "out m_LocalPipe" + ", " + EAccountType.k_EAccountTypeGameServer + ") : " + m_LocalUser + " -- " + m_LocalPipe);
		}

		if (GUILayout.Button("ReleaseUser(m_LocalPipe, m_LocalUser)")) {
			SteamClient.ReleaseUser(m_LocalPipe, m_LocalUser);
			print("SteamClient.ReleaseUser(" + m_LocalPipe + ", " + m_LocalUser + ")");
		}

		if (GUILayout.Button("GetISteamUser(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSER_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamUser(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSER_INTERFACE_VERSION);
			print("SteamClient.GetISteamUser(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMUSER_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamGameServer(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVER_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamGameServer(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVER_INTERFACE_VERSION);
			print("SteamClient.GetISteamGameServer(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMGAMESERVER_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("SetLocalIPBinding(TestConstants.k_IpAdress127_0_0_1, TestConstants.k_Port27015)")) {
			SteamClient.SetLocalIPBinding(TestConstants.k_IpAdress127_0_0_1, TestConstants.k_Port27015);
			print("SteamClient.SetLocalIPBinding(" + TestConstants.k_IpAdress127_0_0_1 + ", " + TestConstants.k_Port27015 + ")");
		}

		if (GUILayout.Button("GetISteamFriends(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMFRIENDS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamFriends(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMFRIENDS_INTERFACE_VERSION);
			print("SteamClient.GetISteamFriends(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMFRIENDS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamUtils(SteamAPI.GetHSteamPipe(), Constants.STEAMUTILS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamUtils(SteamAPI.GetHSteamPipe(), Constants.STEAMUTILS_INTERFACE_VERSION);
			print("SteamClient.GetISteamUtils(" + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMUTILS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamMatchmaking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKING_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamMatchmaking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKING_INTERFACE_VERSION);
			print("SteamClient.GetISteamMatchmaking(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMMATCHMAKING_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamMatchmakingServers(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamMatchmakingServers(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION);
			print("SteamClient.GetISteamMatchmakingServers(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamGenericInterface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPTICKET_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamGenericInterface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPTICKET_INTERFACE_VERSION);
			print("SteamClient.GetISteamGenericInterface(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMAPPTICKET_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamUserStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSERSTATS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamUserStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUSERSTATS_INTERFACE_VERSION);
			print("SteamClient.GetISteamUserStats(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMUSERSTATS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamGameServerStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVERSTATS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamGameServerStats(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMGAMESERVERSTATS_INTERFACE_VERSION);
			print("SteamClient.GetISteamGameServerStats(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMGAMESERVERSTATS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamApps(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamApps(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPS_INTERFACE_VERSION);
			print("SteamClient.GetISteamApps(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMAPPS_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamNetworking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMNETWORKING_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamNetworking(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMNETWORKING_INTERFACE_VERSION);
			print("SteamClient.GetISteamNetworking(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMNETWORKING_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamRemoteStorage(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMREMOTESTORAGE_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamRemoteStorage(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMREMOTESTORAGE_INTERFACE_VERSION);
			print("SteamClient.GetISteamRemoteStorage(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMREMOTESTORAGE_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamScreenshots(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMSCREENSHOTS_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamScreenshots(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMSCREENSHOTS_INTERFACE_VERSION);
			print("SteamClient.GetISteamScreenshots(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMSCREENSHOTS_INTERFACE_VERSION + ") : " + ret);
		}

		GUILayout.Label("GetIPCCallCount() : " + SteamClient.GetIPCCallCount());

		//GUILayout.Label("SteamClient.SetWarningMessageHook : " + SteamClient.SetWarningMessageHook()); // N/A - Check out SteamTest.cs for example usage.

		if (GUILayout.Button("BShutdownIfAllPipesClosed()")) {
			bool ret = SteamClient.BShutdownIfAllPipesClosed();
			print("SteamClient.BShutdownIfAllPipesClosed() : " + ret);
		}

		if (GUILayout.Button("GetISteamHTTP(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTTP_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamHTTP(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTTP_INTERFACE_VERSION);
			print("SteamClient.GetISteamHTTP(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMHTTP_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamUnifiedMessages(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUNIFIEDMESSAGES_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamUnifiedMessages(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUNIFIEDMESSAGES_INTERFACE_VERSION);
			print("SteamClient.GetISteamUnifiedMessages(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMUNIFIEDMESSAGES_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamController(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMCONTROLLER_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamController(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMCONTROLLER_INTERFACE_VERSION);
			print("SteamClient.GetISteamController(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMCONTROLLER_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamUGC(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUGC_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamUGC(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMUGC_INTERFACE_VERSION);
			print("SteamClient.GetISteamUGC(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMUGC_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamAppList(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPLIST_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamAppList(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMAPPLIST_INTERFACE_VERSION);
			print("SteamClient.GetISteamAppList(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMAPPLIST_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamMusic(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSIC_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamMusic(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSIC_INTERFACE_VERSION);
			print("SteamClient.GetISteamMusic(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMMUSIC_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamMusicRemote(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSICREMOTE_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamMusicRemote(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMMUSICREMOTE_INTERFACE_VERSION);
			print("SteamClient.GetISteamMusicRemote(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMMUSICREMOTE_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamHTMLSurface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTMLSURFACE_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamHTMLSurface(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMHTMLSURFACE_INTERFACE_VERSION);
			print("SteamClient.GetISteamHTMLSurface(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMHTMLSURFACE_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamInventory(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMINVENTORY_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamInventory(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMINVENTORY_INTERFACE_VERSION);
			print("SteamClient.GetISteamInventory(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMINVENTORY_INTERFACE_VERSION + ") : " + ret);
		}

		if (GUILayout.Button("GetISteamVideo(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMVIDEO_INTERFACE_VERSION)")) {
			System.IntPtr ret = SteamClient.GetISteamVideo(SteamAPI.GetHSteamUser(), SteamAPI.GetHSteamPipe(), Constants.STEAMVIDEO_INTERFACE_VERSION);
			print("SteamClient.GetISteamVideo(" + SteamAPI.GetHSteamUser() + ", " + SteamAPI.GetHSteamPipe() + ", " + Constants.STEAMVIDEO_INTERFACE_VERSION + ") : " + ret);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

}