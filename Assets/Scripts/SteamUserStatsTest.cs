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

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserStatsStored_t> m_UserStatsStored;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;
	protected Callback<UserStatsUnloaded_t> m_UserStatsUnloaded;
	protected Callback<UserAchievementIconFetched_t> m_UserAchievementIconFetched;
	//protected Callback<PS3TrophiesInstalled_t> m_PS3TrophiesInstalled;

	private CallResult<UserStatsReceived_t> OnUserStatsReceivedCallResult;
	private CallResult<LeaderboardFindResult_t> OnLeaderboardFindResultCallResult;
	private CallResult<LeaderboardScoresDownloaded_t> OnLeaderboardScoresDownloadedCallResult;
	private CallResult<LeaderboardScoreUploaded_t> OnLeaderboardScoreUploadedCallResult;
	private CallResult<NumberOfCurrentPlayers_t> OnNumberOfCurrentPlayersCallResult;
	private CallResult<GlobalAchievementPercentagesReady_t> OnGlobalAchievementPercentagesReadyCallResult;
	private CallResult<LeaderboardUGCSet_t> OnLeaderboardUGCSetCallResult;
	private CallResult<GlobalStatsReceived_t> OnGlobalStatsReceivedCallResult;

	public void OnEnable() {
		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);
		m_UserStatsUnloaded = Callback<UserStatsUnloaded_t>.Create(OnUserStatsUnloaded);
		m_UserAchievementIconFetched = Callback<UserAchievementIconFetched_t>.Create(OnUserAchievementIconFetched);
		//m_PS3TrophiesInstalled = Callback<PS3TrophiesInstalled_t>.Create(OnPS3TrophiesInstalled); // PS3 Only.

		OnUserStatsReceivedCallResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);
		OnLeaderboardFindResultCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
		OnLeaderboardScoresDownloadedCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
		OnLeaderboardScoreUploadedCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);
		OnNumberOfCurrentPlayersCallResult = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
		OnGlobalAchievementPercentagesReadyCallResult = CallResult<GlobalAchievementPercentagesReady_t>.Create(OnGlobalAchievementPercentagesReady);
		OnLeaderboardUGCSetCallResult = CallResult<LeaderboardUGCSet_t>.Create(OnLeaderboardUGCSet);
		OnGlobalStatsReceivedCallResult = CallResult<GlobalStatsReceived_t>.Create(OnGlobalStatsReceived);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_NumGamesStat: " + m_NumGamesStat);
		GUILayout.Label("m_FeetTraveledStat: " + m_FeetTraveledStat);
		GUILayout.Label("m_AchievedWinOneGame: " + m_AchievedWinOneGame);
		GUILayout.Label("m_SteamLeaderboard: " + m_SteamLeaderboard);
		GUILayout.Label("m_SteamLeaderboardEntries: " + m_SteamLeaderboardEntries);
		GUILayout.Label("m_Icon:");
		GUILayout.Label(m_Icon);
		GUILayout.EndArea();

		if (GUILayout.Button("RequestCurrentStats()")) {
			bool ret = SteamUserStats.RequestCurrentStats();
			print("SteamUserStats.RequestCurrentStats() : " + ret);
		}

		{
			bool ret = SteamUserStats.GetStat("NumGames", out m_NumGamesStat);
			GUILayout.Label("GetStat(\"NumGames\", out m_NumGamesStat) : " + ret + " -- " + m_NumGamesStat);
		}

		{
			bool ret = SteamUserStats.GetStat("FeetTraveled", out m_FeetTraveledStat);
			GUILayout.Label("GetStat(\"FeetTraveled\", out m_FeetTraveledStat) : " + ret + " -- " + m_FeetTraveledStat);
		}

		if (GUILayout.Button("SetStat(\"NumGames\", m_NumGamesStat + 1)")) {
			bool ret = SteamUserStats.SetStat("NumGames", m_NumGamesStat + 1);
			print("SteamUserStats.SetStat(" + "\"NumGames\"" + ", " + m_NumGamesStat + 1 + ") : " + ret);
		}

		if (GUILayout.Button("SetStat(\"FeetTraveled\", m_FeetTraveledStat + 1)")) {
			bool ret = SteamUserStats.SetStat("FeetTraveled", m_FeetTraveledStat + 1);
			print("SteamUserStats.SetStat(" + "\"FeetTraveled\"" + ", " + m_FeetTraveledStat + 1 + ") : " + ret);
		}

		if (GUILayout.Button("UpdateAvgRateStat(\"AverageSpeed\", 100, 60.0)")) {
			bool ret = SteamUserStats.UpdateAvgRateStat("AverageSpeed", 100, 60.0);
			print("SteamUserStats.UpdateAvgRateStat(" + "\"AverageSpeed\"" + ", " + 100 + ", " + 60.0 + ") : " + ret);
		}

		{
			bool ret = SteamUserStats.GetAchievement("ACH_WIN_ONE_GAME", out m_AchievedWinOneGame);
			GUILayout.Label("GetAchievement(\"ACH_WIN_ONE_GAME\", out m_AchievedWinOneGame) : " + ret + " -- " + m_AchievedWinOneGame);
		}

		if (GUILayout.Button("SetAchievement(\"ACH_WIN_ONE_GAME\")")) {
			bool ret = SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
			print("SteamUserStats.SetAchievement(" + "\"ACH_WIN_ONE_GAME\"" + ") : " + ret);
		}

		if (GUILayout.Button("ClearAchievement(\"ACH_WIN_ONE_GAME\")")) {
			bool ret = SteamUserStats.ClearAchievement("ACH_WIN_ONE_GAME");
			print("SteamUserStats.ClearAchievement(" + "\"ACH_WIN_ONE_GAME\"" + ") : " + ret);
		}

		{
			bool Achieved;
			uint UnlockTime;
			bool ret = SteamUserStats.GetAchievementAndUnlockTime("ACH_WIN_ONE_GAME", out Achieved, out UnlockTime);
			GUILayout.Label("GetAchievementAndUnlockTime(\"ACH_WIN_ONE_GAME\", out Achieved, out UnlockTime) : " + ret + " -- " + Achieved + " -- " + UnlockTime);
		}

		if (GUILayout.Button("StoreStats()")) {
			bool ret = SteamUserStats.StoreStats();
			print("SteamUserStats.StoreStats() : " + ret);
		}

		if (GUILayout.Button("GetAchievementIcon(\"ACH_WIN_ONE_GAME\")")) {
			int ret = SteamUserStats.GetAchievementIcon("ACH_WIN_ONE_GAME");
			print("SteamUserStats.GetAchievementIcon(" + "\"ACH_WIN_ONE_GAME\"" + ") : " + ret);
			if (ret != 0) {
				m_Icon = SteamUtilsTest.GetSteamImageAsTexture2D(ret);
			}
		}

		GUILayout.Label("GetAchievementDisplayAttribute(\"ACH_WIN_ONE_GAME\", \"name\") : " + SteamUserStats.GetAchievementDisplayAttribute("ACH_WIN_ONE_GAME", "name"));

		if (GUILayout.Button("IndicateAchievementProgress(\"ACH_WIN_100_GAMES\", 10, 100)")) {
			bool ret = SteamUserStats.IndicateAchievementProgress("ACH_WIN_100_GAMES", 10, 100);
			print("SteamUserStats.IndicateAchievementProgress(" + "\"ACH_WIN_100_GAMES\"" + ", " + 10 + ", " + 100 + ") : " + ret);
		}

		GUILayout.Label("GetNumAchievements() : " + SteamUserStats.GetNumAchievements());

		GUILayout.Label("GetAchievementName(0) : " + SteamUserStats.GetAchievementName(0));

		if (GUILayout.Button("RequestUserStats(TestConstants.Instance.k_SteamId_rlabrecque)")) {
			SteamAPICall_t handle = SteamUserStats.RequestUserStats(TestConstants.Instance.k_SteamId_rlabrecque);
			OnUserStatsReceivedCallResult.Set(handle);
			print("SteamUserStats.RequestUserStats(" + TestConstants.Instance.k_SteamId_rlabrecque + ") : " + handle);
		}

		{
			int Data;
			bool ret = SteamUserStats.GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, "NumWins", out Data);
			GUILayout.Label("GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, \"NumWins\", out Data) : " + ret + " -- " + Data);
		}

		{
			float Data;
			bool ret = SteamUserStats.GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, "MaxFeetTraveled", out Data);
			GUILayout.Label("GetUserStat(TestConstants.Instance.k_SteamId_rlabrecque, \"MaxFeetTraveled\", out Data) : " + ret + " -- " + Data);
		}

		{
			bool Achieved;
			bool ret = SteamUserStats.GetUserAchievement(TestConstants.Instance.k_SteamId_rlabrecque, "ACH_TRAVEL_FAR_ACCUM", out Achieved);
			GUILayout.Label("GetUserAchievement(TestConstants.Instance.k_SteamId_rlabrecque, \"ACH_TRAVEL_FAR_ACCUM\", out Achieved) : " + ret + " -- " + Achieved);
		}

		{
			bool Achieved;
			uint UnlockTime;
			bool ret = SteamUserStats.GetUserAchievementAndUnlockTime(TestConstants.Instance.k_SteamId_rlabrecque, "ACH_WIN_ONE_GAME", out Achieved, out UnlockTime);
			GUILayout.Label("GetUserAchievementAndUnlockTime(TestConstants.Instance.k_SteamId_rlabrecque, \"ACH_WIN_ONE_GAME\", out Achieved, out UnlockTime) : " + ret + " -- " + Achieved + " -- " + UnlockTime);
		}

		if (GUILayout.Button("ResetAllStats(true)")) {
			bool ret = SteamUserStats.ResetAllStats(true);
			print("SteamUserStats.ResetAllStats(" + true + ") : " + ret);
		}

		if (GUILayout.Button("FindOrCreateLeaderboard(\"Feet Traveled\", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric)")) {
			SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard("Feet Traveled", ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			OnLeaderboardFindResultCallResult.Set(handle);
			print("SteamUserStats.FindOrCreateLeaderboard(" + "\"Feet Traveled\"" + ", " + ELeaderboardSortMethod.k_ELeaderboardSortMethodAscending + ", " + ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric + ") : " + handle);
		}

		if (GUILayout.Button("FindLeaderboard(\"Feet Traveled\")")) {
			SteamAPICall_t handle = SteamUserStats.FindLeaderboard("Feet Traveled");
			OnLeaderboardFindResultCallResult.Set(handle);
			print("SteamUserStats.FindLeaderboard(" + "\"Feet Traveled\"" + ") : " + handle);
		}

		// Spams SteamAPI Warnings that the SteamLeaderboard does not exist.
		if (m_SteamLeaderboard != new SteamLeaderboard_t(0)) {
			GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardName(m_SteamLeaderboard));

			GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardEntryCount(m_SteamLeaderboard));

			GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardSortMethod(m_SteamLeaderboard));

			GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardDisplayType(m_SteamLeaderboard));
		}
		else {
			GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : ");
			GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : ");
		}

		if (GUILayout.Button("DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5)")) {
			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 5);
			OnLeaderboardScoresDownloadedCallResult.Set(handle);
			print("SteamUserStats.DownloadLeaderboardEntries(" + m_SteamLeaderboard + ", " + ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal + ", " + 1 + ", " + 5 + ") : " + handle);
		}

		if (GUILayout.Button("DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, Users, Users.Length)")) {
			CSteamID[] Users = { SteamUser.GetSteamID() };
			SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntriesForUsers(m_SteamLeaderboard, Users, Users.Length);
			OnLeaderboardScoresDownloadedCallResult.Set(handle);
			print("SteamUserStats.DownloadLeaderboardEntriesForUsers(" + m_SteamLeaderboard + ", " + Users + ", " + Users.Length + ") : " + handle);
		}

		if (GUILayout.Button("GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out LeaderboardEntry, null, 0)")) {
			LeaderboardEntry_t LeaderboardEntry;
			bool ret = SteamUserStats.GetDownloadedLeaderboardEntry(m_SteamLeaderboardEntries, 0, out LeaderboardEntry, null, 0);
			print("SteamUserStats.GetDownloadedLeaderboardEntry(" + m_SteamLeaderboardEntries + ", " + 0 + ", " + "out LeaderboardEntry" + ", " + null + ", " + 0 + ") : " + ret + " -- " + LeaderboardEntry);
		}

		if (GUILayout.Button("UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, null, 0)")) {
			SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, (int)m_FeetTraveledStat, null, 0);
			OnLeaderboardScoreUploadedCallResult.Set(handle);
			print("SteamUserStats.UploadLeaderboardScore(" + m_SteamLeaderboard + ", " + ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate + ", " + (int)m_FeetTraveledStat + ", " + null + ", " + 0 + ") : " + handle);
		}

		if (GUILayout.Button("AttachLeaderboardUGC(m_SteamLeaderboard, UGCHandle_t.Invalid)")) {
			SteamAPICall_t handle = SteamUserStats.AttachLeaderboardUGC(m_SteamLeaderboard, UGCHandle_t.Invalid);
			OnLeaderboardUGCSetCallResult.Set(handle);
			print("SteamUserStats.AttachLeaderboardUGC(" + m_SteamLeaderboard + ", " + UGCHandle_t.Invalid + ") : " + handle);
		}

		if (GUILayout.Button("GetNumberOfCurrentPlayers()")) {
			SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
			OnNumberOfCurrentPlayersCallResult.Set(handle);
			print("SteamUserStats.GetNumberOfCurrentPlayers() : " + handle);
		}

		if (GUILayout.Button("RequestGlobalAchievementPercentages()")) {
			SteamAPICall_t handle = SteamUserStats.RequestGlobalAchievementPercentages();
			OnGlobalAchievementPercentagesReadyCallResult.Set(handle);
			print("SteamUserStats.RequestGlobalAchievementPercentages() : " + handle);
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

		if (GUILayout.Button("RequestGlobalStats(3)")) {
			SteamAPICall_t handle = SteamUserStats.RequestGlobalStats(3);
			OnGlobalStatsReceivedCallResult.Set(handle);
			print("SteamUserStats.RequestGlobalStats(" + 3 + ") : " + handle);
		}

		/* TODO - Spams SteamAPI warnings
		 * Does SpaceWar have a stat marked as "aggregated" to try out these functions?
		{
			long Data;
			bool ret = SteamUserStats.GetGlobalStat("", out Data);
			GUILayout.Label("GetGlobalStat(\"\", out Data) : " + ret + " -- " + Data);
		}
		*/

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

		//SteamUserStats.InstallPS3Trophies() // PS3 Only.

		//SteamUserStats.GetTrophySpaceRequiredBeforeInstall() // PS3 Only.

		//SteamUserStats.SetUserStatsData() // PS3 Only.

		//SteamUserStats.GetUserStatsData() // PS3 Only.
	}

	void OnUserStatsReceived(UserStatsReceived_t pCallback) {
		Debug.Log("[" + UserStatsReceived_t.k_iCallback + " - UserStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_steamIDUser);

		// The Callback version is for the local player RequestCurrentStats(), and the CallResult version is for other players with RequestUserStats()
	}

	void OnUserStatsReceived(UserStatsReceived_t pCallback, bool bIOFailure) {
		Debug.Log("[" + UserStatsReceived_t.k_iCallback + " - UserStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_steamIDUser);

		// The Callback version is for the local player RequestCurrentStats(), and the CallResult version is for other players with RequestUserStats()
	}

	void OnUserStatsStored(UserStatsStored_t pCallback) {
		Debug.Log("[" + UserStatsStored_t.k_iCallback + " - UserStatsStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	void OnUserAchievementStored(UserAchievementStored_t pCallback) {
		Debug.Log("[" + UserAchievementStored_t.k_iCallback + " - UserAchievementStored] - " + pCallback.m_nGameID + " -- " + pCallback.m_bGroupAchievement + " -- " + pCallback.m_rgchAchievementName + " -- " + pCallback.m_nCurProgress + " -- " + pCallback.m_nMaxProgress);
	}

	void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardFindResult_t.k_iCallback + " - LeaderboardFindResult] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_bLeaderboardFound);

		if (pCallback.m_bLeaderboardFound != 0) {
			m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
		}
	}

	void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardScoresDownloaded_t.k_iCallback + " - LeaderboardScoresDownloaded] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_hSteamLeaderboardEntries + " -- " + pCallback.m_cEntryCount);

		m_SteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;
	}

	void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardScoreUploaded_t.k_iCallback + " - LeaderboardScoreUploaded] - " + pCallback.m_bSuccess + " -- " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_nScore + " -- " + pCallback.m_bScoreChanged + " -- " + pCallback.m_nGlobalRankNew + " -- " + pCallback.m_nGlobalRankPrevious);
	}

	void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure) {
		Debug.Log("[" + NumberOfCurrentPlayers_t.k_iCallback + " - NumberOfCurrentPlayers] - " + pCallback.m_bSuccess + " -- " + pCallback.m_cPlayers);
	}

	void OnUserStatsUnloaded(UserStatsUnloaded_t pCallback) {
		Debug.Log("[" + UserStatsUnloaded_t.k_iCallback + " - UserStatsUnloaded] - " + pCallback.m_steamIDUser);
	}

	void OnUserAchievementIconFetched(UserAchievementIconFetched_t pCallback) {
		Debug.Log("[" + UserAchievementIconFetched_t.k_iCallback + " - UserAchievementIconFetched] - " + pCallback.m_nGameID + " -- " + pCallback.m_rgchAchievementName + " -- " + pCallback.m_bAchieved + " -- " + pCallback.m_nIconHandle);

		m_Icon = SteamUtilsTest.GetSteamImageAsTexture2D(pCallback.m_nIconHandle);
	}

	void OnGlobalAchievementPercentagesReady(GlobalAchievementPercentagesReady_t pCallback, bool bIOFailure) {
		Debug.Log("[" + GlobalAchievementPercentagesReady_t.k_iCallback + " - GlobalAchievementPercentagesReady] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}

	void OnLeaderboardUGCSet(LeaderboardUGCSet_t pCallback, bool bIOFailure) {
		Debug.Log("[" + LeaderboardUGCSet_t.k_iCallback + " - LeaderboardUGCSet] - " + pCallback.m_eResult + " -- " + pCallback.m_hSteamLeaderboard);
	}

	//void OnPS3TrophiesInstalled(PS3TrophiesInstalled_t pCallback) {
	//	Debug.Log("[" + PS3TrophiesInstalled_t.k_iCallback + " - PS3TrophiesInstalled] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_ulRequiredDiskSpace);
	//}

	void OnGlobalStatsReceived(GlobalStatsReceived_t pCallback, bool bIOFailure) {
		Debug.Log("[" + GlobalStatsReceived_t.k_iCallback + " - GlobalStatsReceived] - " + pCallback.m_nGameID + " -- " + pCallback.m_eResult);
	}
}