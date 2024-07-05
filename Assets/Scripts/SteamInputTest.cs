using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamInputTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private bool m_InputInitialized;
	private int m_nInputs;

	protected Callback<SteamInputDeviceConnected_t> m_SteamInputDeviceConnected;
	protected Callback<SteamInputDeviceDisconnected_t> m_SteamInputDeviceDisconnected;
	protected Callback<SteamInputConfigurationLoaded_t> m_SteamInputConfigurationLoaded;
	protected Callback<SteamInputGamepadSlotChange_t> m_SteamInputGamepadSlotChange;

	public void OnEnable() {
		m_InputInitialized = SteamInput.Init(false);
		print("SteamInput.Init() - " + m_InputInitialized);
		m_InputHandles = new InputHandle_t[Constants.STEAM_INPUT_MAX_COUNT];

		if (m_InputInitialized) {
			SteamInput.EnableDeviceCallbacks();
			Precache();
		}

		// TODO: Activate some default ActionSet?

		m_SteamInputDeviceConnected = Callback<SteamInputDeviceConnected_t>.Create(OnSteamInputDeviceConnected);
		m_SteamInputDeviceDisconnected = Callback<SteamInputDeviceDisconnected_t>.Create(OnSteamInputDeviceDisconnected);
		m_SteamInputConfigurationLoaded = Callback<SteamInputConfigurationLoaded_t>.Create(OnSteamInputConfigurationLoaded);
		m_SteamInputGamepadSlotChange = Callback<SteamInputGamepadSlotChange_t>.Create(OnSteamInputGamepadSlotChange);
	}

	void OnDisable() {
		m_InputInitialized = false;
		print("SteamInput.Shutdown() - " + SteamInput.Shutdown());
	}

	enum EActionSets {
		InGameControls,
		MenuControls,
	}

	enum EAnalogActions_InGameControls {
		Move,
		Camera,
		Throttle,
	}

	enum EDigitalActions_InGameControls {
		fire,
		Jump,
		pause_menu,
	}

	enum EDigitalActions_MenuControls {
		menu_up,
		menu_down,
		menu_left,
		menu_right,
		menu_select,
		menu_cancel,
		pause_menu,
	}

	int m_nActionSets;
	int m_nInGameControlsAnalogActions;
	int m_nInGameControlsDigitalActions;
	int m_nMenuControlsDigitalActions;

	string[] m_ActionSetNames;
	string[] m_InGameControlsAnalogActionNames;
	string[] m_InGameControlsDigitalActionNames;
	string[] m_MenuControlsDigitalActionNames;

	InputActionSetHandle_t[] m_ActionSets;
	InputAnalogActionHandle_t[] m_InGameControlsAnalogActions;
	InputDigitalActionHandle_t[] m_InGameControlsDigitalActions;
	InputDigitalActionHandle_t[] m_MenuControlsDigitalActions;

	InputHandle_t[] m_InputHandles;

	void Precache() {
		// ActionSets
		m_ActionSetNames = System.Enum.GetNames(typeof(EActionSets));
		m_nActionSets = m_ActionSetNames.Length;
		m_ActionSets = new InputActionSetHandle_t[m_nActionSets];

		for(int i = 0; i < m_nActionSets; ++i) {
			m_ActionSets[i] = SteamInput.GetActionSetHandle(m_ActionSetNames[i]);
			print("SteamInput.GetActionSetHandle(" + m_ActionSetNames[i] + ") - " + m_ActionSets[i]);
		}

		// Actions

		// InGameControls Analog Actions
		m_InGameControlsAnalogActionNames = System.Enum.GetNames(typeof(EAnalogActions_InGameControls));
		m_nInGameControlsAnalogActions = m_InGameControlsAnalogActionNames.Length;
		m_InGameControlsAnalogActions = new InputAnalogActionHandle_t[m_nInGameControlsAnalogActions];

		for (int i = 0; i < m_nInGameControlsAnalogActions; ++i) {
			m_InGameControlsAnalogActions[i] = SteamInput.GetAnalogActionHandle(m_InGameControlsAnalogActionNames[i]);
			print("SteamInput.GetAnalogActionHandle(" + m_InGameControlsAnalogActionNames[i] + ") - " + m_InGameControlsAnalogActions[i]);
		}

		// InGameControls Digital Actions
		m_InGameControlsDigitalActionNames = System.Enum.GetNames(typeof(EDigitalActions_InGameControls));
		m_nInGameControlsDigitalActions = m_InGameControlsDigitalActionNames.Length;
		m_InGameControlsDigitalActions = new InputDigitalActionHandle_t[m_nInGameControlsDigitalActions];

		for (int i = 0; i < m_nInGameControlsDigitalActions; ++i) {
			m_InGameControlsDigitalActions[i] = SteamInput.GetDigitalActionHandle(m_InGameControlsDigitalActionNames[i]);
			print("SteamInput.GetDigitalActionHandle(" + m_InGameControlsDigitalActionNames[i] + ") - " + m_InGameControlsDigitalActions[i]);
		}

		// MenuControls Digital Actions
		m_MenuControlsDigitalActionNames = System.Enum.GetNames(typeof(EDigitalActions_MenuControls));
		m_nMenuControlsDigitalActions = m_MenuControlsDigitalActionNames.Length;
		m_MenuControlsDigitalActions = new InputDigitalActionHandle_t[m_nMenuControlsDigitalActions];

		for (int i = 0; i < m_nMenuControlsDigitalActions; ++i) {
			m_MenuControlsDigitalActions[i] = SteamInput.GetDigitalActionHandle(m_MenuControlsDigitalActionNames[i]);
			print("SteamInput.GetDigitalActionHandle(" + m_MenuControlsDigitalActionNames[i] + ") - " + m_MenuControlsDigitalActions[i]);
		}
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_InputInitialized: " + m_InputInitialized);
		GUILayout.Label("m_nInputs: " + m_nInputs);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		if (!m_InputInitialized) {
			return;
		}

		//SteamInput.Shutdown() // Called in OnDisable()

		if (GUILayout.Button("SetInputActionManifestFilePath(\"\")")) {
			bool ret = SteamInput.SetInputActionManifestFilePath("");
			print("SteamInput.SetInputActionManifestFilePath(" + "\"\"" + ") : " + ret);
		}

		//SteamInput.RunFrame() // N/A - This is called automatically by SteamAPI.RunCallbacks()

		//SteamInput.BWaitForData() // N/A - Only should be called when using a dedicated Input thread.

		GUILayout.Label("BNewDataAvailable() : " + SteamInput.BNewDataAvailable());

		{
			m_nInputs = SteamInput.GetConnectedControllers(m_InputHandles);
			GUILayout.Label("GetConnectedControllers(m_InputHandles) : " + m_nInputs);
		}

		//SteamInput.EnableDeviceCallbacks() // Called in OnEnable()

		//SteamInput.EnableActionEventCallbacks() // TODO

		//SteamInput.GetActionSetHandle() // Called in Precache()

		for (int i = 0; i < m_nInputs; ++i) {
			GUILayout.Label("Input " + i + " - " + m_InputHandles[i]);
		
			for (int j = 0; j < m_nActionSets; ++j) {
				if (GUILayout.Button("ActivateActionSet(m_InputHandles[i], m_ActionSets[j])")) {
					SteamInput.ActivateActionSet(m_InputHandles[i], m_ActionSets[j]);
					print("SteamInput.ActivateActionSet(" + m_InputHandles[i] + ", " + m_ActionSets[j] + ")");
				}
			}

			GUILayout.Label("GetCurrentActionSet(m_InputHandles[i]) : " + SteamInput.GetCurrentActionSet(m_InputHandles[i]));

			//SteamInput.ActivateActionSetLayer() // TODO

			//SteamInput.DeactivateActionSetLayer() // TODO

			//SteamInput.DeactivateAllActionSetLayers() // TODO

			//SteamInput.GetActiveActionSetLayers() // TODO

			//SteamInput.GetDigitalActionHandle() // Called in Precache()

			GUILayout.Label("InGameControls Digital Actions:");
			for (int j = 0; j < m_nInGameControlsDigitalActions; ++j) {
				InputDigitalActionData_t ret = SteamInput.GetDigitalActionData(m_InputHandles[i], m_InGameControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_InputHandles[i] + ", " + m_InGameControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_InGameControlsDigitalActionNames[j]);
			}

			GUILayout.Label("MenuControls Digital Actions:");
			for (int j = 0; j < m_nMenuControlsDigitalActions; ++j) {
				InputDigitalActionData_t ret = SteamInput.GetDigitalActionData(m_InputHandles[i], m_MenuControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_InputHandles[i] + ", " + m_MenuControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_MenuControlsDigitalActionNames[j]);
			}

			if (GUILayout.Button("GetDigitalActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins)")) {
				EInputActionOrigin[] origins = new EInputActionOrigin[Constants.STEAM_INPUT_MAX_ORIGINS];
				int ret = SteamInput.GetDigitalActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins);
				print("SteamInput.GetDigitalActionOrigins(" + m_InputHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire] + ", " + origins + ") : " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsDigitalActionNames[(int)EDigitalActions_InGameControls.fire]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			if (GUILayout.Button("GetStringForDigitalActionName(m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire])")) {
				string ret = SteamInput.GetStringForDigitalActionName(m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire]);
				print("SteamInput.GetStringForDigitalActionName(" + m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire] + ") : " + ret);
			}

			//SteamInput.GetAnalogActionHandle() // Called in Precache()

			GUILayout.Label("InGameControls Analog Actions:");
			for (int j = 0; j < m_nInGameControlsAnalogActions; ++j) {
				GUILayout.Label("GetAnalogActionData(m_InputHandles[i], m_InGameControlsAnalogActions[j]) : " + SteamInput.GetAnalogActionData(m_InputHandles[i], m_InGameControlsAnalogActions[j]));
			}

			if (GUILayout.Button("GetAnalogActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins)")) {
				EInputActionOrigin[] origins = new EInputActionOrigin[Constants.STEAM_INPUT_MAX_ORIGINS];
				int ret = SteamInput.GetAnalogActionOrigins(m_InputHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins);
				print("SteamInput.GetAnalogActionOrigins(" + m_InputHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle] + ", " + origins + ") : " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsAnalogActionNames[(int)EAnalogActions_InGameControls.Throttle]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			if (GUILayout.Button("GetGlyphPNGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small, 0)")) {
				string ret = SteamInput.GetGlyphPNGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small, 0);
				print("SteamInput.GetGlyphPNGForActionOrigin(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A + ", " + ESteamInputGlyphSize.k_ESteamInputGlyphSize_Small + ", " + 0 + ") : " + ret);
			}

			if (GUILayout.Button("GetGlyphSVGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, 0)")) {
				string ret = SteamInput.GetGlyphSVGForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A, 0);
				print("SteamInput.GetGlyphSVGForActionOrigin(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A + ", " + 0 + ") : " + ret);
			}

			if (GUILayout.Button("GetGlyphForActionOrigin_Legacy(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A)")) {
				string ret = SteamInput.GetGlyphForActionOrigin_Legacy(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A);
				print("SteamInput.GetGlyphForActionOrigin_Legacy(" + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A + ") : " + ret);
			}

			GUILayout.Label("GetStringForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A) : " + SteamInput.GetStringForActionOrigin(EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A));

			GUILayout.Label("GetStringForAnalogActionName(m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle]) : " + SteamInput.GetStringForAnalogActionName(m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle]));

			GUILayout.Label("InGameControls Analog Actions:");
			for (int j = 0; j < m_nInGameControlsAnalogActions; ++j) {
				if (GUILayout.Button("StopAnalogActionMomentum(m_InputHandles[i], m_InGameControlsAnalogActions[j])")) {
					SteamInput.StopAnalogActionMomentum(m_InputHandles[i], m_InGameControlsAnalogActions[j]);
					print("SteamInput.StopAnalogActionMomentum(" + m_InputHandles[i] + ", " + m_InGameControlsAnalogActions[j] + ")");
				}
			}

			if (GUILayout.Button("GetMotionData(m_InputHandles[i])")) {
				InputMotionData_t ret = SteamInput.GetMotionData(m_InputHandles[i]);
				print("SteamInput.GetMotionData(" + m_InputHandles[i] + ") : " + ret);
			}

			if (GUILayout.Button("TriggerVibration(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue)")) {
				SteamInput.TriggerVibration(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue);
				print("SteamInput.TriggerVibration(" + m_InputHandles[i] + ", " + ushort.MaxValue + ", " + ushort.MaxValue + ")");
			}

			if (GUILayout.Button("TriggerVibrationExtended(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue)")) {
				SteamInput.TriggerVibrationExtended(m_InputHandles[i], ushort.MaxValue, ushort.MaxValue, ushort.MaxValue, ushort.MaxValue);
				print("SteamInput.TriggerVibrationExtended(" + m_InputHandles[i] + ", " + ushort.MaxValue + ", " + ushort.MaxValue + ", " + ushort.MaxValue + ", " + ushort.MaxValue + ")");
			}

			//SteamInput.TriggerSimpleHapticEvent() // TODO

			if (GUILayout.Button("SetLEDColor(m_InputHandles[i], 0, 0, 255, (int)ESteamInputLEDFlag.k_ESteamInputLEDFlag_SetColor)")) {
				SteamInput.SetLEDColor(m_InputHandles[i], 0, 0, 255, (int)ESteamInputLEDFlag.k_ESteamInputLEDFlag_SetColor);
				print("SteamInput.SetLEDColor(" + m_InputHandles[i] + ", " + 0 + ", " + 0 + ", " + 255 + ", " + (int)ESteamInputLEDFlag.k_ESteamInputLEDFlag_SetColor + ")");
			}

			if (GUILayout.Button("Legacy_TriggerHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000)")) {
				SteamInput.Legacy_TriggerHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000);
				print("SteamInput.Legacy_TriggerHapticPulse(" + m_InputHandles[i] + ", " + ESteamControllerPad.k_ESteamControllerPad_Right + ", " + 5000 + ")");
			}

			if (GUILayout.Button("Legacy_TriggerRepeatedHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0)")) {
				SteamInput.Legacy_TriggerRepeatedHapticPulse(m_InputHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0);
				print("SteamInput.Legacy_TriggerRepeatedHapticPulse(" + m_InputHandles[i] + ", " + ESteamControllerPad.k_ESteamControllerPad_Right + ", " + 5000 + ", " + 0 + ", " + 0 + ", " + 0 + ")");
			}

			if (GUILayout.Button("ShowBindingPanel(m_InputHandles[i])")) {
				bool ret = SteamInput.ShowBindingPanel(m_InputHandles[i]);
				print("SteamInput.ShowBindingPanel(" + m_InputHandles[i] + ") : " + ret);
			}

			GUILayout.Label("GetInputTypeForHandle(m_InputHandles[i]) : " + SteamInput.GetInputTypeForHandle(m_InputHandles[i]));

			GUILayout.Label("GetControllerForGamepadIndex(0) : " + SteamInput.GetControllerForGamepadIndex(0));

			GUILayout.Label("GetGamepadIndexForController(m_InputHandles[i]) : " + SteamInput.GetGamepadIndexForController(m_InputHandles[i]));

			if (GUILayout.Button("GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)")) {
				string ret = SteamInput.GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				print("SteamInput.GetStringForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)")) {
				string ret = SteamInput.GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				print("SteamInput.GetGlyphForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("GetActionOriginFromXboxOrigin(m_InputHandles[i], EXboxOrigin.k_EXboxOrigin_A)")) {
				EInputActionOrigin ret = SteamInput.GetActionOriginFromXboxOrigin(m_InputHandles[i], EXboxOrigin.k_EXboxOrigin_A);
				print("SteamInput.GetActionOriginFromXboxOrigin(" + m_InputHandles[i] + ", " + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A)")) {
				EInputActionOrigin ret = SteamInput.TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A);
				print("SteamInput.TranslateActionOrigin(" + ESteamInputType.k_ESteamInputType_XBoxOneController + ", " + EInputActionOrigin.k_EInputActionOrigin_XBoxOne_A + ") : " + ret);
			}

			if (GUILayout.Button("GetDeviceBindingRevision(m_InputHandles[i], out pMajor, out pMinor)")) {
				int pMajor;
				int pMinor;
				bool ret = SteamInput.GetDeviceBindingRevision(m_InputHandles[i], out pMajor, out pMinor);
				print("SteamInput.GetDeviceBindingRevision(" + m_InputHandles[i] + ", " + "out pMajor" + ", " + "out pMinor" + ") : " + ret + " -- " + pMajor + " -- " + pMinor);
			}

			if (GUILayout.Button("GetRemotePlaySessionID(m_InputHandles[i])")) {
				uint ret = SteamInput.GetRemotePlaySessionID(m_InputHandles[i]);
				print("SteamInput.GetRemotePlaySessionID(" + m_InputHandles[i] + ") : " + ret);
			}
			}

			GUILayout.Label("GetSessionInputConfigurationSettings() : " + SteamInput.GetSessionInputConfigurationSettings());

			//SteamInput.SetDualSenseTriggerEffect() // Nearly unsupported as it relies on proprietary code.

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnSteamInputDeviceConnected(SteamInputDeviceConnected_t pCallback) {
		Debug.Log("[" + SteamInputDeviceConnected_t.k_iCallback + " - SteamInputDeviceConnected] - " + pCallback.m_ulConnectedDeviceHandle);
	}

	void OnSteamInputDeviceDisconnected(SteamInputDeviceDisconnected_t pCallback) {
		Debug.Log("[" + SteamInputDeviceDisconnected_t.k_iCallback + " - SteamInputDeviceDisconnected] - " + pCallback.m_ulDisconnectedDeviceHandle);
	}

	void OnSteamInputConfigurationLoaded(SteamInputConfigurationLoaded_t pCallback) {
		Debug.Log("[" + SteamInputConfigurationLoaded_t.k_iCallback + " - SteamInputConfigurationLoaded] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulDeviceHandle + " -- " + pCallback.m_ulMappingCreator + " -- " + pCallback.m_unMajorRevision + " -- " + pCallback.m_unMinorRevision + " -- " + pCallback.m_bUsesSteamInputAPI + " -- " + pCallback.m_bUsesGamepadAPI);
	}

	void OnSteamInputGamepadSlotChange(SteamInputGamepadSlotChange_t pCallback) {
		Debug.Log("[" + SteamInputGamepadSlotChange_t.k_iCallback + " - SteamInputGamepadSlotChange] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulDeviceHandle + " -- " + pCallback.m_eDeviceType + " -- " + pCallback.m_nOldGamepadSlot + " -- " + pCallback.m_nNewGamepadSlot);
	}
}