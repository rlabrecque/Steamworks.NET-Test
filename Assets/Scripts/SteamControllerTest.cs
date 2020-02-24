using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamControllerTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private bool m_ControllerInitialized;
	private int m_nControllers;

	public void OnEnable() {
		m_ControllerInitialized = SteamController.Init();
		print("SteamController.Init() - " + m_ControllerInitialized);
		m_ControllerHandles = new ControllerHandle_t[Constants.STEAM_CONTROLLER_MAX_COUNT];

		if (m_ControllerInitialized) {
			Precache();
		}

		// TODO: Activate some default ActionSet?

	}

	void OnDisable() {
		m_ControllerInitialized = false;
		print("SteamController.Shutdown() - " + SteamController.Shutdown());
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

	ControllerActionSetHandle_t[] m_ActionSets;
	ControllerAnalogActionHandle_t[] m_InGameControlsAnalogActions;
	ControllerDigitalActionHandle_t[] m_InGameControlsDigitalActions;
	ControllerDigitalActionHandle_t[] m_MenuControlsDigitalActions;

	ControllerHandle_t[] m_ControllerHandles;

	void Precache() {
		// ActionSets
		m_ActionSetNames = System.Enum.GetNames(typeof(EActionSets));
		m_nActionSets = m_ActionSetNames.Length;
		m_ActionSets = new ControllerActionSetHandle_t[m_nActionSets];

		for(int i = 0; i < m_nActionSets; ++i) {
			m_ActionSets[i] = SteamController.GetActionSetHandle(m_ActionSetNames[i]);
			print("SteamController.GetActionSetHandle(" + m_ActionSetNames[i] + ") - " + m_ActionSets[i]);
		}

		// Actions

		// InGameControls Analog Actions
		m_InGameControlsAnalogActionNames = System.Enum.GetNames(typeof(EAnalogActions_InGameControls));
		m_nInGameControlsAnalogActions = m_InGameControlsAnalogActionNames.Length;
		m_InGameControlsAnalogActions = new ControllerAnalogActionHandle_t[m_nInGameControlsAnalogActions];

		for (int i = 0; i < m_nInGameControlsAnalogActions; ++i) {
			m_InGameControlsAnalogActions[i] = SteamController.GetAnalogActionHandle(m_InGameControlsAnalogActionNames[i]);
			print("SteamController.GetAnalogActionHandle(" + m_InGameControlsAnalogActionNames[i] + ") - " + m_InGameControlsAnalogActions[i]);
		}

		// InGameControls Digital Actions
		m_InGameControlsDigitalActionNames = System.Enum.GetNames(typeof(EDigitalActions_InGameControls));
		m_nInGameControlsDigitalActions = m_InGameControlsDigitalActionNames.Length;
		m_InGameControlsDigitalActions = new ControllerDigitalActionHandle_t[m_nInGameControlsDigitalActions];

		for (int i = 0; i < m_nInGameControlsDigitalActions; ++i) {
			m_InGameControlsDigitalActions[i] = SteamController.GetDigitalActionHandle(m_InGameControlsDigitalActionNames[i]);
			print("SteamController.GetDigitalActionHandle(" + m_InGameControlsDigitalActionNames[i] + ") - " + m_InGameControlsDigitalActions[i]);
		}

		// MenuControls Digital Actions
		m_MenuControlsDigitalActionNames = System.Enum.GetNames(typeof(EDigitalActions_MenuControls));
		m_nMenuControlsDigitalActions = m_MenuControlsDigitalActionNames.Length;
		m_MenuControlsDigitalActions = new ControllerDigitalActionHandle_t[m_nMenuControlsDigitalActions];

		for (int i = 0; i < m_nMenuControlsDigitalActions; ++i) {
			m_MenuControlsDigitalActions[i] = SteamController.GetDigitalActionHandle(m_MenuControlsDigitalActionNames[i]);
			print("SteamController.GetDigitalActionHandle(" + m_MenuControlsDigitalActionNames[i] + ") - " + m_MenuControlsDigitalActions[i]);
		}
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_ControllerInitialized: " + m_ControllerInitialized);
		GUILayout.Label("m_nControllers: " + m_nControllers);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		if (!m_ControllerInitialized) {
			return;
		}

		//SteamController.Shutdown() // Called in OnDisable()

		//SteamController.RunFrame() // N/A - This is called automatically by SteamAPI.RunCallbacks()

		{
			m_nControllers = SteamController.GetConnectedControllers(m_ControllerHandles);
			GUILayout.Label("GetConnectedControllers(m_ControllerHandles) : " + m_nControllers);
		}

		//SteamController.GetActionSetHandle() // Called in Precache()

		for (int i = 0; i < m_nControllers; ++i) {
			GUILayout.Label("Controller " + i + " - " + m_ControllerHandles[i]);
		
			for (int j = 0; j < m_nActionSets; ++j) {
				if (GUILayout.Button("ActivateActionSet(m_ControllerHandles[i], m_ActionSets[j])")) {
					SteamController.ActivateActionSet(m_ControllerHandles[i], m_ActionSets[j]);
					print("SteamController.ActivateActionSet(" + m_ControllerHandles[i] + ", " + m_ActionSets[j] + ")");
				}
			}

			GUILayout.Label("GetCurrentActionSet(m_ControllerHandles[i]) : " + SteamController.GetCurrentActionSet(m_ControllerHandles[i]));

			//SteamController.ActivateActionSetLayer() // TODO

			//SteamController.DeactivateActionSetLayer() // TODO

			//SteamController.DeactivateAllActionSetLayers() // TODO

			//SteamController.GetActiveActionSetLayers() // TODO

			//SteamController.GetDigitalActionHandle() // Called in Precache()

			GUILayout.Label("InGameControls Digital Actions:");
			for (int j = 0; j < m_nInGameControlsDigitalActions; ++j) {
				InputDigitalActionData_t ret = SteamController.GetDigitalActionData(m_ControllerHandles[i], m_InGameControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_ControllerHandles[i] + ", " + m_InGameControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_InGameControlsDigitalActionNames[j]);
			}

			GUILayout.Label("MenuControls Digital Actions:");
			for (int j = 0; j < m_nMenuControlsDigitalActions; ++j) {
				InputDigitalActionData_t ret = SteamController.GetDigitalActionData(m_ControllerHandles[i], m_MenuControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_ControllerHandles[i] + ", " + m_MenuControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_MenuControlsDigitalActionNames[j]);
			}

			if (GUILayout.Button("GetDigitalActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins)")) {
				EControllerActionOrigin[] origins = new EControllerActionOrigin[Constants.STEAM_CONTROLLER_MAX_ORIGINS];
				int ret = SteamController.GetDigitalActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins);
				print("SteamController.GetDigitalActionOrigins(" + m_ControllerHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire] + ", " + origins + ") : " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsDigitalActionNames[(int)EDigitalActions_InGameControls.fire]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			//SteamController.GetAnalogActionHandle() // Called in Precache()

			GUILayout.Label("InGameControls Analog Actions:");
			for (int j = 0; j < m_nInGameControlsAnalogActions; ++j) {
				GUILayout.Label("GetAnalogActionData(m_ControllerHandles[i], m_InGameControlsAnalogActions[j]) : " + SteamController.GetAnalogActionData(m_ControllerHandles[i], m_InGameControlsAnalogActions[j]));
			}

			if (GUILayout.Button("GetAnalogActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins)")) {
				EControllerActionOrigin[] origins = new EControllerActionOrigin[Constants.STEAM_CONTROLLER_MAX_ORIGINS];
				int ret = SteamController.GetAnalogActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins);
				print("SteamController.GetAnalogActionOrigins(" + m_ControllerHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle] + ", " + origins + ") : " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsAnalogActionNames[(int)EAnalogActions_InGameControls.Throttle]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			if (GUILayout.Button("GetGlyphForActionOrigin(EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A)")) {
				string ret = SteamController.GetGlyphForActionOrigin(EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A);
				print("SteamController.GetGlyphForActionOrigin(" + EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A + ") : " + ret);
			}

			if (GUILayout.Button("GetStringForActionOrigin(EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A)")) {
				string ret = SteamController.GetStringForActionOrigin(EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A);
				print("SteamController.GetStringForActionOrigin(" + EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A + ") : " + ret);
			}

			GUILayout.Label("InGameControls Analog Actions:");
			for (int j = 0; j < m_nInGameControlsAnalogActions; ++j) {
				if (GUILayout.Button("StopAnalogActionMomentum(m_ControllerHandles[i], m_InGameControlsAnalogActions[j])")) {
					SteamController.StopAnalogActionMomentum(m_ControllerHandles[i], m_InGameControlsAnalogActions[j]);
					print("SteamController.StopAnalogActionMomentum(" + m_ControllerHandles[i] + ", " + m_InGameControlsAnalogActions[j] + ")");
				}
			}

			if (GUILayout.Button("GetMotionData(m_ControllerHandles[i])")) {
				InputMotionData_t ret = SteamController.GetMotionData(m_ControllerHandles[i]);
				print("SteamController.GetMotionData(" + m_ControllerHandles[i] + ") : " + ret);
			}

			if (GUILayout.Button("TriggerHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000)")) {
				SteamController.TriggerHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000);
				print("SteamController.TriggerHapticPulse(" + m_ControllerHandles[i] + ", " + ESteamControllerPad.k_ESteamControllerPad_Right + ", " + 5000 + ")");
			}

			if (GUILayout.Button("TriggerRepeatedHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0)")) {
				SteamController.TriggerRepeatedHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0);
				print("SteamController.TriggerRepeatedHapticPulse(" + m_ControllerHandles[i] + ", " + ESteamControllerPad.k_ESteamControllerPad_Right + ", " + 5000 + ", " + 0 + ", " + 0 + ", " + 0 + ")");
			}

			if (GUILayout.Button("TriggerVibration(m_ControllerHandles[i], ushort.MaxValue, ushort.MaxValue)")) {
				SteamController.TriggerVibration(m_ControllerHandles[i], ushort.MaxValue, ushort.MaxValue);
				print("SteamController.TriggerVibration(" + m_ControllerHandles[i] + ", " + ushort.MaxValue + ", " + ushort.MaxValue + ")");
			}

			if (GUILayout.Button("SetLEDColor(m_ControllerHandles[i], 0, 0, 255, (int)ESteamControllerLEDFlag.k_ESteamControllerLEDFlag_SetColor)")) {
				SteamController.SetLEDColor(m_ControllerHandles[i], 0, 0, 255, (int)ESteamControllerLEDFlag.k_ESteamControllerLEDFlag_SetColor);
				print("SteamController.SetLEDColor(" + m_ControllerHandles[i] + ", " + 0 + ", " + 0 + ", " + 255 + ", " + (int)ESteamControllerLEDFlag.k_ESteamControllerLEDFlag_SetColor + ")");
			}

			if (GUILayout.Button("ShowBindingPanel(m_ControllerHandles[i])")) {
				bool ret = SteamController.ShowBindingPanel(m_ControllerHandles[i]);
				print("SteamController.ShowBindingPanel(" + m_ControllerHandles[i] + ") : " + ret);
			}

			GUILayout.Label("GetInputTypeForHandle(m_ControllerHandles[i]) : " + SteamController.GetInputTypeForHandle(m_ControllerHandles[i]));

			GUILayout.Label("GetControllerForGamepadIndex(0) : " + SteamController.GetControllerForGamepadIndex(0));

			GUILayout.Label("GetGamepadIndexForController(m_ControllerHandles[i]) : " + SteamController.GetGamepadIndexForController(m_ControllerHandles[i]));

			if (GUILayout.Button("GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)")) {
				string ret = SteamController.GetStringForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				print("SteamController.GetStringForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A)")) {
				string ret = SteamController.GetGlyphForXboxOrigin(EXboxOrigin.k_EXboxOrigin_A);
				print("SteamController.GetGlyphForXboxOrigin(" + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("GetActionOriginFromXboxOrigin(m_ControllerHandles[i], EXboxOrigin.k_EXboxOrigin_A)")) {
				EControllerActionOrigin ret = SteamController.GetActionOriginFromXboxOrigin(m_ControllerHandles[i], EXboxOrigin.k_EXboxOrigin_A);
				print("SteamController.GetActionOriginFromXboxOrigin(" + m_ControllerHandles[i] + ", " + EXboxOrigin.k_EXboxOrigin_A + ") : " + ret);
			}

			if (GUILayout.Button("TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A)")) {
				EControllerActionOrigin ret = SteamController.TranslateActionOrigin(ESteamInputType.k_ESteamInputType_XBoxOneController, EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A);
				print("SteamController.TranslateActionOrigin(" + ESteamInputType.k_ESteamInputType_XBoxOneController + ", " + EControllerActionOrigin.k_EControllerActionOrigin_XBoxOne_A + ") : " + ret);
			}

			if (GUILayout.Button("GetControllerBindingRevision(m_ControllerHandles[i], out pMajor, out pMinor)")) {
				int pMajor;
				int pMinor;
				bool ret = SteamController.GetControllerBindingRevision(m_ControllerHandles[i], out pMajor, out pMinor);
				print("SteamController.GetControllerBindingRevision(" + m_ControllerHandles[i] + ", " + "out pMajor" + ", " + "out pMinor" + ") : " + ret + " -- " + pMajor + " -- " + pMinor);
			}
			}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

}