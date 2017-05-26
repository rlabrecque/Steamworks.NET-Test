using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamVideoTest : MonoBehaviour {
	private Vector2 m_ScrollPos;

	protected Callback<BroadcastUploadStart_t> m_BroadcastUploadStart;
	protected Callback<BroadcastUploadStop_t> m_BroadcastUploadStop;
	protected Callback<GetVideoURLResult_t> m_GetVideoURLResult;
	protected Callback<GetOPFSettingsResult_t> m_GetOPFSettingsResult;

	public void OnEnable() {
		m_BroadcastUploadStart = Callback<BroadcastUploadStart_t>.Create(OnBroadcastUploadStart);
		m_BroadcastUploadStop = Callback<BroadcastUploadStop_t>.Create(OnBroadcastUploadStop);
		m_GetVideoURLResult = Callback<GetVideoURLResult_t>.Create(OnGetVideoURLResult);
		m_GetOPFSettingsResult = Callback<GetOPFSettingsResult_t>.Create(OnGetOPFSettingsResult);
	}

	public void RenderOnGUI() {
		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		if (GUILayout.Button("GetVideoURL(TestConstants.Instance.k_AppId_FreeToPlay)")) {
			SteamVideo.GetVideoURL(TestConstants.Instance.k_AppId_FreeToPlay);
			print("SteamVideo.GetVideoURL(" + TestConstants.Instance.k_AppId_FreeToPlay + ")");
		}

		{
			int NumViewers;
			bool ret = SteamVideo.IsBroadcasting(out NumViewers);
			GUILayout.Label("IsBroadcasting(out NumViewers) : " + ret + " -- " + NumViewers);
		}

		if (GUILayout.Button("GetOPFSettings(TestConstants.Instance.k_AppId_FreeToPlay)")) {
			SteamVideo.GetOPFSettings(TestConstants.Instance.k_AppId_FreeToPlay);
			print("SteamVideo.GetOPFSettings(" + TestConstants.Instance.k_AppId_FreeToPlay + ")");
		}

		if (GUILayout.Button("GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out Buffer, ref ValueBufferSize)")) {
			string Buffer;
			int ValueBufferSize = 0;
			bool ret = SteamVideo.GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out Buffer, ref ValueBufferSize);
			if(ret) {
			ret = SteamVideo.GetOPFStringForApp(TestConstants.Instance.k_AppId_FreeToPlay, out Buffer, ref ValueBufferSize);
			}
			print("SteamVideo.GetOPFStringForApp(" + TestConstants.Instance.k_AppId_FreeToPlay + ", " + "out Buffer" + ", " + "ref ValueBufferSize" + ") : " + ret + " -- " + Buffer + " -- " + ValueBufferSize);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnBroadcastUploadStart(BroadcastUploadStart_t pCallback) {
		Debug.Log("[" + BroadcastUploadStart_t.k_iCallback + " - BroadcastUploadStart]");
	}

	void OnBroadcastUploadStop(BroadcastUploadStop_t pCallback) {
		Debug.Log("[" + BroadcastUploadStop_t.k_iCallback + " - BroadcastUploadStop] - " + pCallback.m_eResult);
	}

	void OnGetVideoURLResult(GetVideoURLResult_t pCallback) {
		Debug.Log("[" + GetVideoURLResult_t.k_iCallback + " - GetVideoURLResult] - " + pCallback.m_eResult + " -- " + pCallback.m_unVideoAppID + " -- " + pCallback.m_rgchURL);
	}

	void OnGetOPFSettingsResult(GetOPFSettingsResult_t pCallback) {
		Debug.Log("[" + GetOPFSettingsResult_t.k_iCallback + " - GetOPFSettingsResult] - " + pCallback.m_eResult + " -- " + pCallback.m_unVideoAppID);
	}
}