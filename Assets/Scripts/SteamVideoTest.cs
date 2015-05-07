using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamVideoTest : MonoBehaviour {
	protected Callback<GetVideoURLResult_t> m_GetVideoURLResult;
	protected Callback<BroadcastUploadStart_t> m_BroadcastUploadStart;
	protected Callback<BroadcastUploadStop_t> m_BroadcastUploadStop;

	public void OnEnable() {
		m_BroadcastUploadStart = Callback<BroadcastUploadStart_t>.Create(OnBroadcastUploadStart);
		m_BroadcastUploadStop = Callback<BroadcastUploadStop_t>.Create(OnBroadcastUploadStop);
		m_GetVideoURLResult = Callback<GetVideoURLResult_t>.Create(OnGetVideoURLResult);
	}

	public void RenderOnGUI() {
		if (GUILayout.Button("GetVideoURL(343450)")) {
			SteamVideo.GetVideoURL((AppId_t)343450); // Free To Play
			print("SteamVideo.GetVideoURL(343450)");
		}

		{
			int NumViewers;
			bool ret = SteamVideo.IsBroadcasting(out NumViewers);
			GUILayout.Label("IsBroadcasting(out NumViewers) : " + ret + " -- " + NumViewers);
		}
	}
	
	void OnBroadcastUploadStart(BroadcastUploadStart_t pCallback) {
		Debug.Log("[" + BroadcastUploadStart_t.k_iCallback + " - BroadcastUploadStart] - ");
	}

	void OnBroadcastUploadStop(BroadcastUploadStop_t pCallback) {
		Debug.Log("[" + BroadcastUploadStop_t.k_iCallback + " - BroadcastUploadStop] - " + pCallback.m_eResult);
	}

	void OnGetVideoURLResult(GetVideoURLResult_t pCallback) {
		Debug.Log("[" + GetVideoURLResult_t.k_iCallback + " - GetVideoURLResult] - " + pCallback.m_eResult + " -- " + pCallback.m_unVideoAppID + " -- " + pCallback.m_rgchURL);
	}
}
