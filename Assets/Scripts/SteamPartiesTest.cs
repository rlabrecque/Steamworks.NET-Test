using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamPartiesTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private uint m_BeaconIndex;
	private PartyBeaconID_t m_PartyBeaconID;
	private uint m_NumLocations;
	private SteamPartyBeaconLocation_t[] m_BeaconLocationList;
	private CSteamID m_OtherUsersSteamID;

	protected Callback<ReservationNotificationCallback_t> m_ReservationNotificationCallback;
	protected Callback<AvailableBeaconLocationsUpdated_t> m_AvailableBeaconLocationsUpdated;
	protected Callback<ActiveBeaconsUpdated_t> m_ActiveBeaconsUpdated;

	private CallResult<JoinPartyCallback_t> OnJoinPartyCallbackCallResult;
	private CallResult<CreateBeaconCallback_t> OnCreateBeaconCallbackCallResult;
	private CallResult<ChangeNumOpenSlotsCallback_t> OnChangeNumOpenSlotsCallbackCallResult;

	public void OnEnable() {
		m_BeaconIndex = 0;
		m_NumLocations = 0;
		m_PartyBeaconID = PartyBeaconID_t.Invalid;
		m_OtherUsersSteamID = CSteamID.Nil;

		m_ReservationNotificationCallback = Callback<ReservationNotificationCallback_t>.Create(OnReservationNotificationCallback);
		m_AvailableBeaconLocationsUpdated = Callback<AvailableBeaconLocationsUpdated_t>.Create(OnAvailableBeaconLocationsUpdated);
		m_ActiveBeaconsUpdated = Callback<ActiveBeaconsUpdated_t>.Create(OnActiveBeaconsUpdated);

		OnJoinPartyCallbackCallResult = CallResult<JoinPartyCallback_t>.Create(OnJoinPartyCallback);
		OnCreateBeaconCallbackCallResult = CallResult<CreateBeaconCallback_t>.Create(OnCreateBeaconCallback);
		OnChangeNumOpenSlotsCallbackCallResult = CallResult<ChangeNumOpenSlotsCallback_t>.Create(OnChangeNumOpenSlotsCallback);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_BeaconIndex: " + m_BeaconIndex);
		GUILayout.Label("m_PartyBeaconID: " + m_PartyBeaconID);
		GUILayout.Label("m_NumLocations: " + m_NumLocations);
		GUILayout.Label("m_BeaconLocationList: " + m_BeaconLocationList);
		GUILayout.Label("m_OtherUsersSteamID: " + m_OtherUsersSteamID);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		GUILayout.Label("GetNumActiveBeacons() : " + SteamParties.GetNumActiveBeacons());

		if (GUILayout.Button("GetBeaconByIndex(m_BeaconIndex)")) {
			m_PartyBeaconID = SteamParties.GetBeaconByIndex(m_BeaconIndex);
			print("SteamParties.GetBeaconByIndex(" + m_BeaconIndex + ") : " + m_PartyBeaconID);
		}

		if (GUILayout.Button("GetBeaconDetails(m_PartyBeaconID, out m_OtherUsersSteamID, out m_BeaconLocationList[0], out Metadata, 1024)")) {
			m_BeaconLocationList = new SteamPartyBeaconLocation_t[1];
			string Metadata;
			bool ret = SteamParties.GetBeaconDetails(m_PartyBeaconID, out m_OtherUsersSteamID, out m_BeaconLocationList[0], out Metadata, 1024);
			print("SteamParties.GetBeaconDetails(" + m_PartyBeaconID + ", " + "out m_OtherUsersSteamID" + ", " + "out m_BeaconLocationList[0]" + ", " + "out Metadata" + ", " + 1024 + ") : " + ret + " -- " + m_OtherUsersSteamID + " -- " + m_BeaconLocationList[0] + " -- " + Metadata);
		}

		if (GUILayout.Button("JoinParty(m_PartyBeaconID)")) {
			SteamAPICall_t handle = SteamParties.JoinParty(m_PartyBeaconID);
			OnJoinPartyCallbackCallResult.Set(handle);
			print("SteamParties.JoinParty(" + m_PartyBeaconID + ") : " + handle);
		}

		if (GUILayout.Button("GetNumAvailableBeaconLocations(out m_NumLocations)")) {
			bool ret = SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations);
			print("SteamParties.GetNumAvailableBeaconLocations(" + "out m_NumLocations" + ") : " + ret + " -- " + m_NumLocations);
		}

		if (GUILayout.Button("GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations)")) {
			m_BeaconLocationList = new SteamPartyBeaconLocation_t[m_NumLocations];
			bool ret = SteamParties.GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations);
			print("SteamParties.GetAvailableBeaconLocations(" + m_BeaconLocationList + ", " + m_NumLocations + ") : " + ret);
		}

		if (GUILayout.Button("CreateBeacon(1, ref m_BeaconLocationList[0], \"TestConnectString\", \"TestMetadata\")")) {
			SteamAPICall_t handle = SteamParties.CreateBeacon(1, ref m_BeaconLocationList[0], "TestConnectString", "TestMetadata");
			OnCreateBeaconCallbackCallResult.Set(handle);
			print("SteamParties.CreateBeacon(" + 1 + ", " + "ref m_BeaconLocationList[0]" + ", " + "\"TestConnectString\"" + ", " + "\"TestMetadata\"" + ") : " + handle + " -- " + m_BeaconLocationList[0]);
		}

		if (GUILayout.Button("OnReservationCompleted(m_PartyBeaconID, m_OtherUsersSteamID)")) {
			SteamParties.OnReservationCompleted(m_PartyBeaconID, m_OtherUsersSteamID);
			print("SteamParties.OnReservationCompleted(" + m_PartyBeaconID + ", " + m_OtherUsersSteamID + ")");
		}

		if (GUILayout.Button("CancelReservation(m_PartyBeaconID, m_OtherUsersSteamID)")) {
			SteamParties.CancelReservation(m_PartyBeaconID, m_OtherUsersSteamID);
			print("SteamParties.CancelReservation(" + m_PartyBeaconID + ", " + m_OtherUsersSteamID + ")");
		}

		if (GUILayout.Button("ChangeNumOpenSlots(m_PartyBeaconID, 2)")) {
			SteamAPICall_t handle = SteamParties.ChangeNumOpenSlots(m_PartyBeaconID, 2);
			OnChangeNumOpenSlotsCallbackCallResult.Set(handle);
			print("SteamParties.ChangeNumOpenSlots(" + m_PartyBeaconID + ", " + 2 + ") : " + handle);
		}

		if (GUILayout.Button("DestroyBeacon(m_PartyBeaconID)")) {
			bool ret = SteamParties.DestroyBeacon(m_PartyBeaconID);
			m_PartyBeaconID = PartyBeaconID_t.Invalid;
			print("SteamParties.DestroyBeacon(" + m_PartyBeaconID + ") : " + ret);
		}

		if (GUILayout.Button("GetBeaconLocationData(m_BeaconLocationList[0], ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName, out DataString, 1024)")) {
			string DataString;
			bool ret = SteamParties.GetBeaconLocationData(m_BeaconLocationList[0], ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName, out DataString, 1024);
			print("SteamParties.GetBeaconLocationData(" + m_BeaconLocationList[0] + ", " + ESteamPartyBeaconLocationData.k_ESteamPartyBeaconLocationDataName + ", " + "out DataString" + ", " + 1024 + ") : " + ret + " -- " + DataString);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnJoinPartyCallback(JoinPartyCallback_t pCallback, bool bIOFailure) {
		Debug.Log("[" + JoinPartyCallback_t.k_iCallback + " - JoinPartyCallback] - " + pCallback.m_eResult + " -- " + pCallback.m_ulBeaconID + " -- " + pCallback.m_SteamIDBeaconOwner + " -- " + pCallback.m_rgchConnectString);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_PartyBeaconID = pCallback.m_ulBeaconID;
			m_OtherUsersSteamID = pCallback.m_SteamIDBeaconOwner;
		}
	}

	void OnCreateBeaconCallback(CreateBeaconCallback_t pCallback, bool bIOFailure) {
		Debug.Log("[" + CreateBeaconCallback_t.k_iCallback + " - CreateBeaconCallback] - " + pCallback.m_eResult + " -- " + pCallback.m_ulBeaconID);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_PartyBeaconID = pCallback.m_ulBeaconID;
		}
	}

	void OnReservationNotificationCallback(ReservationNotificationCallback_t pCallback) {
		Debug.Log("[" + ReservationNotificationCallback_t.k_iCallback + " - ReservationNotificationCallback] - " + pCallback.m_ulBeaconID + " -- " + pCallback.m_steamIDJoiner);

		m_PartyBeaconID = pCallback.m_ulBeaconID;
		m_OtherUsersSteamID = pCallback.m_steamIDJoiner;
	}

	void OnChangeNumOpenSlotsCallback(ChangeNumOpenSlotsCallback_t pCallback, bool bIOFailure) {
		Debug.Log("[" + ChangeNumOpenSlotsCallback_t.k_iCallback + " - ChangeNumOpenSlotsCallback] - " + pCallback.m_eResult);
	}

	void OnAvailableBeaconLocationsUpdated(AvailableBeaconLocationsUpdated_t pCallback) {
		Debug.Log("[" + AvailableBeaconLocationsUpdated_t.k_iCallback + " - AvailableBeaconLocationsUpdated]");

		bool ret = SteamParties.GetNumAvailableBeaconLocations(out m_NumLocations);
		print("SteamParties.GetNumAvailableBeaconLocations(" + "out m_NumLocations" + ") : " + ret + " -- " + m_NumLocations);
		m_BeaconLocationList = new SteamPartyBeaconLocation_t[m_NumLocations];
		bool ret2 = SteamParties.GetAvailableBeaconLocations(m_BeaconLocationList, m_NumLocations);
		print("SteamParties.GetAvailableBeaconLocations(" + m_BeaconLocationList + ", " + m_NumLocations + ") : " + ret);
	}

	void OnActiveBeaconsUpdated(ActiveBeaconsUpdated_t pCallback) {
		Debug.Log("[" + ActiveBeaconsUpdated_t.k_iCallback + " - ActiveBeaconsUpdated]");
	}
}