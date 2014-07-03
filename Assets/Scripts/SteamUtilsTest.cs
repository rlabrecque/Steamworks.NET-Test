using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUtilsTest : MonoBehaviour {
	private Texture2D m_Image;

	protected Callback<IPCountry_t> m_IPCountry;
	protected Callback<LowBatteryPower_t> m_LowBatteryPower;
	//protected Callback<SteamAPICallCompleted_t> m_SteamAPICallCompleted;
	protected Callback<SteamShutdown_t> m_SteamShutdown;
	protected Callback<GamepadTextInputDismissed_t> m_GamepadTextInputDismissed;

	private CallResult<CheckFileSignature_t> OnCheckFileSignatureCallResult;

	public void OnEnable() {
		m_IPCountry = Callback<IPCountry_t>.Create(OnIPCountry);
		m_LowBatteryPower = Callback<LowBatteryPower_t>.Create(OnLowBatteryPower);
		//m_SteamAPICallCompleted = Callback<SteamAPICallCompleted_t>.Create(OnSteamAPICallCompleted); // N/A - Far too spammy to test like this!
		m_SteamShutdown = Callback<SteamShutdown_t>.Create(OnSteamShutdown);
		m_GamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(OnGamepadTextInputDismissed);

		OnCheckFileSignatureCallResult = CallResult<CheckFileSignature_t>.Create(OnCheckFileSignature);
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

		//GUILayout.Label("SteamUtils.SetWarningMessageHook() : " + SteamUtils.SetWarningMessageHook()); // N/A - Check out SteamTest.cs for example usage.

		GUILayout.Label("SteamUtils.IsOverlayEnabled() : " + SteamUtils.IsOverlayEnabled());
		GUILayout.Label("SteamUtils.BOverlayNeedsPresent() : " + SteamUtils.BOverlayNeedsPresent());

		if (GUILayout.Button("SteamUtils.CheckFileSignature(\"FileNotFound.txt\")")) {
			SteamAPICall_t handle = SteamUtils.CheckFileSignature("FileNotFound.txt");
			OnCheckFileSignatureCallResult.Set(handle);
			print("SteamUtils.CheckFileSignature(\"FileNotFound.txt\") - " + handle);
		}

		if(GUILayout.Button("SteamUtils.ShowGamepadTextInput(k_EGamepadTextInputModeNormal, k_EGamepadTextInputLineModeSingleLine, \"Description Test!\", 32)")) {
			bool ret = SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "Description Test!", 32, "");
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

	void OnCheckFileSignature(CheckFileSignature_t pCallback, bool bIOFailure) {
		Debug.Log("[" + CheckFileSignature_t.k_iCallback + " - CheckFileSignature] - " + pCallback.m_eCheckFileSignature);
	}

	void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback) {
		Debug.Log("[" + GamepadTextInputDismissed_t.k_iCallback + " - GamepadTextInputDismissed] - " + pCallback.m_bSubmitted + " -- " + pCallback.m_unSubmittedText);
	}
}
