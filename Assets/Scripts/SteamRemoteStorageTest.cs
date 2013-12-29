using UnityEngine;
using System.Collections;
using Steamworks;

class SteamRemoteStorageTest : MonoBehaviour {
	const string MESSAGE_FILE_NAME = "message.dat";
	private string m_Message = "";
	private int m_FileCount;
	private int m_FileSize;
	private int m_TotalBytes;
	private int m_FileSizeInBytes;
	private UGCFileWriteStreamHandle_t m_FileStream;
	private UGCHandle_t m_UGCHandle;
	private PublishedFileId_t m_PublishedFileId;
	private PublishedFileUpdateHandle_t m_PublishedFileUpdateHandle;

	CallResult<RemoteStorageFileShareResult_t> RemoteStorageFileShareResult;
	CallResult<RemoteStoragePublishFileResult_t> RemoteStoragePublishFileResult;
	CallResult<RemoteStorageDeletePublishedFileResult_t> RemoteStorageDeletePublishedFileResult;
	CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t> RemoteStorageEnumerateUserPublishedFilesResult;
	CallResult<RemoteStorageSubscribePublishedFileResult_t> RemoteStorageSubscribePublishedFileResult;
	CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> RemoteStorageEnumerateUserSubscribedFilesResult;
	CallResult<RemoteStorageUnsubscribePublishedFileResult_t> RemoteStorageUnsubscribePublishedFileResult;
	CallResult<RemoteStorageUpdatePublishedFileResult_t> RemoteStorageUpdatePublishedFileResult;
	CallResult<RemoteStorageDownloadUGCResult_t> RemoteStorageDownloadUGCResult;
	CallResult<RemoteStorageGetPublishedFileDetailsResult_t> RemoteStorageGetPublishedFileDetailsResult;
	CallResult<RemoteStorageEnumerateWorkshopFilesResult_t> RemoteStorageEnumerateWorkshopFilesResult;
	CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t> RemoteStorageGetPublishedItemVoteDetailsResult;
	CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t> RemoteStorageUpdateUserPublishedItemVoteResult;
	CallResult<RemoteStorageUserVoteDetails_t> RemoteStorageUserVoteDetails;
	CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t> RemoteStorageEnumerateUserSharedWorkshopFilesResult;
	CallResult<RemoteStorageSetUserPublishedFileActionResult_t> RemoteStorageSetUserPublishedFileActionResult;
	CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t> RemoteStorageEnumeratePublishedFilesByUserActionResult;

