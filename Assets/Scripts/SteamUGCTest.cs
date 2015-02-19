using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUGCTest : MonoBehaviour {
	private UGCQueryHandle_t m_UGCQueryHandle;
	private PublishedFileId_t m_PublishedFileId;
	private UGCUpdateHandle_t m_UGCUpdateHandle;

	protected Callback<ItemInstalled_t> m_ItemInstalled;

	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;
	private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;
	private CallResult<CreateItemResult_t> OnCreateItemResultCallResult;
	private CallResult<SubmitItemUpdateResult_t> OnSubmitItemUpdateResultCallResult;

	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;
	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;

	public void OnEnable() {
		m_ItemInstalled = Callback<ItemInstalled_t>.Create(OnItemInstalled);

		OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create(OnSteamUGCQueryCompleted);
		OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(OnSteamUGCRequestUGCDetailsResult);
		OnCreateItemResultCallResult = CallResult<CreateItemResult_t>.Create(OnCreateItemResult);
		OnSubmitItemUpdateResultCallResult = CallResult<SubmitItemUpdateResult_t>.Create(OnSubmitItemUpdateResult);

		// These come from ISteamRemoteStorage but they are used here as well...
		OnRemoteStorageSubscribePublishedFileResultCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnRemoteStorageSubscribePublishedFileResult);
		OnRemoteStorageUnsubscribePublishedFileResultCallResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_UGCQueryHandle: " + m_UGCQueryHandle);
		GUILayout.Label("m_PublishedFileId: " + m_PublishedFileId);
		GUILayout.Label("m_UGCUpdateHandle: " + m_UGCUpdateHandle);
		GUILayout.EndArea();

		if (GUILayout.Button("CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc, AppId_t.Invalid, SteamUtils.GetAppID(), 1)")) {
			m_UGCQueryHandle = SteamUGC.CreateQueryUserUGCRequest(SteamUser.GetSteamID().GetAccountID(), EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc, AppId_t.Invalid, SteamUtils.GetAppID(), 1);
			print("SteamUGC.CreateQueryUserUGCRequest(" + SteamUser.GetSteamID().GetAccountID() + ", EUserUGCList.k_EUserUGCList_Published, EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots, EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc, " + AppId_t.Invalid + ", " + SteamUtils.GetAppID() + ", 1) : " + m_UGCQueryHandle);
		}

		if (GUILayout.Button("CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides, AppId_t.Invalid, SteamUtils.GetAppID(), 1)")) {
			m_UGCQueryHandle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides, AppId_t.Invalid, SteamUtils.GetAppID(), 1);
			print("SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides, " + AppId_t.Invalid + ", " + SteamUtils.GetAppID() + ", 1) : " + m_UGCQueryHandle);
		}

		if (GUILayout.Button("SendQueryUGCRequest(m_UGCQueryHandle)")) {
			SteamAPICall_t handle = SteamUGC.SendQueryUGCRequest(m_UGCQueryHandle);
			OnSteamUGCQueryCompletedCallResult.Set(handle);
			print("SteamUGC.SendQueryUGCRequest(" + m_UGCQueryHandle + ") : " + handle);
		}

		if (GUILayout.Button("GetQueryUGCResult(m_UGCQueryHandle, 0, out Details)")) {
			SteamUGCDetails_t Details;
			bool ret = SteamUGC.GetQueryUGCResult(m_UGCQueryHandle, 0, out Details);
			print("SteamUGC.GetQueryUGCResult(" + m_UGCQueryHandle + ", 0, out Details) : " + ret);
			print(Details.m_nPublishedFileId + " -- " + Details.m_eResult + " -- " + Details.m_eFileType + " -- " + Details.m_nCreatorAppID + " -- " + Details.m_nConsumerAppID + " -- " + Details.m_rgchTitle + " -- " + Details.m_rgchDescription + " -- " + Details.m_ulSteamIDOwner + " -- " + Details.m_rtimeCreated + " -- " + Details.m_rtimeUpdated + " -- " + Details.m_rtimeAddedToUserList + " -- " + Details.m_eVisibility + " -- " + Details.m_bBanned + " -- " + Details.m_bAcceptedForUse + " -- " + Details.m_bTagsTruncated + " -- " + Details.m_rgchTags + " -- " + Details.m_hFile + " -- " + Details.m_hPreviewFile + " -- " + Details.m_pchFileName + " -- " + Details.m_nFileSize + " -- " + Details.m_nPreviewFileSize + " -- " + Details.m_rgchURL + " -- " + Details.m_unVotesUp + " -- " + Details.m_unVotesDown + " -- " + Details.m_flScore + " -- " + Details.m_unNumChildren);
		}

		if (GUILayout.Button("ReleaseQueryUGCRequest(m_UGCQueryHandle)")) {
			print("SteamUGC.ReleaseQueryUGCRequest(" + m_UGCQueryHandle + ") : " + SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle));
		}

		if (GUILayout.Button("AddRequiredTag(m_UGCQueryHandle, \"Co-op\")")) {
			print("SteamUGC.AddRequiredTag(" + m_UGCQueryHandle + ", \"Co-op\") : " + SteamUGC.AddRequiredTag(m_UGCQueryHandle, "Co-op"));
		}

		if (GUILayout.Button("AddExcludedTag(m_UGCQueryHandle, \"Co-op\")")) {
			print("SteamUGC.AddExcludedTag(" + m_UGCQueryHandle + ", \"Co-op\") : " + SteamUGC.AddExcludedTag(m_UGCQueryHandle, "Co-op"));
		}

		if (GUILayout.Button("SetReturnLongDescription(m_UGCQueryHandle, true)")) {
			print("SteamUGC.SetReturnLongDescription(" + m_UGCQueryHandle + ", true) : " + SteamUGC.SetReturnLongDescription(m_UGCQueryHandle, true));
		}
	
		if (GUILayout.Button("SetReturnTotalOnly(m_UGCQueryHandle, true)")) {
			print("SteamUGC.SetReturnTotalOnly(" + m_UGCQueryHandle + ", true) : " + SteamUGC.SetReturnTotalOnly(m_UGCQueryHandle, true));
		}

		if (GUILayout.Button("SetAllowCachedResponse(m_UGCQueryHandle, 5)")) {
			print("SteamUGC.SetAllowCachedResponse(" + m_UGCQueryHandle + ", 5) : " + SteamUGC.SetAllowCachedResponse(m_UGCQueryHandle, 5));
		}

		if (GUILayout.Button("SetCloudFileNameFilter(m_UGCQueryHandle, \"\")")) {
			print("SteamUGC.SetCloudFileNameFilter(" + m_UGCQueryHandle + ", \"\") : " + SteamUGC.SetCloudFileNameFilter(m_UGCQueryHandle, ""));
		}

		if (GUILayout.Button("SetMatchAnyTag(m_UGCQueryHandle, true)")) {
			print("SteamUGC.SetMatchAnyTag(" + m_UGCQueryHandle + ", true) : " + SteamUGC.SetMatchAnyTag(m_UGCQueryHandle, true));
		}

		if (GUILayout.Button("SetSearchText(m_UGCQueryHandle, \"AciD\")")) {
			print("SteamUGC.SetSearchText(" + m_UGCQueryHandle + ", \"AciD\") : " + SteamUGC.SetSearchText(m_UGCQueryHandle, "AciD"));
		}

		if (GUILayout.Button("SetRankedByTrendDays(m_UGCQueryHandle, 7)")) {
			print("SteamUGC.SetRankedByTrendDays(" + m_UGCQueryHandle + ", 7) : " + SteamUGC.SetRankedByTrendDays(m_UGCQueryHandle, 7));
		}

		if (GUILayout.Button("RequestUGCDetails(m_PublishedFileId, 5)")) {
			SteamAPICall_t handle = SteamUGC.RequestUGCDetails(m_PublishedFileId, 5);
			OnSteamUGCRequestUGCDetailsResultCallResult.Set(handle);
			print("SteamUGC.RequestUGCDetails(m_PublishedFileId, 5) : " + handle);
		}

		if (GUILayout.Button("CreateItem((AppId_t)480, EWorkshopFileType.k_EWorkshopFileTypeWebGuide)")) {
			SteamAPICall_t handle = SteamUGC.CreateItem((AppId_t)480, EWorkshopFileType.k_EWorkshopFileTypeWebGuide);
			OnCreateItemResultCallResult.Set(handle);
			print("SteamUGC.CreateItem((AppId_t)480, EWorkshopFileType.k_EWorkshopFileTypeWebGuide) : " + handle);
		}

		if (GUILayout.Button("StartItemUpdate((AppId_t)480, m_PublishedFileId)")) {
			m_UGCUpdateHandle = SteamUGC.StartItemUpdate((AppId_t)480, m_PublishedFileId);
			print("SteamUGC.StartItemUpdate((AppId_t)480, " + m_PublishedFileId + ") : " + m_UGCUpdateHandle);
		}

		if (GUILayout.Button("SetItemTitle(m_UGCUpdateHandle, \"This is a Test\")")) {
			bool ret = SteamUGC.SetItemTitle(m_UGCUpdateHandle, "This is a Test");
			print("SteamUGC.SetItemTitle(" + m_UGCUpdateHandle + ", \"This is a Test\") : " + ret);
		}

		if (GUILayout.Button("SetItemDescription(m_UGCUpdateHandle, \"This is the test description.\")")) {
			bool ret = SteamUGC.SetItemDescription(m_UGCUpdateHandle, "This is the test description.");
			print("SteamUGC.SetItemDescription(" + m_UGCUpdateHandle + ", \"This is the test description.\") : " + ret);
		}

		if (GUILayout.Button("SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate)")) {
			bool ret = SteamUGC.SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
			print("SteamUGC.SetItemVisibility(" + m_UGCUpdateHandle + ", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate) : " + ret);
		}

		if (GUILayout.Button("SetItemTags(m_UGCUpdateHandle, new string[] {\"Tag One\", \"Tag Two\", \"Test Tags\", \"Sorry\"})")) {
			bool ret = SteamUGC.SetItemTags(m_UGCUpdateHandle, new string[] {"Tag One", "Tag Two", "Test Tags", "Sorry"});
			print("SteamUGC.SetItemTags(" + m_UGCUpdateHandle + ", new string[] {\"Tag One\", \"Tag Two\", \"Test Tags\", \"Sorry\"}) : " + ret);
		}

		if (GUILayout.Button("SetItemContent(m_UGCUpdateHandle, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.SetItemContent(m_UGCUpdateHandle, Application.dataPath + "/controller.vdf");
			print("SteamUGC.SetItemContent(" + m_UGCUpdateHandle + ", Application.dataPath + \"/controller.vdf\") : " + ret);
		}

		if (GUILayout.Button("SetItemPreview(m_UGCUpdateHandle, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.SetItemPreview(m_UGCUpdateHandle, Application.dataPath + "/controller.vdf");
			print("SteamUGC.SetItemPreview(" + m_UGCUpdateHandle + ", Application.dataPath + \"/controller.vdf\") : " + ret);
		}

		if (GUILayout.Button("SubmitItemUpdate(m_UGCUpdateHandle, \"Test Changenote\")")) {
			SteamAPICall_t handle = SteamUGC.SubmitItemUpdate(m_UGCUpdateHandle, "Test Changenote");
			OnSubmitItemUpdateResultCallResult.Set(handle);
			print("SteamUGC.SubmitItemUpdate(" + m_UGCUpdateHandle + ", \"Test Changenote\") : " + handle);
		}

		{
			ulong BytesProcessed;
			ulong BytesTotal;
			EItemUpdateStatus ret = SteamUGC.GetItemUpdateProgress(m_UGCUpdateHandle, out BytesProcessed, out BytesTotal);
			GUILayout.Label("GetItemUpdateProgress(m_UGCUpdateHandle, out BytesProcessed, out BytesTotal) : " + ret + " -- " + BytesProcessed + " -- " + BytesTotal);
		}

		if (GUILayout.Button("SubscribeItem((PublishedFileId_t)113142309)")) {
			SteamAPICall_t handle = SteamUGC.SubscribeItem((PublishedFileId_t)113142309);
			OnRemoteStorageSubscribePublishedFileResultCallResult.Set(handle);
			print("SteamUGC. : " + handle);
		}

		if (GUILayout.Button("UnsubscribeItem((PublishedFileId_t)113142309)")) {
			SteamAPICall_t handle = SteamUGC.UnsubscribeItem((PublishedFileId_t)113142309);
			OnRemoteStorageUnsubscribePublishedFileResultCallResult.Set(handle);
			print("SteamUGC. : " + handle);
		}

		GUILayout.Label("GetNumSubscribedItems() : " + SteamUGC.GetNumSubscribedItems());

		if (GUILayout.Button("GetSubscribedItems(PublishedFileID, (uint)PublishedFileID.Length)")) {
			PublishedFileId_t[] PublishedFileID = new PublishedFileId_t[1];
			uint ret = SteamUGC.GetSubscribedItems(PublishedFileID, (uint)PublishedFileID.Length);
			m_PublishedFileId = PublishedFileID[0];
			print("SteamUGC.GetSubscribedItems(" + PublishedFileID + ", (uint)PublishedFileID.Length) : " + ret + " -- " + PublishedFileID[0]);
		}

		{
			ulong SizeOnDisk;
			string Folder;
			bool LegacyItem;
			bool ret = SteamUGC.GetItemInstallInfo(m_PublishedFileId, out SizeOnDisk, out Folder, 1024, out LegacyItem);
			GUILayout.Label("GetItemInstallInfo(m_PublishedFileId, out SizeOnDisk, out Folder, 1024) : " + ret + " -- " + SizeOnDisk + " -- " + Folder + " -- " + LegacyItem);
		}

		{
			bool NeedsUpdate;
			bool IsDownloading;
			ulong BytesDownloaded;
			ulong BytesTotal;
			bool ret = SteamUGC.GetItemUpdateInfo(m_PublishedFileId, out NeedsUpdate, out IsDownloading, out BytesDownloaded, out BytesTotal);
			GUILayout.Label("GetItemUpdateInfo(m_PublishedFileId, out NeedsUpdate, out IsDownloading, out BytesDownloaded, out BytesTotal) : " + ret + " -- " + NeedsUpdate + " -- " + IsDownloading + " -- " + BytesDownloaded + " -- " + BytesTotal);
		}
	}

	void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SteamUGCQueryCompleted_t.k_iCallback + " - SteamUGCQueryCompleted_t] - " + pCallback.m_handle + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unNumResultsReturned + " -- " + pCallback.m_unTotalMatchingResults + " -- " + pCallback.m_bCachedData);
	}

	void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SteamUGCRequestUGCDetailsResult_t.k_iCallback + " - SteamUGCRequestUGCDetailsResult_t] - " + pCallback.m_details + " -- " + pCallback.m_bCachedData);
		Debug.Log(pCallback.m_details.m_nPublishedFileId + " -- " + pCallback.m_details.m_eResult + " -- " + pCallback.m_details.m_eFileType + " -- " + pCallback.m_details.m_nCreatorAppID + " -- " + pCallback.m_details.m_nConsumerAppID + " -- " + pCallback.m_details.m_rgchTitle + " -- " + pCallback.m_details.m_rgchDescription + " -- " + pCallback.m_details.m_ulSteamIDOwner + " -- " + pCallback.m_details.m_rtimeCreated + " -- " + pCallback.m_details.m_rtimeUpdated + " -- " + pCallback.m_details.m_rtimeAddedToUserList + " -- " + pCallback.m_details.m_eVisibility + " -- " + pCallback.m_details.m_bBanned + " -- " + pCallback.m_details.m_bAcceptedForUse + " -- " + pCallback.m_details.m_bTagsTruncated + " -- " + pCallback.m_details.m_rgchTags + " -- " + pCallback.m_details.m_hFile + " -- " + pCallback.m_details.m_hPreviewFile + " -- " + pCallback.m_details.m_pchFileName + " -- " + pCallback.m_details.m_nFileSize + " -- " + pCallback.m_details.m_nPreviewFileSize + " -- " + pCallback.m_details.m_rgchURL + " -- " + pCallback.m_details.m_unVotesUp + " -- " + pCallback.m_details.m_unVotesDown + " -- " + pCallback.m_details.m_flScore + " -- " + pCallback.m_details.m_unNumChildren);
	}

	void OnCreateItemResult(CreateItemResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + CreateItemResult_t.k_iCallback + " - CreateItemResult_t] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	void OnSubmitItemUpdateResult(SubmitItemUpdateResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult_t] - " + pCallback.m_eResult + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
	}

	void OnItemInstalled(ItemInstalled_t pCallback) {
		Debug.Log("[" + ItemInstalled_t.k_iCallback + " - ItemInstalled_t] - " + pCallback.m_unAppID + " -- " + pCallback.m_nPublishedFileId);
	}

	// These are from ISteamRemoteStorage, but also used here.
	void OnRemoteStorageSubscribePublishedFileResult(RemoteStorageSubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageSubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageSubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUnsubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageUnsubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}
}
