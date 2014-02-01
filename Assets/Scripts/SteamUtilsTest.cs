using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUtilsTest : MonoBehaviour {
	Texture2D m_Image;

	CallResult<CheckFileSignature_t> OnCheckFileSignatureCallResult;

	public void OnEnable() {
		new Callback<IPCountry_t>(OnIPCountry);
		new Callback<LowBatteryPower_t>(OnLowBatteryPower);
		//new Callback<SteamAPICallCompleted_t>(OnSteamAPICallCompleted); // N/A - CallbackDispatcher already uses this and the current impl only allows one callback at a time!
		new Callback<SteamShutdown_t>(OnSteamShutdown);
		OnCheckFileSignatureCallResult = new CallResult<CheckFileSignature_t>(OnCheckFileSignature);
#if _PS3
		new Callback<NetStartDialogFinished_t>(OnNetStartDialogFinished);
		new Callback<NetStartDialogUnloaded_t>(OnNetStartDialogUnloaded);
		new Callback<PS3SystemMenuClosed_t>(OnPS3SystemMenuClosed);
		new Callback<PS3NPMessageSelected_t>(OnPS3NPMessageSelected);
		new Callback<PS3KeyboardDialogFinished_t>(OnPS3KeyboardDialogFinished);
		new Callback<PS3PSNStatusChange_t>(OnPS3PSNStatusChange);
#endif
		new Callback<GamepadTextInputDismissed_t>(OnGamepadTextInputDismissed);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Image:");
		GUILayout.Label(m_Image);
		GUILayout.EndArea();

		GUILayout.Label("SteamUtils.GetSecondsSinceAppActive() : " + SteamUtils.GetSecondsSinceAppActive());
		GUILayout.Label("SteamUtils.GetConnectedUniverse() : " + SteamUtils.GetConnectedUniverse());
		GUILayout.Label("SteamUtils.GetServerRealTime() : " + SteamUtils.GetServerRealTime());
		GUILayout.Label("SteamUtils.GetIPCountry() : " + SteamUtils.GetIPCountry());

		{
			uint ImageWidth = 0;
			uint ImageHeight = 0;
			bool ret = SteamUtils.GetImageSize(1, out ImageWidth, out ImageHeight);
			GUILayout.Label("SteamUtils.GetImageSize(1, out ImageWidth, out ImageHeight) : " + ret + " -- " + ImageWidth + " -- " + ImageHeight);

			if (GUILayout.Button("SteamUtils.GetImageRGBA(1, Image, (int)(ImageWidth * ImageHeight * 4)")) {
				if (ImageWidth > 0 && ImageHeight > 0) {
					byte[] Image = new byte[ImageWidth * ImageHeight * 4];
					ret = SteamUtils.GetImageRGBA(1, Image, (int)(ImageWidth * ImageHeight * 4));
					print("SteamUtils.GetImageRGBA(1, " + Image + ", " + (int)(ImageWidth * ImageHeight * 4) + ") - " + ret + " -- " + ImageWidth + " -- " + ImageHeight);
					if (ret) {
						m_Image = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
						m_Image.LoadRawTextureData(Image);
						m_Image.Apply();
					}
				}
			}
		}

		{
			uint IP;
			ushort Port;
			bool ret = SteamUtils.GetCSERIPPort(out IP, out Port);
			GUILayout.Label("SteamUtils.GetCSERIPPort(out IP, out Port) : " + ret + " -- " + IP + " -- " + Port);
		}

		GUILayout.Label("SteamUtils.GetCurrentBatteryPower() : " + SteamUtils.GetCurrentBatteryPower());
		GUILayout.Label("SteamUtils.GetAppID() : " + SteamUtils.GetAppID());

		if (GUILayout.Button("SteamUtils.SetOverlayNotificationPosition(k_EPositionTopRight)")) {
			SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight);
			print("SteamUtils.SetOverlayNotificationPosition(k_EPositionTopRight)");
		}

		//GUILayout.Label("SteamUtils.IsAPICallCompleted() : " + SteamUtils.IsAPICallCompleted()); // N/A - These 3 functions are used to dispatch CallResults.
		//GUILayout.Label("SteamUtils.GetAPICallFailureReason() : " + SteamUtils.GetAPICallFailureReason()); // N/A
		//GUILayout.Label("SteamUtils.GetAPICallResult() : " + SteamUtils.GetAPICallResult()); // N/A

		if (GUILayout.Button("SteamUtils.RunFrame()")) {
			SteamUtils.RunFrame();
			print("SteamUtils.RunFrame()");
		}

		GUILayout.Label("SteamUtils.GetIPCCallCount() : " + SteamUtils.GetIPCCallCount());

		//GUILayout.Label("SteamUtils.SetWarningMessageHook() : " + SteamUtils.SetWarningMessageHook()); // TODO

		GUILayout.Label("SteamUtils.IsOverlayEnabled() : " + SteamUtils.IsOverlayEnabled());
		GUILayout.Label("SteamUtils.BOverlayNeedsPresent() : " + SteamUtils.BOverlayNeedsPresent());
#if !_PS3
		if (GUILayout.Button("SteamUtils.CheckFileSignature(\"FileNotFound.txt\")")) {
			SteamAPICall_t handle = SteamUtils.CheckFileSignature("FileNotFound.txt");
			OnCheckFileSignatureCallResult.SetAPICallHandle(handle);
			print("SteamUtils.CheckFileSignature(\"FileNotFound.txt\") - " + handle);
		}
#else
		//GUILayout.Label("SteamUtils.PostPS3SysutilCallback() : " + SteamUtils.PostPS3SysutilCallback());
		GUILayout.Label("SteamUtils.BIsReadyToShutdown() : " + SteamUtils.BIsReadyToShutdown());
		GUILayout.Label("SteamUtils.BIsPSNOnline() : " + SteamUtils.BIsPSNOnline());
		//GUILayout.Label("SteamUtils.SetPSNGameBootInviteStrings() : " + SteamUtils.SetPSNGameBootInviteStrings());
#endif
		if(GUILayout.Button("SteamUtils.ShowGamepadTextInput(k_EGamepadTextInputModeNormal, k_EGamepadTextInputLineModeSingleLine, \"Description Test!\", 32)")) {
			bool ret = SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "Description Test!", 32);
			print("SteamUtils.ShowGamepadTextInput(k_EGamepadTextInputModeNormal, k_EGamepadTextInputLineModeSingleLine, \"Description Test!\", 32) - " + ret);
		}

		GUILayout.Label("SteamUtils.GetEnteredGamepadTextLength() : " + SteamUtils.GetEnteredGamepadTextLength()); // TODO - only to be called from within OnGamepadTextInputDismissed?
		
		{
			string Text;
			bool ret = SteamUtils.GetEnteredGamepadTextInput(out Text, 32);
			GUILayout.Label("SteamUtils.GetEnteredGamepadTextInput(out Text, 32) - " + ret + " -- " + Text);
		}

		GUILayout.Label("SteamUtils.GetSteamUILanguage() : " + SteamUtils.GetSteamUILanguage());
	}

	void OnIPCountry(IPCountry_t pCallback) {
		Debug.Log("[" + IPCountry_t.k_iCallback + " - IPCountry]");
	}

	void OnLowBatteryPower(LowBatteryPower_t pCallback) {
		Debug.Log("[" + LowBatteryPower_t.k_iCallback + " - LowBatteryPower] - " + pCallback.m_nMinutesBatteryLeft);
	}

	void OnSteamShutdown(SteamShutdown_t pCallback) {
		Debug.Log("[" + SteamShutdown_t.k_iCallback + " - SteamShutdown]");
	}

	void OnCheckFileSignature(SteamAPICall_t handle, CheckFileSignature_t pCallback) {
		Debug.Log("[" + CheckFileSignature_t.k_iCallback + " - CheckFileSignature] - " + pCallback.m_eCheckFileSignature);
	}