	public void OnEnable() {
		new Callback<RemoteStorageAppSyncedClient_t>(OnRemoteStorageAppSyncedClient);
		new Callback<RemoteStorageAppSyncedServer_t>(OnRemoteStorageAppSyncedServer);
		new Callback<RemoteStorageAppSyncProgress_t>(OnRemoteStorageAppSyncProgress);
		new Callback<RemoteStorageAppSyncStatusCheck_t>(OnRemoteStorageAppSyncStatusCheck);
		new Callback<RemoteStorageConflictResolution_t>(OnRemoteStorageConflictResolution);
		RemoteStorageFileShareResult = new CallResult<RemoteStorageFileShareResult_t>(OnRemoteStorageFileShareResult);
		RemoteStoragePublishFileResult = new CallResult<RemoteStoragePublishFileResult_t>(OnRemoteStoragePublishFileResult);
		RemoteStorageDeletePublishedFileResult = new CallResult<RemoteStorageDeletePublishedFileResult_t>(OnRemoteStorageDeletePublishedFileResult);
		RemoteStorageEnumerateUserPublishedFilesResult = new CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t>(OnRemoteStorageEnumerateUserPublishedFilesResult);
		RemoteStorageSubscribePublishedFileResult = new CallResult<RemoteStorageSubscribePublishedFileResult_t>(OnRemoteStorageSubscribePublishedFileResult);
		RemoteStorageEnumerateUserSubscribedFilesResult = new CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t>(OnRemoteStorageEnumerateUserSubscribedFilesResult);
		RemoteStorageUnsubscribePublishedFileResult = new CallResult<RemoteStorageUnsubscribePublishedFileResult_t>(OnRemoteStorageUnsubscribePublishedFileResult);
		RemoteStorageUpdatePublishedFileResult = new CallResult<RemoteStorageUpdatePublishedFileResult_t>(OnRemoteStorageUpdatePublishedFileResult);
		RemoteStorageDownloadUGCResult = new CallResult<RemoteStorageDownloadUGCResult_t>(OnRemoteStorageDownloadUGCResult);
		RemoteStorageGetPublishedFileDetailsResult = new CallResult<RemoteStorageGetPublishedFileDetailsResult_t>(OnRemoteStorageGetPublishedFileDetailsResult);
		RemoteStorageEnumerateWorkshopFilesResult = new CallResult<RemoteStorageEnumerateWorkshopFilesResult_t>(OnRemoteStorageEnumerateWorkshopFilesResult);
		RemoteStorageGetPublishedItemVoteDetailsResult = new CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t>(OnRemoteStorageGetPublishedItemVoteDetailsResult);
		new Callback<RemoteStoragePublishedFileSubscribed_t>(OnRemoteStoragePublishedFileSubscribed);
		new Callback<RemoteStoragePublishedFileUnsubscribed_t>(OnRemoteStoragePublishedFileUnsubscribed);
		new Callback<RemoteStoragePublishedFileDeleted_t>(OnRemoteStoragePublishedFileDeleted);
		RemoteStorageUpdateUserPublishedItemVoteResult = new CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t>(OnRemoteStorageUpdateUserPublishedItemVoteResult);
		RemoteStorageUserVoteDetails = new CallResult<RemoteStorageUserVoteDetails_t>(OnRemoteStorageUserVoteDetails);
		RemoteStorageEnumerateUserSharedWorkshopFilesResult = new CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t>(OnRemoteStorageEnumerateUserSharedWorkshopFilesResult);
		RemoteStorageSetUserPublishedFileActionResult = new CallResult<RemoteStorageSetUserPublishedFileActionResult_t>(OnRemoteStorageSetUserPublishedFileActionResult);
		RemoteStorageEnumeratePublishedFilesByUserActionResult = new CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t>(OnRemoteStorageEnumeratePublishedFilesByUserActionResult);
		new Callback<RemoteStoragePublishFileProgress_t>(OnRemoteStoragePublishFileProgress);
		new Callback<RemoteStoragePublishedFileUpdated_t>(OnRemoteStoragePublishedFileUpdated);
	}
	
	public void RenderOnGUI(SteamTest.EGUIState state) {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Message: ");
		m_Message = GUILayout.TextField(m_Message, 40);
		GUILayout.Label("m_FileCount: " + m_FileCount);
		GUILayout.Label("m_FileSize: " + m_FileSize);
		GUILayout.Label("m_TotalBytes: " + m_TotalBytes);
		GUILayout.Label("m_FileSizeInBytes: " + m_FileSizeInBytes);
		GUILayout.Label("m_FileStream: " + m_FileStream);
		GUILayout.Label("m_UGCHandle: " + m_UGCHandle);
		GUILayout.Label("m_PublishedFileId: " + m_PublishedFileId);
		GUILayout.Label("m_PublishedFileUpdateHandle: " + m_PublishedFileUpdateHandle);
		GUILayout.EndArea();

		if (state == SteamTest.EGUIState.SteamRemoteStorage) {
			RenderPageOne();
		}
		else {
			RenderPageTwo();
		}
	}

	private void RenderPageOne() {
		if (GUILayout.Button("FileWrite(MESSAGE_FILE_NAME, Data, Data.Length)")) {
			if (System.Text.Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes) {
				print("Remote Storage: Quota Exceeded! - Bytes: " + System.Text.Encoding.UTF8.GetByteCount(m_Message) + " - Max: " + m_TotalBytes);
			}
			else {
				byte[] Data = new byte[System.Text.Encoding.UTF8.GetByteCount(m_Message)];
				System.Text.Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, Data, 0);

				bool ret = SteamRemoteStorage.FileWrite(MESSAGE_FILE_NAME, Data, Data.Length);
				print("FileWrite(" + MESSAGE_FILE_NAME + ", Data, " + Data.Length + ") - " + ret);
			}
		}

		if (GUILayout.Button("FileRead(MESSAGE_FILE_NAME, Data, Data.Length)")) {
			if (m_FileSize > 40) {
				byte[] c = { 0 };
				Debug.Log("RemoteStorage: File was larger than expected. . .");
				SteamRemoteStorage.FileWrite(MESSAGE_FILE_NAME, c, 1);
			}
			else {
				byte[] Data = new byte[40];
				int ret = SteamRemoteStorage.FileRead(MESSAGE_FILE_NAME, Data, Data.Length);
				m_Message = System.Text.Encoding.UTF8.GetString(Data, 0, ret);
				print("FileRead(" + MESSAGE_FILE_NAME + ", Data, " + Data.Length + ") - " + ret);
			}
		}

