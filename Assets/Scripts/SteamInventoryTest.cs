using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamInventoryTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private SteamInventoryResult_t m_SteamInventoryResult;
	private SteamItemDetails_t[] m_SteamItemDetails;
	private SteamItemDef_t[] m_SteamItemDef;
	private byte[] m_SerializedBuffer;

	protected Callback<SteamInventoryResultReady_t> m_SteamInventoryResultReady;
	protected Callback<SteamInventoryFullUpdate_t> m_SteamInventoryFullUpdate;
	protected Callback<SteamInventoryDefinitionUpdate_t> m_SteamInventoryDefinitionUpdate;

	private CallResult<SteamInventoryEligiblePromoItemDefIDs_t> OnSteamInventoryEligiblePromoItemDefIDsCallResult;

	public void OnEnable() {
		m_SteamInventoryResult = SteamInventoryResult_t.Invalid;
		m_SteamItemDetails = null;
		m_SteamItemDef = null;
		m_SerializedBuffer = null;

		m_SteamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnSteamInventoryResultReady);
		m_SteamInventoryFullUpdate = Callback<SteamInventoryFullUpdate_t>.Create(OnSteamInventoryFullUpdate);
		m_SteamInventoryDefinitionUpdate = Callback<SteamInventoryDefinitionUpdate_t>.Create(OnSteamInventoryDefinitionUpdate);

		OnSteamInventoryEligiblePromoItemDefIDsCallResult = CallResult<SteamInventoryEligiblePromoItemDefIDs_t>.Create(OnSteamInventoryEligiblePromoItemDefIDs);
	}

	public void OnDisable() {
		DestroyResult();
	}

	void DestroyResult() {
		if (m_SteamInventoryResult != SteamInventoryResult_t.Invalid) {
			SteamInventory.DestroyResult(m_SteamInventoryResult);
			print("SteamInventory.DestroyResult(" + m_SteamInventoryResult + ")");
			m_SteamInventoryResult = SteamInventoryResult_t.Invalid;
		}
	}

	// These are hardcoded in the game and match the item definition IDs which were uploaded to Steam.
	public static class ESpaceWarItemDefIDs {
		public static readonly SteamItemDef_t k_SpaceWarItem_TimedDropList = (SteamItemDef_t)10;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipDecoration1 = (SteamItemDef_t)100;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipDecoration2 = (SteamItemDef_t)101;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipDecoration3 = (SteamItemDef_t)102;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipDecoration4 = (SteamItemDef_t)103;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipWeapon1 = (SteamItemDef_t)110;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipWeapon2 = (SteamItemDef_t)111;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipSpecial1 = (SteamItemDef_t)120;
		public static readonly SteamItemDef_t k_SpaceWarItem_ShipSpecial2 = (SteamItemDef_t)121;
	};

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_SteamInventoryResult: " + m_SteamInventoryResult);
		GUILayout.Label("m_SteamItemDetails: " + m_SteamItemDetails);
		GUILayout.Label("m_SteamItemDef: " + m_SteamItemDef);
		GUILayout.Label("m_SerializedBuffer: " + m_SerializedBuffer);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		// INVENTORY ASYNC RESULT MANAGEMENT

		GUILayout.Label("GetResultStatus(m_SteamInventoryResult) : " + SteamInventory.GetResultStatus(m_SteamInventoryResult));

		if (GUILayout.Button("GetResultItems(m_SteamInventoryResult, m_SteamItemDetails, ref OutItemsArraySize)")) {
			uint OutItemsArraySize = 0;
			bool ret = SteamInventory.GetResultItems(m_SteamInventoryResult, null, ref OutItemsArraySize);
			if (ret && OutItemsArraySize > 0) {
				m_SteamItemDetails = new SteamItemDetails_t[OutItemsArraySize];
				ret = SteamInventory.GetResultItems(m_SteamInventoryResult, m_SteamItemDetails, ref OutItemsArraySize);
				print("SteamInventory.GetResultItems(" + m_SteamInventoryResult + ", m_SteamItemDetails, out OutItemsArraySize) - " + ret + " -- " + OutItemsArraySize);

				System.Text.StringBuilder test = new System.Text.StringBuilder();
				for (int i = 0; i < OutItemsArraySize; ++i) {
					test.AppendFormat("{0} - {1} - {2} - {3} - {4}\n", i, m_SteamItemDetails[i].m_itemId, m_SteamItemDetails[i].m_iDefinition, m_SteamItemDetails[i].m_unQuantity, m_SteamItemDetails[i].m_unFlags);
				}
				print(test);
			}
			else {
				print("SteamInventory.GetResultItems(" + m_SteamInventoryResult + ", null, out OutItemsArraySize) - " + ret + " -- " + OutItemsArraySize);
			}
		}

		if (GUILayout.Button("GetResultTimestamp(m_SteamInventoryResult)")) {
			uint ret = SteamInventory.GetResultTimestamp(m_SteamInventoryResult);
			print("SteamInventory.GetResultTimestamp(" + m_SteamInventoryResult + ") : " + ret);
		}

		if (GUILayout.Button("CheckResultSteamID(m_SteamInventoryResult, SteamUser.GetSteamID())")) {
			bool ret = SteamInventory.CheckResultSteamID(m_SteamInventoryResult, SteamUser.GetSteamID());
			print("SteamInventory.CheckResultSteamID(" + m_SteamInventoryResult + ", " + SteamUser.GetSteamID() + ") : " + ret);
		}

		if (GUILayout.Button("DestroyResult(m_SteamInventoryResult)")) {
			DestroyResult();
		}

		// INVENTORY ASYNC QUERY

		if (GUILayout.Button("GetAllItems(out m_SteamInventoryResult)")) {
			bool ret = SteamInventory.GetAllItems(out m_SteamInventoryResult);
			print("SteamInventory.GetAllItems(" + "out m_SteamInventoryResult" + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		if (GUILayout.Button("GetItemsByID(out m_SteamInventoryResult, InstanceIDs, (uint)InstanceIDs.Length)")) {
			SteamItemInstanceID_t[] InstanceIDs = { (SteamItemInstanceID_t)0, (SteamItemInstanceID_t)1, };
			bool ret = SteamInventory.GetItemsByID(out m_SteamInventoryResult, InstanceIDs, (uint)InstanceIDs.Length);
			print("SteamInventory.GetItemsByID(" + "out m_SteamInventoryResult" + ", " + InstanceIDs + ", " + (uint)InstanceIDs.Length + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		// RESULT SERIALIZATION AND AUTHENTICATION

		if (GUILayout.Button("SerializeResult(m_SteamInventoryResult, m_SerializedBuffer, out OutBufferSize)")) {
			uint OutBufferSize;
			bool ret = SteamInventory.SerializeResult(m_SteamInventoryResult, null, out OutBufferSize);
			if(ret) {
				m_SerializedBuffer = new byte[OutBufferSize];
				ret = SteamInventory.SerializeResult(m_SteamInventoryResult, m_SerializedBuffer, out OutBufferSize);
				print("SteamInventory.SerializeResult(m_SteamInventoryResult, m_SerializedBuffer, out OutBufferSize) - " + ret + " -- " + OutBufferSize + " -- " + System.Text.Encoding.UTF8.GetString(m_SerializedBuffer, 0, m_SerializedBuffer.Length));
			}
			else {
				print("SteamInventory.SerializeResult(m_SteamInventoryResult, null, out OutBufferSize) - " + ret + " -- " + OutBufferSize);
			}
		}

		if (GUILayout.Button("DeserializeResult(out m_SteamInventoryResult, m_SerializedBuffer, (uint)m_SerializedBuffer.Length)")) {
			bool ret = SteamInventory.DeserializeResult(out m_SteamInventoryResult, m_SerializedBuffer, (uint)m_SerializedBuffer.Length);
			print("SteamInventory.DeserializeResult(" + "out m_SteamInventoryResult" + ", " + m_SerializedBuffer + ", " + (uint)m_SerializedBuffer.Length + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		// INVENTORY ASYNC MODIFICATION

		if (GUILayout.Button("GenerateItems(out m_SteamInventoryResult, ArrayItemDefs, null, (uint)ArrayItemDefs.Length)")) {
			SteamItemDef_t[] ArrayItemDefs = { ESpaceWarItemDefIDs.k_SpaceWarItem_ShipDecoration1, ESpaceWarItemDefIDs.k_SpaceWarItem_ShipDecoration2 };
			bool ret = SteamInventory.GenerateItems(out m_SteamInventoryResult, ArrayItemDefs, null, (uint)ArrayItemDefs.Length);
			print("SteamInventory.GenerateItems(" + "out m_SteamInventoryResult" + ", " + ArrayItemDefs + ", " + null + ", " + (uint)ArrayItemDefs.Length + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		if (GUILayout.Button("GrantPromoItems(out m_SteamInventoryResult)")) {
			bool ret = SteamInventory.GrantPromoItems(out m_SteamInventoryResult);
			print("SteamInventory.GrantPromoItems(" + "out m_SteamInventoryResult" + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		if (GUILayout.Button("AddPromoItem(out m_SteamInventoryResult, ESpaceWarItemDefIDs.k_SpaceWarItem_ShipWeapon1)")) {
			bool ret = SteamInventory.AddPromoItem(out m_SteamInventoryResult, ESpaceWarItemDefIDs.k_SpaceWarItem_ShipWeapon1);
			print("SteamInventory.AddPromoItem(" + "out m_SteamInventoryResult" + ", " + ESpaceWarItemDefIDs.k_SpaceWarItem_ShipWeapon1 + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		if (GUILayout.Button("AddPromoItems(out m_SteamInventoryResult, ArrayItemDefs, (uint)ArrayItemDefs.Length)")) {
			SteamItemDef_t[] ArrayItemDefs = { ESpaceWarItemDefIDs.k_SpaceWarItem_ShipWeapon1, ESpaceWarItemDefIDs.k_SpaceWarItem_ShipWeapon2 };
			bool ret = SteamInventory.AddPromoItems(out m_SteamInventoryResult, ArrayItemDefs, (uint)ArrayItemDefs.Length);
			print("SteamInventory.AddPromoItems(" + "out m_SteamInventoryResult" + ", " + ArrayItemDefs + ", " + (uint)ArrayItemDefs.Length + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		if (GUILayout.Button("ConsumeItem(out m_SteamInventoryResult, m_SteamItemDetails[0].m_itemId, 1)")) {
			if (m_SteamItemDetails != null) {
				bool ret = SteamInventory.ConsumeItem(out m_SteamInventoryResult, m_SteamItemDetails[0].m_itemId, 1);
				print("SteamInventory.ConsumeItem(out m_SteamInventoryResult, " + m_SteamItemDetails[0].m_itemId + ", 1) - " + ret + " -- " + m_SteamInventoryResult);
			}
		}

		if (GUILayout.Button("ExchangeItems(TODO)")) {
			if (m_SteamItemDetails != null) {
				bool ret = SteamInventory.ExchangeItems(out m_SteamInventoryResult, null, null, 0, null, null, 0); // TODO
				print("SteamInventory.ExchangeItems(TODO) - " + ret + " -- " + m_SteamInventoryResult);
			}
		}

		if (GUILayout.Button("TransferItemQuantity(out m_SteamInventoryResult, m_SteamItemDetails[0].m_itemId, 1, SteamItemInstanceID_t.Invalid)")) {
			if (m_SteamItemDetails != null) {
				bool ret = SteamInventory.TransferItemQuantity(out m_SteamInventoryResult, m_SteamItemDetails[0].m_itemId, 1, SteamItemInstanceID_t.Invalid);
				print("SteamInventory.TransferItemQuantity(out m_SteamInventoryResult, " + m_SteamItemDetails[0].m_itemId + ", 1, SteamItemInstanceID_t.Invalid) - " + ret + " -- " + m_SteamInventoryResult);
			}
		}

		// TIMED DROPS AND PLAYTIME CREDIT

		if (GUILayout.Button("SendItemDropHeartbeat()")) {
			SteamInventory.SendItemDropHeartbeat();
			print("SteamInventory.SendItemDropHeartbeat()");
		}

		if (GUILayout.Button("TriggerItemDrop(out m_SteamInventoryResult, ESpaceWarItemDefIDs.k_SpaceWarItem_TimedDropList)")) {
			bool ret = SteamInventory.TriggerItemDrop(out m_SteamInventoryResult, ESpaceWarItemDefIDs.k_SpaceWarItem_TimedDropList);
			print("SteamInventory.TriggerItemDrop(" + "out m_SteamInventoryResult" + ", " + ESpaceWarItemDefIDs.k_SpaceWarItem_TimedDropList + ") : " + ret + " -- " + m_SteamInventoryResult);
		}

		// IN-GAME TRADING

		if (GUILayout.Button("TradeItems(TODO)")) {
			if (m_SteamItemDetails != null) {
				bool ret = SteamInventory.TradeItems(out m_SteamInventoryResult, SteamUser.GetSteamID(), null, null, 0, null, null, 0); // TODO... Difficult
				print("SteamInventory.TradeItems(TODO) - " + ret + " -- " + m_SteamInventoryResult);
			}
		}

		// ITEM DEFINITIONS

		if (GUILayout.Button("LoadItemDefinitions()")) {
			bool ret = SteamInventory.LoadItemDefinitions();
			print("SteamInventory.LoadItemDefinitions() : " + ret);
		}

		if (GUILayout.Button("GetItemDefinitionIDs(ItemDefIDs, ref length)")) {
			uint length;
			bool ret = SteamInventory.GetItemDefinitionIDs(null, out length);
			if (ret) {
				m_SteamItemDef = new SteamItemDef_t[length];
				ret = SteamInventory.GetItemDefinitionIDs(m_SteamItemDef, out length);
				print("SteamInventory.GetItemDefinitionIDs(m_SteamItemDef, out length) - " + ret + " -- " + length);
			}
			else {
				print("SteamInventory.GetItemDefinitionIDs(null, out length) - " + ret + " -- " + length);
			}
		}

		if (GUILayout.Button("GetItemDefinitionProperty(ESpaceWarItemDefIDs.k_SpaceWarItem_ShipDecoration1, null, out ValueBuffer, ref length)")) {
			uint length = 2048;
			string ValueBuffer;
			bool ret = SteamInventory.GetItemDefinitionProperty(ESpaceWarItemDefIDs.k_SpaceWarItem_ShipDecoration1, null, out ValueBuffer, ref length);
			print("SteamInventory.GetItemDefinitionProperty(" + ESpaceWarItemDefIDs.k_SpaceWarItem_ShipDecoration1 + ", " + null + ", " + "out ValueBuffer" + ", " + "ref length" + ") : " + ret + " -- " + ValueBuffer + " -- " + length);
		}

		if (GUILayout.Button("RequestEligiblePromoItemDefinitionsIDs(SteamUser.GetSteamID())")) {
			SteamAPICall_t handle = SteamInventory.RequestEligiblePromoItemDefinitionsIDs(SteamUser.GetSteamID());
			OnSteamInventoryEligiblePromoItemDefIDsCallResult.Set(handle);
			print("SteamInventory.RequestEligiblePromoItemDefinitionsIDs(" + SteamUser.GetSteamID() + ") : " + handle);
		}

		//SteamInventory.GetEligiblePromoItemDefinitionIDs() // Should be handled within the SteamInventoryEligiblePromoItemDefIDs_t CallResult!

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnSteamInventoryResultReady(SteamInventoryResultReady_t pCallback) {
		Debug.Log("[" + SteamInventoryResultReady_t.k_iCallback + " - SteamInventoryResultReady] - " + pCallback.m_handle + " -- " + pCallback.m_result);

		m_SteamInventoryResult = pCallback.m_handle;
	}

	void OnSteamInventoryFullUpdate(SteamInventoryFullUpdate_t pCallback) {
		Debug.Log("[" + SteamInventoryFullUpdate_t.k_iCallback + " - SteamInventoryFullUpdate] - " + pCallback.m_handle);

		m_SteamInventoryResult = pCallback.m_handle;
	}

	void OnSteamInventoryDefinitionUpdate(SteamInventoryDefinitionUpdate_t pCallback) {
		Debug.Log("[" + SteamInventoryDefinitionUpdate_t.k_iCallback + " - SteamInventoryDefinitionUpdate]");
	}

	void OnSteamInventoryEligiblePromoItemDefIDs(SteamInventoryEligiblePromoItemDefIDs_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SteamInventoryEligiblePromoItemDefIDs_t.k_iCallback + " - SteamInventoryEligiblePromoItemDefIDs] - " + pCallback.m_result + " -- " + pCallback.m_steamID + " -- " + pCallback.m_numEligiblePromoItemDefs + " -- " + pCallback.m_bCachedData);

		uint ItemDefIDsArraySize = (uint)pCallback.m_numEligiblePromoItemDefs;
		SteamItemDef_t[] ItemDefIDs = new SteamItemDef_t[ItemDefIDsArraySize];
		bool ret = SteamInventory.GetEligiblePromoItemDefinitionIDs(pCallback.m_steamID, ItemDefIDs, ref ItemDefIDsArraySize);
		print("SteamInventory.GetEligiblePromoItemDefinitionIDs(pCallback.m_steamID, ItemDefIDs, ref ItemDefIDsArraySize) - " + ret + " -- " + ItemDefIDsArraySize);
	}
}