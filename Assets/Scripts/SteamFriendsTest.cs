using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamFriendsTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private CSteamID m_Friend;
	private CSteamID m_Clan;
	private CSteamID m_CoPlayFriend;
	private Texture2D m_SmallAvatar;
	private Texture2D m_MediumAvatar;
	private Texture2D m_LargeAvatar;

	protected Callback<PersonaStateChange_t> m_PersonaStateChange;
	protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
	protected Callback<GameServerChangeRequested_t> m_GameServerChangeRequested;
	protected Callback<GameLobbyJoinRequested_t> m_GameLobbyJoinRequested;
	protected Callback<AvatarImageLoaded_t> m_AvatarImageLoaded;
	protected Callback<FriendRichPresenceUpdate_t> m_FriendRichPresenceUpdate;
	protected Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested;
	protected Callback<GameConnectedClanChatMsg_t> m_GameConnectedClanChatMsg;
	protected Callback<GameConnectedChatJoin_t> m_GameConnectedChatJoin;
	protected Callback<GameConnectedChatLeave_t> m_GameConnectedChatLeave;
	protected Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFriendChatMsg;

	private CallResult<ClanOfficerListResponse_t> OnClanOfficerListResponseCallResult;
	private CallResult<DownloadClanActivityCountsResult_t> OnDownloadClanActivityCountsResultCallResult;
	private CallResult<JoinClanChatRoomCompletionResult_t> OnJoinClanChatRoomCompletionResultCallResult;
	private CallResult<FriendsGetFollowerCount_t> OnFriendsGetFollowerCountCallResult;
	private CallResult<FriendsIsFollowing_t> OnFriendsIsFollowingCallResult;
	private CallResult<FriendsEnumerateFollowingList_t> OnFriendsEnumerateFollowingListCallResult;
	private CallResult<SetPersonaNameResponse_t> OnSetPersonaNameResponseCallResult;

	public void OnEnable() {
		if (SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate) == 0) {
			Debug.LogError("You must have atleast one friend to use the SteamFriends test!");
			enabled = false;
			return;
		}

		if (SteamFriends.GetClanCount() == 0) {
			Debug.LogError("You must have atleast one clan to use the SteamFriends test!");
			enabled = false;
			return;
		}

		m_PersonaStateChange = Callback<PersonaStateChange_t>.Create(OnPersonaStateChange);
		m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
		m_GameServerChangeRequested = Callback<GameServerChangeRequested_t>.Create(OnGameServerChangeRequested);
		m_GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
		m_AvatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
		m_FriendRichPresenceUpdate = Callback<FriendRichPresenceUpdate_t>.Create(OnFriendRichPresenceUpdate);
		m_GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
		m_GameConnectedClanChatMsg = Callback<GameConnectedClanChatMsg_t>.Create(OnGameConnectedClanChatMsg);
		m_GameConnectedChatJoin = Callback<GameConnectedChatJoin_t>.Create(OnGameConnectedChatJoin);
		m_GameConnectedChatLeave = Callback<GameConnectedChatLeave_t>.Create(OnGameConnectedChatLeave);
		m_GameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnGameConnectedFriendChatMsg);

		OnClanOfficerListResponseCallResult = CallResult<ClanOfficerListResponse_t>.Create(OnClanOfficerListResponse);
		OnDownloadClanActivityCountsResultCallResult = CallResult<DownloadClanActivityCountsResult_t>.Create(OnDownloadClanActivityCountsResult);
		OnJoinClanChatRoomCompletionResultCallResult = CallResult<JoinClanChatRoomCompletionResult_t>.Create(OnJoinClanChatRoomCompletionResult);
		OnFriendsGetFollowerCountCallResult = CallResult<FriendsGetFollowerCount_t>.Create(OnFriendsGetFollowerCount);
		OnFriendsIsFollowingCallResult = CallResult<FriendsIsFollowing_t>.Create(OnFriendsIsFollowing);
		OnFriendsEnumerateFollowingListCallResult = CallResult<FriendsEnumerateFollowingList_t>.Create(OnFriendsEnumerateFollowingList);
		OnSetPersonaNameResponseCallResult = CallResult<SetPersonaNameResponse_t>.Create(OnSetPersonaNameResponse);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Friend: " + m_Friend);
		GUILayout.Label("m_Clan: " + m_Clan);
		GUILayout.Label("m_CoPlayFriend: " + m_CoPlayFriend);
		GUILayout.Label("m_SmallAvatar:");
		GUILayout.Label(m_SmallAvatar);
		GUILayout.Label("m_MediumAvatar:");
		GUILayout.Label(m_MediumAvatar);
		GUILayout.Label("m_LargeAvatar:");
		GUILayout.Label(m_LargeAvatar);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		GUILayout.Label("GetPersonaName() : " + SteamFriends.GetPersonaName());

		if (GUILayout.Button("SetPersonaName(SteamFriends.GetPersonaName())")) {
			SteamAPICall_t handle = SteamFriends.SetPersonaName(SteamFriends.GetPersonaName());
			OnSetPersonaNameResponseCallResult.Set(handle);
			print("SteamFriends.SetPersonaName(" + SteamFriends.GetPersonaName() + ") : " + handle);
		}

		GUILayout.Label("GetPersonaState() : " + SteamFriends.GetPersonaState());

		GUILayout.Label("GetFriendCount(EFriendFlags.k_EFriendFlagImmediate) : " + SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate));

		{
			m_Friend = SteamFriends.GetFriendByIndex(0, EFriendFlags.k_EFriendFlagImmediate);
			GUILayout.Label("GetFriendByIndex(0, EFriendFlags.k_EFriendFlagImmediate) : " + m_Friend);
		}

		GUILayout.Label("GetFriendRelationship(m_Friend) : " + SteamFriends.GetFriendRelationship(m_Friend));

		GUILayout.Label("GetFriendPersonaState(m_Friend) : " + SteamFriends.GetFriendPersonaState(m_Friend));

		GUILayout.Label("GetFriendPersonaName(m_Friend) : " + SteamFriends.GetFriendPersonaName(m_Friend));

		{
			var fgi = new FriendGameInfo_t();
			bool ret = SteamFriends.GetFriendGamePlayed(m_Friend, out fgi);
			GUILayout.Label("GetFriendGamePlayed(m_Friend, out fgi) : " + ret + " -- " + fgi.m_gameID + " -- " + fgi.m_unGameIP + " -- " + fgi.m_usGamePort + " -- " + fgi.m_usQueryPort + " -- " + fgi.m_steamIDLobby);
		}

		GUILayout.Label("GetFriendPersonaNameHistory(m_Friend, 1) : " + SteamFriends.GetFriendPersonaNameHistory(m_Friend, 1));

		GUILayout.Label("GetFriendSteamLevel(m_Friend) : " + SteamFriends.GetFriendSteamLevel(m_Friend));

		GUILayout.Label("GetPlayerNickname(m_Friend) : " + SteamFriends.GetPlayerNickname(m_Friend));

		{
			int FriendsGroupCount = SteamFriends.GetFriendsGroupCount();
			GUILayout.Label("GetFriendsGroupCount() : " + FriendsGroupCount);

			if (FriendsGroupCount > 0) {
				FriendsGroupID_t FriendsGroupID = SteamFriends.GetFriendsGroupIDByIndex(0);
				GUILayout.Label("SteamFriends.GetFriendsGroupIDByIndex(0) : " + FriendsGroupID);

				GUILayout.Label("GetFriendsGroupName(FriendsGroupID) : " + SteamFriends.GetFriendsGroupName(FriendsGroupID));

				int FriendsGroupMembersCount = SteamFriends.GetFriendsGroupMembersCount(FriendsGroupID);
				GUILayout.Label("GetFriendsGroupMembersCount(FriendsGroupID) : " + FriendsGroupMembersCount);

				if (FriendsGroupMembersCount > 0) {
					CSteamID[] FriendsGroupMembersList = new CSteamID[FriendsGroupMembersCount];
					SteamFriends.GetFriendsGroupMembersList(FriendsGroupID, FriendsGroupMembersList, FriendsGroupMembersCount);
					GUILayout.Label("GetFriendsGroupMembersList(FriendsGroupID, FriendsGroupMembersList, FriendsGroupMembersCount) : " + FriendsGroupMembersList[0]);
				}
			}
		}

		GUILayout.Label("HasFriend(m_Friend, EFriendFlags.k_EFriendFlagImmediate) : " + SteamFriends.HasFriend(m_Friend, EFriendFlags.k_EFriendFlagImmediate));

		GUILayout.Label("GetClanCount() : " + SteamFriends.GetClanCount());

		m_Clan = SteamFriends.GetClanByIndex(0);
		GUILayout.Label("GetClanByIndex(0) : " + m_Clan);

		GUILayout.Label("GetClanName(m_Clan) : " + SteamFriends.GetClanName(m_Clan));

		GUILayout.Label("GetClanTag(m_Clan) : " + SteamFriends.GetClanTag(m_Clan));

		{
			int Online;
			int InGame;
			int Chatting;
			bool ret = SteamFriends.GetClanActivityCounts(m_Clan, out Online, out InGame, out Chatting);
			GUILayout.Label("GetClanActivityCounts(m_Clan, out Online, out InGame, out Chatting) : " + ret + " -- " + Online + " -- " + InGame + " -- " + Chatting);
		}

		if (GUILayout.Button("DownloadClanActivityCounts(Clans, Clans.Length)")) {
			CSteamID[] Clans = { m_Clan, TestConstants.Instance.k_SteamId_Group_SteamUniverse };
			SteamAPICall_t handle = SteamFriends.DownloadClanActivityCounts(Clans, Clans.Length);
			OnDownloadClanActivityCountsResultCallResult.Set(handle); // This call never seems to produce the CallResult.
			print("SteamFriends.DownloadClanActivityCounts(" + Clans + ", " + Clans.Length + ") : " + handle);
		}

		{
			int FriendCount = SteamFriends.GetFriendCountFromSource(m_Clan);
			GUILayout.Label("GetFriendCountFromSource(m_Clan) : " + FriendCount);

			if (FriendCount > 0) {
				GUILayout.Label("GetFriendFromSourceByIndex(m_Clan, 0) : " + SteamFriends.GetFriendFromSourceByIndex(m_Clan, 0));
			}
		}

		GUILayout.Label("IsUserInSource(m_Friend, m_Clan) : " + SteamFriends.IsUserInSource(m_Friend, m_Clan));

		if (GUILayout.Button("SetInGameVoiceSpeaking(SteamUser.GetSteamID(), false)")) {
			SteamFriends.SetInGameVoiceSpeaking(SteamUser.GetSteamID(), false);
			print("SteamFriends.SetInGameVoiceSpeaking(" + SteamUser.GetSteamID() + ", " + false + ")");
		}

		if (GUILayout.Button("ActivateGameOverlay(\"Friends\")")) {
			SteamFriends.ActivateGameOverlay("Friends");
			print("SteamFriends.ActivateGameOverlay(" + "\"Friends\"" + ")");
		}

		if (GUILayout.Button("ActivateGameOverlayToUser(\"friendadd\", TestConstants.Instance.k_SteamId_rlabrecque)")) {
			SteamFriends.ActivateGameOverlayToUser("friendadd", TestConstants.Instance.k_SteamId_rlabrecque);
			print("SteamFriends.ActivateGameOverlayToUser(" + "\"friendadd\"" + ", " + TestConstants.Instance.k_SteamId_rlabrecque + ")");
		}

		if (GUILayout.Button("ActivateGameOverlayToWebPage(\"http://steamworks.github.io\")")) {
			SteamFriends.ActivateGameOverlayToWebPage("http://steamworks.github.io");
			print("SteamFriends.ActivateGameOverlayToWebPage(" + "\"http://steamworks.github.io\"" + ")");
		}

		if (GUILayout.Button("ActivateGameOverlayToStore(TestConstants.Instance.k_AppId_TeamFortress2, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None)")) {
			SteamFriends.ActivateGameOverlayToStore(TestConstants.Instance.k_AppId_TeamFortress2, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
			print("SteamFriends.ActivateGameOverlayToStore(" + TestConstants.Instance.k_AppId_TeamFortress2 + ", " + EOverlayToStoreFlag.k_EOverlayToStoreFlag_None + ")");
		}

		if (GUILayout.Button("SetPlayedWith(TestConstants.Instance.k_SteamId_rlabrecque)")) {
			SteamFriends.SetPlayedWith(TestConstants.Instance.k_SteamId_rlabrecque);
			print("SteamFriends.SetPlayedWith(" + TestConstants.Instance.k_SteamId_rlabrecque + ")");
		}

		if (GUILayout.Button("ActivateGameOverlayInviteDialog(TestConstants.Instance.k_SteamId_rlabrecque)")) {
			SteamFriends.ActivateGameOverlayInviteDialog(TestConstants.Instance.k_SteamId_rlabrecque);
			print("SteamFriends.ActivateGameOverlayInviteDialog(" + TestConstants.Instance.k_SteamId_rlabrecque + ")");
		}

		if (GUILayout.Button("GetSmallFriendAvatar(m_Friend)")) {
			int ret = SteamFriends.GetSmallFriendAvatar(m_Friend);
			print("SteamFriends.GetSmallFriendAvatar(" + m_Friend + ") : " + ret);
			m_SmallAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(ret);
		}

		if (GUILayout.Button("GetMediumFriendAvatar(m_Friend)")) {
			int ret = SteamFriends.GetMediumFriendAvatar(m_Friend);
			print("SteamFriends.GetMediumFriendAvatar(" + m_Friend + ") : " + ret);
			m_MediumAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(ret);
		}

		if (GUILayout.Button("GetLargeFriendAvatar(m_Friend)")) {
			int ret = SteamFriends.GetLargeFriendAvatar(m_Friend);
			print("SteamFriends.GetLargeFriendAvatar(" + m_Friend + ") : " + ret);
			m_LargeAvatar = SteamUtilsTest.GetSteamImageAsTexture2D(ret);
		}

		if (GUILayout.Button("RequestUserInformation(m_Friend, false)")) {
			bool ret = SteamFriends.RequestUserInformation(m_Friend, false);
			print("SteamFriends.RequestUserInformation(" + m_Friend + ", " + false + ") : " + ret);
		}

		if (GUILayout.Button("RequestClanOfficerList(m_Clan)")) {
			SteamAPICall_t handle = SteamFriends.RequestClanOfficerList(m_Clan);
			OnClanOfficerListResponseCallResult.Set(handle);
			print("SteamFriends.RequestClanOfficerList(" + m_Clan + ") : " + handle);
		}

		GUILayout.Label("GetClanOwner(m_Clan) : " + SteamFriends.GetClanOwner(m_Clan));

		GUILayout.Label("GetClanOfficerCount(m_Clan) : " + SteamFriends.GetClanOfficerCount(m_Clan));

		GUILayout.Label("GetClanOfficerByIndex(m_Clan, 0) : " + SteamFriends.GetClanOfficerByIndex(m_Clan, 0));

		GUILayout.Label("GetUserRestrictions() : " + SteamFriends.GetUserRestrictions());

		if (GUILayout.Button("SetRichPresence(\"status\", \"Testing 1.. 2.. 3..\")")) {
			bool ret = SteamFriends.SetRichPresence("status", "Testing 1.. 2.. 3..");
			print("SteamFriends.SetRichPresence(" + "\"status\"" + ", " + "\"Testing 1.. 2.. 3..\"" + ") : " + ret);
		}

		if (GUILayout.Button("ClearRichPresence()")) {
			SteamFriends.ClearRichPresence();
			print("SteamFriends.ClearRichPresence()");
		}

		GUILayout.Label("GetFriendRichPresence(SteamUser.GetSteamID(), \"status\") : " + SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), "status"));

		GUILayout.Label("GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()) : " + SteamFriends.GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()));

		GUILayout.Label("GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0) : " + SteamFriends.GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0));

		if (GUILayout.Button("RequestFriendRichPresence(m_Friend)")) {
			SteamFriends.RequestFriendRichPresence(m_Friend);
			print("SteamFriends.RequestFriendRichPresence(" + m_Friend + ")");
		}

		if (GUILayout.Button("InviteUserToGame(SteamUser.GetSteamID(), \"testing\")")) {
			bool ret = SteamFriends.InviteUserToGame(SteamUser.GetSteamID(), "testing");
			print("SteamFriends.InviteUserToGame(" + SteamUser.GetSteamID() + ", " + "\"testing\"" + ") : " + ret);
		}

		GUILayout.Label("GetCoplayFriendCount() : " + SteamFriends.GetCoplayFriendCount());

		if (GUILayout.Button("GetCoplayFriend(0)")) {
			m_CoPlayFriend = SteamFriends.GetCoplayFriend(0);
			print("SteamFriends.GetCoplayFriend(" + 0 + ") : " + m_CoPlayFriend);
		}

		GUILayout.Label("GetFriendCoplayTime(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayTime(m_CoPlayFriend));

		GUILayout.Label("GetFriendCoplayGame(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayGame(m_CoPlayFriend));

		if (GUILayout.Button("JoinClanChatRoom(m_Clan)")) {
			SteamAPICall_t handle = SteamFriends.JoinClanChatRoom(m_Clan);
			OnJoinClanChatRoomCompletionResultCallResult.Set(handle);
			print("SteamFriends.JoinClanChatRoom(" + m_Clan + ") : " + handle);
		}

		if (GUILayout.Button("LeaveClanChatRoom(m_Clan)")) {
			bool ret = SteamFriends.LeaveClanChatRoom(m_Clan);
			print("SteamFriends.LeaveClanChatRoom(" + m_Clan + ") : " + ret);
		}

		GUILayout.Label("GetClanChatMemberCount(m_Clan) : " + SteamFriends.GetClanChatMemberCount(m_Clan));

		GUILayout.Label("GetChatMemberByIndex(m_Clan, 0) : " + SteamFriends.GetChatMemberByIndex(m_Clan, 0));

		if (GUILayout.Button("SendClanChatMessage(m_Clan, \"Test\")")) {
			bool ret = SteamFriends.SendClanChatMessage(m_Clan, "Test");
			print("SteamFriends.SendClanChatMessage(" + m_Clan + ", " + "\"Test\"" + ") : " + ret);
		}

		//GUILayout.Label("SteamFriends.GetClanChatMessage() : " + SteamFriends.GetClanChatMessage()); // N/A - Must be called from within the callback OnGameConnectedClanChatMsg

		GUILayout.Label("IsClanChatAdmin(m_Clan, m_Friend) : " + SteamFriends.IsClanChatAdmin(m_Clan, m_Friend));

		GUILayout.Label("IsClanChatWindowOpenInSteam(m_Clan) : " + SteamFriends.IsClanChatWindowOpenInSteam(m_Clan));

		if (GUILayout.Button("OpenClanChatWindowInSteam(m_Clan)")) {
			bool ret = SteamFriends.OpenClanChatWindowInSteam(m_Clan);
			print("SteamFriends.OpenClanChatWindowInSteam(" + m_Clan + ") : " + ret);
		}

		if (GUILayout.Button("CloseClanChatWindowInSteam(m_Clan)")) {
			bool ret = SteamFriends.CloseClanChatWindowInSteam(m_Clan);
			print("SteamFriends.CloseClanChatWindowInSteam(" + m_Clan + ") : " + ret);
		}

		if (GUILayout.Button("SetListenForFriendsMessages(true)")) {
			bool ret = SteamFriends.SetListenForFriendsMessages(true);
			print("SteamFriends.SetListenForFriendsMessages(" + true + ") : " + ret);
		}

		if (GUILayout.Button("ReplyToFriendMessage(SteamUser.GetSteamID(), \"Testing!\")")) {
			bool ret = SteamFriends.ReplyToFriendMessage(SteamUser.GetSteamID(), "Testing!");
			print("SteamFriends.ReplyToFriendMessage(" + SteamUser.GetSteamID() + ", " + "\"Testing!\"" + ") : " + ret);
		}

		//GUILayout.Label("SteamFriends.GetFriendMessage() : " + SteamFriends.GetFriendMessage()); // N/A - Must be called from within the callback OnGameConnectedFriendChatMsg

		if (GUILayout.Button("GetFollowerCount(SteamUser.GetSteamID())")) {
			SteamAPICall_t handle = SteamFriends.GetFollowerCount(SteamUser.GetSteamID());
			OnFriendsGetFollowerCountCallResult.Set(handle);
			print("SteamFriends.GetFollowerCount(" + SteamUser.GetSteamID() + ") : " + handle);
		}

		if (GUILayout.Button("IsFollowing(m_Friend)")) {
			SteamAPICall_t handle = SteamFriends.IsFollowing(m_Friend);
			OnFriendsIsFollowingCallResult.Set(handle);
			print("SteamFriends.IsFollowing(" + m_Friend + ") : " + handle);
		}

		if (GUILayout.Button("EnumerateFollowingList(0)")) {
			SteamAPICall_t handle = SteamFriends.EnumerateFollowingList(0);
			OnFriendsEnumerateFollowingListCallResult.Set(handle);
			print("SteamFriends.EnumerateFollowingList(" + 0 + ") : " + handle);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnPersonaStateChange(PersonaStateChange_t pCallback) {
		Debug.Log("[" + PersonaStateChange_t.k_iCallback + " - PersonaStateChange] - " + pCallback.m_ulSteamID + " -- " + pCallback.m_nChangeFlags);
	}

	void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
		Debug.Log("[" + GameOverlayActivated_t.k_iCallback + " - GameOverlayActivated] - " + pCallback.m_bActive);
	}

	void OnGameServerChangeRequested(GameServerChangeRequested_t pCallback) {
		Debug.Log("[" + GameServerChangeRequested_t.k_iCallback + " - GameServerChangeRequested] - " + pCallback.m_rgchServer + " -- " + pCallback.m_rgchPassword);
	}

	void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t pCallback) {
		Debug.Log("[" + GameLobbyJoinRequested_t.k_iCallback + " - GameLobbyJoinRequested] - " + pCallback.m_steamIDLobby + " -- " + pCallback.m_steamIDFriend);
	}

	void OnAvatarImageLoaded(AvatarImageLoaded_t pCallback) {
		Debug.Log("[" + AvatarImageLoaded_t.k_iCallback + " - AvatarImageLoaded] - " + pCallback.m_steamID + " -- " + pCallback.m_iImage + " -- " + pCallback.m_iWide + " -- " + pCallback.m_iTall);
	}

	void OnClanOfficerListResponse(ClanOfficerListResponse_t pCallback, bool bIOFailure) {
		Debug.Log("[" + ClanOfficerListResponse_t.k_iCallback + " - ClanOfficerListResponse] - " + pCallback.m_steamIDClan + " -- " + pCallback.m_cOfficers + " -- " + pCallback.m_bSuccess);
	}

	void OnFriendRichPresenceUpdate(FriendRichPresenceUpdate_t pCallback) {
		Debug.Log("[" + FriendRichPresenceUpdate_t.k_iCallback + " - FriendRichPresenceUpdate] - " + pCallback.m_steamIDFriend + " -- " + pCallback.m_nAppID);
	}

	void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t pCallback) {
		Debug.Log("[" + GameRichPresenceJoinRequested_t.k_iCallback + " - GameRichPresenceJoinRequested] - " + pCallback.m_steamIDFriend + " -- " + pCallback.m_rgchConnect);
	}

	void OnGameConnectedClanChatMsg(GameConnectedClanChatMsg_t pCallback) {
		Debug.Log("[" + GameConnectedClanChatMsg_t.k_iCallback + " - GameConnectedClanChatMsg] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_steamIDUser + " -- " + pCallback.m_iMessageID);

		string Text;
		EChatEntryType ChatEntryType;
		CSteamID Chatter;
		int ret = SteamFriends.GetClanChatMessage(pCallback.m_steamIDClanChat, pCallback.m_iMessageID, out Text, 2048, out ChatEntryType, out Chatter); // Must be called from within OnGameConnectedClanChatMsg
		print(ret + " " + Chatter + ": " + Text);
	}

	void OnGameConnectedChatJoin(GameConnectedChatJoin_t pCallback) {
		Debug.Log("[" + GameConnectedChatJoin_t.k_iCallback + " - GameConnectedChatJoin] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_steamIDUser);
	}

	void OnGameConnectedChatLeave(GameConnectedChatLeave_t pCallback) {
		Debug.Log("[" + GameConnectedChatLeave_t.k_iCallback + " - GameConnectedChatLeave] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_steamIDUser + " -- " + pCallback.m_bKicked + " -- " + pCallback.m_bDropped);
	}

	void OnDownloadClanActivityCountsResult(DownloadClanActivityCountsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + DownloadClanActivityCountsResult_t.k_iCallback + " - DownloadClanActivityCountsResult] - " + pCallback.m_bSuccess);
	}

	void OnJoinClanChatRoomCompletionResult(JoinClanChatRoomCompletionResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + JoinClanChatRoomCompletionResult_t.k_iCallback + " - JoinClanChatRoomCompletionResult] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_eChatRoomEnterResponse);
	}

	void OnGameConnectedFriendChatMsg(GameConnectedFriendChatMsg_t pCallback) {
		Debug.Log("[" + GameConnectedFriendChatMsg_t.k_iCallback + " - GameConnectedFriendChatMsg] - " + pCallback.m_steamIDUser + " -- " + pCallback.m_iMessageID);

		string Text;
		EChatEntryType ChatEntryType;
		int ret = SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out Text, 2048, out ChatEntryType); // Must be called from within OnGameConnectedFriendChatMsg
		print(ret + " " + pCallback.m_steamIDUser + ": " + Text);
	}

	void OnFriendsGetFollowerCount(FriendsGetFollowerCount_t pCallback, bool bIOFailure) {
		Debug.Log("[" + FriendsGetFollowerCount_t.k_iCallback + " - FriendsGetFollowerCount] - " + pCallback.m_eResult + " -- " + pCallback.m_steamID + " -- " + pCallback.m_nCount);
	}

	void OnFriendsIsFollowing(FriendsIsFollowing_t pCallback, bool bIOFailure) {
		Debug.Log("[" + FriendsIsFollowing_t.k_iCallback + " - FriendsIsFollowing] - " + pCallback.m_eResult + " -- " + pCallback.m_steamID + " -- " + pCallback.m_bIsFollowing);
	}

	void OnFriendsEnumerateFollowingList(FriendsEnumerateFollowingList_t pCallback, bool bIOFailure) {
		Debug.Log("[" + FriendsEnumerateFollowingList_t.k_iCallback + " - FriendsEnumerateFollowingList] - " + pCallback.m_eResult + " -- " + pCallback.m_rgSteamID + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount);
	}

	void OnSetPersonaNameResponse(SetPersonaNameResponse_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SetPersonaNameResponse_t.k_iCallback + " - SetPersonaNameResponse] - " + pCallback.m_bSuccess + " -- " + pCallback.m_bLocalSuccess + " -- " + pCallback.m_result);
	}
}