		if (GUILayout.Button("FileForget(MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.FileForget(MESSAGE_FILE_NAME);
			print("FileForget(" + MESSAGE_FILE_NAME + ") - " + ret);
		}

		if (GUILayout.Button("FileDelete(MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.FileDelete(MESSAGE_FILE_NAME);
			print("FileDelete(" + MESSAGE_FILE_NAME + ") - " + ret);
		}

		if (GUILayout.Button("FileShare(MESSAGE_FILE_NAME)")) {
			ulong handle = SteamRemoteStorage.FileShare(MESSAGE_FILE_NAME);
			RemoteStorageFileShareResult.SetAPICallHandle(handle);
			print("FileShare(" + MESSAGE_FILE_NAME + ") - " + handle);
		}

		if (GUILayout.Button("SetSyncPlatforms(MESSAGE_FILE_NAME, k_ERemoteStoragePlatformAll)")) {
			bool ret = SteamRemoteStorage.SetSyncPlatforms(MESSAGE_FILE_NAME, ERemoteStoragePlatform.k_ERemoteStoragePlatformAll);
			print("SetSyncPlatforms(" + MESSAGE_FILE_NAME + ", ERemoteStoragePlatform.k_ERemoteStoragePlatformAll) - " + ret);
		}

		if (GUILayout.Button("FileWriteStreamOpen(MESSAGE_FILE_NAME)")) {
			m_FileStream = SteamRemoteStorage.FileWriteStreamOpen(MESSAGE_FILE_NAME);
			print("FileWriteStreamOpen(" + MESSAGE_FILE_NAME + ") - " + m_FileStream);
		}

		if (GUILayout.Button("FileWriteStreamWriteChunk(m_FileStream, Data, Data.Length)")) {
			if (System.Text.Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes) {
				print("Remote Storage: Quota Exceeded! - Bytes: " + System.Text.Encoding.UTF8.GetByteCount(m_Message) + " - Max: " + m_TotalBytes);
			}
			else {
				byte[] Data = new byte[System.Text.Encoding.UTF8.GetByteCount(m_Message)];
				System.Text.Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, Data, 0);

				bool ret = SteamRemoteStorage.FileWriteStreamWriteChunk(m_FileStream, Data, Data.Length);
				print("FileWriteStreamWriteChunk(" + m_FileStream + ", Data, " + Data.Length + ") - " + ret);
			}
		}

		if (GUILayout.Button("FileWriteStreamClose(m_FileStream)")) {
			bool ret = SteamRemoteStorage.FileWriteStreamClose(m_FileStream);
			print("FileWriteStreamClose(" + m_FileStream + ") - " + ret);
		}

		if (GUILayout.Button("FileWriteStreamCancel(m_FileStream)")) {
			bool ret = SteamRemoteStorage.FileWriteStreamCancel(m_FileStream);
			print("FileWriteStreamCancel(" + m_FileStream + ") - " + ret);
		}

		GUILayout.Label("FileExists(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FileExists(MESSAGE_FILE_NAME));
		GUILayout.Label("FilePersisted(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FilePersisted(MESSAGE_FILE_NAME));
		GUILayout.Label("GetFileSize(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileSize(MESSAGE_FILE_NAME));
		GUILayout.Label("GetFileTimestamp(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileTimestamp(MESSAGE_FILE_NAME));
		GUILayout.Label("GetSyncPlatforms(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetSyncPlatforms(MESSAGE_FILE_NAME));

		m_FileCount = SteamRemoteStorage.GetFileCount();
		GUILayout.Label("GetFileCount() : " + m_FileCount);
		for (int i = 0; i < m_FileCount; ++i) {
			string FileName = SteamRemoteStorage.GetFileNameAndSize(i, out m_FileSize);
			GUILayout.Label("GetFileNameAndSize(i, out FileSize) : " + FileName + " -- " + m_FileSize);
		}

		{
			int AvailableBytes;
			bool ret = SteamRemoteStorage.GetQuota(out m_TotalBytes, out AvailableBytes);
			GUILayout.Label("GetQuota(out m_TotalBytes, out AvailableBytes) : " + ret + " -- " + m_TotalBytes + " -- " + AvailableBytes);
		}

		GUILayout.Label("IsCloudEnabledForAccount() : " + SteamRemoteStorage.IsCloudEnabledForAccount());

		{
			bool CloudEnabled = SteamRemoteStorage.IsCloudEnabledForApp();
			GUILayout.Label("IsCloudEnabledForApp() : " + CloudEnabled);

			if (GUILayout.Button("SetCloudEnabledForApp(!CloudEnabled)")) {
				SteamRemoteStorage.SetCloudEnabledForApp(!CloudEnabled);
				print("SetCloudEnabledForApp(!CloudEnabled)");
			}
		}
	}

	private void RenderPageTwo() {
		if (GUILayout.Button("UGCDownload(m_UGCHandle, 0)")) {
			ulong handle = SteamRemoteStorage.UGCDownload(m_UGCHandle, 0);
			RemoteStorageDownloadUGCResult.SetAPICallHandle(handle);
			print("UGCDownload(" + m_UGCHandle + ", 0) - " + handle);
		}

		{
			int BytesDownloaded;
			int BytesExpected;
			bool ret = SteamRemoteStorage.GetUGCDownloadProgress(m_UGCHandle, out BytesDownloaded, out BytesExpected);
			GUILayout.Label("GetUGCDownloadProgress(m_UGCHandle, out BytesDownloaded, out BytesExpected) : " + ret + " -- " + BytesDownloaded + " -- " + BytesExpected);
		}

		{
			uint AppID;
			string Name;
			ulong SteamIDOwner;
			bool ret = SteamRemoteStorage.GetUGCDetails(m_UGCHandle, out AppID, out Name, out m_FileSizeInBytes, out SteamIDOwner);
			GUILayout.Label("GetUGCDetails(m_UGCHandle, out AppID, Name, out FileSizeInBytes, out SteamIDOwner) : " + ret + " -- " + AppID + " -- " + Name + " -- " + m_FileSizeInBytes + " -- " + SteamIDOwner);
		}

		if (GUILayout.Button("UGCRead(m_UGCHandle, Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close)")) {
			byte[] Data = new byte[m_FileSizeInBytes];
			int ret = SteamRemoteStorage.UGCRead(m_UGCHandle, Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close);
			print("UGCRead(" + m_UGCHandle + ", Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close) - " + ret + " -- " + System.Text.Encoding.UTF8.GetString(Data, 0, ret));
		}

		GUILayout.Label("GetCachedUGCCount() : " + SteamRemoteStorage.GetCachedUGCCount());
		GUILayout.Label("GetCachedUGCHandle(0) : " + SteamRemoteStorage.GetCachedUGCHandle(0));

#if _PS3 || _SERVER
		//GUILayout.Label(" : " + SteamRemoteStorage.GetFileListFromServer());
		//GUILayout.Label(" : " + SteamRemoteStorage.FileFetch(string pchFile));
		//GUILayout.Label(" : " + SteamRemoteStorage.FilePersist(string pchFile));
		//GUILayout.Label(" : " + SteamRemoteStorage.SynchronizeToClient());
		//GUILayout.Label(" : " + SteamRemoteStorage.SynchronizeToServer());
		//GUILayout.Label(" : " + SteamRemoteStorage.ResetFileRequestState());
#endif

		if (GUILayout.Button("PublishWorkshopFile([...])")) {
			string[] Tags = { "Test1", "Test2", "Test3" };
			ulong handle = SteamRemoteStorage.PublishWorkshopFile(MESSAGE_FILE_NAME, null, SteamUtils.GetAppID(), "Title!", "Description!", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, Tags, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
			RemoteStoragePublishFileResult.SetAPICallHandle(handle);
			print("PublishWorkshopFile(" + MESSAGE_FILE_NAME + ", null, " + SteamUtils.GetAppID() + ", \"Title!\", \"Description!\", k_ERemoteStoragePublishedFileVisibilityPublic, SteamParamStringArray(list), k_EWorkshopFileTypeCommunity)");
		}

		if (GUILayout.Button("CreatePublishedFileUpdateRequest(m_PublishedFileId)")) {
			m_PublishedFileUpdateHandle = SteamRemoteStorage.CreatePublishedFileUpdateRequest(m_PublishedFileId);
			print("CreatePublishedFileUpdateRequest(" + m_PublishedFileId + ") - " + m_PublishedFileUpdateHandle);
		}

		if (GUILayout.Button("UpdatePublishedFileFile(m_PublishedFileUpdateHandle, MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileFile(m_PublishedFileUpdateHandle, MESSAGE_FILE_NAME);
			print("UpdatePublishedFileFile(" + m_PublishedFileUpdateHandle + ", " + MESSAGE_FILE_NAME + ") - " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null);
			print("UpdatePublishedFilePreviewFile(" + m_PublishedFileUpdateHandle + ", " + null + ") - " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, \"New Title\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, "New Title");
			print("UpdatePublishedFileTitle(" + m_PublishedFileUpdateHandle + ", \"New Title\") - " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, \"New Description\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, "New Description");
			print("UpdatePublishedFileDescription(" + m_PublishedFileUpdateHandle + ", \"New Description\") - " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, k_ERemoteStoragePublishedFileVisibilityPublic)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			print("UpdatePublishedFileVisibility(" + m_PublishedFileUpdateHandle + ", " + ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic + ") - " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[] {\"First\", \"Second\", \"Third\"})")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[] {"First", "Second", "Third"});
			print("UpdatePublishedFileTags(" + m_PublishedFileUpdateHandle + ", " + new string[] { "First", "Second", "Third" } + ") - " + ret);
		}
				
		if(GUILayout.Button("CommitPublishedFileUpdate(m_PublishedFileUpdateHandle)")) {
			ulong handle = SteamRemoteStorage.CommitPublishedFileUpdate(m_PublishedFileUpdateHandle);
			RemoteStorageUpdatePublishedFileResult.SetAPICallHandle(handle);
			print("CommitPublishedFileUpdate(" + m_PublishedFileUpdateHandle + ") - " + handle);
		}

		if (GUILayout.Button("GetPublishedFileDetails(m_PublishedFileId, 0)")) {
			ulong handle = SteamRemoteStorage.GetPublishedFileDetails(m_PublishedFileId, 0);
			RemoteStorageGetPublishedFileDetailsResult.SetAPICallHandle(handle);
			print("GetPublishedFileDetails(" + m_UGCHandle + ", 0) - " + handle);
		}

		if (GUILayout.Button("DeletePublishedFile(m_PublishedFileId)")) {
			ulong handle = SteamRemoteStorage.DeletePublishedFile(m_PublishedFileId);
			RemoteStorageDeletePublishedFileResult.SetAPICallHandle(handle);
			print("DeletePublishedFile(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("EnumerateUserPublishedFiles(0)")) {
			ulong handle = SteamRemoteStorage.EnumerateUserPublishedFiles(0);
			RemoteStorageEnumerateUserPublishedFilesResult.SetAPICallHandle(handle);
			print("EnumerateUserPublishedFiles(0) - " + handle);
		}

		if (GUILayout.Button("SubscribePublishedFile(m_PublishedFileId)")) {
			ulong handle = SteamRemoteStorage.SubscribePublishedFile(m_PublishedFileId);
			RemoteStorageSubscribePublishedFileResult.SetAPICallHandle(handle);
			print("SubscribePublishedFile(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("EnumerateUserSubscribedFiles(0)")) {
			ulong handle = SteamRemoteStorage.EnumerateUserSubscribedFiles(0);
			RemoteStorageEnumerateUserSubscribedFilesResult.SetAPICallHandle(handle);
			print("EnumerateUserSubscribedFiles(0) - " + handle);
		}

		if (GUILayout.Button("UnsubscribePublishedFile(m_PublishedFileId)")) {
			ulong handle = SteamRemoteStorage.UnsubscribePublishedFile(m_PublishedFileId);
			RemoteStorageUnsubscribePublishedFileResult.SetAPICallHandle(handle);
			print("UnsubscribePublishedFile(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, \"Changelog!\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, "Changelog!");
			print("UpdatePublishedFileSetChangeDescription(" + m_PublishedFileUpdateHandle + ", \"Changelog!\") - " + ret);
		}

		if (GUILayout.Button("GetPublishedItemVoteDetails(m_PublishedFileId)")) {
			ulong handle = SteamRemoteStorage.GetPublishedItemVoteDetails(m_PublishedFileId);
			RemoteStorageGetPublishedItemVoteDetailsResult.SetAPICallHandle(handle);
			print("GetPublishedItemVoteDetails(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("UpdateUserPublishedItemVote(m_PublishedFileId, true)")) {
			ulong handle = SteamRemoteStorage.UpdateUserPublishedItemVote(m_PublishedFileId, true);
			RemoteStorageUpdateUserPublishedItemVoteResult.SetAPICallHandle(handle);
			print("UpdateUserPublishedItemVote(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("GetUserPublishedItemVoteDetails(m_PublishedFileId)")) {
			ulong handle = SteamRemoteStorage.GetUserPublishedItemVoteDetails(m_PublishedFileId);
			RemoteStorageUserVoteDetails.SetAPICallHandle(handle);
			print("GetUserPublishedItemVoteDetails(" + m_PublishedFileId + ") - " + handle);
		}

		if (GUILayout.Button("EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0, null, null)")) {
			ulong handle = SteamRemoteStorage.EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0, null, null);
			RemoteStorageEnumerateUserSharedWorkshopFilesResult.SetAPICallHandle(handle);
			print("EnumerateUserSharedWorkshopFiles(" + SteamUser.GetSteamID() + ", 0, System.IntPtr.Zero, System.IntPtr.Zero) - " + handle);
		}

		if (GUILayout.Button("PublishVideo([...])")) {
			ulong handle = SteamRemoteStorage.PublishVideo(EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube, "William Hunter", "Rmvb4Hktv7U", null, SteamUtils.GetAppID(), "Test Video", "Desc", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, null);
			RemoteStoragePublishFileResult.SetAPICallHandle(handle);
			print("PublishVideo(k_EWorkshopVideoProviderYoutube, \"William Hunter\", \"Rmvb4Hktv7U\", null, SteamUtils.GetAppID(), \"Test Video\", \"Desc\", k_ERemoteStoragePublishedFileVisibilityPublic, null)");
		}

		if (GUILayout.Button("SetUserPublishedFileAction(m_PublishedFileId, k_EWorkshopFileActionPlayed)")) {
			ulong handle = SteamRemoteStorage.SetUserPublishedFileAction(m_PublishedFileId, EWorkshopFileAction.k_EWorkshopFileActionPlayed);
			RemoteStorageSetUserPublishedFileActionResult.SetAPICallHandle(handle);
			print("SetUserPublishedFileAction(" + m_PublishedFileId + ", " + EWorkshopFileAction.k_EWorkshopFileActionPlayed + ") - " + handle);
		}

		if (GUILayout.Button("EnumeratePublishedFilesByUserAction(k_EWorkshopFileActionPlayed, 0)")) {
			ulong handle = SteamRemoteStorage.EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0);
			RemoteStorageEnumeratePublishedFilesByUserActionResult.SetAPICallHandle(handle);
			print("EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0) - " + handle);
		}

		if (GUILayout.Button("EnumeratePublishedWorkshopFiles(k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, IntPtr.Zero, IntPtr.Zero)")) {
			ulong handle = SteamRemoteStorage.EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, null, null);
			RemoteStorageEnumerateWorkshopFilesResult.SetAPICallHandle(handle);
			print("EnumeratePublishedWorkshopFiles(k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, IntPtr.Zero, IntPtr.Zero) - " + handle);
		}

		// There is absolutely no documentation on how to use this function, or what CallResult it gives you...
		// If you figure out how to use this then let me know!
		/*if (GUILayout.Button("UGCDownloadToLocation(m_UGCHandle, \"C:\\\", 0)")) {
			ulong handle = SteamRemoteStorage.UGCDownloadToLocation(m_UGCHandle, "C:\\", 0);

			print("UGCDownloadToLocation(m_UGCHandle, \"C:\\\", 0)");
		}*/
	}

	void OnRemoteStorageAppSyncedClient(RemoteStorageAppSyncedClient_t pCallback) {
		Debug.Log("[" + RemoteStorageAppSyncedClient_t.k_iCallback + " - RemoteStorageAppSyncedClient] - " + pCallback.m_nAppID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unNumDownloads);
	}

	void OnRemoteStorageAppSyncedServer(RemoteStorageAppSyncedServer_t pCallback) {
		Debug.Log("[" + RemoteStorageAppSyncedServer_t.k_iCallback + " - RemoteStorageAppSyncedServer] - " + pCallback.m_nAppID + " -- " + pCallback.m_eResult + " -- " + pCallback.m_unNumUploads);
	}

	void OnRemoteStorageAppSyncProgress(RemoteStorageAppSyncProgress_t pCallback) {
		Debug.Log("[" + RemoteStorageAppSyncProgress_t.k_iCallback + " - RemoteStorageAppSyncProgress] - " + pCallback.m_rgchCurrentFile + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_uBytesTransferredThisChunk + " -- " + pCallback.m_dAppPercentComplete + " -- " + pCallback.m_bUploading);
	}

	void OnRemoteStorageAppSyncStatusCheck(RemoteStorageAppSyncStatusCheck_t pCallback) {
		Debug.Log("[" + RemoteStorageAppSyncStatusCheck_t.k_iCallback + " - RemoteStorageAppSyncStatusCheck] - " + pCallback.m_nAppID + " -- " + pCallback.m_eResult);
	}

	void OnRemoteStorageConflictResolution(RemoteStorageConflictResolution_t pCallback) {
		Debug.Log("[" + RemoteStorageConflictResolution_t.k_iCallback + " - RemoteStorageConflictResolution] - " + pCallback.m_nAppID + " -- " + pCallback.m_eResult);
	}

	void OnRemoteStorageFileShareResult(ulong handle, RemoteStorageFileShareResult_t pCallback) {
		Debug.Log("[" + RemoteStorageFileShareResult_t.k_iCallback + " - RemoteStorageFileShareResult] - " + pCallback.m_eResult + " -- " + pCallback.m_hFile);
		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	void OnRemoteStoragePublishFileResult(ulong handle, RemoteStoragePublishFileResult_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishFileResult_t.k_iCallback + " - RemoteStoragePublishFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_PublishedFileId = pCallback.m_nPublishedFileId;
		}
	}

	void OnRemoteStorageDeletePublishedFileResult(ulong handle, RemoteStorageDeletePublishedFileResult_t pCallback) {
		Debug.Log("[" + RemoteStorageDeletePublishedFileResult_t.k_iCallback + " - RemoteStorageDeletePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageEnumerateUserPublishedFilesResult(ulong handle, RemoteStorageEnumerateUserPublishedFilesResult_t pCallback) {
		Debug.Log("[" + RemoteStorageEnumerateUserPublishedFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserPublishedFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
	}

	void OnRemoteStorageSubscribePublishedFileResult(ulong handle, RemoteStorageSubscribePublishedFileResult_t pCallback) {
		Debug.Log("[" + RemoteStorageSubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageSubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageEnumerateUserSubscribedFilesResult(ulong handle, RemoteStorageEnumerateUserSubscribedFilesResult_t pCallback) {
		Debug.Log("[" + RemoteStorageEnumerateUserSubscribedFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserSubscribedFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgRTimeSubscribed);
	}

	void OnRemoteStorageUnsubscribePublishedFileResult(ulong handle, RemoteStorageUnsubscribePublishedFileResult_t pCallback) {
		Debug.Log("[" + RemoteStorageUnsubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageUnsubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageUpdatePublishedFileResult(ulong handle, RemoteStorageUpdatePublishedFileResult_t pCallback) {
		Debug.Log("[" + RemoteStorageUpdatePublishedFileResult_t.k_iCallback + " - RemoteStorageUpdatePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
	}

	void OnRemoteStorageDownloadUGCResult(ulong handle, RemoteStorageDownloadUGCResult_t pCallback) {
		Debug.Log("[" + RemoteStorageDownloadUGCResult_t.k_iCallback + " - RemoteStorageDownloadUGCResult] - " + pCallback.m_eResult + " -- " + pCallback.m_hFile + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_nSizeInBytes + " -- " + pCallback.m_pchFileName + " -- " + pCallback.m_ulSteamIDOwner);
	}

	void OnRemoteStorageGetPublishedFileDetailsResult(ulong handle, RemoteStorageGetPublishedFileDetailsResult_t pCallback) {
		Debug.Log("[" + RemoteStorageGetPublishedFileDetailsResult_t.k_iCallback + " - RemoteStorageGetPublishedFileDetailsResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nCreatorAppID + " -- " + pCallback.m_nConsumerAppID + " -- " + pCallback.m_rgchTitle + " -- " + pCallback.m_rgchDescription + " -- " + pCallback.m_hFile + " -- " + pCallback.m_hPreviewFile + " -- " + pCallback.m_ulSteamIDOwner + " -- " + pCallback.m_rtimeCreated + " -- " + pCallback.m_rtimeUpdated + " -- " + pCallback.m_eVisibility + " -- " + pCallback.m_bBanned + " -- " + pCallback.m_rgchTags + " -- " + pCallback.m_bTagsTruncated + " -- " + pCallback.m_pchFileName + " -- " + pCallback.m_nFileSize + " -- " + pCallback.m_nPreviewFileSize + " -- " + pCallback.m_rgchURL + " -- " + pCallback.m_eFileType + " -- " + pCallback.m_bAcceptedForUse);
		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	void OnRemoteStorageEnumerateWorkshopFilesResult(ulong handle, RemoteStorageEnumerateWorkshopFilesResult_t pCallback) {
		Debug.Log("[" + RemoteStorageEnumerateWorkshopFilesResult_t.k_iCallback + " - RemoteStorageEnumerateWorkshopFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgScore + " -- " + pCallback.m_nAppId + " -- " + pCallback.m_unStartIndex);
		for (int i = 0; i < pCallback.m_nResultsReturned; ++i) {
			print(i + ": " + pCallback.m_rgPublishedFileId[i]);
		}

		if(pCallback.m_nResultsReturned >= 1) {
			m_PublishedFileId = pCallback.m_rgPublishedFileId[0];
		}
	}

	void OnRemoteStorageGetPublishedItemVoteDetailsResult(ulong handle, RemoteStorageGetPublishedItemVoteDetailsResult_t pCallback) {
		Debug.Log("[" + RemoteStorageGetPublishedItemVoteDetailsResult_t.k_iCallback + " - RemoteStorageGetPublishedItemVoteDetailsResult_t] - " + pCallback.m_eResult + " -- " + pCallback.m_unPublishedFileId + " -- " + pCallback.m_nVotesFor + " -- " + pCallback.m_nVotesAgainst + " -- " + pCallback.m_nReports + " -- " + pCallback.m_fScore);
	}

	void OnRemoteStoragePublishedFileSubscribed(RemoteStoragePublishedFileSubscribed_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishedFileSubscribed_t.k_iCallback + " - RemoteStoragePublishedFileSubscribed] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nAppID);
	}

	void OnRemoteStoragePublishedFileUnsubscribed(RemoteStoragePublishedFileUnsubscribed_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishedFileUnsubscribed_t.k_iCallback + " - RemoteStoragePublishedFileUnsubscribed] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nAppID);
	}

	void OnRemoteStoragePublishedFileDeleted(RemoteStoragePublishedFileDeleted_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishedFileDeleted_t.k_iCallback + " - RemoteStoragePublishedFileDeleted] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nAppID);
	}

	void OnRemoteStorageUpdateUserPublishedItemVoteResult(ulong handle, RemoteStorageUpdateUserPublishedItemVoteResult_t pCallback) {
		Debug.Log("[" + RemoteStorageUpdateUserPublishedItemVoteResult_t.k_iCallback + " - RemoteStorageUpdateUserPublishedItemVoteResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageUserVoteDetails(ulong handle, RemoteStorageUserVoteDetails_t pCallback) {
		Debug.Log("[" + RemoteStorageUserVoteDetails_t.k_iCallback + " - RemoteStorageUserVoteDetails] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eVote);
	}

	void OnRemoteStorageEnumerateUserSharedWorkshopFilesResult(ulong handle, RemoteStorageEnumerateUserSharedWorkshopFilesResult_t pCallback) {
		Debug.Log("[" + RemoteStorageEnumerateUserSharedWorkshopFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserSharedWorkshopFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
	}

	void OnRemoteStorageSetUserPublishedFileActionResult(ulong handle, RemoteStorageSetUserPublishedFileActionResult_t pCallback) {
		Debug.Log("[" + RemoteStorageSetUserPublishedFileActionResult_t.k_iCallback + " - RemoteStorageSetUserPublishedFileActionResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eAction);
	}

	void OnRemoteStorageEnumeratePublishedFilesByUserActionResult(ulong handle, RemoteStorageEnumeratePublishedFilesByUserActionResult_t pCallback) {
		Debug.Log("[" + RemoteStorageEnumeratePublishedFilesByUserActionResult_t.k_iCallback + " - RemoteStorageEnumeratePublishedFilesByUserActionResult] - " + pCallback.m_eResult + " -- " + pCallback.m_eAction + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgRTimeUpdated);
	}

	void OnRemoteStoragePublishFileProgress(RemoteStoragePublishFileProgress_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishFileProgress_t.k_iCallback + " - RemoteStoragePublishFileProgress] - " + pCallback.m_dPercentFile + " -- " + pCallback.m_bPreview);
	}

	void OnRemoteStoragePublishedFileUpdated(RemoteStoragePublishedFileUpdated_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishedFileUpdated_t.k_iCallback + " - RemoteStoragePublishedFileUpdated] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_hFile);
	}
}