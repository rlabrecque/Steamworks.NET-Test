using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamAppsTest : MonoBehaviour {
	protected Callback<DlcInstalled_t> m_DlcInstalled;
	protected Callback<RegisterActivationCodeResponse_t> m_RegisterActivationCodeResponse;
	protected Callback<AppProofOfPurchaseKeyResponse_t> m_AppProofOfPurchaseKeyResponse;
	protected Callback<NewLaunchQueryParameters_t> m_NewLaunchQueryParameters;

	public void OnEnable() {
		m_DlcInstalled = Callback<DlcInstalled_t>.Create(OnDlcInstalled);
		m_RegisterActivationCodeResponse = Callback<RegisterActivationCodeResponse_t>.Create(OnRegisterActivationCodeResponse);
		m_AppProofOfPurchaseKeyResponse = Callback<AppProofOfPurchaseKeyResponse_t>.Create(OnAppProofOfPurchaseKeyResponse);
		m_NewLaunchQueryParameters = Callback<NewLaunchQueryParameters_t>.Create(OnNewLaunchQueryParameters);
	}

	public void RenderOnGUI() {
		GUILayout.Label("SteamApps.BIsSubscribed() : " + SteamApps.BIsSubscribed());
		GUILayout.Label("SteamApps.BIsLowViolence() : " + SteamApps.BIsLowViolence());
		GUILayout.Label("SteamApps.BIsCybercafe() : " + SteamApps.BIsCybercafe());
		GUILayout.Label("SteamApps.BIsVACBanned() : " + SteamApps.BIsVACBanned());
		GUILayout.Label("SteamApps.GetCurrentGameLanguage() : " + SteamApps.GetCurrentGameLanguage());
		GUILayout.Label("SteamApps.GetAvailableGameLanguages() : " + SteamApps.GetAvailableGameLanguages());
		GUILayout.Label("SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()) : " + SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()));
		GUILayout.Label("SteamApps.BIsDlcInstalled(110902) : " + SteamApps.BIsDlcInstalled((AppId_t)110902)); // pieterw test DLC
		GUILayout.Label("SteamApps.GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()) : " + SteamApps.GetEarliestPurchaseUnixTime(SteamUtils.GetAppID()));
		GUILayout.Label("SteamApps.BIsSubscribedFromFreeWeekend() : " + SteamApps.BIsSubscribedFromFreeWeekend());
		GUILayout.Label("SteamApps.GetDLCCount() : " + SteamApps.GetDLCCount());

		for (int iDLC = 0; iDLC < SteamApps.GetDLCCount(); ++iDLC) {
			AppId_t AppID;
			bool Available;
			string Name;
			bool ret = SteamApps.BGetDLCDataByIndex(iDLC, out AppID, out Available, out Name, 128);
			GUILayout.Label("SteamApps.BGetDLCDataByIndex(" + iDLC + ", out AppID, out Available, out Name, 128) : " + ret + " -- " + AppID + " -- " + Available + " -- " + Name);
		}

		if (GUILayout.Button("SteamApps.InstallDLC(110902)")) {
			SteamApps.InstallDLC((AppId_t)110902); // pieterw test DLC
		}

		if (GUILayout.Button("SteamApps.UninstallDLC(110902)")) {
			SteamApps.UninstallDLC((AppId_t)110902); // pieterw test DLC
		}

		if (GUILayout.Button("SteamApps.RequestAppProofOfPurchaseKey(SteamUtils.GetAppID())")) {
			SteamApps.RequestAppProofOfPurchaseKey(SteamUtils.GetAppID());
		}

		{
			string Name;
			bool ret = SteamApps.GetCurrentBetaName(out Name, 128);
			if (Name == null) {
				Name = "";
			}
			GUILayout.Label("SteamApps.GetCurrentBetaName(out Name, 128) : " + ret + " -- " + Name);
		}

		if (GUILayout.Button("SteamApps.MarkContentCorrupt(true)")) {
			print("SteamApps.MarkContentCorrupt(true) : " + SteamApps.MarkContentCorrupt(true));
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

			GUILayout.Label("SteamApps.GetAppInstallDir(480, out Folder, 260) : " + ret + " -- " + Folder);
		}

		GUILayout.Label("SteamApps.BIsAppInstalled(480) : " + SteamApps.BIsAppInstalled(SteamUtils.GetAppID()));
		GUILayout.Label("SteamApps.GetAppOwner() : " + SteamApps.GetAppOwner());

		// Run the test and then use steam://run/480//?test=testing;param2=value2; in your browser to try this out
		GUILayout.Label("SteamApps.GetLaunchQueryParam(\"test\") : " + SteamApps.GetLaunchQueryParam("test"));

		{
			ulong BytesDownloaded;
			ulong BytesTotal;
			bool ret = SteamApps.GetDlcDownloadProgress((AppId_t)110902, out BytesDownloaded, out BytesTotal);
			GUILayout.Label("SteamApps.GetDlcDownloadProgress((AppId_t)110902, out BytesDownloaded, out BytesTotal): " + ret + " -- " + BytesDownloaded + " -- " + BytesTotal);
		}

		GUILayout.Label("SteamApps.GetAppBuildId(): " + SteamApps.GetAppBuildId());
#if _PS3
		if (GUILayout.Button("SteamApps.RegisterActivationCode(\"???\")")) {
			SteamAPICall_t handle = SteamApps.RegisterActivationCode("???");
			new CallResult<RegisterActivationCodeResponse_t>(OnRegisterActivationCodeResponse, handle);
			new CallResult<AppProofOfPurchaseKeyResponse_t>(OnAppProofOfPurchaseKeyResponse, handle);
		}
#endif
	}

	void OnDlcInstalled(DlcInstalled_t pCallback) {
		Debug.Log("[" + DlcInstalled_t.k_iCallback + " - DlcInstalled] - " + pCallback.m_nAppID);
	}

	void OnRegisterActivationCodeResponse(RegisterActivationCodeResponse_t pCallback) {
		Debug.Log("[" + RegisterActivationCodeResponse_t.k_iCallback + " - RegisterActivationCodeResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_unPackageRegistered);
	}

	void OnAppProofOfPurchaseKeyResponse(AppProofOfPurchaseKeyResponse_t pCallback) {
		Debug.Log("[" + AppProofOfPurchaseKeyResponse_t.k_iCallback + " - AppProofOfPurchaseKeyResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_rgchKey);
	}

	void OnNewLaunchQueryParameters(NewLaunchQueryParameters_t pCallback) {
		Debug.Log("[" + NewLaunchQueryParameters_t.k_iCallback + " - NewLaunchQueryParameters]");
	}
}
