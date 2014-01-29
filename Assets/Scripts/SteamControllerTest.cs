using UnityEngine;
using System.Collections;
using Steamworks;
using System.IO;

public class SteamControllerTest : MonoBehaviour {

	public void OnEnable() {
	}

	public void RenderOnGUI() {
		// We already Init and Shutdown in SteamTest.cs
		/*if (GUILayout.Button("Init(Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamController.Init(Application.dataPath + "/controller.vdf");
			print("Init(" + Application.dataPath + "/controller.vdf" + ") - " + ret);
		}

		if (GUILayout.Button("Shutdown()")) {
			bool ret = SteamController.Shutdown();
			print("Shutdown() - " + ret);
		}*/

		{
			SteamControllerState_t State;
			bool ret = SteamController.GetControllerState(0, out State);
			if (ret) {
				GUILayout.Label("GetControllerState(0, out controllerState) - " + ret);
				GUILayout.Label("SteamControllerState_t.unPacketNum - " + State.unPacketNum);
				GUILayout.Label("SteamControllerState_t.ulButtons - " + State.ulButtons);
				GUILayout.Label("STEAM_RIGHT_TRIGGER_MASK - " + (State.ulButtons & Constants.STEAM_RIGHT_TRIGGER_MASK));
				GUILayout.Label("STEAM_LEFT_TRIGGER_MASK - " + (State.ulButtons & Constants.STEAM_LEFT_TRIGGER_MASK));
				GUILayout.Label("STEAM_RIGHT_BUMPER_MASK - " + (State.ulButtons & Constants.STEAM_RIGHT_BUMPER_MASK));
				GUILayout.Label("STEAM_LEFT_BUMPER_MASK - " + (State.ulButtons & Constants.STEAM_LEFT_BUMPER_MASK));
				GUILayout.Label("STEAM_BUTTON_0_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_0_MASK));
				GUILayout.Label("STEAM_BUTTON_1_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_1_MASK));
				GUILayout.Label("STEAM_BUTTON_2_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_2_MASK));
				GUILayout.Label("STEAM_BUTTON_3_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_3_MASK));
				GUILayout.Label("STEAM_TOUCH_0_MASK - " + (State.ulButtons & Constants.STEAM_TOUCH_0_MASK));
				GUILayout.Label("STEAM_TOUCH_1_MASK - " + (State.ulButtons & Constants.STEAM_TOUCH_1_MASK));
				GUILayout.Label("STEAM_TOUCH_2_MASK - " + (State.ulButtons & Constants.STEAM_TOUCH_2_MASK));
				GUILayout.Label("STEAM_TOUCH_3_MASK - " + (State.ulButtons & Constants.STEAM_TOUCH_3_MASK));
				GUILayout.Label("STEAM_BUTTON_MENU_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_MENU_MASK));
				GUILayout.Label("STEAM_BUTTON_STEAM_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_STEAM_MASK));
				GUILayout.Label("STEAM_BUTTON_ESCAPE_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_ESCAPE_MASK));
				GUILayout.Label("STEAM_BUTTON_BACK_LEFT_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_BACK_LEFT_MASK));
				GUILayout.Label("STEAM_BUTTON_BACK_RIGHT_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_BACK_RIGHT_MASK));
				GUILayout.Label("STEAM_BUTTON_LEFTPAD_CLICKED_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_LEFTPAD_CLICKED_MASK));
				GUILayout.Label("STEAM_BUTTON_RIGHTPAD_CLICKED_MASK - " + (State.ulButtons & Constants.STEAM_BUTTON_RIGHTPAD_CLICKED_MASK));
				GUILayout.Label("STEAM_LEFTPAD_FINGERDOWN_MASK - " + (State.ulButtons & Constants.STEAM_LEFTPAD_FINGERDOWN_MASK));
				GUILayout.Label("STEAM_RIGHTPAD_FINGERDOWN_MASK - " + (State.ulButtons & Constants.STEAM_RIGHTPAD_FINGERDOWN_MASK));
				GUILayout.Label("SteamControllerState_t.sLeftPadX - " + State.sLeftPadX);
				GUILayout.Label("SteamControllerState_t.sLeftPadY - " + State.sLeftPadY);
				GUILayout.Label("SteamControllerState_t.sRightPadX - " + State.sRightPadX);
				GUILayout.Label("SteamControllerState_t.sRightPadY - " + State.sRightPadY);
			}
		}

		if (GUILayout.Button("TriggerHapticPulse(0, k_ESteamControllerPad_Right, 1000)")) {
			SteamController.TriggerHapticPulse(0, ESteamControllerPad.k_ESteamControllerPad_Right, 1000);
			print("SteamController.TriggerHapticPulse(0, ESteamControllerPad.k_ESteamControllerPad_Right, 1000)");
		}

		if (GUILayout.Button("SetOverrideMode(\"menu\")")) {
			SteamController.SetOverrideMode("menu");
			print("SetOverrideMode(\"menu\")");
		}
	}
}
