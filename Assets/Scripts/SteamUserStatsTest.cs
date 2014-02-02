using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUserStatsTest : MonoBehaviour {
	private int m_NumGamesStat;
	private float m_FeetTraveledStat;
	private bool m_AchievedWinOneGame;
	private SteamLeaderboard_t m_SteamLeaderboard;
	private SteamLeaderboardEntries_t m_SteamLeaderboardEntries;
	private Texture2D m_Icon;

	private CallResult<UserStatsReceived_t> UserStatsReceived;
	private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;
	private CallResult<LeaderboardScoresDownloaded_t> LeaderboardScoresDownloaded;
	private CallResult<LeaderboardScoreUploaded_t> LeaderboardScoreUploaded;
	private CallResult<LeaderboardUGCSet_t> LeaderboardUGCSet;
	private CallResult<NumberOfCurrentPlayers_t> NumberOfCurrentPlayers;
	private CallResult<GlobalAchievementPercentagesReady_t> GlobalAchievementPercentagesReady;
	private CallResult<GlobalStatsReceived_t> GlobalStatsReceived;

	public void OnEnable() {
		UserStatsReceived = new CallResult<UserStatsReceived_t>(OnUserStatsReceived);
		new Callback<UserStatsStored_t>(OnUserStatsStored);
		new Callback<UserAchievementStored_t>(OnUserAchievementStored);
		LeaderboardFindResult = new CallResult<LeaderboardFindResult_t>(OnLeaderboardFindResult);
		LeaderboardScoresDownloaded = new CallResult<LeaderboardScoresDownloaded_t>(OnLeaderboardScoresDownloaded);
		LeaderboardScoreUploaded = new CallResult<LeaderboardScoreUploaded_t>(OnLeaderboardScoreUploaded);
		NumberOfCurrentPlayers = new CallResult<NumberOfCurrentPlayers_t>(OnNumberOfCurrentPlayers);
		new Callback<UserStatsUnloaded_t>(OnUserStatsUnloaded);
		new Callback<UserAchievementIconFetched_t>(OnUserAchievementIconFetched);
		GlobalAchievementPercentagesReady = new CallResult<GlobalAchievementPercentagesReady_t>(OnGlobalAchievementPercentagesReady);
		LeaderboardUGCSet = new CallResult<LeaderboardUGCSet_t>(OnLeaderboardUGCSet);
#if _PS3
		new Callback<PS3TrophiesInstalled_t>(OnPS3TrophiesInstalled);
#endif
		GlobalStatsReceived = new CallResult<GlobalStatsReceived_t>(OnGlobalStatsReceived);
	}

	public void RenderOnGUI(SteamTest.EGUIState state) {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_NumGamesStat: " + m_NumGamesStat);
		GUILayout.Label("m_FeetTraveledStat: " + m_FeetTraveledStat);
		GUILayout.Label("m_AchievedWinOneGame: " + m_AchievedWinOneGame);
		GUILayout.Label("m_SteamLeaderboard: " + m_SteamLeaderboard);
		GUILayout.Label("m_SteamLeaderboardEntries: " + m_SteamLeaderboardEntries);
		GUILayout.Label("m_Icon:");
		GUILayout.Label(m_Icon);
		GUILayout.EndArea();

		if (state == SteamTest.EGUIState.SteamUserStatsTest) {
			RenderPageOne();
		}
		else {
			RenderPageTwo();
		}
	}

	private void RenderPageOne() {
		if (GUILayout.Button("RequestCurrentStats()")) {
			bool ret = SteamUserStats.RequestCurrentStats();
			print("RequestCurrentStats() - " + ret);
		}

		{
			bool ret = SteamUserStats.GetStat("NumGames", out m_NumGamesStat);
			GUILayout.Label("GetStat(\"NumGames\", out m_NumGamesStat) - " + ret + " -- " + m_NumGamesStat);
		}

		{
			bool ret = SteamUserStats.GetStat("FeetTraveled", out m_FeetTraveledStat);
			GUILayout.Label("GetStat(\"FeetTraveled\", out m_FeetTraveledStat) - " + ret + " -- " + m_FeetTraveledStat);
		}

		if (GUILayout.Button("SetStat(\"NumGames\", m_NumGamesStat + 1)")) {
			bool ret = SteamUserStats.SetStat("NumGames", m_NumGamesStat + 1);
			print("SetStat(\"NumGames\", " + (m_NumGamesStat + 1) + ") - " + ret);
		}

		if (GUILayout.Button("SetStat(\"FeetTraveled\", m_FeetTraveledStat + 1)")) {
			bool ret = SteamUserStats.SetStat("FeetTraveled", m_FeetTraveledStat + 1);
			print("SetStat(\"FeetTraveled\", " + (m_FeetTraveledStat + 1) + ") - " + ret);
		}

		if (GUILayout.Button("UpdateAvgRateStat(\"AverageSpeed\", 100, 60.0)")) {
			bool ret = SteamUserStats.UpdateAvgRateStat("AverageSpeed", 100, 60.0);
			print("UpdateAvgRateStat(\"AverageSpeed\", 100, 60.0) - " + ret);
		}

		{
			bool ret = SteamUserStats.GetAchievement("ACH_WIN_ONE_GAME", out m_AchievedWinOneGame);
			GUILayout.Label("GetAchievement(\"ACH_WIN_ONE_GAME\", out m_AchievedWinOneGame) - " + ret + " -- " + m_AchievedWinOneGame);
		}

		if (GUILayout.Button("SetAchievement(\"ACH_WIN_ONE_GAME\")")) {
			bool ret = SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
			print("SetAchievement(\"ACH_WIN_ONE_GAME\") - " + ret);
		}

		if (GUILayout.Button("ClearAchievement(\"ACH_WIN_ONE_GAME\")")) {
			bool ret = SteamUserStats.ClearAchievement("ACH_WIN_ONE_GAME");
			print("ClearAchievement(\"ACH_WIN_ONE_GAME\") - " + ret);
		}

		{
			bool Achieved;
			uint UnlockTime;
			bool ret = SteamUserStats.GetAchievementAndUnlockTime("ACH_WIN_ONE_GAME", out Achieved, out UnlockTime);
			GUILayout.Label("GetAchievementAndUnlockTime(\"ACH_WIN_ONE_GAME\", out Achieved, out UnlockTime) - " + ret + " -- " + Achieved + " -- " + UnlockTime);
		}

		if (GUILayout.Button("StoreStats()")) {
			bool ret = SteamUserStats.StoreStats();
			print("StoreStats() - " + ret);
		}

		if (GUILayout.Button("GetAchievementIcon(\"ACH_WIN_ONE_GAME\")")) {
			int icon = SteamUserStats.GetAchievementIcon("ACH_WIN_ONE_GAME");
			print("SteamUserStats.GetAchievementIcon(\"ACH_WIN_ONE_GAME\") - " + icon);

			if (icon != 0) {
				uint Width = 0;
				uint Height = 0;
				bool ret = SteamUtils.GetImageSize(icon, out Width, out Height);

				if (ret && Width > 0 && Height > 0) {
					byte[] RGBA = new byte[Width * Height * 4];
					ret = SteamUtils.GetImageRGBA(icon, RGBA, RGBA.Length);
					if (ret) {
						m_Icon = new Texture2D((int)Width, (int)Height, TextureFormat.RGBA32, false, true);
						m_Icon.LoadRawTextureData(RGBA);
						m_Icon.Apply();
					}
				}
			}
		}

		GUILayout.Label("GetAchievementDisplayAttribute(\"ACH_WIN_ONE_GAME\", \"name\") : " + SteamUserStats.GetAchievementDisplayAttribute("ACH_WIN_ONE_GAME", "name"));

		if (GUILayout.Button("IndicateAchievementProgress(\"ACH_WIN_100_GAMES\", 10, 100)")) {
			bool ret = SteamUserStats.IndicateAchievementProgress("ACH_WIN_100_GAMES", 10, 100);
			print("IndicateAchievementProgress(\"ACH_WIN_100_GAMES\", 10, 100) - " + ret);
		}

		GUILayout.Label("GetNumAchievements() : " + SteamUserStats.GetNumAchievements());
		GUILayout.Label("GetAchievementName(0) : " + SteamUserStats.GetAchievementName(0));

		if (GUILayout.Button("RequestUserStats(SteamUser.GetSteamID())")) {
			SteamAPICall_t handle = SteamUserStats.RequestUserStats(new CSteamID(76561197991230424)); //rlabrecque
			UserStatsReceived.Set(handle);
			print("RequestUserStats(" + SteamUser.GetSteamID() + ") - " + handle);
		}

		{
			int Data;
			bool ret = SteamUserStats.GetUserStat(new CSteamID(76561197991230424), "NumWins", out Data); //rlabrecque
			GUILayout.Label("GetUserStat(SteamUser.GetSteamID(), \"NumWins\", out Data) : " + ret + " -- " + Data);
		}

		{
			float Data;
			bool ret = SteamUserStats.GetUserStat(new CSteamID(76561197991230424), "MaxFeetTraveled", out Data); //rlabrecque
			GUILayout.Label("GetUserStat(SteamUser.GetSteamID(), \"NumWins\", out Data) : " + ret + " -- " + Data);
		}

		{
			bool Achieved;
			bool ret = SteamUserStats.GetUserAchievement(new CSteamID(76561197991230424), "ACH_TRAVEL_FAR_ACCUM", out Achieved); //rlabrecque
			GUILayout.Label("GetUserAchievement(SteamUser.GetSteamID(), \"ACH_TRAVEL_FAR_ACCUM\", out Achieved) : " + ret + " -- " + Achieved);
		}

		{
			bool Achieved;
			uint UnlockTime;
			bool ret = SteamUserStats.GetUserAchievementAndUnlockTime(new CSteamID(76561197991230424), "ACH_WIN_ONE_GAME", out Achieved, out UnlockTime); //rlabrecque
			GUILayout.Label("GetUserAchievementAndUnlockTime(SteamUser.GetSteamID(), ACH_TRAVEL_FAR_SINGLE\", out Achieved, out UnlockTime) : " + ret + " -- " + Achieved + " -- " + UnlockTime);
		}

		if (GUILayout.Button("ResetAllStats(true)")) {
			bool ret = SteamUserStats.ResetAllStats(true);
			print("ResetAllStats(true) - " + ret);
		}
	}

	private void RenderPageTwo() {
		if (GUILayout.Button("FindOrCreateLeaderboard(\"Feet Traveled\", k_ELeaderboardSortMethodAscending, k_ELeaderboardDisplayTypeNumeric)")) {
			SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard("Feet Traveled", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			LeaderboardFindResult.Set(handle);
			print("FindOrCreateLeaderboard(\"Feet Traveled\", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric) - " + handle);
		}

		if (GUILayout.Button("FindLeaderboard(\"Feet Traveled\")")) {
			SteamAPICall_t handle = SteamUserStats.FindLeaderboard("Feet Traveled");
			LeaderboardFindResult.Set(handle);
			print("FindLeaderboard(\"Feet Traveled\") - " + handle);
		}

		GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardName(m_SteamLeaderboard));
		GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardEntryCount(m_SteamLeaderboard));
		GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardSortMethod(m_SteamLeaderboard));
		GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardDisplayType(m_SteamLeaderboard));

		if (GUILayout.Button("DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5)")) {
			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5);
			LeaderboardScoresDownloaded.Set(handle);
			print("DownloadLeaderboardEntries(" + m_SteamLeaderboard + ", ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5) - " + handle);
		}

		if (GUILayout.Button("DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, Users, Users.Length)")) {
			CSteamID[] Users = { SteamUser.GetSteamID() };
			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, Users, Users.Length);
			LeaderboardScoresDownloaded.Set(handle);
			print("DownloadLeaderboardEntriesForUsers(" + m_SteamLeaderboard + ", Users, Users.Length) - " + handle);
		}

		if (GUILayout.Button("GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out LeaderboardEntry, out Details, 0)")) {
			LeaderboardEntry_t LeaderboardEntry;
			int Details;
			bool ret = SteamUserStats.GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out LeaderboardEntry, out Details, 0);
			print("GetDownloadedLeaderboardEntry(" + m_SteamLeaderboardEntries + ", 0, out LeaderboardEntry, out Details, 0) - " + ret + " -- " + LeaderboardEntry.m_steamIDUser + " -- " + LeaderboardEntry.m_nGlobalRank + " -- " + LeaderboardEntry.m_nScore + " -- " + LeaderboardEntry.m_cDetails + " -- " + LeaderboardEntry.m_hUGC);
		}

		if (GUILayout.Button("UploadLeaderboardScore(m_SteamLeaderboard, k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, ScoreDetails, 0)")) {
			int[] ScoreDetails = new int[1];
			SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, ScoreDetails, 0);
			LeaderboardScoreUploaded.Set(handle);
			print("UploadLeaderboardScore(" + m_SteamLeaderboard + ", ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, " + (int)m_FeetTraveledStat + ", ScoreDetails, 0) - " + handle);
		}

		if (GUILayout.Button("SteamUserStats.AttachLeaderboardUGC(m_SteamLeaderboard, RemoteStorageTest.m_UGCHandle)")) {
			SteamAPICall_t handle = SteamUserStats.AttachLeaderboardUGC(m_SteamLeaderboard, UGCHandle_t.Invalid);
			LeaderboardUGCSet.Set(handle);
			print("SteamUserStats.AttachLeaderboardUGC(" + m_SteamLeaderboard + ", " + UGCHandle_t.Invalid + ") - " + handle);
		}

		if (GUILayout.Button("GetNumberOfCurrentPlayers()")) {
			SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
			NumberOfCurrentPlayers.Set(handle);
			print("GetNumberOfCurrentPlayers() - " + handle);
		}

		if (GUILayout.Button("RequestGlobalAchievementPercentages()")) {
			SteamAPICall_t handle = SteamUserStats.RequestGlobalAchievementPercentages();
			GlobalAchievementPercentagesReady.Set(handle);
			print("RequestGlobalAchievementPercentages() - " + handle);
		}

		{
			int Iterator;

			{
				string Name;
				float Percent;
				bool Achieved;
				Iterator = SteamUserStats.GetMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved);
				if (Iterator != -1) {
					GUILayout.Label("GetMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + Iterator + " -- " + Name + " -- " + Percent + " -- " + Achieved);
				}
				else {
					GUILayout.Label("GetMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + Iterator);
				}
			}

			if (Iterator != -1) {
				string Name;
				float Percent;
				bool Achieved;
				Iterator = SteamUserStats.GetNextMostAchievedAchievementInfo(Iterator, out Name, 120, out Percent, out Achieved);
				GUILayout.Label("GetNextMostAchievedAchievementInfo(out Name, 120, out Percent, out Achieved) : " + Iterator + " -- " + Name + " -- " + Percent + " -- " + Achieved);
			}
		}

		{
			float Percent;
			bool ret = SteamUserStats.GetAchievementAchievedPercent("ACH_WIN_100_GAMES", out Percent);
			GUILayout.Label("GetAchievementAchievedPercent(\"ACH_WIN_100_GAMES\", out Percent) : " + ret + " -- " + Percent);
		}

		if (GUILayout.Button("SteamUserStats.RequestGlobalStats(3)")) {
			SteamAPICall_t handle = SteamUserStats.RequestGlobalStats(3);
			GlobalStatsReceived.Set(handle);
			print("SteamUserStats.RequestGlobalStats(3) - " + handle);
		}

		
		// TODO - Does SpaceWar have a stat marked as "aggregated"?
		{
			long Data;
			bool ret = SteamUserStats.GetGlobalStat("", out Data);
			GUILayout.Label("GetGlobalStat(\"\", out Data) : " + ret + " -- " + Data);
		}

		{
			double Data;
			bool ret = SteamUserStats.GetGlobalStat("", out Data);
			GUILayout.Label("GetGlobalStat(\"\", out Data) : " + ret + " -- " + Data);
		}

		{
			long[] Data = new long[1];
			int ret = SteamUserStats.GetGlobalStatHistory("", Data, (uint)Data.Length);
			if (ret != 0) {
				GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)Data.Length + ") : " + ret + " -- " + Data[0]);
			}
			else {
				GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)Data.Length + ") : " + ret + " -- ");
			}
		}

		{
			double[] Data = new double[1];
			int ret = SteamUserStats.GetGlobalStatHistory("", Data, (uint)Data.Length);
			if (ret != 0) {
				GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)Data.Length + ") : " + ret + " -- " + Data[0]);
			}
			else {
				GUILayout.Label("GetGlobalStatHistory(\"\", Data, " + (uint)Data.Length + ") : " + ret + " -- ");
			}
		}
		