#if _PS3
	void OnNetStartDialogFinished(NetStartDialogFinished_t pCallback) {
		Debug.Log("[" + NetStartDialogFinished_t.k_iCallback + " - NetStartDialogFinished]");
	}

	void OnNetStartDialogUnloaded(NetStartDialogUnloaded_t pCallback) {
		Debug.Log("[" + NetStartDialogUnloaded_t.k_iCallback + " - NetStartDialogUnloaded]");
	}

	void OnPS3SystemMenuClosed(PS3SystemMenuClosed_t pCallback) {
		Debug.Log("[" + PS3SystemMenuClosed_t.k_iCallback + " - PS3SystemMenuClosed]");
	}

	void OnPS3NPMessageSelected(PS3NPMessageSelected_t pCallback) {
		Debug.Log("[" + PS3NPMessageSelected_t.k_iCallback + " - PS3NPMessageSelected] - " + pCallback.dataid);
	}

	void OnPS3KeyboardDialogFinished(PS3KeyboardDialogFinished_t pCallback) {
		Debug.Log("[" + PS3KeyboardDialogFinished_t.k_iCallback + " - PS3KeyboardDialogFinished]");
	}

	void OnPS3PSNStatusChange(PS3PSNStatusChange_t pCallback) {
		Debug.Log("[" + PS3PSNStatusChange_t.k_iCallback + " - PS3PSNStatusChange] - " + pCallback.m_bPSNOnline);
	}
#endif

	void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback) {
		Debug.Log("[" + GamepadTextInputDismissed_t.k_iCallback + " - GamepadTextInputDismissed] - " + pCallback.m_bSubmitted + " -- " + pCallback.m_unSubmittedText);
	}
}
