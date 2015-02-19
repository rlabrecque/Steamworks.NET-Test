using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamVideoTest : MonoBehaviour {
	protected Callback<GetVideoURLResult_t> m_GetVideoURLResult;

	public void OnEnable() {
		m_GetVideoURLResult = Callback<GetVideoURLResult_t>.Create(OnGetVideoURLResult);
	}

	public void RenderOnGUI() {
		if (GUILayout.Button("GetVideoURL(343450)")) {
			SteamVideo.GetVideoURL((AppId_t)343450); // Free To Play
			print("SteamVideo.GetVideoURL(343450)");
		}
	}

	void OnGetVideoURLResult(GetVideoURLResult_t pCallback) {
		Debug.Log("[" + GetVideoURLResult_t.k_iCallback + " - GetVideoURLResult] - " + pCallback.m_eResult + " -- " + pCallback.m_unVideoAppID + " -- " + pCallback.m_rgchURL);
	}
}
