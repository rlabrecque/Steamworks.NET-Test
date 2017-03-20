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

	public static Texture2D GetSteamImageAsTexture2D(int iImage) {
		Texture2D ret = null;
		uint ImageWidth;
		uint ImageHeight;
		bool bIsValid = SteamUtils.GetImageSize(iImage, out ImageWidth, out ImageHeight);

		if (bIsValid) {
			byte[] Image = new byte[ImageWidth * ImageHeight * 4];

			bIsValid = SteamUtils.GetImageRGBA(iImage, Image, (int)(ImageWidth * ImageHeight * 4));
			if (bIsValid) {
				ret = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
				ret.LoadRawTextureData(Image);
				ret.Apply();
			}
		}

		return ret;
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Image:");
		GUILayout.Label(m_Image);
		GUILayout.EndArea();

		GUILayout.Label("GetSecondsSinceAppActive() : " + SteamUtils.GetSecondsSinceAppActive());

		GUILayout.Label("GetSecondsSinceComputerActive() : " + SteamUtils.GetSecondsSinceComputerActive());

		GUILayout.Label("GetConnectedUniverse() : " + SteamUtils.GetConnectedUniverse());

		GUILayout.Label("GetServerRealTime() : " + SteamUtils.GetServerRealTime());

		GUILayout.Label("GetIPCountry() : " + SteamUtils.GetIPCountry());

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
			GUILayout.Label("GetCSERIPPort(out IP, out Port) : " + ret + " -- " + IP + " -- " + Port);
		}

		GUILayout.Label("GetCurrentBatteryPower() : " + SteamUtils.GetCurrentBatteryPower());

		GUILayout.Label("GetAppID() : " + SteamUtils.GetAppID());

		if (GUILayout.Button("SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight)")) {
			SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight);
			print("SteamUtils.SetOverlayNotificationPosition(" + ENotificationPosition.k_EPositionTopRight + ")");
		}

		//GUILayout.Label("SteamUtils.IsAPICallCompleted() : " + SteamUtils.IsAPICallCompleted()); // N/A - These 3 functions are used to dispatch CallResults.

		//GUILayout.Label("SteamUtils.GetAPICallFailureReason() : " + SteamUtils.GetAPICallFailureReason()); // N/A

		//GUILayout.Label("SteamUtils.GetAPICallResult() : " + SteamUtils.GetAPICallResult()); // N/A

		GUILayout.Label("GetIPCCallCount() : " + SteamUtils.GetIPCCallCount());

		//GUILayout.Label("SteamUtils.SetWarningMessageHook() : " + SteamUtils.SetWarningMessageHook()); // N/A - Check out SteamTest.cs for example usage.

		GUILayout.Label("IsOverlayEnabled() : " + SteamUtils.IsOverlayEnabled());

		GUILayout.Label("BOverlayNeedsPresent() : " + SteamUtils.BOverlayNeedsPresent());

		if (GUILayout.Button("CheckFileSignature(\"FileNotFound.txt\")")) {
			SteamAPICall_t handle = SteamUtils.CheckFileSignature("FileNotFound.txt");
			OnCheckFileSignatureCallResult.Set(handle);
			print("SteamUtils.CheckFileSignature(" + "\"FileNotFound.txt\"" + ") : " + handle);
		}

		if (GUILayout.Button("ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, \"Description Test!\", 32, \"test\")")) {
			bool ret = SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, "Description Test!", 32, "test");
			print("SteamUtils.ShowGamepadTextInput(" + EGamepadTextInputMode.k_EGamepadTextInputModeNormal + ", " + EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine + ", " + "\"Description Test!\"" + ", " + 32 + ", " + "\"test\"" + ") : " + ret);
		}

		// Only called from within GamepadTextInputDismissed_t Callback!
		/*GUILayout.Label("SteamUtils.GetEnteredGamepadTextLength() : " + SteamUtils.GetEnteredGamepadTextLength());

		{
			string Text;
			bool ret = SteamUtils.GetEnteredGamepadTextInput(out Text, 32);
			GUILayout.Label("SteamUtils.GetEnteredGamepadTextInput(out Text, 32) - " + ret + " -- " + Text);
		}*/

		GUILayout.Label("GetSteamUILanguage() : " + SteamUtils.GetSteamUILanguage());

		GUILayout.Label("IsSteamRunningInVR() : " + SteamUtils.IsSteamRunningInVR());

		if (GUILayout.Button("SetOverlayNotificationInset(400, 400)")) {
			SteamUtils.SetOverlayNotificationInset(400, 400);
			print("SteamUtils.SetOverlayNotificationInset(" + 400 + ", " + 400 + ")");
		}

		GUILayout.Label("IsSteamInBigPictureMode() : " + SteamUtils.IsSteamInBigPictureMode());

		if (GUILayout.Button("StartVRDashboard()")) {
			SteamUtils.StartVRDashboard();
			print("SteamUtils.StartVRDashboard()");
		}
	}

	void OnIPCountry(IPCountry_t pCallback) {
		Debug.Log("[" + IPCountry_t.k_iCallback + " - IPCountry]");
	}

	void OnLowBatteryPower(LowBatteryPower_t pCallback) {
		Debug.Log("[" + LowBatteryPower_t.k_iCallback + " - LowBatteryPower] - " + pCallback.m_nMinutesBatteryLeft);
	}

	//void OnSteamAPICallCompleted(SteamAPICallCompleted_t pCallback) {
	//	Debug.Log("[" + SteamAPICallCompleted_t.k_iCallback + " - SteamAPICallCompleted] - " + pCallback.m_hAsyncCall + " -- " + pCallback.m_iCallback + " -- " + pCallback.m_cubParam);
	//}

	void OnSteamShutdown(SteamShutdown_t pCallback) {
		Debug.Log("[" + SteamShutdown_t.k_iCallback + " - SteamShutdown]");
	}

	void OnCheckFileSignature(CheckFileSignature_t pCallback, bool bIOFailure) {
		Debug.Log("[" + CheckFileSignature_t.k_iCallback + " - CheckFileSignature] - " + pCallback.m_eCheckFileSignature);
	}

	void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t pCallback) {
		Debug.Log("[" + GamepadTextInputDismissed_t.k_iCallback + " - GamepadTextInputDismissed] - " + pCallback.m_bSubmitted + " -- " + pCallback.m_unSubmittedText);

		if(pCallback.m_bSubmitted) {
			uint Length = SteamUtils.GetEnteredGamepadTextLength();
			Debug.Log("SteamUtils.GetEnteredGamepadTextLength() - " + Length);

			string Text;
			bool ret = SteamUtils.GetEnteredGamepadTextInput(out Text, pCallback.m_unSubmittedText + 1);
			Debug.Log("SteamUtils.GetEnteredGamepadTextInput(out Text, pCallback.m_unSubmittedText + 1) - " + ret + " -- " + Text);
		}
	}
}