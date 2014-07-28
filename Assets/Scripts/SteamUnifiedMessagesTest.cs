using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUnifiedMessagesTest : MonoBehaviour {
	private ClientUnifiedMessageHandle m_ClientUnifiedMessageHandle;

	protected Callback<SteamUnifiedMessagesSendMethodResult_t> m_SteamUnifiedMessagesSendMethodResult;
	
	public void OnEnable() {
		m_SteamUnifiedMessagesSendMethodResult = Callback<SteamUnifiedMessagesSendMethodResult_t>.Create(OnSteamUnifiedMessagesSendMethodResult);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_ClientUnifiedMessageHandle: " + m_ClientUnifiedMessageHandle);
		GUILayout.EndArea();

		// TODO: I don't know what I'm doing. This whole interface has essentially Zero Documentation.
		if (GUILayout.Button("SendMethod(\"CMsgTest_MessageToServer_Request\", null, 0, 1111)")) {
			m_ClientUnifiedMessageHandle = SteamUnifiedMessages.SendMethod("Player.GetGameBadgeLevels#1", null, 0, 1111); 
			print("SteamUnifiedMessages.SendMethod(\"CMsgTest_MessageToServer_Request\", null, 0, 1111) : " + m_ClientUnifiedMessageHandle);
		}

		if (GUILayout.Button("GetMethodResponseInfo(m_ClientUnifiedMessageHandle, out ResponseSize, out Result)")) {
			uint ResponseSize;
			EResult Result;
			bool ret = SteamUnifiedMessages.GetMethodResponseInfo(m_ClientUnifiedMessageHandle, out ResponseSize, out Result);
			print("SteamUnifiedMessages.GetMethodResponseInfo(" + m_ClientUnifiedMessageHandle + ", out ResponseSize, out Result) : " + ret + " -- " + ResponseSize + " -- " + Result);
		}

		if (GUILayout.Button("GetMethodResponseData(m_ClientUnifiedMessageHandle, new byte[1], 1, true)")) {
			bool ret = SteamUnifiedMessages.GetMethodResponseData(m_ClientUnifiedMessageHandle, new byte[1], 1, true);
			print("SteamUnifiedMessages.GetMethodResponseData(" + m_ClientUnifiedMessageHandle + ", new byte[1], 1, true) : " + ret);
		}

		if (GUILayout.Button("ReleaseMethod(m_ClientUnifiedMessageHandle)")) {
			bool ret = SteamUnifiedMessages.ReleaseMethod(m_ClientUnifiedMessageHandle);
			print("SteamUnifiedMessages.ReleaseMethod(" + m_ClientUnifiedMessageHandle + ") : " + ret);
		}

		if (GUILayout.Button("SteamUnifiedMessages.SendNotification(\"MsgTest.NotifyServer#1\", null, 0)")) {
			bool ret = SteamUnifiedMessages.SendNotification("MsgTest.NotifyServer#1", null, 0);
			print("SteamUnifiedMessages.SendNotification(\"MsgTest.NotifyServer#1\", null, 0) : " + ret);
		}
	}

	void OnSteamUnifiedMessagesSendMethodResult(SteamUnifiedMessagesSendMethodResult_t pCallback) {
		Debug.Log("[" + SteamUnifiedMessagesSendMethodResult_t.k_iCallback + " - SteamUnifiedMessagesSendMethodResult] - " + pCallback.m_hHandle + " -- " + pCallback.m_unContext + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unResponseSize);
	}
}
