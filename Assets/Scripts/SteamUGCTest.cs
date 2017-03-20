using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamUGCTest : MonoBehaviour {
	private UGCQueryHandle_t m_UGCQueryHandle;
	private PublishedFileId_t m_PublishedFileId;
	private UGCUpdateHandle_t m_UGCUpdateHandle;

	protected Callback<ItemInstalled_t> m_ItemInstalled;
	protected Callback<DownloadItemResult_t> m_DownloadItemResult;

	private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;
	private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;
	private CallResult<CreateItemResult_t> OnCreateItemResultCallResult;
	private CallResult<SubmitItemUpdateResult_t> OnSubmitItemUpdateResultCallResult;
	private CallResult<UserFavoriteItemsListChanged_t> OnUserFavoriteItemsListChangedCallResult;
	private CallResult<SetUserItemVoteResult_t> OnSetUserItemVoteResultCallResult;
	private CallResult<GetUserItemVoteResult_t> OnGetUserItemVoteResultCallResult;
	private CallResult<StartPlaytimeTrackingResult_t> OnStartPlaytimeTrackingResultCallResult;
	private CallResult<StopPlaytimeTrackingResult_t> OnStopPlaytimeTrackingResultCallResult;

	public void OnEnable() {
		// These come from ISteamRemoteStorage but they are used here as well...
		OnRemoteStorageSubscribePublishedFileResultCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnRemoteStorageSubscribePublishedFileResult);
		OnRemoteStorageUnsubscribePublishedFileResultCallResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);

		m_ItemInstalled = Callback<ItemInstalled_t>.Create(OnItemInstalled);
		m_DownloadItemResult = Callback<DownloadItemResult_t>.Create(OnDownloadItemResult);

		OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create(OnSteamUGCQueryCompleted);
		OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create(OnSteamUGCRequestUGCDetailsResult);
		OnCreateItemResultCallResult = CallResult<CreateItemResult_t>.Create(OnCreateItemResult);
		OnSubmitItemUpdateResultCallResult = CallResult<SubmitItemUpdateResult_t>.Create(OnSubmitItemUpdateResult);
		OnUserFavoriteItemsListChangedCallResult = CallResult<UserFavoriteItemsListChanged_t>.Create(OnUserFavoriteItemsListChanged);
		OnSetUserItemVoteResultCallResult = CallResult<SetUserItemVoteResult_t>.Create(OnSetUserItemVoteResult);
		OnGetUserItemVoteResultCallResult = CallResult<GetUserItemVoteResult_t>.Create(OnGetUserItemVoteResult);
		OnStartPlaytimeTrackingResultCallResult = CallResult<StartPlaytimeTrackingResult_t>.Create(OnStartPlaytimeTrackingResult);
		OnStopPlaytimeTrackingResultCallResult = CallResult<StopPlaytimeTrackingResult_t>.Create(OnStopPlaytimeTrackingResult);
	}

	// These come from ISteamRemoteStorage but they are used here as well...
	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;
	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;

	void OnRemoteStorageSubscribePublishedFileResult(RemoteStorageSubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageSubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageSubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUnsubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageUnsubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
		m_PublishedFileId = pCallback.m_nPublishedFileId;
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
			print("SteamUGC.CreateQueryUserUGCRequest(" + SteamUser.GetSteamID().GetAccountID() + ", " + EUserUGCList.k_EUserUGCList_Published + ", " + EUGCMatchingUGCType.k_EUGCMatchingUGCType_Screenshots + ", " + EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc + ", " + AppId_t.Invalid + ", " + SteamUtils.GetAppID() + ", " + 1 + ") : " + m_UGCQueryHandle);
		}

		if (GUILayout.Button("CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides, AppId_t.Invalid, SteamUtils.GetAppID(), 1)")) {
			m_UGCQueryHandle = SteamUGC.CreateQueryAllUGCRequest(EUGCQuery.k_EUGCQuery_RankedByPublicationDate, EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides, AppId_t.Invalid, SteamUtils.GetAppID(), 1);
			print("SteamUGC.CreateQueryAllUGCRequest(" + EUGCQuery.k_EUGCQuery_RankedByPublicationDate + ", " + EUGCMatchingUGCType.k_EUGCMatchingUGCType_AllGuides + ", " + AppId_t.Invalid + ", " + SteamUtils.GetAppID() + ", " + 1 + ") : " + m_UGCQueryHandle);
		}

		if (GUILayout.Button("CreateQueryUGCDetailsRequest(PublishedFileIDs, (uint)PublishedFileIDs.Length)")) {
			PublishedFileId_t[] PublishedFileIDs = new PublishedFileId_t[] { TestConstants.Instance.k_PublishedFileId_Champions };
			m_UGCQueryHandle = SteamUGC.CreateQueryUGCDetailsRequest(PublishedFileIDs, (uint)PublishedFileIDs.Length);
			print("SteamUGC.CreateQueryUGCDetailsRequest(" + PublishedFileIDs + ", " + (uint)PublishedFileIDs.Length + ") : " + m_UGCQueryHandle);
		}

		if (GUILayout.Button("SendQueryUGCRequest(m_UGCQueryHandle)")) {
			SteamAPICall_t handle = SteamUGC.SendQueryUGCRequest(m_UGCQueryHandle);
			OnSteamUGCQueryCompletedCallResult.Set(handle);
			print("SteamUGC.SendQueryUGCRequest(" + m_UGCQueryHandle + ") : " + handle);
		}

		if (GUILayout.Button("GetQueryUGCResult(m_UGCQueryHandle, 0, out Details)")) {
			SteamUGCDetails_t Details;
			bool ret = SteamUGC.GetQueryUGCResult(m_UGCQueryHandle, 0, out Details);
			print(Details.m_nPublishedFileId + " -- " + Details.m_eResult + " -- " + Details.m_eFileType + " -- " + Details.m_nCreatorAppID + " -- " + Details.m_nConsumerAppID + " -- " + Details.m_rgchTitle + " -- " + Details.m_rgchDescription + " -- " + Details.m_ulSteamIDOwner + " -- " + Details.m_rtimeCreated + " -- " + Details.m_rtimeUpdated + " -- " + Details.m_rtimeAddedToUserList + " -- " + Details.m_eVisibility + " -- " + Details.m_bBanned + " -- " + Details.m_bAcceptedForUse + " -- " + Details.m_bTagsTruncated + " -- " + Details.m_rgchTags + " -- " + Details.m_hFile + " -- " + Details.m_hPreviewFile + " -- " + Details.m_pchFileName + " -- " + Details.m_nFileSize + " -- " + Details.m_nPreviewFileSize + " -- " + Details.m_rgchURL + " -- " + Details.m_unVotesUp + " -- " + Details.m_unVotesDown + " -- " + Details.m_flScore + " -- " + Details.m_unNumChildren);
			print("SteamUGC.GetQueryUGCResult(" + m_UGCQueryHandle + ", " + 0 + ", " + "out Details" + ") : " + ret + " -- " + Details);
		}

		if (GUILayout.Button("GetQueryUGCPreviewURL(m_UGCQueryHandle, 0, out URL, 1024)")) {
			string URL;
			bool ret = SteamUGC.GetQueryUGCPreviewURL(m_UGCQueryHandle, 0, out URL, 1024);
			print("SteamUGC.GetQueryUGCPreviewURL(" + m_UGCQueryHandle + ", " + 0 + ", " + "out URL" + ", " + 1024 + ") : " + ret + " -- " + URL);
		}

		if (GUILayout.Button("GetQueryUGCMetadata(m_UGCQueryHandle, 0, out Metadata, Constants.k_cchDeveloperMetadataMax)")) {
			string Metadata;
			bool ret = SteamUGC.GetQueryUGCMetadata(m_UGCQueryHandle, 0, out Metadata, Constants.k_cchDeveloperMetadataMax);
			print("SteamUGC.GetQueryUGCMetadata(" + m_UGCQueryHandle + ", " + 0 + ", " + "out Metadata" + ", " + Constants.k_cchDeveloperMetadataMax + ") : " + ret + " -- " + Metadata);
		}

		if (GUILayout.Button("GetQueryUGCChildren(m_UGCQueryHandle, 0, PublishedFileIDs, (uint)PublishedFileIDs.Length)")) {
			PublishedFileId_t[] PublishedFileIDs = new PublishedFileId_t[1]; // Use SteamUGCDetails_t.m_unNumChildren instead...
			bool ret = SteamUGC.GetQueryUGCChildren(m_UGCQueryHandle, 0, PublishedFileIDs, (uint)PublishedFileIDs.Length);
			print("SteamUGC.GetQueryUGCChildren(" + m_UGCQueryHandle + ", " + 0 + ", " + PublishedFileIDs + ", " + (uint)PublishedFileIDs.Length + ") : " + ret);
		}

		if (GUILayout.Button("GetQueryUGCStatistic(m_UGCQueryHandle, 0, EItemStatistic.k_EItemStatistic_NumSubscriptions, out StatValue)")) {
			ulong StatValue;
			bool ret = SteamUGC.GetQueryUGCStatistic(m_UGCQueryHandle, 0, EItemStatistic.k_EItemStatistic_NumSubscriptions, out StatValue);
			print("SteamUGC.GetQueryUGCStatistic(" + m_UGCQueryHandle + ", " + 0 + ", " + EItemStatistic.k_EItemStatistic_NumSubscriptions + ", " + "out StatValue" + ") : " + ret + " -- " + StatValue);
		}

		if (GUILayout.Button("GetQueryUGCNumAdditionalPreviews(m_UGCQueryHandle, 0)")) {
			uint ret = SteamUGC.GetQueryUGCNumAdditionalPreviews(m_UGCQueryHandle, 0);
			print("SteamUGC.GetQueryUGCNumAdditionalPreviews(" + m_UGCQueryHandle + ", " + 0 + ") : " + ret);
		}

		if (GUILayout.Button("GetQueryUGCAdditionalPreview(m_UGCQueryHandle, 0, 0, out pchURLOrVideoID, 1024, out pchOriginalFileName, 260, out pPreviewType)")) {
			// Should check GetQueryUGCNumAdditionalPreviews first.
			string pchURLOrVideoID;
			string pchOriginalFileName;
			EItemPreviewType pPreviewType;
			bool ret = SteamUGC.GetQueryUGCAdditionalPreview(m_UGCQueryHandle, 0, 0, out pchURLOrVideoID, 1024, out pchOriginalFileName, 260, out pPreviewType);
			print("SteamUGC.GetQueryUGCAdditionalPreview(" + m_UGCQueryHandle + ", " + 0 + ", " + 0 + ", " + "out pchURLOrVideoID" + ", " + 1024 + ", " + "out pchOriginalFileName" + ", " + 260 + ", " + "out pPreviewType" + ") : " + ret + " -- " + pchURLOrVideoID + " -- " + pchOriginalFileName + " -- " + pPreviewType);
		}

		if (GUILayout.Button("GetQueryUGCNumKeyValueTags(m_UGCQueryHandle, 0)")) {
			uint ret = SteamUGC.GetQueryUGCNumKeyValueTags(m_UGCQueryHandle, 0);
			print("SteamUGC.GetQueryUGCNumKeyValueTags(" + m_UGCQueryHandle + ", " + 0 + ") : " + ret);
		}

		if (GUILayout.Button("GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0, 0, out Key, 260, out Value, 260)")) {
			string Key;
			string Value;
			bool ret = SteamUGC.GetQueryUGCKeyValueTag(m_UGCQueryHandle, 0, 0, out Key, 260, out Value, 260);
			print("SteamUGC.GetQueryUGCKeyValueTag(" + m_UGCQueryHandle + ", " + 0 + ", " + 0 + ", " + "out Key" + ", " + 260 + ", " + "out Value" + ", " + 260 + ") : " + ret + " -- " + Key + " -- " + Value);
		}

		if (GUILayout.Button("ReleaseQueryUGCRequest(m_UGCQueryHandle)")) {
			bool ret = SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
			print("SteamUGC.ReleaseQueryUGCRequest(" + m_UGCQueryHandle + ") : " + ret);
		}

		if (GUILayout.Button("AddRequiredTag(m_UGCQueryHandle, \"Co-op\")")) {
			bool ret = SteamUGC.AddRequiredTag(m_UGCQueryHandle, "Co-op");
			print("SteamUGC.AddRequiredTag(" + m_UGCQueryHandle + ", " + "\"Co-op\"" + ") : " + ret);
		}

		if (GUILayout.Button("AddExcludedTag(m_UGCQueryHandle, \"Co-op\")")) {
			bool ret = SteamUGC.AddExcludedTag(m_UGCQueryHandle, "Co-op");
			print("SteamUGC.AddExcludedTag(" + m_UGCQueryHandle + ", " + "\"Co-op\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnOnlyIDs(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnOnlyIDs(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnOnlyIDs(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnKeyValueTags(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnKeyValueTags(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnKeyValueTags(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnLongDescription(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnLongDescription(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnLongDescription(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnMetadata(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnMetadata(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnMetadata(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnChildren(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnChildren(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnChildren(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnAdditionalPreviews(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnAdditionalPreviews(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnAdditionalPreviews(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetReturnTotalOnly(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetReturnTotalOnly(m_UGCQueryHandle, true);
			print("SteamUGC.SetReturnTotalOnly(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetLanguage(m_UGCQueryHandle, \"english\")")) {
			bool ret = SteamUGC.SetLanguage(m_UGCQueryHandle, "english");
			print("SteamUGC.SetLanguage(" + m_UGCQueryHandle + ", " + "\"english\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetAllowCachedResponse(m_UGCQueryHandle, 5)")) {
			bool ret = SteamUGC.SetAllowCachedResponse(m_UGCQueryHandle, 5);
			print("SteamUGC.SetAllowCachedResponse(" + m_UGCQueryHandle + ", " + 5 + ") : " + ret);
		}

		if (GUILayout.Button("SetCloudFileNameFilter(m_UGCQueryHandle, \"\")")) {
			bool ret = SteamUGC.SetCloudFileNameFilter(m_UGCQueryHandle, "");
			print("SteamUGC.SetCloudFileNameFilter(" + m_UGCQueryHandle + ", " + "\"\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetMatchAnyTag(m_UGCQueryHandle, true)")) {
			bool ret = SteamUGC.SetMatchAnyTag(m_UGCQueryHandle, true);
			print("SteamUGC.SetMatchAnyTag(" + m_UGCQueryHandle + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("SetSearchText(m_UGCQueryHandle, \"AciD\")")) {
			bool ret = SteamUGC.SetSearchText(m_UGCQueryHandle, "AciD");
			print("SteamUGC.SetSearchText(" + m_UGCQueryHandle + ", " + "\"AciD\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetRankedByTrendDays(m_UGCQueryHandle, 7)")) {
			bool ret = SteamUGC.SetRankedByTrendDays(m_UGCQueryHandle, 7);
			print("SteamUGC.SetRankedByTrendDays(" + m_UGCQueryHandle + ", " + 7 + ") : " + ret);
		}

		if (GUILayout.Button("AddRequiredKeyValueTag(m_UGCQueryHandle, \"TestKey\", \"TestValue\")")) {
			bool ret = SteamUGC.AddRequiredKeyValueTag(m_UGCQueryHandle, "TestKey", "TestValue");
			print("SteamUGC.AddRequiredKeyValueTag(" + m_UGCQueryHandle + ", " + "\"TestKey\"" + ", " + "\"TestValue\"" + ") : " + ret);
		}

		if (GUILayout.Button("RequestUGCDetails(m_PublishedFileId, 5)")) {
			SteamAPICall_t handle = SteamUGC.RequestUGCDetails(m_PublishedFileId, 5);
			OnSteamUGCRequestUGCDetailsResultCallResult.Set(handle);
			print("SteamUGC.RequestUGCDetails(" + m_PublishedFileId + ", " + 5 + ") : " + handle);
		}

		if (GUILayout.Button("CreateItem((AppId_t)480, EWorkshopFileType.k_EWorkshopFileTypeWebGuide)")) {
			SteamAPICall_t handle = SteamUGC.CreateItem((AppId_t)480, EWorkshopFileType.k_EWorkshopFileTypeWebGuide);
			OnCreateItemResultCallResult.Set(handle);
			print("SteamUGC.CreateItem(" + (AppId_t)480 + ", " + EWorkshopFileType.k_EWorkshopFileTypeWebGuide + ") : " + handle);
		}

		if (GUILayout.Button("StartItemUpdate((AppId_t)480, m_PublishedFileId)")) {
			m_UGCUpdateHandle = SteamUGC.StartItemUpdate((AppId_t)480, m_PublishedFileId);
			print("SteamUGC.StartItemUpdate(" + (AppId_t)480 + ", " + m_PublishedFileId + ") : " + m_UGCUpdateHandle);
		}

		if (GUILayout.Button("SetItemTitle(m_UGCUpdateHandle, \"This is a Test\")")) {
			bool ret = SteamUGC.SetItemTitle(m_UGCUpdateHandle, "This is a Test");
			print("SteamUGC.SetItemTitle(" + m_UGCUpdateHandle + ", " + "\"This is a Test\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetItemDescription(m_UGCUpdateHandle, \"This is the test description.\")")) {
			bool ret = SteamUGC.SetItemDescription(m_UGCUpdateHandle, "This is the test description.");
			print("SteamUGC.SetItemDescription(" + m_UGCUpdateHandle + ", " + "\"This is the test description.\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetItemUpdateLanguage(m_UGCUpdateHandle, \"english\")")) {
			bool ret = SteamUGC.SetItemUpdateLanguage(m_UGCUpdateHandle, "english");
			print("SteamUGC.SetItemUpdateLanguage(" + m_UGCUpdateHandle + ", " + "\"english\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetItemMetadata(m_UGCUpdateHandle, \"This is the test metadata.\")")) {
			bool ret = SteamUGC.SetItemMetadata(m_UGCUpdateHandle, "This is the test metadata.");
			print("SteamUGC.SetItemMetadata(" + m_UGCUpdateHandle + ", " + "\"This is the test metadata.\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate)")) {
			bool ret = SteamUGC.SetItemVisibility(m_UGCUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
			print("SteamUGC.SetItemVisibility(" + m_UGCUpdateHandle + ", " + ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate + ") : " + ret);
		}

		if (GUILayout.Button("SetItemTags(m_UGCUpdateHandle, new string[] {\"Tag One\", \"Tag Two\", \"Test Tags\", \"Sorry\"})")) {
			bool ret = SteamUGC.SetItemTags(m_UGCUpdateHandle, new string[] {"Tag One", "Tag Two", "Test Tags", "Sorry"});
			print("SteamUGC.SetItemTags(" + m_UGCUpdateHandle + ", " + new string[] {"Tag One", "Tag Two", "Test Tags", "Sorry"} + ") : " + ret);
		}

		if (GUILayout.Button("SetItemContent(m_UGCUpdateHandle, Application.dataPath + \"/Scenes\")")) {
			bool ret = SteamUGC.SetItemContent(m_UGCUpdateHandle, Application.dataPath + "/Scenes");
			print("SteamUGC.SetItemContent(" + m_UGCUpdateHandle + ", " + Application.dataPath + "/Scenes" + ") : " + ret);
		}

		if (GUILayout.Button("SetItemPreview(m_UGCUpdateHandle, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.SetItemPreview(m_UGCUpdateHandle, Application.dataPath + "/controller.vdf");
			print("SteamUGC.SetItemPreview(" + m_UGCUpdateHandle + ", " + Application.dataPath + "/controller.vdf" + ") : " + ret);
		}

		if (GUILayout.Button("RemoveItemKeyValueTags(m_UGCUpdateHandle, \"TestKey\")")) {
			bool ret = SteamUGC.RemoveItemKeyValueTags(m_UGCUpdateHandle, "TestKey");
			print("SteamUGC.RemoveItemKeyValueTags(" + m_UGCUpdateHandle + ", " + "\"TestKey\"" + ") : " + ret);
		}

		if (GUILayout.Button("AddItemKeyValueTag(m_UGCUpdateHandle, \"TestKey\", \"TestValue\")")) {
			bool ret = SteamUGC.AddItemKeyValueTag(m_UGCUpdateHandle, "TestKey", "TestValue");
			print("SteamUGC.AddItemKeyValueTag(" + m_UGCUpdateHandle + ", " + "\"TestKey\"" + ", " + "\"TestValue\"" + ") : " + ret);
		}

		if (GUILayout.Button("AddItemPreviewFile(m_UGCUpdateHandle, Application.dataPath + \"/controller.vdf\", EItemPreviewType.k_EItemPreviewType_Image)")) {
			bool ret = SteamUGC.AddItemPreviewFile(m_UGCUpdateHandle, Application.dataPath + "/controller.vdf", EItemPreviewType.k_EItemPreviewType_Image);
			print("SteamUGC.AddItemPreviewFile(" + m_UGCUpdateHandle + ", " + Application.dataPath + "/controller.vdf" + ", " + EItemPreviewType.k_EItemPreviewType_Image + ") : " + ret);
		}

		if (GUILayout.Button("AddItemPreviewVideo(m_UGCUpdateHandle, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.AddItemPreviewVideo(m_UGCUpdateHandle, Application.dataPath + "/controller.vdf");
			print("SteamUGC.AddItemPreviewVideo(" + m_UGCUpdateHandle + ", " + Application.dataPath + "/controller.vdf" + ") : " + ret);
		}

		if (GUILayout.Button("UpdateItemPreviewFile(m_UGCUpdateHandle, 0, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.UpdateItemPreviewFile(m_UGCUpdateHandle, 0, Application.dataPath + "/controller.vdf");
			print("SteamUGC.UpdateItemPreviewFile(" + m_UGCUpdateHandle + ", " + 0 + ", " + Application.dataPath + "/controller.vdf" + ") : " + ret);
		}

		if (GUILayout.Button("UpdateItemPreviewVideo(m_UGCUpdateHandle, 0, Application.dataPath + \"/controller.vdf\")")) {
			bool ret = SteamUGC.UpdateItemPreviewVideo(m_UGCUpdateHandle, 0, Application.dataPath + "/controller.vdf");
			print("SteamUGC.UpdateItemPreviewVideo(" + m_UGCUpdateHandle + ", " + 0 + ", " + Application.dataPath + "/controller.vdf" + ") : " + ret);
		}

		if (GUILayout.Button("RemoveItemPreview(m_UGCUpdateHandle, 0)")) {
			bool ret = SteamUGC.RemoveItemPreview(m_UGCUpdateHandle, 0);
			print("SteamUGC.RemoveItemPreview(" + m_UGCUpdateHandle + ", " + 0 + ") : " + ret);
		}

		if (GUILayout.Button("SubmitItemUpdate(m_UGCUpdateHandle, \"Test Changenote\")")) {
			SteamAPICall_t handle = SteamUGC.SubmitItemUpdate(m_UGCUpdateHandle, "Test Changenote");
			OnSubmitItemUpdateResultCallResult.Set(handle);
			print("SteamUGC.SubmitItemUpdate(" + m_UGCUpdateHandle + ", " + "\"Test Changenote\"" + ") : " + handle);
		}

		{
			ulong BytesProcessed;
			ulong BytesTotal;
			EItemUpdateStatus ret = SteamUGC.GetItemUpdateProgress(m_UGCUpdateHandle, out BytesProcessed, out BytesTotal);
			GUILayout.Label("GetItemUpdateProgress(m_UGCUpdateHandle, out BytesProcessed, out BytesTotal) : " + ret + " -- " + BytesProcessed + " -- " + BytesTotal);
		}

		if (GUILayout.Button("SetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions, true)")) {
			SteamAPICall_t handle = SteamUGC.SetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions, true);
			OnSetUserItemVoteResultCallResult.Set(handle);
			print("SteamUGC.SetUserItemVote(" + TestConstants.Instance.k_PublishedFileId_Champions + ", " + true + ") : " + handle);
		}

		if (GUILayout.Button("GetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions)")) {
			SteamAPICall_t handle = SteamUGC.GetUserItemVote(TestConstants.Instance.k_PublishedFileId_Champions);
			OnGetUserItemVoteResultCallResult.Set(handle);
			print("SteamUGC.GetUserItemVote(" + TestConstants.Instance.k_PublishedFileId_Champions + ") : " + handle);
		}

		if (GUILayout.Button("AddItemToFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions)")) {
			SteamAPICall_t handle = SteamUGC.AddItemToFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions);
			OnUserFavoriteItemsListChangedCallResult.Set(handle);
			print("SteamUGC.AddItemToFavorites(" + SteamUtils.GetAppID() + ", " + TestConstants.Instance.k_PublishedFileId_Champions + ") : " + handle);
		}

		if (GUILayout.Button("RemoveItemFromFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions)")) {
			SteamAPICall_t handle = SteamUGC.RemoveItemFromFavorites(SteamUtils.GetAppID(), TestConstants.Instance.k_PublishedFileId_Champions);
			OnUserFavoriteItemsListChangedCallResult.Set(handle);
			print("SteamUGC.RemoveItemFromFavorites(" + SteamUtils.GetAppID() + ", " + TestConstants.Instance.k_PublishedFileId_Champions + ") : " + handle);
		}

		if (GUILayout.Button("SubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions)")) {
			SteamAPICall_t handle = SteamUGC.SubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions);
			OnRemoteStorageSubscribePublishedFileResultCallResult.Set(handle);
			print("SteamUGC.SubscribeItem(" + TestConstants.Instance.k_PublishedFileId_Champions + ") : " + handle);
		}

		if (GUILayout.Button("UnsubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions)")) {
			SteamAPICall_t handle = SteamUGC.UnsubscribeItem(TestConstants.Instance.k_PublishedFileId_Champions);
			OnRemoteStorageUnsubscribePublishedFileResultCallResult.Set(handle);
			print("SteamUGC.UnsubscribeItem(" + TestConstants.Instance.k_PublishedFileId_Champions + ") : " + handle);
		}

		GUILayout.Label("GetNumSubscribedItems() : " + SteamUGC.GetNumSubscribedItems());

		if (GUILayout.Button("GetSubscribedItems(PublishedFileID, (uint)PublishedFileID.Length)")) {
			PublishedFileId_t[] PublishedFileID = new PublishedFileId_t[1];
			uint ret = SteamUGC.GetSubscribedItems(PublishedFileID, (uint)PublishedFileID.Length);
			m_PublishedFileId = PublishedFileID[0];
			print("SteamUGC.GetSubscribedItems(" + PublishedFileID + ", " + (uint)PublishedFileID.Length + ") : " + ret);
			print(m_PublishedFileId);
		}

		GUILayout.Label("GetItemState(PublishedFileID) : " + (EItemState)SteamUGC.GetItemState(m_PublishedFileId));

		{
			ulong SizeOnDisk;
			string Folder;
			uint punTimeStamp;
			bool ret = SteamUGC.GetItemInstallInfo(m_PublishedFileId, out SizeOnDisk, out Folder, 1024, out punTimeStamp);
			GUILayout.Label("GetItemInstallInfo(m_PublishedFileId, out SizeOnDisk, out Folder, 1024, out punTimeStamp) : " + ret + " -- " + SizeOnDisk + " -- " + Folder + " -- " + punTimeStamp);
		}

		if (GUILayout.Button("GetItemDownloadInfo(m_PublishedFileId, out BytesDownloaded, out BytesTotal)")) {
			ulong BytesDownloaded;
			ulong BytesTotal;
			bool ret = SteamUGC.GetItemDownloadInfo(m_PublishedFileId, out BytesDownloaded, out BytesTotal);
			print("SteamUGC.GetItemDownloadInfo(" + m_PublishedFileId + ", " + "out BytesDownloaded" + ", " + "out BytesTotal" + ") : " + ret + " -- " + BytesDownloaded + " -- " + BytesTotal);
		}

		if (GUILayout.Button("DownloadItem(m_PublishedFileId, true)")) {
			bool ret = SteamUGC.DownloadItem(m_PublishedFileId, true);
			print("SteamUGC.DownloadItem(" + m_PublishedFileId + ", " + true + ") : " + ret);
		}

		if (GUILayout.Button("BInitWorkshopForGameServer((DepotId_t)481, \"C:/UGCTest\")")) {
			bool ret = SteamUGC.BInitWorkshopForGameServer((DepotId_t)481, "C:/UGCTest");
			print("SteamUGC.BInitWorkshopForGameServer(" + (DepotId_t)481 + ", " + "\"C:/UGCTest\"" + ") : " + ret);
		}

		if (GUILayout.Button("SuspendDownloads(true)")) {
			SteamUGC.SuspendDownloads(true);
			print("SteamUGC.SuspendDownloads(" + true + ")");
		}

		if (GUILayout.Button("StartPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length)")) {
			PublishedFileId_t[] PublishedFileIds = new PublishedFileId_t[] { TestConstants.Instance.k_PublishedFileId_Champions };
			SteamAPICall_t handle = SteamUGC.StartPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length);
			OnStartPlaytimeTrackingResultCallResult.Set(handle);
			print("SteamUGC.StartPlaytimeTracking(" + PublishedFileIds + ", " + (uint)PublishedFileIds.Length + ") : " + handle);
		}

		if (GUILayout.Button("StopPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length)")) {
			PublishedFileId_t[] PublishedFileIds = new PublishedFileId_t[] { TestConstants.Instance.k_PublishedFileId_Champions };
			SteamAPICall_t handle = SteamUGC.StopPlaytimeTracking(PublishedFileIds, (uint)PublishedFileIds.Length);
			OnStopPlaytimeTrackingResultCallResult.Set(handle);
			print("SteamUGC.StopPlaytimeTracking(" + PublishedFileIds + ", " + (uint)PublishedFileIds.Length + ") : " + handle);
		}

		if (GUILayout.Button("StopPlaytimeTrackingForAllItems()")) {
			SteamAPICall_t handle = SteamUGC.StopPlaytimeTrackingForAllItems();
			OnStopPlaytimeTrackingResultCallResult.Set(handle);
			print("SteamUGC.StopPlaytimeTrackingForAllItems() : " + handle);
		}
	}

	void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SteamUGCQueryCompleted_t.k_iCallback + " - SteamUGCQueryCompleted] - " + pCallback.m_handle + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unNumResultsReturned + " -- " + pCallback.m_unTotalMatchingResults + " -- " + pCallback.m_bCachedData);
	}

	void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SteamUGCRequestUGCDetailsResult_t.k_iCallback + " - SteamUGCRequestUGCDetailsResult] - " + pCallback.m_details + " -- " + pCallback.m_bCachedData);

		Debug.Log(pCallback.m_details.m_nPublishedFileId + " -- " + pCallback.m_details.m_eResult + " -- " + pCallback.m_details.m_eFileType + " -- " + pCallback.m_details.m_nCreatorAppID + " -- " + pCallback.m_details.m_nConsumerAppID + " -- " + pCallback.m_details.m_rgchTitle + " -- " + pCallback.m_details.m_rgchDescription + " -- " + pCallback.m_details.m_ulSteamIDOwner + " -- " + pCallback.m_details.m_rtimeCreated + " -- " + pCallback.m_details.m_rtimeUpdated + " -- " + pCallback.m_details.m_rtimeAddedToUserList + " -- " + pCallback.m_details.m_eVisibility + " -- " + pCallback.m_details.m_bBanned + " -- " + pCallback.m_details.m_bAcceptedForUse + " -- " + pCallback.m_details.m_bTagsTruncated + " -- " + pCallback.m_details.m_rgchTags + " -- " + pCallback.m_details.m_hFile + " -- " + pCallback.m_details.m_hPreviewFile + " -- " + pCallback.m_details.m_pchFileName + " -- " + pCallback.m_details.m_nFileSize + " -- " + pCallback.m_details.m_nPreviewFileSize + " -- " + pCallback.m_details.m_rgchURL + " -- " + pCallback.m_details.m_unVotesUp + " -- " + pCallback.m_details.m_unVotesDown + " -- " + pCallback.m_details.m_flScore + " -- " + pCallback.m_details.m_unNumChildren);
	}

	void OnCreateItemResult(CreateItemResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + CreateItemResult_t.k_iCallback + " - CreateItemResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);

		m_PublishedFileId = pCallback.m_nPublishedFileId;
	}

	void OnSubmitItemUpdateResult(SubmitItemUpdateResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult] - " + pCallback.m_eResult + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
	}

	void OnItemInstalled(ItemInstalled_t pCallback) {
		Debug.Log("[" + ItemInstalled_t.k_iCallback + " - ItemInstalled] - " + pCallback.m_unAppID + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnDownloadItemResult(DownloadItemResult_t pCallback) {
		Debug.Log("[" + DownloadItemResult_t.k_iCallback + " - DownloadItemResult] - " + pCallback.m_unAppID + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eResult);
	}

	void OnUserFavoriteItemsListChanged(UserFavoriteItemsListChanged_t pCallback, bool bIOFailure) {
		Debug.Log("[" + UserFavoriteItemsListChanged_t.k_iCallback + " - UserFavoriteItemsListChanged] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eResult + " -- " + pCallback.m_bWasAddRequest);
	}

	void OnSetUserItemVoteResult(SetUserItemVoteResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + SetUserItemVoteResult_t.k_iCallback + " - SetUserItemVoteResult] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eResult + " -- " + pCallback.m_bVoteUp);
	}

	void OnGetUserItemVoteResult(GetUserItemVoteResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + GetUserItemVoteResult_t.k_iCallback + " - GetUserItemVoteResult] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eResult + " -- " + pCallback.m_bVotedUp + " -- " + pCallback.m_bVotedDown + " -- " + pCallback.m_bVoteSkipped);
	}

	void OnStartPlaytimeTrackingResult(StartPlaytimeTrackingResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + StartPlaytimeTrackingResult_t.k_iCallback + " - StartPlaytimeTrackingResult] - " + pCallback.m_eResult);
	}

	void OnStopPlaytimeTrackingResult(StopPlaytimeTrackingResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + StopPlaytimeTrackingResult_t.k_iCallback + " - StopPlaytimeTrackingResult] - " + pCallback.m_eResult);
	}
}