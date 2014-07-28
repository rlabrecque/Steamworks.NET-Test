using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamAppListTest : MonoBehaviour {
	private AppId_t[] m_AppList;

	protected Callback<SteamAppInstalled_t> m_SteamAppInstalled;
	protected Callback<SteamAppUninstalled_t> m_SteamAppUninstalled;
	
	public void OnEnable() {
		m_SteamAppInstalled = Callback<SteamAppInstalled_t>.Create(OnSteamAppInstalled);
		m_SteamAppUninstalled = Callback<SteamAppUninstalled_t>.Create(OnSteamAppUninstalled);
	}

	public void RenderOnGUI() {
		GUILayout.Label("GetNumInstalledApps() : " + SteamAppList.GetNumInstalledApps());

		{
			m_AppList = new AppId_t[1];
			uint ret = SteamAppList.GetInstalledApps(m_AppList, 1);
			GUILayout.Label("GetInstalledApps(m_AppList, 1) : " + ret + " -- " + m_AppList[0]);

		}

		{
			string Name;
			int ret = SteamAppList.GetAppName(m_AppList[0], out Name, 256);
			GUILayout.Label("GetAppName(m_AppList[0], out Name, 256) : " + ret + " -- " + Name);
		}

		{
			string Directory;
			int ret = SteamAppList.GetAppInstallDir(m_AppList[0], out Directory, 260);
			GUILayout.Label("GetAppInstallDir(m_AppList[0], out Directory, 260) : " + ret + " -- " + Directory);
		}

		GUILayout.Label("GetAppBuildId() : " + SteamAppList.GetAppBuildId(m_AppList[0]));
	}

	void OnSteamAppInstalled(SteamAppInstalled_t pCallback) {
		Debug.Log("[" + SteamAppInstalled_t.k_iCallback + " - SteamAppInstalled] - " + pCallback.m_nAppID);
	}

	void OnSteamAppUninstalled(SteamAppUninstalled_t pCallback) {
		Debug.Log("[" + SteamAppUninstalled_t.k_iCallback + " - SteamAppUninstalled] - " + pCallback.m_nAppID);
	}
}
