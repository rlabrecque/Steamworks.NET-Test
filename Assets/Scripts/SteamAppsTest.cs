using UnityEngine;
using System.Collections;
using Steamworks;

class SteamAppsTest : MonoBehaviour {
	public static void RegisterCallbacks() {
		new Callback<DlcInstalled_t>(OnDlcInstalled);
#if _PS3
		new Callback<RegisterActivationCodeResponse_t>(OnRegisterActivationCodeResponse);
		new Callback<AppProofOfPurchaseKeyResponse_t>(OnAppProofOfPurchaseKeyResponse);
#endif
		new Callback<NewLaunchQueryParameters_t>(OnNewLaunchQueryParameters);
	}

	public static void RenderOnGUI() {
		GUILayout.Label("SteamApps.BIsSubscribed() : " + SteamApps.BIsSubscribed());
		GUILayout.Label("SteamApps.BIsLowViolence() : " + SteamApps.BIsLowViolence());
		GUILayout.Label("SteamApps.BIsCybercafe() : " + SteamApps.BIsCybercafe());
		GUILayout.Label("SteamApps.BIsVACBanned() : " + SteamApps.BIsVACBanned());
		GUILayout.Label("SteamApps.GetCurrentGameLanguage() : " + SteamApps.GetCurrentGameLanguage());
		GUILayout.Label("SteamApps.GetAvailableGameLanguages() : " + SteamApps.GetAvailableGameLanguages());
		GUILayout.Label("SteamApps.BIsSubscribedApp(480) : " + SteamApps.BIsSubscribedApp(480)); // SpaceWar
		GUILayout.Label("SteamApps.BIsDlcInstalled(110902) : " + SteamApps.BIsDlcInstalled(110902)); // pieterw test DLC
		GUILayout.Label("SteamApps.GetEarliestPurchaseUnixTime(480) : " + SteamApps.GetEarliestPurchaseUnixTime(480)); // SpaceWar
		GUILayout.Label("SteamApps.BIsSubscribedFromFreeWeekend() : " + SteamApps.BIsSubscribedFromFreeWeekend());
		GUILayout.Label("SteamApps.GetDLCCount() : " + SteamApps.GetDLCCount());

		for (int iDLC = 0; iDLC < SteamApps.GetDLCCount(); ++iDLC) {
			uint AppID;
			bool Available;
			string Name;
			bool ret = SteamApps.BGetDLCDataByIndex(iDLC, out AppID, out Available, out Name, 128);
			GUILayout.Label("SteamApps.BGetDLCDataByIndex(" + iDLC + ", out AppID, out Available, out Name, 128) : " + ret + " -- " + AppID + " -- " + Available + " -- " + Name); // ??
		}

		if (GUILayout.Button("SteamApps.InstallDLC(110902)")) {
			SteamApps.InstallDLC(110902); // pieterw test DLC
		}

		if (GUILayout.Button("SteamApps.UninstallDLC(110902)")) {
			SteamApps.UninstallDLC(110902); // pieterw test DLC
		}

		if (GUILayout.Button("SteamApps.RequestAppProofOfPurchaseKey(480)")) {
			SteamApps.RequestAppProofOfPurchaseKey(480); // SpaceWar
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

		if (GUILayout.Button("SteamApps.GetInstalledDepots(480, Depots, 32)")) {
			uint[] Depots = new uint[32];
			uint ret = SteamApps.GetInstalledDepots(480, Depots, 32); // SpaceWar
			for (int i = 0; i < ret; ++i) {
				print("SteamApps.GetInstalledDepots(480, Depots, 32) : " + ret + " -- #" + i + " -- " + Depots[i]);
			}
		}

		{
			string Folder;
			uint ret = SteamApps.GetAppInstallDir(480, out Folder, 260); // SpaceWar
			if (Folder == null) {
				Folder = "";
			}

			GUILayout.Label("SteamApps.GetAppInstallDir(480, out Folder, 260) : " + ret + " -- " + Folder);
		}

		GUILayout.Label("SteamApps.BIsAppInstalled(480) : " + SteamApps.BIsAppInstalled(480)); // SpaceWar
		GUILayout.Label("SteamApps.GetAppOwner() : " + SteamApps.GetAppOwner());

		// Run the test and then use steam://run/480//?test=testing;param2=value2; in your browser to try this out
		GUILayout.Label("SteamApps.GetLaunchQueryParam(\"test\") : " + SteamApps.GetLaunchQueryParam("test"));
#if _PS3
		if (GUILayout.Button("SteamApps.RegisterActivationCode(\"???\")")) {
			ulong handle = SteamApps.RegisterActivationCode("???");
			new CallResult<RegisterActivationCodeResponse_t>(OnRegisterActivationCodeResponse, handle);
			new CallResult<AppProofOfPurchaseKeyResponse_t>(OnAppProofOfPurchaseKeyResponse, handle);
		}
#endif
	}

	static void OnDlcInstalled(DlcInstalled_t pCallback) {
		Debug.Log("[" + DlcInstalled_t.k_iCallback + " - DlcInstalled] - " + pCallback.m_nAppID);
	}

#if _PS3
	static void OnRegisterActivationCodeResponse(RegisterActivationCodeResponse_t pCallback) {
		Debug.Log("[" + RegisterActivationCodeResponse_t.k_iCallback + " - RegisterActivationCodeResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_unPackageRegistered);
	}

	static void OnAppProofOfPurchaseKeyResponse(AppProofOfPurchaseKeyResponse_t pCallback) {
		Debug.Log("[" + AppProofOfPurchaseKeyResponse_t.k_iCallback + " - AppProofOfPurchaseKeyResponse] - " + pCallback.m_eResult + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_rgchKey);
	}
#endif

	static void OnNewLaunchQueryParameters(NewLaunchQueryParameters_t pCallback) {
		Debug.Log("[" + NewLaunchQueryParameters_t.k_iCallback + " - NewLaunchQueryParameters]");
	}
}
