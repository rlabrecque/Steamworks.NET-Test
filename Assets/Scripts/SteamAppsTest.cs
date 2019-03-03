using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamAppsTest : MonoBehaviour {
	private Vector2 m_ScrollPos;

	protected Callback<DlcInstalled_t> m_DlcInstalled;
	protected Callback<RegisterActivationCodeResponse_t> m_RegisterActivationCodeResponse;
	protected Callback<NewUrlLaunchParameters_t> m_NewUrlLaunchParameters;
	protected Callback<AppProofOfPurchaseKeyResponse_t> m_AppProofOfPurchaseKeyResponse;

	private CallResult<FileDetailsResult_t> OnFileDetailsResultCallResult;

	public void OnEnable() {
		m_DlcInstalled = Callback<DlcInstalled_t>.Create(OnDlcInstalled);
		m_RegisterActivationCodeResponse = Callback<RegisterActivationCodeResponse_t>.Create(OnRegisterActivationCodeResponse);
		m_NewUrlLaunchParameters = Callback<NewUrlLaunchParameters_t>.Create(OnNewUrlLaunchParameters);
		m_AppProofOfPurchaseKeyResponse = Callback<AppProofOfPurchaseKeyResponse_t>.Create(OnAppProofOfPurchaseKeyResponse);

		OnFileDetailsResultCallResult = CallResult<FileDetailsResult_t>.Create(OnFileDetailsResult);
	}

	public void RenderOnGUI() {
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		GUILayout.Label("BIsSubscribed() : " + SteamApps.BIsSubscribed());

		GUILayout.Label("BIsLowViolence() : " + SteamApps.BIsLowViolence());

		GUILayout.Label("BIsCybercafe() : " + SteamApps.BIsCybercafe());

		GUILayout.Label("BIsVACBanned() : " + SteamApps.BIsVACBanned());

		GUILayout.Label("GetCurrentGameLanguage() : " + SteamApps.GetCurrentGameLanguage());

		GUILayout.Label("GetAvailableGameLanguages() : " + SteamApps.GetAvailableGameLanguages());

		GUILayout.Label("BIsSubscribedApp(SteamUtils.GetAppID()) : " + SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()));

		GUILayout.Label("BIsDlcInstalled(TestConstants.Instance.k_AppId_PieterwTestDLC) : " + SteamApps.BIsDlcInstalled(TestConstants.Instance.k_AppId_PieterwTestDLC));

		GUILayout.Label("GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()) : " + SteamApps.GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()));

		GUILayout.Label("BIsSubscribedFromFreeWeekend() : " + SteamApps.BIsSubscribedFromFreeWeekend());

		GUILayout.Label("GetDLCCount() : " + SteamApps.GetDLCCount());

		for (int iDLC = 0; iDLC < SteamApps.GetDLCCount(); ++iDLC) {
			AppId_t AppID;
			bool Available;
			string Name;
			bool ret = SteamApps.BGetDLCDataByIndex(iDLC, out AppID, out Available, out Name, 128);
			GUILayout.Label("BGetDLCDataByIndex(" + iDLC + ", out AppID, out Available, out Name, 128) : " + ret + " -- " + AppID + " -- " + Available + " -- " + Name);
		}

		if (GUILayout.Button("InstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC)")) {
			SteamApps.InstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC);
			print("SteamApps.InstallDLC(" + TestConstants.Instance.k_AppId_PieterwTestDLC + ")");
		}

		if (GUILayout.Button("UninstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC)")) {
			SteamApps.UninstallDLC(TestConstants.Instance.k_AppId_PieterwTestDLC);
			print("SteamApps.UninstallDLC(" + TestConstants.Instance.k_AppId_PieterwTestDLC + ")");
		}

		if (GUILayout.Button("RequestAppProofOfPurchaseKey(SteamUtils.GetAppID())")) {
			SteamApps.RequestAppProofOfPurchaseKey(SteamUtils.GetAppID());
			print("SteamApps.RequestAppProofOfPurchaseKey(" + SteamUtils.GetAppID() + ")");
		}

		{
			string Name;
			bool ret = SteamApps.GetCurrentBetaName(out Name, 128);
			if (Name == null) {
				Name = "";
			}
			GUILayout.Label("GetCurrentBetaName(out Name, 128) : " + ret + " -- " + Name);
		}

		if (GUILayout.Button("MarkContentCorrupt(true)")) {
			bool ret = SteamApps.MarkContentCorrupt(true);
			print("SteamApps.MarkContentCorrupt(" + true + ") : " + ret);
		}

		if (GUILayout.Button("SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), Depots, 32)")) {
			DepotId_t[] Depots = new DepotId_t[32];
			uint ret = SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), Depots, 32);
			for (int i = 0; i < ret; ++i) {
				print("SteamApps.GetInstalledDepots(SteamUtils.GetAppID(), Depots, 32) : " + ret + " -- #" + i + " -- " + Depots[i]);
			}
		}

		{
			string Folder;
			uint ret = SteamApps.GetAppInstallDir(SteamUtils.GetAppID(), out Folder, 260);
			if (Folder == null) {
				Folder = "";
			}
			GUILayout.Label("GetAppInstallDir(SteamUtils.GetAppID(), out Folder, 260) : " + ret + " -- " + Folder);
		}

		GUILayout.Label("BIsAppInstalled(SteamUtils.GetAppID()) : " + SteamApps.BIsAppInstalled(SteamUtils.GetAppID()));

		GUILayout.Label("GetAppOwner() : " + SteamApps.GetAppOwner());

		{
			// Run the test and then use steam://run/480//?test=testing;param2=value2; in your browser to try this out
			string ret = SteamApps.GetLaunchQueryParam("test");
			GUILayout.Label("GetLaunchQueryParam(\"test\") : " + ret);
		}

		{
			ulong BytesDownloaded;
			ulong BytesTotal;
			bool ret = SteamApps.GetDlcDownloadProgress(TestConstants.Instance.k_AppId_PieterwTestDLC, out BytesDownloaded, out BytesTotal);
			GUILayout.Label("GetDlcDownloadProgress(TestConstants.Instance.k_AppId_PieterwTestDLC, out BytesDownloaded, out BytesTotal) : " + ret + " -- " + BytesDownloaded + " -- " + BytesTotal);
		}

		GUILayout.Label("GetAppBuildId() : " + SteamApps.GetAppBuildId());

		if (GUILayout.Button("RequestAllProofOfPurchaseKeys()")) {
			SteamApps.RequestAllProofOfPurchaseKeys();
			print("SteamApps.RequestAllProofOfPurchaseKeys()");
		}

		if (GUILayout.Button("GetFileDetails(\"steam_api.dll\")")) {
			SteamAPICall_t handle = SteamApps.GetFileDetails("steam_api.dll");
			OnFileDetailsResultCallResult.Set(handle);
			print("SteamApps.GetFileDetails(" + "\"steam_api.dll\"" + ") : " + handle);
		}

		{
			string CommandLine;
			int ret = SteamApps.GetLaunchCommandLine(out CommandLine, 260);
			if (CommandLine == null) {
				CommandLine = "";
			}
			GUILayout.Label("GetLaunchCommandLine(out CommandLine, 260) : " + ret + " -- " + CommandLine);
		}

		GUILayout.Label("BIsSubscribedFromFamilySharing() : " + SteamApps.BIsSubscribedFromFamilySharing());

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnDlcInstalled(DlcInstalled_t pCallback) {
		Debug.Log("[" + DlcInstalled_t.k_iCallback + " - DlcInstalled] - " + pCallback.m_nAppID);
	}

	void OnRegisterActivationCodeResponse(RegisterActivationCodeResponse_t pCallback) {
		Debug.Log("[" + RegisterActivationCodeResponse_t.k_iCallback + " - RegisterActivationCodeResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_unPackageRegistered);
	}

	void OnNewUrlLaunchParameters(NewUrlLaunchParameters_t pCallback) {
		Debug.Log("[" + NewUrlLaunchParameters_t.k_iCallback + " - NewUrlLaunchParameters]");
	}

	void OnAppProofOfPurchaseKeyResponse(AppProofOfPurchaseKeyResponse_t pCallback) {
		Debug.Log("[" + AppProofOfPurchaseKeyResponse_t.k_iCallback + " - AppProofOfPurchaseKeyResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_cchKeyLength + " -- " + pCallback.m_rgchKey);
	}

	void OnFileDetailsResult(FileDetailsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + FileDetailsResult_t.k_iCallback + " - FileDetailsResult] - " + pCallback.m_eResult + " -- " + pCallback.m_ulFileSize + " -- " + pCallback.m_FileSHA + " -- " + pCallback.m_unFlags);
	}
}