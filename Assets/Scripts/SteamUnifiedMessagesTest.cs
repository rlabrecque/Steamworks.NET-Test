using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUnifiedMessagesTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private ClientUnifiedMessageHandle m_ClientUnifiedMessageHandle;

	protected Callback<SteamUnifiedMessagesSendMethodResult_t> m_SteamUnifiedMessagesSendMethodResult;

	public void OnEnable() {
		m_SteamUnifiedMessagesSendMethodResult = Callback<SteamUnifiedMessagesSendMethodResult_t>.Create(OnSteamUnifiedMessagesSendMethodResult);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_ClientUnifiedMessageHandle: " + m_ClientUnifiedMessageHandle);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		// TODO: I don't know what I'm doing. This whole interface has essentially Zero Documentation.
		if (GUILayout.Button("SendMethod(\"Player.GetGameBadgeLevels#1\", null, 0, 1111)")) {
			ClientUnifiedMessageHandle ret = SteamUnifiedMessages.SendMethod("Player.GetGameBadgeLevels#1", null, 0, 1111);
			m_ClientUnifiedMessageHandle = ret;
			print("SteamUnifiedMessages.SendMethod(" + "\"Player.GetGameBadgeLevels#1\"" + ", " + null + ", " + 0 + ", " + 1111 + ") : " + ret);
		}

		if (GUILayout.Button("GetMethodResponseInfo(m_ClientUnifiedMessageHandle, out ResponseSize, out Result)")) {
			uint ResponseSize;
			EResult Result;
			bool ret = SteamUnifiedMessages.GetMethodResponseInfo(m_ClientUnifiedMessageHandle, out ResponseSize, out Result);
			print("SteamUnifiedMessages.GetMethodResponseInfo(" + m_ClientUnifiedMessageHandle + ", " + "out ResponseSize" + ", " + "out Result" + ") : " + ret + " -- " + ResponseSize + " -- " + Result);
		}

		if (GUILayout.Button("GetMethodResponseData(m_ClientUnifiedMessageHandle, new byte[1], 1, true)")) {
			bool ret = SteamUnifiedMessages.GetMethodResponseData(m_ClientUnifiedMessageHandle, new byte[1], 1, true);
			print("SteamUnifiedMessages.GetMethodResponseData(" + m_ClientUnifiedMessageHandle + ", " + new byte[1] + ", " + 1 + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("ReleaseMethod(m_ClientUnifiedMessageHandle)")) {
			bool ret = SteamUnifiedMessages.ReleaseMethod(m_ClientUnifiedMessageHandle);
			print("SteamUnifiedMessages.ReleaseMethod(" + m_ClientUnifiedMessageHandle + ") : " + ret);
		}

		if (GUILayout.Button("SendNotification(\"MsgTest.NotifyServer#1\", null, 0)")) {
			bool ret = SteamUnifiedMessages.SendNotification("MsgTest.NotifyServer#1", null, 0);
			print("SteamUnifiedMessages.SendNotification(" + "\"MsgTest.NotifyServer#1\"" + ", " + null + ", " + 0 + ") : " + ret);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnSteamUnifiedMessagesSendMethodResult(SteamUnifiedMessagesSendMethodResult_t pCallback) {
		Debug.Log("[" + SteamUnifiedMessagesSendMethodResult_t.k_iCallback + " - SteamUnifiedMessagesSendMethodResult] - " + pCallback.m_hHandle + " -- " + pCallback.m_unContext + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unResponseSize);
	}
}