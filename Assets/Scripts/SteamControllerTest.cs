using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamControllerTest : MonoBehaviour {
	enum EActionSets {
		InGameControls,
		MenuControls,
	}
	string[] m_ActionSetNames;
	int m_nActionSets;
	ControllerActionSetHandle_t[] m_ActionSets;

	enum EAnalogActions_InGameControls {
		Move,
		Camera,
		Throttle,
	}
	string[] m_InGameControlsAnalogActionNames;
	int m_nInGameControlsAnalogActions;
	ControllerAnalogActionHandle_t[] m_InGameControlsAnalogActions;

	enum EDigitalActions_InGameControls {
		fire,
		Jump,
		pause_menu,
	}
	string[] m_InGameControlsDigitalActionNames;
	int m_nInGameControlsDigitalActions;
	ControllerDigitalActionHandle_t[] m_InGameControlsDigitalActions;

	enum EDigitalActions_MenuControls {
		menu_up,
		menu_down,
		menu_left,
		menu_right,
		menu_select,
		menu_cancel,
		pause_menu,
	}
	string[] m_MenuControlsDigitalActionNames;
	int m_nMenuControlsDigitalActions;
	ControllerDigitalActionHandle_t[] m_MenuControlsDigitalActions;
	
	bool m_ControllerInitialized;
	ControllerHandle_t[] m_ControllerHandles;

	void OnEnable() {
		m_ControllerInitialized = SteamController.Init();
		m_ControllerHandles = new ControllerHandle_t[Constants.STEAM_CONTROLLER_MAX_COUNT];
		print("SteamController.Init() - " + m_ControllerInitialized);

		if (m_ControllerInitialized) {
			Precache();
		}

		// TODO: Activate some default ActionSet?
	}

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

	void OnDisable() {
		m_ControllerInitialized = false;
		print("SteamController.Shutdown() - " + SteamController.Shutdown());
	}

	public void Update() {
		if (!m_ControllerInitialized) {
			return;
		}

		// Always call RunFrame when the Controller interface is initialized to poll for new data from Steam.
		// Do this even when no controllers are connected to find out when a controller gets connected.
		SteamController.RunFrame();
	}
	
	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Initialized: " + m_ControllerInitialized);
		GUILayout.Label("m_nActionSets: " + m_nActionSets);
		for (int i = 0; i < m_nActionSets; ++i) {
			GUILayout.Label(" " + m_ActionSetNames[i] + ": " + m_ActionSets[i]);
		}
		GUILayout.EndArea();
		
		if (!m_ControllerInitialized) {
			return;
		}
		
		int nControllers = SteamController.GetConnectedControllers(m_ControllerHandles);
		GUILayout.Label("GetConnectedControllers(m_ControllerHandles) - " + nControllers);

		for (int i = 0; i < nControllers; ++i) {
			GUILayout.Label("Controller " + i + " - " + m_ControllerHandles[i]);

			if (GUILayout.Button("ShowBindingPanel(m_ControllerHandles[i])")) {
				bool ret = SteamController.ShowBindingPanel(m_ControllerHandles[i]);
				print("SteamController.ShowBindingPanel(" + m_ControllerHandles[i] + ") - " + ret);
			}

			GUILayout.Label("GetCurrentActionSet(m_ControllerHandles[i]) - " + SteamController.GetCurrentActionSet(m_ControllerHandles[i]));

			for (int j = 0; j < m_nActionSets; ++j) {
				if (GUILayout.Button("ActivateActionSet(m_ControllerHandles[i], m_ActionSets[j]) - " + m_ActionSetNames[j])) {
					SteamController.ActivateActionSet(m_ControllerHandles[i], m_ActionSets[j]);
					print("SteamController.ActivateActionSet(" + m_ControllerHandles[i] + ",  " + m_ActionSets[j] + ")");
				}
			}

			GUILayout.Label("InGameControls Digital Actions:");
			for (int j = 0; j < m_nInGameControlsDigitalActions; ++j) {
				ControllerDigitalActionData_t ret = SteamController.GetDigitalActionData(m_ControllerHandles[i], m_InGameControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_ControllerHandles[i] + ", " + m_InGameControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_InGameControlsDigitalActionNames[j]);
			}

			GUILayout.Label("MenuControls Digital Actions:");
			for (int j = 0; j < m_nMenuControlsDigitalActions; ++j) {
				ControllerDigitalActionData_t ret = SteamController.GetDigitalActionData(m_ControllerHandles[i], m_MenuControlsDigitalActions[j]);
				GUILayout.Label("GetDigitalActionData(" + m_ControllerHandles[i] + ", " + m_MenuControlsDigitalActions[j] + ") - " + ret.bState + " -- " + ret.bActive + " -- " + m_MenuControlsDigitalActionNames[j]);
			}

			GUILayout.Label("InGameControls Analog Actions:");
			for (int j = 0; j < m_nInGameControlsAnalogActions; ++j) {
				ControllerAnalogActionData_t ret = SteamController.GetAnalogActionData(m_ControllerHandles[i], m_InGameControlsAnalogActions[j]);
				GUILayout.Label("GetAnalogActionData(" + m_ControllerHandles[i] + ", " + m_InGameControlsAnalogActions[j] + ") - " + ret.eMode + " -- " + ret.bActive  + " -- " + ret.x + ", " + ret.y + " -- " + m_InGameControlsAnalogActionNames[j]);
				if (GUILayout.Button("StopAnalogActionMomentum(m_ControllerHandles[i], m_InGameControlsAnalogActions[j])")) {
					SteamController.StopAnalogActionMomentum(m_ControllerHandles[i], m_InGameControlsAnalogActions[j]);
					print("SteamController.StopAnalogActionMomentum(" + m_ControllerHandles[i] + ", " + m_InGameControlsAnalogActions[j] + ")");
				}
			}

			if (GUILayout.Button("GetDigitalActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins)")) {
				EControllerActionOrigin[] origins = new EControllerActionOrigin[Constants.STEAM_CONTROLLER_MAX_ORIGINS];
				int ret = SteamController.GetDigitalActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire], origins);
				print("SteamController.GetDigitalActionOrigins(" + m_ControllerHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsDigitalActions[(int)EDigitalActions_InGameControls.fire] + ", origins) - " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsDigitalActionNames[(int)EDigitalActions_InGameControls.fire]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			if (GUILayout.Button("GetAnalogActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins)")) {
				EControllerActionOrigin[] origins = new EControllerActionOrigin[Constants.STEAM_CONTROLLER_MAX_ORIGINS];
				int ret = SteamController.GetAnalogActionOrigins(m_ControllerHandles[i], m_ActionSets[(int)EActionSets.InGameControls], m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle], origins);
				print("SteamController.GetAnalogActionOrigins(" + m_ControllerHandles[i] + ", " + m_ActionSets[(int)EActionSets.InGameControls] + ", " + m_InGameControlsAnalogActions[(int)EAnalogActions_InGameControls.Throttle] + ", origins) - " + ret);
				print(ret + " origins for: " + m_ActionSetNames[(int)EActionSets.InGameControls] + "::" + m_InGameControlsAnalogActionNames[(int)EAnalogActions_InGameControls.Throttle]);
				for (int j = 0; j < ret; ++j) {
					print(j + ": " + origins[j]);
				}
			}

			if (GUILayout.Button("TriggerHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000)")) {
				SteamController.TriggerHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000);
				print("SteamController.TriggerHapticPulse(" + m_ControllerHandles[i] + ", ESteamControllerPad.k_ESteamControllerPad_Right, 5000)");
			}

			if (GUILayout.Button("TriggerRepeatedHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0)")) {
				SteamController.TriggerRepeatedHapticPulse(m_ControllerHandles[i], ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0);
				print("SteamController.TriggerRepeatedHapticPulse(" + m_ControllerHandles[i] + ", ESteamControllerPad.k_ESteamControllerPad_Right, 5000, 0, 0, 0)");
			}
		}
	}
}
