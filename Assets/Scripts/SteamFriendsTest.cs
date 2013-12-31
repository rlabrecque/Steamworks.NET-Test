using UnityEngine;
using System.Collections;
using Steamworks;

class SteamFriendsTest : MonoBehaviour {
	CSteamID m_Friend;
	CSteamID m_Clan;
	CSteamID m_CoPlayFriend;
	Texture2D m_SmallAvatar;
	Texture2D m_MediumAvatar;
	Texture2D m_LargeAvatar;

	CallResult<ClanOfficerListResponse_t> OnFriendRichPresenceCallResult;
	CallResult<JoinClanChatRoomCompletionResult_t> OnJoinClanChatRoomCompletionResultCallResult;
	CallResult<FriendsGetFollowerCount_t> OnFriendsGetFollowerCountCallResult;
	CallResult<FriendsIsFollowing_t> OnFriendsIsFollowingCallResult;
	CallResult<FriendsEnumerateFollowingList_t> OnFriendsEnumerateFollowingListCallResult;
	CallResult<SetPersonaNameResponse_t> OnSetPersonaNameResponseCallResult;

	public void OnEnable() {
		new Callback<PersonaStateChange_t>(OnPersonaStateChange);
		new Callback<GameOverlayActivated_t>(OnGameOverlayActivated);
		new Callback<GameServerChangeRequested_t>(OnGameServerChangeRequested);
		new Callback<GameLobbyJoinRequested_t>(OnGameLobbyJoinRequested);
		new Callback<AvatarImageLoaded_t>(OnAvatarImageLoaded);
		OnFriendRichPresenceCallResult = new CallResult<ClanOfficerListResponse_t>(OnClanOfficerListResponse);
		new Callback<FriendRichPresenceUpdate_t>(OnFriendRichPresenceUpdate);
		new Callback<GameRichPresenceJoinRequested_t>(OnGameRichPresenceJoinRequested);
		new Callback<GameConnectedClanChatMsg_t>(OnGameConnectedClanChatMsg);
		new Callback<GameConnectedChatJoin_t>(OnGameConnectedChatJoin);
		new Callback<GameConnectedChatLeave_t>(OnGameConnectedChatLeave);
		new Callback<DownloadClanActivityCountsResult_t>(OnDownloadClanActivityCountsResult);
		OnJoinClanChatRoomCompletionResultCallResult = new CallResult<JoinClanChatRoomCompletionResult_t>(OnJoinClanChatRoomCompletionResult);
		new Callback<GameConnectedFriendChatMsg_t>(OnGameConnectedFriendChatMsg);
		OnFriendsGetFollowerCountCallResult = new CallResult<FriendsGetFollowerCount_t>(OnFriendsGetFollowerCount);
		OnFriendsIsFollowingCallResult = new CallResult<FriendsIsFollowing_t>(OnFriendsIsFollowing);
		OnFriendsEnumerateFollowingListCallResult = new CallResult<FriendsEnumerateFollowingList_t>(OnFriendsEnumerateFollowingList);
		OnSetPersonaNameResponseCallResult = new CallResult<SetPersonaNameResponse_t>(OnSetPersonaNameResponse);
	}

	public void RenderOnGUI(SteamTest.EGUIState state) {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Friend: " + m_Friend);
		GUILayout.Label("m_Clan: " + m_Clan);
		GUILayout.Label("m_SmallAvatar:");
		GUILayout.Label(m_SmallAvatar);
		GUILayout.Label("m_MediumAvatar:");
		GUILayout.Label(m_MediumAvatar);
		GUILayout.Label("m_LargeAvatar:");
		GUILayout.Label(m_LargeAvatar);
		GUILayout.EndArea();

		if (state == SteamTest.EGUIState.SteamFriends) {
			RenderPageOne();
		}
		else {
			RenderPageTwo();
		}
	}