#if _PS3
		if (GUILayout.Button("InstallPS3Trophies()")) {
			bool ret = SteamUserStats.InstallPS3Trophies();
			print("InstallPS3Trophies() - " + ret);
		}

		if (GUILayout.Button("GetTrophySpaceRequiredBeforeInstall()")) {
			ulong ret = SteamUserStats.GetTrophySpaceRequiredBeforeInstall();
			print("GetTrophySpaceRequiredBeforeInstall() - " + ret);
		}

		if (GUILayout.Button("SetUserStatsData(System.IntPtr.Zero, 0)")) {
			bool ret = SteamUserStats.SetUserStatsData(System.IntPtr.Zero, 0);
			print(" - " + ret);
		}

		if (GUILayout.Button("")) {
			uint Written;
			bool ret = SteamUserStats.GetUserStatsData(System.IntPtr.Zero, 0, out Written);
			print("GetUserStatsData(System.IntPtr.Zero, 0, out Written) - " + ret + " -- " + Written);
		}
#endif
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback, bool bIOFailure) {
		Debug.Log("[" + UserStatsStored_t.k_iCallback + " - UserStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_steamIDUser);
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback) {
		Debug.Log("[" + UserStatsStored_t.k_iCallback + " - UserStatsStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	private void OnUserAchievementStored(UserAchievementStored_t pCallback) {
		Debug.Log("[" + UserAchievementStored_t.k_iCallback + " - UserAchievementStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_bGroupAchievement + " -- " + pCallback.m_rgchAchievementName + " -- " + pCallback.m_nCurProgress + " -- " + pCallback.m_nMaxProgress);
	}

	private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardFindResult_t.k_iCallback + " - LeaderboardFindResult] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_bLeaderboardFound);

		if (pCallback.m_bLeaderboardFound != 0) {
			m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
		}
	}

	private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardScoresDownloaded_t.k_iCallback + " - LeaderboardScoresDownloaded] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_hSteamLeaderboardEntries + " -- " + pCallback.m_cEntryCount);
		m_SteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;
	}

	private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardScoreUploaded_t.k_iCallback + " - LeaderboardScoreUploaded] - " + pCallback.m_bSuccess + " -- " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_nScore + " -- " + pCallback.m_bScoreChanged + " -- " + pCallback.m_nGlobalRankNew + " -- " + pCallback.m_nGlobalRankPrevious);
	}

	private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure) {
		Debug.Log("[" + NumberOfCurrentPlayers_t.k_iCallback + " - NumberOfCurrentPlayers] - " + pCallback.m_bSuccess + " -- " + pCallback.m_cPlayers);
	}

	private void OnUserStatsUnloaded(UserStatsUnloaded_t pCallback) {
		Debug.Log("[" + UserStatsUnloaded_t.k_iCallback + " - UserStatsUnloaded] - " + pCallback.m_steamIDUser);
	}

	private void OnUserAchievementIconFetched(UserAchievementIconFetched_t pCallback) {
		Debug.Log("[" + UserAchievementIconFetched_t.k_iCallback + " - UserAchievementIconFetched_t] - " + pCallback.m_nGameID + " -- " + pCallback.m_rgchAchievementName + " -- " + pCallback.m_bAchieved + " -- " + pCallback.m_nIconHandle);
	}

	private void OnGlobalAchievementPercentagesReady(GlobalAchievementPercentagesReady_t pCallback, bool bIOFailure) {
		Debug.Log("[" + GlobalAchievementPercentagesReady_t.k_iCallback + " - GlobalAchievementPercentagesReady] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	private void OnLeaderboardUGCSet(LeaderboardUGCSet_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardUGCSet_t.k_iCallback + " - LeaderboardUGCSet] - " + pCallback.m_eResult + " -- " + pCallback.m_hSteamLeaderboard);
	}

	private void OnPS3TrophiesInstalled(PS3TrophiesInstalled_t pCallback) {
		Debug.Log("[" + PS3TrophiesInstalled_t.k_iCallback + " - PS3TrophiesInstalled] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_ulRequiredDiskSpace);
	}

	private void OnGlobalStatsReceived(GlobalStatsReceived_t pCallback, bool bIOFailure) {
		Debug.Log("[" + GlobalStatsReceived_t.k_iCallback + " - GlobalStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}
}
