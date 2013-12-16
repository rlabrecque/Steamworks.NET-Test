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
				GUISteamFriends();
				break;
			case EGUIState.SteamUser:
				GUISteamUser();
				break;
			case EGUIState.SteamUtils:
				GUISteamUtils();
				break;
		}
	}

	void GUISteamFriends() {
		GUILayout.Label("SteamFriends.GetPersonaName : " + SteamFriends.GetPersonaName());
		//GUILayout.Label("SteamFriends.SetPersonaName : " + SteamFriends.SetPersonaName()); // Button
		GUILayout.Label("SteamFriends.GetPersonaState : " + SteamFriends.GetPersonaState());
		GUILayout.Label("SteamFriends.GetFriendCount : " + SteamFriends.GetFriendCount((int)EFriendFlags.k_EFriendFlagImmediate));
		GUILayout.Label("SteamFriends.GetFriendByIndex : " + SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate)); // Todo: Loop this
		GUILayout.Label("SteamFriends.GetFriendRelationship : " + SteamFriends.GetFriendRelationship(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate)));
		GUILayout.Label("SteamFriends.GetFriendPersonaState : " + SteamFriends.GetFriendPersonaState(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate)));
		GUILayout.Label("SteamFriends.GetFriendPersonaName : " + SteamFriends.GetFriendPersonaName(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate)));
		//GUILayout.Label("SteamFriends.GetFriendGamePlayed : " + SteamFriends.GetFriendGamePlayed()); // Todo
		GUILayout.Label("SteamFriends.GetFriendPersonaNameHistory : " + SteamFriends.GetFriendPersonaNameHistory(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate), 0));
		GUILayout.Label("SteamFriends.GetPlayerNickname : " + SteamFriends.GetPlayerNickname(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate)));
		//GUILayout.Label("SteamFriends.HasFriend : " + SteamFriends.HasFriend()); // N/A
		GUILayout.Label("SteamFriends.GetClanCount : " + SteamFriends.GetClanCount());
		GUILayout.Label("SteamFriends.GetClanByIndex : " + SteamFriends.GetClanByIndex(0)); // Todo: Loop this
		GUILayout.Label("SteamFriends.GetClanName : " + SteamFriends.GetClanName(SteamFriends.GetClanByIndex(0)));
		GUILayout.Label("SteamFriends.GetClanTag : " + SteamFriends.GetClanTag(SteamFriends.GetClanByIndex(0)));
		//GUILayout.Label("SteamFriends.GetClanActivityCounts : " + SteamFriends.GetClanActivityCounts()); // Todo
		//GUILayout.Label("SteamFriends.DownloadClanActivityCounts : " + SteamFriends.DownloadClanActivityCounts()); // Todo
		GUILayout.Label("SteamFriends.GetFriendCountFromSource : " + SteamFriends.GetFriendCountFromSource(SteamFriends.GetClanByIndex(0)));
		GUILayout.Label("SteamFriends.GetFriendFromSourceByIndex : " + SteamFriends.GetFriendFromSourceByIndex(SteamFriends.GetClanByIndex(0), 0));
		GUILayout.Label("SteamFriends.IsUserInSource : " + SteamFriends.IsUserInSource(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate), SteamFriends.GetClanByIndex(0)));
		//GUILayout.Label("SteamFriends.SetInGameVoiceSpeaking : " + SteamFriends.SetInGameVoiceSpeaking()); N/A
		//GUILayout.Label("SteamFriends.ActivateGameOverlay : " + SteamFriends.ActivateGameOverlay("Friends")); // Button
		//GUILayout.Label("SteamFriends.ActivateGameOverlayToUser : " + SteamFriends.ActivateGameOverlayToUser()); // Button
		//GUILayout.Label("SteamFriends.ActivateGameOverlayToWebPage : " + SteamFriends.ActivateGameOverlayToWebPage()); // Button
		//GUILayout.Label("SteamFriends.ActivateGameOverlayToStore : " + SteamFriends.ActivateGameOverlayToStore()); // Button
		//GUILayout.Label("SteamFriends.SetPlayedWith : " + SteamFriends.SetPlayedWith()); // N/A
		//GUILayout.Label("SteamFriends.ActivateGameOverlayInviteDialog : " + SteamFriends.ActivateGameOverlayInviteDialog()); // N/A
		//GUILayout.Label("SteamFriends.GetSmallFriendAvatar : " + SteamFriends.GetSmallFriendAvatar()); // Image
		//GUILayout.Label("SteamFriends.GetMediumFriendAvatar : " + SteamFriends.GetMediumFriendAvatar()); // Image
		//GUILayout.Label("SteamFriends.GetLargeFriendAvatar : " + SteamFriends.GetLargeFriendAvatar()); // Image
		GUILayout.Label("SteamFriends.RequestUserInformation : " + SteamFriends.RequestUserInformation(SteamFriends.GetFriendByIndex(0, (int)EFriendFlags.k_EFriendFlagImmediate), false));
		//GUILayout.Label("SteamFriends.RequestClanOfficerList : " + SteamFriends.RequestClanOfficerList()); Button
		GUILayout.Label("SteamFriends.GetClanOwner : " + SteamFriends.GetClanOwner(SteamFriends.GetClanByIndex(0)));
		GUILayout.Label("SteamFriends.GetClanOfficerCount : " + SteamFriends.GetClanOfficerCount(SteamFriends.GetClanByIndex(0)));
		GUILayout.Label("SteamFriends.GetClanOfficerByIndex : " + SteamFriends.GetClanOfficerByIndex(SteamFriends.GetClanByIndex(0), 0));
		GUILayout.Label("SteamFriends.GetUserRestrictions : " + SteamFriends.GetUserRestrictions());
		//GUILayout.Label("SteamFriends.SetRichPresence : " + SteamFriends.SetRichPresence()); // N/A
		//GUILayout.Label("SteamFriends.ClearRichPresence : " + SteamFriends.ClearRichPresence()); // N/A
		//GUILayout.Label("SteamFriends.GetFriendRichPresence : " + SteamFriends.GetFriendRichPresence()); // N/A
		//GUILayout.Label("SteamFriends.GetFriendRichPresenceKeyCount : " + SteamFriends.GetFriendRichPresenceKeyCount()); // N/A
		//GUILayout.Label("SteamFriends.GetFriendRichPresenceKeyByIndex : " + SteamFriends.GetFriendRichPresenceKeyByIndex()); // N/A
		//GUILayout.Label("SteamFriends.RequestFriendRichPresence : " + SteamFriends.RequestFriendRichPresence()); // N/A
		//GUILayout.Label("SteamFriends.InviteUserToGame : " + SteamFriends.InviteUserToGame()); // N/A
		GUILayout.Label("SteamFriends.GetCoplayFriendCount : " + SteamFriends.GetCoplayFriendCount());
		GUILayout.Label("SteamFriends.GetCoplayFriend : " + SteamFriends.GetCoplayFriend(0));
		GUILayout.Label("SteamFriends.GetFriendCoplayTime : " + SteamFriends.GetFriendCoplayTime(SteamFriends.GetCoplayFriend(0)));
		GUILayout.Label("SteamFriends.GetFriendCoplayGame : " + SteamFriends.GetFriendCoplayGame(SteamFriends.GetCoplayFriend(0)));
		//GUILayout.Label("SteamFriends.JoinClanChatRoom : " + SteamFriends.JoinClanChatRoom()); // N/A
		//GUILayout.Label("SteamFriends.LeaveClanChatRoom : " + SteamFriends.LeaveClanChatRoom()); //  N/A
		//GUILayout.Label("SteamFriends.GetClanChatMemberCount : " + SteamFriends.GetClanChatMemberCount()); // N/A
		//GUILayout.Label("SteamFriends.GetChatMemberByIndex : " + SteamFriends.GetChatMemberByIndex()); // N/A
		//GUILayout.Label("SteamFriends.SendClanChatMessage : " + SteamFriends.SendClanChatMessage()); // N/A
		//GUILayout.Label("SteamFriends.GetClanChatMessage : " + SteamFriends.GetClanChatMessage()); // N/A
		//GUILayout.Label("SteamFriends.IsClanChatAdmin : " + SteamFriends.IsClanChatAdmin()); // N/A
		//GUILayout.Label("SteamFriends.IsClanChatWindowOpenInSteam : " + SteamFriends.IsClanChatWindowOpenInSteam()); // N/A
		//GUILayout.Label("SteamFriends.OpenClanChatWindowInSteam : " + SteamFriends.OpenClanChatWindowInSteam()); // N/A
		//GUILayout.Label("SteamFriends.CloseClanChatWindowInSteam : " + SteamFriends.CloseClanChatWindowInSteam()); // N/A
		//GUILayout.Label("SteamFriends.SetListenForFriendsMessages : " + SteamFriends.SetListenForFriendsMessages()); // N/A
		//GUILayout.Label("SteamFriends.ReplyToFriendMessage : " + SteamFriends.ReplyToFriendMessage()); // N/A
		//GUILayout.Label("SteamFriends.GetFriendMessage : " + SteamFriends.GetFriendMessage()); // N/A
		//GUILayout.Label("SteamFriends.GetFollowerCount : " + SteamFriends.GetFollowerCount()); // N/A
		//GUILayout.Label("SteamFriends.IsFollowing : " + SteamFriends.IsFollowing()); // N/A
		//GUILayout.Label("SteamFriends.EnumerateFollowingList : " + SteamFriends.EnumerateFollowingList()); // N/A
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

	void GUISteamUtils() {
		GUILayout.Label("SteamUtils.GetSecondsSinceAppActive : " + SteamUtils.GetSecondsSinceAppActive());
		GUILayout.Label("SteamUtils.GetConnectedUniverse : " + SteamUtils.GetConnectedUniverse());
		GUILayout.Label("SteamUtils.GetServerRealTime : " + SteamUtils.GetServerRealTime());
		GUILayout.Label("SteamUtils.GetIPCountry : " + SteamUtils.GetIPCountry());
		//GUILayout.Label("SteamUtils.GetImageSize : " + SteamUtils.GetImageSize()); // ??
		//GUILayout.Label("SteamUtils.GetImageRGBA : " + SteamUtils.GetImageRGBA()); // ??
		//GUILayout.Label("SteamUtils.GetCSERIPPort : " + SteamUtils.GetCSERIPPort()); // ??
		GUILayout.Label("SteamUtils.GetCurrentBatteryPower : " + SteamUtils.GetCurrentBatteryPower());
		GUILayout.Label("SteamUtils.GetAppID : " + SteamUtils.GetAppID());
		//GUILayout.Label("SteamUtils.SetOverlayNotificationPosition : " + SteamUtils.SetOverlayNotificationPosition()); // Button
		//GUILayout.Label("SteamUtils.IsAPICallCompleted : " + SteamUtils.IsAPICallCompleted()); // N/A
		//GUILayout.Label("SteamUtils.GetAPICallFailureReason : " + SteamUtils.GetAPICallFailureReason()); // N/A
		//GUILayout.Label("SteamUtils.GetAPICallResult : " + SteamUtils.GetAPICallResult()); // N/A
		//GUILayout.Label("SteamUtils.RunFrame : " + SteamUtils.RunFrame()); // N/A
		GUILayout.Label("SteamUtils.GetIPCCallCount : " + SteamUtils.GetIPCCallCount());
		//GUILayout.Label("SteamUtils.SetWarningMessageHook : " + SteamUtils.SetWarningMessageHook()); // N/A
		GUILayout.Label("SteamUtils.IsOverlayEnabled : " + SteamUtils.IsOverlayEnabled());
		GUILayout.Label("SteamUtils.BOverlayNeedsPresent : " + SteamUtils.BOverlayNeedsPresent());
#if !_PS3
		//GUILayout.Label("SteamUtils.CheckFileSignature : " + SteamUtils.CheckFileSignature()); // Button + CallResult?
#else
		//GUILayout.Label("SteamUtils.PostPS3SysutilCallback : " + SteamUtils.PostPS3SysutilCallback());
		GUILayout.Label("SteamUtils.BIsReadyToShutdown : " + SteamUtils.BIsReadyToShutdown());
		GUILayout.Label("SteamUtils.BIsPSNOnline : " + SteamUtils.BIsPSNOnline());
		//GUILayout.Label("SteamUtils.SetPSNGameBootInviteStrings : " + SteamUtils.SetPSNGameBootInviteStrings());
#endif
		//GUILayout.Label("SteamUtils.ShowGamepadTextInput : " + SteamUtils.ShowGamepadTextInput()); // Button
		GUILayout.Label("SteamUtils.GetEnteredGamepadTextLength : " + SteamUtils.GetEnteredGamepadTextLength());
		//GUILayout.Label("SteamUtils.GetEnteredGamepadTextInput : " + SteamUtils.GetEnteredGamepadTextInput()); // Todo
		GUILayout.Label("SteamUtils.GetSteamUILanguage : " + SteamUtils.GetSteamUILanguage());
	}
}