	private void RenderPageOne() {
		GUILayout.Label("SteamFriends.GetPersonaName() : " + SteamFriends.GetPersonaName());

		if (GUILayout.Button("SteamFriends.SetPersonaName(SteamFriends.GetPersonaName())")) {
			SteamAPICall_t handle = SteamFriends.SetPersonaName(SteamFriends.GetPersonaName());
			OnSetPersonaNameResponseCallResult.SetAPICallHandle(handle);
			print("SteamFriends.SetPersonaName(" + SteamFriends.GetPersonaName() + ") : " + handle);
		}

		GUILayout.Label("SteamFriends.GetPersonaState() : " + SteamFriends.GetPersonaState());
		GUILayout.Label("SteamFriends.GetFriendCount(k_EFriendFlagImmediate) : " + SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate));
		if (SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate) == 0) {
			Debug.LogError("You must have atleast one friend to use this Test");
			return;
		}

		m_Friend = SteamFriends.GetFriendByIndex(0, EFriendFlags.k_EFriendFlagImmediate);
		GUILayout.Label("SteamFriends.GetFriendByIndex(0, k_EFriendFlagImmediate) : " + m_Friend);
		GUILayout.Label("SteamFriends.GetFriendRelationship(m_Friend) : " + SteamFriends.GetFriendRelationship(m_Friend));
		GUILayout.Label("SteamFriends.GetFriendPersonaState(m_Friend) : " + SteamFriends.GetFriendPersonaState(m_Friend));
		GUILayout.Label("SteamFriends.GetFriendPersonaName(m_Friend) : " + SteamFriends.GetFriendPersonaName(m_Friend));

		{
			var fgi = new FriendGameInfo_t();
			bool ret = SteamFriends.GetFriendGamePlayed(m_Friend, out fgi);
			GUILayout.Label("SteamFriends.GetFriendGamePlayed(m_Friend, out fgi) : " + ret + " -- " + fgi.m_gameID + " -- " + fgi.m_unGameIP + " -- " + fgi.m_usGamePort + " -- " + fgi.m_usQueryPort + " -- " + fgi.m_steamIDLobby);
		}

		GUILayout.Label("SteamFriends.GetFriendPersonaNameHistory(m_Friend, 1) : " + SteamFriends.GetFriendPersonaNameHistory(m_Friend, 1));
		GUILayout.Label("SteamFriends.GetPlayerNickname(m_Friend) : " + SteamFriends.GetPlayerNickname(m_Friend));
		GUILayout.Label("SteamFriends.HasFriend(m_Friend, k_EFriendFlagImmediate) : " + SteamFriends.HasFriend(m_Friend, EFriendFlags.k_EFriendFlagImmediate));

		GUILayout.Label("SteamFriends.GetClanCount() : " + SteamFriends.GetClanCount());
		if (SteamFriends.GetClanCount() == 0) {
			Debug.LogError("You must have atleast one clan to use this Test");
			return;
		}

		m_Clan = SteamFriends.GetClanByIndex(0);
		GUILayout.Label("SteamFriends.GetClanByIndex(0) : " + m_Clan);
		GUILayout.Label("SteamFriends.GetClanName(m_Clan) : " + SteamFriends.GetClanName(m_Clan));
		GUILayout.Label("SteamFriends.GetClanTag(m_Clan) : " + SteamFriends.GetClanTag(m_Clan));

		{
			int Online;
			int InGame;
			int Chatting;
			bool ret = SteamFriends.GetClanActivityCounts(m_Clan, out Online, out InGame, out Chatting);
			GUILayout.Label("SteamFriends.GetClanActivityCounts(m_Clan, out Online, out InGame, out Chatting) : " + ret + " -- " + Online + " -- " + InGame + " -- " + Chatting);
		}

		if (GUILayout.Button("SteamFriends.DownloadClanActivityCounts(m_Clans, 2)")) {
			CSteamID[] Clans = { m_Clan, new CSteamID(103582791434672565) }; // m_Clan, Steam Universe

			print("SteamFriends.DownloadClanActivityCounts(" + Clans + ", 2) : " + SteamFriends.DownloadClanActivityCounts(Clans, 2));
		}

		{
			int FriendCount = SteamFriends.GetFriendCountFromSource(m_Clan);
			GUILayout.Label("SteamFriends.GetFriendCountFromSource(m_Clan) : " + FriendCount);

			if (FriendCount > 0) {
				GUILayout.Label("SteamFriends.GetFriendFromSourceByIndex(m_Clan, 0) : " + SteamFriends.GetFriendFromSourceByIndex(m_Clan, 0));
			}
		}

		GUILayout.Label("SteamFriends.IsUserInSource(m_Friend, m_Clan) : " + SteamFriends.IsUserInSource(m_Friend, m_Clan));

		if (GUILayout.Button("SteamFriends.SetInGameVoiceSpeaking(SteamUser.GetSteamID(), false)")) {
			SteamFriends.SetInGameVoiceSpeaking(SteamUser.GetSteamID(), false);
			print("SteamClient.SetInGameVoiceSpeaking(" + SteamUser.GetSteamID() + ", false);");
		}

		if (GUILayout.Button("SteamFriends.ActivateGameOverlay(\"Friends\")")) {
			SteamFriends.ActivateGameOverlay("Friends");
			print("SteamClient.ActivateGameOverlay(\"Friends\")");
		}

		if (GUILayout.Button("SteamFriends.ActivateGameOverlayToUser(\"friendadd\", 76561197991230424)")) {
			SteamFriends.ActivateGameOverlayToUser("friendadd", new CSteamID(76561197991230424)); // rlabrecque
			print("SteamClient.ActivateGameOverlay(\"friendadd\", 76561197991230424)");
		}

		if (GUILayout.Button("SteamFriends.ActivateGameOverlayToWebPage(\"http://google.com\")")) {
			SteamFriends.ActivateGameOverlayToWebPage("http://google.com");
			print("SteamClient.ActivateGameOverlay(\"http://google.com\")");
		}

		if (GUILayout.Button("SteamFriends.ActivateGameOverlayToStore(440, k_EOverlayToStoreFlag_None)")) {
			SteamFriends.ActivateGameOverlayToStore((AppId_t)440, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None); // 440 = TF2
			print("SteamClient.ActivateGameOverlay(440, k_EOverlayToStoreFlag_None)");
		}

		if (GUILayout.Button("SteamFriends.SetPlayedWith(76561197991230424)")) {
			SteamFriends.SetPlayedWith(new CSteamID(76561197991230424)); //rlabrecque
			print("SteamClient.SetPlayedWith(76561197991230424)");
		}

		if (GUILayout.Button("SteamFriends.ActivateGameOverlayInviteDialog(76561197991230424)")) {
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(76561197991230424)); //rlabrecque
			print("SteamClient.ActivateGameOverlayInviteDialog(76561197991230424)");
		}

		if (GUILayout.Button("SteamFriends.GetSmallFriendAvatar(m_Friend)")) {
			int FriendAvatar = SteamFriends.GetSmallFriendAvatar(m_Friend);
			print("SteamFriends.GetSmallFriendAvatar(" + m_Friend + ") - " + FriendAvatar);

			uint ImageWidth;
			uint ImageHeight;
			bool ret = SteamUtils.GetImageSize(FriendAvatar, out ImageWidth, out ImageHeight);

			if (ret && ImageWidth > 0 && ImageHeight > 0) {
				byte[] Image = new byte[ImageWidth * ImageHeight * 4];

				ret = SteamUtils.GetImageRGBA(FriendAvatar, Image, (int)(ImageWidth * ImageHeight * 4));

				m_SmallAvatar = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
				m_SmallAvatar.LoadRawTextureData(Image); //TODO: The image is upside down! "@ares_p: in Unity all texture data starts from "bottom" (OpenGL convention)"
				m_SmallAvatar.Apply();
			}
		}

		if (GUILayout.Button("SteamFriends.GetMediumFriendAvatar(m_Friend)")) {
			int FriendAvatar = SteamFriends.GetMediumFriendAvatar(m_Friend);
			print("SteamFriends.GetMediumFriendAvatar(" + m_Friend + ") - " + FriendAvatar);

			uint ImageWidth;
			uint ImageHeight;
			bool ret = SteamUtils.GetImageSize(FriendAvatar, out ImageWidth, out ImageHeight);

			if (ret && ImageWidth > 0 && ImageHeight > 0) {
				byte[] Image = new byte[ImageWidth * ImageHeight * 4];

				ret = SteamUtils.GetImageRGBA(FriendAvatar, Image, (int)(ImageWidth * ImageHeight * 4));
				m_MediumAvatar = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
				m_MediumAvatar.LoadRawTextureData(Image);
				m_MediumAvatar.Apply();
			}
		}

		if (GUILayout.Button("SteamFriends.GetLargeFriendAvatar(m_Friend)")) {
			int FriendAvatar = SteamFriends.GetLargeFriendAvatar(m_Friend);
			print("SteamFriends.GetLargeFriendAvatar(" + m_Friend + ") - " + FriendAvatar);

			uint ImageWidth;
			uint ImageHeight;
			bool ret = SteamUtils.GetImageSize(FriendAvatar, out ImageWidth, out ImageHeight);

			if (ret && ImageWidth > 0 && ImageHeight > 0) {
				byte[] Image = new byte[ImageWidth * ImageHeight * 4];

				ret = SteamUtils.GetImageRGBA(FriendAvatar, Image, (int)(ImageWidth * ImageHeight * 4));
				if (ret) {
					m_LargeAvatar = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
					m_LargeAvatar.LoadRawTextureData(Image);
					m_LargeAvatar.Apply();
				}
			}
		}
	}

	private void RenderPageTwo() {
		if (GUILayout.Button("SteamFriends.RequestUserInformation(m_Friend, false)")) {
			print("SteamFriends.RequestUserInformation(" + m_Friend + ", false) - " + SteamFriends.RequestUserInformation(m_Friend, false));
		}

		if (GUILayout.Button("SteamFriends.RequestClanOfficerList(m_Clan)")) {
			SteamAPICall_t handle = SteamFriends.RequestClanOfficerList(m_Clan);
			OnFriendRichPresenceCallResult.SetAPICallHandle(handle);
			print("SteamFriends.RequestClanOfficerList(" + m_Clan + ") - " + handle);
		}

		GUILayout.Label("SteamFriends.GetClanOwner(m_Clan) : " + SteamFriends.GetClanOwner(m_Clan));
		GUILayout.Label("SteamFriends.GetClanOfficerCount(m_Clan) : " + SteamFriends.GetClanOfficerCount(m_Clan));
		GUILayout.Label("SteamFriends.GetClanOfficerByIndex(m_Clan, 0) : " + SteamFriends.GetClanOfficerByIndex(m_Clan, 0));
		GUILayout.Label("SteamFriends.GetUserRestrictions() : " + SteamFriends.GetUserRestrictions());

		if (GUILayout.Button("SteamFriends.SetRichPresence(\"status\", \"Testing 1.. 2.. 3..\")")) {
			print("SteamFriends.SetRichPresence(\"status\", \"Testing 1.. 2.. 3..\") - " + SteamFriends.SetRichPresence("status", "Testing 1.. 2.. 3.."));
		}

		if (GUILayout.Button("SteamFriends.ClearRichPresence()")) {
			SteamFriends.ClearRichPresence();
			print("SteamFriends.ClearRichPresence()");
		}

		GUILayout.Label("SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), \"status\") : " + SteamFriends.GetFriendRichPresence(SteamUser.GetSteamID(), "status"));

		GUILayout.Label("SteamFriends.GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()) : " + SteamFriends.GetFriendRichPresenceKeyCount(SteamUser.GetSteamID()));
		GUILayout.Label("SteamFriends.GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0) : " + SteamFriends.GetFriendRichPresenceKeyByIndex(SteamUser.GetSteamID(), 0));

		if (GUILayout.Button("SteamFriends.RequestFriendRichPresence(m_Friend)")) {
			SteamFriends.RequestFriendRichPresence(m_Friend);
			print("SteamFriends.RequestFriendRichPresence(" + m_Friend + ")");
		}

		if (GUILayout.Button("SteamFriends.InviteUserToGame(SteamUser.GetSteamID(), \"testing\")")) {
			print("SteamFriends.RequestFriendRichPresence(" + SteamUser.GetSteamID() + ", \"testing\") - " + SteamFriends.InviteUserToGame(SteamUser.GetSteamID(), "testing"));
		}

		GUILayout.Label("SteamFriends.GetCoplayFriendCount() : " + SteamFriends.GetCoplayFriendCount());
		if (SteamFriends.GetCoplayFriendCount() == 0) {
			Debug.LogError("You must have atleast one clan to use this Test");
			return;
		}

		m_CoPlayFriend = SteamFriends.GetCoplayFriend(0);
		GUILayout.Label("SteamFriends.GetCoplayFriend(0) : " + m_CoPlayFriend);
		GUILayout.Label("SteamFriends.GetFriendCoplayTime(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayTime(m_CoPlayFriend));
		GUILayout.Label("SteamFriends.GetFriendCoplayGame(m_CoPlayFriend) : " + SteamFriends.GetFriendCoplayGame(m_CoPlayFriend));

		if (GUILayout.Button("SteamFriends.JoinClanChatRoom(m_Clan)")) {
			SteamAPICall_t handle = SteamFriends.JoinClanChatRoom(m_Clan);
			OnJoinClanChatRoomCompletionResultCallResult.SetAPICallHandle(handle);
			print("SteamFriends.JoinClanChatRoom(m_Clan) - " + handle);
		}

		if (GUILayout.Button("SteamFriends.LeaveClanChatRoom(m_Clan)")) {
			print("SteamFriends.LeaveClanChatRoom(m_Clan) - " + SteamFriends.LeaveClanChatRoom(m_Clan));
		}

		GUILayout.Label("SteamFriends.GetClanChatMemberCount(m_Clan) : " + SteamFriends.GetClanChatMemberCount(m_Clan));
		GUILayout.Label("SteamFriends.GetChatMemberByIndex(m_Clan, 0) : " + SteamFriends.GetChatMemberByIndex(m_Clan, 0));

		if (GUILayout.Button("SteamFriends.SendClanChatMessage(m_Clan, \"Test\")")) {
			print("SteamFriends.SendClanChatMessage(m_Clan, \"Test\") - " + SteamFriends.SendClanChatMessage(m_Clan, "Test"));
		}

		//GUILayout.Label("SteamFriends.GetClanChatMessage() : " + SteamFriends.GetClanChatMessage()); // N/A - Must be called from within the callback OnGameConnectedClanChatMsg

		GUILayout.Label("SteamFriends.IsClanChatAdmin(m_Clan, SteamFriends.GetChatMemberByIndex(m_Clan, 0)) : " + SteamFriends.IsClanChatAdmin(m_Clan, SteamFriends.GetChatMemberByIndex(m_Clan, 0)));
		GUILayout.Label("SteamFriends.IsClanChatWindowOpenInSteam(m_Clan) - " + SteamFriends.IsClanChatWindowOpenInSteam(m_Clan));

		if (GUILayout.Button("SteamFriends.OpenClanChatWindowInSteam(m_Clan)")) {
			print("SteamFriends.OpenClanChatWindowInSteam(" + m_Clan + ") - " + SteamFriends.OpenClanChatWindowInSteam(m_Clan));
		}

		if (GUILayout.Button("SteamFriends.CloseClanChatWindowInSteam(m_Clan)")) {
			print("SteamFriends.CloseClanChatWindowInSteam(" + m_Clan + ") - " + SteamFriends.CloseClanChatWindowInSteam(m_Clan));
		}

		if (GUILayout.Button("SteamFriends.SetListenForFriendsMessages(true)")) {
			print("SteamFriends.SetListenForFriendsMessages(true) - " + SteamFriends.SetListenForFriendsMessages(true));
		}

		if (GUILayout.Button("SteamFriends.ReplyToFriendMessage(SteamUser.GetSteamID(), \"Testing!\")")) {
			print("SteamFriends.ReplyToFriendMessage(" + SteamUser.GetSteamID() + ", \"Testing!\") - " + SteamFriends.ReplyToFriendMessage(SteamUser.GetSteamID(), "Testing!"));
		}

		//GUILayout.Label("SteamFriends.GetFriendMessage() : " + SteamFriends.GetFriendMessage()); // N/A - Must be called from within the callback OnGameConnectedFriendChatMsg

		if (GUILayout.Button("SteamFriends.GetFollowerCount(SteamUser.GetSteamID())")) {
			SteamAPICall_t handle = SteamFriends.GetFollowerCount(SteamUser.GetSteamID());
			OnFriendsGetFollowerCountCallResult.SetAPICallHandle(handle);
			print("SteamFriends.GetFollowerCount(" + SteamUser.GetSteamID() + ") - " + handle);
		}

		if (GUILayout.Button("SteamFriends.IsFollowing(m_Friend)")) {
			SteamAPICall_t handle = SteamFriends.IsFollowing(m_Friend);
			OnFriendsIsFollowingCallResult.SetAPICallHandle(handle);
			print("SteamFriends.IsFollowing(m_Friend) - " + handle);
		}

		if (GUILayout.Button("SteamFriends.EnumerateFollowingList(0)")) {
			SteamAPICall_t handle = SteamFriends.EnumerateFollowingList(0);
			OnFriendsEnumerateFollowingListCallResult.SetAPICallHandle(handle);
			print("SteamFriends.EnumerateFollowingList(0) - " + handle);
		}
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

	void OnClanOfficerListResponse(SteamAPICall_t handle, ClanOfficerListResponse_t pCallback) {
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
		int ret = SteamFriends.GetClanChatMessage(pCallback.m_steamIDClanChat, pCallback.m_iMessageID, out Text, 2048, out ChatEntryType, out Chatter);
		print(ret + " " + Chatter + ": " + Text);
	}

	void OnGameConnectedChatJoin(GameConnectedChatJoin_t pCallback) {
		Debug.Log("[" + GameConnectedChatJoin_t.k_iCallback + " - GameConnectedChatJoin] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_steamIDUser);
	}

	void OnGameConnectedChatLeave(GameConnectedChatLeave_t pCallback) {
		Debug.Log("[" + GameConnectedChatLeave_t.k_iCallback + " - GameConnectedChatLeave] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_steamIDUser + " -- " + pCallback.m_bKicked + " -- " + pCallback.m_bDropped);
	}

	void OnDownloadClanActivityCountsResult(DownloadClanActivityCountsResult_t pCallback) {
		Debug.Log("[" + DownloadClanActivityCountsResult_t.k_iCallback + " - DownloadClanActivityCountsResult] - " + pCallback.m_bSuccess);
	}

	void OnJoinClanChatRoomCompletionResult(SteamAPICall_t handle, JoinClanChatRoomCompletionResult_t pCallback) {
		Debug.Log("[" + JoinClanChatRoomCompletionResult_t.k_iCallback + " - JoinClanChatRoomCompletionResult] - " + pCallback.m_steamIDClanChat + " -- " + pCallback.m_eChatRoomEnterResponse);
	}

	void OnGameConnectedFriendChatMsg(GameConnectedFriendChatMsg_t pCallback) {
		Debug.Log("[" + GameConnectedFriendChatMsg_t.k_iCallback + " - GameConnectedFriendChatMsg] - " + pCallback.m_steamIDUser + " -- " + pCallback.m_iMessageID);

		string Text;
		EChatEntryType ChatEntryType;
		int ret = SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out Text, 2048, out ChatEntryType);
		print(ret + " " + pCallback.m_steamIDUser + ": " + Text);
	}

	void OnFriendsGetFollowerCount(SteamAPICall_t handle, FriendsGetFollowerCount_t pCallback) {
		Debug.Log("[" + FriendsGetFollowerCount_t.k_iCallback + " - FriendsGetFollowerCount] - " + pCallback.m_eResult + " -- " + pCallback.m_steamID + " -- " + pCallback.m_nCount);
	}

	void OnFriendsIsFollowing(SteamAPICall_t handle, FriendsIsFollowing_t pCallback) {
		Debug.Log("[" + FriendsIsFollowing_t.k_iCallback + " - FriendsIsFollowing] - " + pCallback.m_eResult + " -- " + pCallback.m_steamID + " -- " + pCallback.m_bIsFollowing);
	}

	void OnFriendsEnumerateFollowingList(SteamAPICall_t handle, FriendsEnumerateFollowingList_t pCallback) {
		Debug.Log("[" + FriendsEnumerateFollowingList_t.k_iCallback + " - FriendsEnumerateFollowingList] - " + pCallback.m_eResult + " -- " + pCallback.m_rgSteamID + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount);
	}

	void OnSetPersonaNameResponse(SteamAPICall_t handle, SetPersonaNameResponse_t pCallback) {
		Debug.Log("[" + SetPersonaNameResponse_t.k_iCallback + " - SetPersonaNameResponse] - " + pCallback.m_bSuccess + " -- " + pCallback.m_bLocalSuccess + " -- " + pCallback.m_result);
	}

}
