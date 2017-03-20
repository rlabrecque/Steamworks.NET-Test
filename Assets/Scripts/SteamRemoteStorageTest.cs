using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamRemoteStorageTest : MonoBehaviour {
	const string MESSAGE_FILE_NAME = "message.dat";

	private string m_Message;
	private int m_FileCount;
	private int m_FileSize;
	private ulong m_TotalBytes;
	private int m_FileSizeInBytes;
	private bool m_CloudEnabled;
	private UGCFileWriteStreamHandle_t m_FileStream;
	private UGCHandle_t m_UGCHandle;
	private PublishedFileId_t m_PublishedFileId;
	private PublishedFileUpdateHandle_t m_PublishedFileUpdateHandle;
	private SteamAPICall_t m_FileReadAsyncHandle;

	protected Callback<RemoteStorageAppSyncedClient_t> m_RemoteStorageAppSyncedClient;
	protected Callback<RemoteStorageAppSyncedServer_t> m_RemoteStorageAppSyncedServer;
	protected Callback<RemoteStorageAppSyncProgress_t> m_RemoteStorageAppSyncProgress;
	protected Callback<RemoteStorageAppSyncStatusCheck_t> m_RemoteStorageAppSyncStatusCheck;
	protected Callback<RemoteStoragePublishedFileSubscribed_t> m_RemoteStoragePublishedFileSubscribed;
	protected Callback<RemoteStoragePublishedFileUnsubscribed_t> m_RemoteStoragePublishedFileUnsubscribed;
	protected Callback<RemoteStoragePublishedFileDeleted_t> m_RemoteStoragePublishedFileDeleted;
	protected Callback<RemoteStoragePublishedFileUpdated_t> m_RemoteStoragePublishedFileUpdated;

	private CallResult<RemoteStorageFileShareResult_t> OnRemoteStorageFileShareResultCallResult;
	private CallResult<RemoteStoragePublishFileResult_t> OnRemoteStoragePublishFileResultCallResult;
	private CallResult<RemoteStorageDeletePublishedFileResult_t> OnRemoteStorageDeletePublishedFileResultCallResult;
	private CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t> OnRemoteStorageEnumerateUserPublishedFilesResultCallResult;
	private CallResult<RemoteStorageSubscribePublishedFileResult_t> OnRemoteStorageSubscribePublishedFileResultCallResult;
	private CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t> OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult;
	private CallResult<RemoteStorageUnsubscribePublishedFileResult_t> OnRemoteStorageUnsubscribePublishedFileResultCallResult;
	private CallResult<RemoteStorageUpdatePublishedFileResult_t> OnRemoteStorageUpdatePublishedFileResultCallResult;
	private CallResult<RemoteStorageDownloadUGCResult_t> OnRemoteStorageDownloadUGCResultCallResult;
	private CallResult<RemoteStorageGetPublishedFileDetailsResult_t> OnRemoteStorageGetPublishedFileDetailsResultCallResult;
	private CallResult<RemoteStorageEnumerateWorkshopFilesResult_t> OnRemoteStorageEnumerateWorkshopFilesResultCallResult;
	private CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t> OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult;
	private CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t> OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult;
	private CallResult<RemoteStorageUserVoteDetails_t> OnRemoteStorageUserVoteDetailsCallResult;
	private CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t> OnRemoteStorageEnumerateUserSharedWorkshopFilesResultCallResult;
	private CallResult<RemoteStorageSetUserPublishedFileActionResult_t> OnRemoteStorageSetUserPublishedFileActionResultCallResult;
	private CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t> OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult;
	private CallResult<RemoteStoragePublishFileProgress_t> OnRemoteStoragePublishFileProgressCallResult;
	private CallResult<RemoteStorageFileWriteAsyncComplete_t> OnRemoteStorageFileWriteAsyncCompleteCallResult;
	private CallResult<RemoteStorageFileReadAsyncComplete_t> OnRemoteStorageFileReadAsyncCompleteCallResult;

	public void OnEnable() {
		m_Message = "";

		m_RemoteStorageAppSyncedClient = Callback<RemoteStorageAppSyncedClient_t>.Create(OnRemoteStorageAppSyncedClient);
		m_RemoteStorageAppSyncedServer = Callback<RemoteStorageAppSyncedServer_t>.Create(OnRemoteStorageAppSyncedServer);
		m_RemoteStorageAppSyncProgress = Callback<RemoteStorageAppSyncProgress_t>.Create(OnRemoteStorageAppSyncProgress);
		m_RemoteStorageAppSyncStatusCheck = Callback<RemoteStorageAppSyncStatusCheck_t>.Create(OnRemoteStorageAppSyncStatusCheck);
		m_RemoteStoragePublishedFileSubscribed = Callback<RemoteStoragePublishedFileSubscribed_t>.Create(OnRemoteStoragePublishedFileSubscribed);
		m_RemoteStoragePublishedFileUnsubscribed = Callback<RemoteStoragePublishedFileUnsubscribed_t>.Create(OnRemoteStoragePublishedFileUnsubscribed);
		m_RemoteStoragePublishedFileDeleted = Callback<RemoteStoragePublishedFileDeleted_t>.Create(OnRemoteStoragePublishedFileDeleted);
		m_RemoteStoragePublishedFileUpdated = Callback<RemoteStoragePublishedFileUpdated_t>.Create(OnRemoteStoragePublishedFileUpdated);

		OnRemoteStorageFileShareResultCallResult = CallResult<RemoteStorageFileShareResult_t>.Create(OnRemoteStorageFileShareResult);
		OnRemoteStoragePublishFileResultCallResult = CallResult<RemoteStoragePublishFileResult_t>.Create(OnRemoteStoragePublishFileResult);
		OnRemoteStorageDeletePublishedFileResultCallResult = CallResult<RemoteStorageDeletePublishedFileResult_t>.Create(OnRemoteStorageDeletePublishedFileResult);
		OnRemoteStorageEnumerateUserPublishedFilesResultCallResult = CallResult<RemoteStorageEnumerateUserPublishedFilesResult_t>.Create(OnRemoteStorageEnumerateUserPublishedFilesResult);
		OnRemoteStorageSubscribePublishedFileResultCallResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(OnRemoteStorageSubscribePublishedFileResult);
		OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult = CallResult<RemoteStorageEnumerateUserSubscribedFilesResult_t>.Create(OnRemoteStorageEnumerateUserSubscribedFilesResult);
		OnRemoteStorageUnsubscribePublishedFileResultCallResult = CallResult<RemoteStorageUnsubscribePublishedFileResult_t>.Create(OnRemoteStorageUnsubscribePublishedFileResult);
		OnRemoteStorageUpdatePublishedFileResultCallResult = CallResult<RemoteStorageUpdatePublishedFileResult_t>.Create(OnRemoteStorageUpdatePublishedFileResult);
		OnRemoteStorageDownloadUGCResultCallResult = CallResult<RemoteStorageDownloadUGCResult_t>.Create(OnRemoteStorageDownloadUGCResult);
		OnRemoteStorageGetPublishedFileDetailsResultCallResult = CallResult<RemoteStorageGetPublishedFileDetailsResult_t>.Create(OnRemoteStorageGetPublishedFileDetailsResult);
		OnRemoteStorageEnumerateWorkshopFilesResultCallResult = CallResult<RemoteStorageEnumerateWorkshopFilesResult_t>.Create(OnRemoteStorageEnumerateWorkshopFilesResult);
		OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult = CallResult<RemoteStorageGetPublishedItemVoteDetailsResult_t>.Create(OnRemoteStorageGetPublishedItemVoteDetailsResult);
		OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult = CallResult<RemoteStorageUpdateUserPublishedItemVoteResult_t>.Create(OnRemoteStorageUpdateUserPublishedItemVoteResult);
		OnRemoteStorageUserVoteDetailsCallResult = CallResult<RemoteStorageUserVoteDetails_t>.Create(OnRemoteStorageUserVoteDetails);
		OnRemoteStorageEnumerateUserSharedWorkshopFilesResultCallResult = CallResult<RemoteStorageEnumerateUserSharedWorkshopFilesResult_t>.Create(OnRemoteStorageEnumerateUserSharedWorkshopFilesResult);
		OnRemoteStorageSetUserPublishedFileActionResultCallResult = CallResult<RemoteStorageSetUserPublishedFileActionResult_t>.Create(OnRemoteStorageSetUserPublishedFileActionResult);
		OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult = CallResult<RemoteStorageEnumeratePublishedFilesByUserActionResult_t>.Create(OnRemoteStorageEnumeratePublishedFilesByUserActionResult);
		OnRemoteStoragePublishFileProgressCallResult = CallResult<RemoteStoragePublishFileProgress_t>.Create(OnRemoteStoragePublishFileProgress);
		OnRemoteStorageFileWriteAsyncCompleteCallResult = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(OnRemoteStorageFileWriteAsyncComplete);
		OnRemoteStorageFileReadAsyncCompleteCallResult = CallResult<RemoteStorageFileReadAsyncComplete_t>.Create(OnRemoteStorageFileReadAsyncComplete);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 120, 0, 120, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Message:");
		m_Message = GUILayout.TextField(m_Message, 40);
		GUILayout.Label("m_FileCount: " + m_FileCount);
		GUILayout.Label("m_FileSize: " + m_FileSize);
		GUILayout.Label("m_TotalBytes: " + m_TotalBytes);
		GUILayout.Label("m_FileSizeInBytes: " + m_FileSizeInBytes);
		GUILayout.Label("m_CloudEnabled: " + m_CloudEnabled);
		GUILayout.Label("m_FileStream: " + m_FileStream);
		GUILayout.Label("m_UGCHandle: " + m_UGCHandle);
		GUILayout.Label("m_PublishedFileId: " + m_PublishedFileId);
		GUILayout.Label("m_PublishedFileUpdateHandle: " + m_PublishedFileUpdateHandle);
		GUILayout.Label("m_FileReadAsyncHandle: " + m_FileReadAsyncHandle);
		GUILayout.EndArea();

		if (GUILayout.Button("FileWrite(MESSAGE_FILE_NAME, Data, Data.Length)")) {
			if ((ulong)System.Text.Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes) {
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

		if (GUILayout.Button("FileWriteAsync(MESSAGE_FILE_NAME, Data, (uint)Data.Length)")) {
			byte[] Data = new byte[System.Text.Encoding.UTF8.GetByteCount(m_Message)];
			System.Text.Encoding.UTF8.GetBytes(m_Message, 0, m_Message.Length, Data, 0);
			SteamAPICall_t handle = SteamRemoteStorage.FileWriteAsync(MESSAGE_FILE_NAME, Data, (uint)Data.Length);
			OnRemoteStorageFileWriteAsyncCompleteCallResult.Set(handle);
			print("SteamRemoteStorage.FileWriteAsync(" + MESSAGE_FILE_NAME + ", " + Data + ", " + (uint)Data.Length + ") : " + handle);
		}

		if (GUILayout.Button("FileReadAsync(MESSAGE_FILE_NAME, Data, (uint)Data.Length)")) {
			if (m_FileSize > 40) {
				Debug.Log("RemoteStorage: File was larger than expected. . .");
			}
			else {
				m_FileReadAsyncHandle = SteamRemoteStorage.FileReadAsync(MESSAGE_FILE_NAME, 0, (uint)m_FileSize);
				OnRemoteStorageFileReadAsyncCompleteCallResult.Set(m_FileReadAsyncHandle);
				print("FileReadAsync(" + MESSAGE_FILE_NAME + ", 0, " + (uint)m_FileSize + ") - " + m_FileReadAsyncHandle);
			}
		}

		//SteamRemoteStorage.FileReadAsyncComplete() // Must be called from the RemoteStorageFileReadAsyncComplete_t CallResult.

		if (GUILayout.Button("FileForget(MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.FileForget(MESSAGE_FILE_NAME);
			print("SteamRemoteStorage.FileForget(" + MESSAGE_FILE_NAME + ") : " + ret);
		}

		if (GUILayout.Button("FileDelete(MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.FileDelete(MESSAGE_FILE_NAME);
			print("SteamRemoteStorage.FileDelete(" + MESSAGE_FILE_NAME + ") : " + ret);
		}

		if (GUILayout.Button("FileShare(MESSAGE_FILE_NAME)")) {
			SteamAPICall_t handle = SteamRemoteStorage.FileShare(MESSAGE_FILE_NAME);
			OnRemoteStorageFileShareResultCallResult.Set(handle);
			print("SteamRemoteStorage.FileShare(" + MESSAGE_FILE_NAME + ") : " + handle);
		}

		if (GUILayout.Button("SetSyncPlatforms(MESSAGE_FILE_NAME, ERemoteStoragePlatform.k_ERemoteStoragePlatformAll)")) {
			bool ret = SteamRemoteStorage.SetSyncPlatforms(MESSAGE_FILE_NAME, ERemoteStoragePlatform.k_ERemoteStoragePlatformAll);
			print("SteamRemoteStorage.SetSyncPlatforms(" + MESSAGE_FILE_NAME + ", " + ERemoteStoragePlatform.k_ERemoteStoragePlatformAll + ") : " + ret);
		}

		if (GUILayout.Button("FileWriteStreamOpen(MESSAGE_FILE_NAME)")) {
			m_FileStream = SteamRemoteStorage.FileWriteStreamOpen(MESSAGE_FILE_NAME);
			print("SteamRemoteStorage.FileWriteStreamOpen(" + MESSAGE_FILE_NAME + ") : " + m_FileStream);
		}

		if (GUILayout.Button("FileWriteStreamWriteChunk(m_FileStream, Data, Data.Length)")) {
			if ((ulong)System.Text.Encoding.UTF8.GetByteCount(m_Message) > m_TotalBytes) {
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
			print("SteamRemoteStorage.FileWriteStreamClose(" + m_FileStream + ") : " + ret);
		}

		if (GUILayout.Button("FileWriteStreamCancel(m_FileStream)")) {
			bool ret = SteamRemoteStorage.FileWriteStreamCancel(m_FileStream);
			print("SteamRemoteStorage.FileWriteStreamCancel(" + m_FileStream + ") : " + ret);
		}

		GUILayout.Label("FileExists(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FileExists(MESSAGE_FILE_NAME));

		GUILayout.Label("FilePersisted(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.FilePersisted(MESSAGE_FILE_NAME));

		GUILayout.Label("GetFileSize(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileSize(MESSAGE_FILE_NAME));

		GUILayout.Label("GetFileTimestamp(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetFileTimestamp(MESSAGE_FILE_NAME));

		GUILayout.Label("GetSyncPlatforms(MESSAGE_FILE_NAME) : " + SteamRemoteStorage.GetSyncPlatforms(MESSAGE_FILE_NAME));

		{
			m_FileCount = SteamRemoteStorage.GetFileCount();
			GUILayout.Label("GetFileCount() : " + m_FileCount);
		}

		for (int i = 0; i < m_FileCount; ++i) {
			int FileSize = 0;
			string FileName = SteamRemoteStorage.GetFileNameAndSize(i, out FileSize);
			GUILayout.Label("GetFileNameAndSize(i, out FileSize) : " + FileName + " -- " + FileSize);

			if(FileName == MESSAGE_FILE_NAME) {
				m_FileSize = FileSize;
			}
		}

		{
			ulong AvailableBytes;
			bool ret = SteamRemoteStorage.GetQuota(out m_TotalBytes, out AvailableBytes);
			GUILayout.Label("GetQuota(out m_TotalBytes, out AvailableBytes) : " + ret + " -- " + m_TotalBytes + " -- " + AvailableBytes);
		}

		GUILayout.Label("IsCloudEnabledForAccount() : " + SteamRemoteStorage.IsCloudEnabledForAccount());

		{
			m_CloudEnabled = SteamRemoteStorage.IsCloudEnabledForApp();
			GUILayout.Label("IsCloudEnabledForApp() : " + m_CloudEnabled);
		}

		if (GUILayout.Button("SetCloudEnabledForApp(!m_CloudEnabled)")) {
			SteamRemoteStorage.SetCloudEnabledForApp(!m_CloudEnabled);
			print("SteamRemoteStorage.SetCloudEnabledForApp(" + !m_CloudEnabled + ")");
		}

		if (GUILayout.Button("UGCDownload(m_UGCHandle, 0)")) {
			SteamAPICall_t handle = SteamRemoteStorage.UGCDownload(m_UGCHandle, 0);
			OnRemoteStorageDownloadUGCResultCallResult.Set(handle);
			print("SteamRemoteStorage.UGCDownload(" + m_UGCHandle + ", " + 0 + ") : " + handle);
		}

		{
			int BytesDownloaded;
			int BytesExpected;
			bool ret = SteamRemoteStorage.GetUGCDownloadProgress(m_UGCHandle, out BytesDownloaded, out BytesExpected);
			GUILayout.Label("GetUGCDownloadProgress(m_UGCHandle, out BytesDownloaded, out BytesExpected) : " + ret + " -- " + BytesDownloaded + " -- " + BytesExpected);
		}

		// Spams warnings if called with an empty handle
		if (m_UGCHandle != (UGCHandle_t)0) {
			AppId_t AppID;
			string Name;
			CSteamID SteamIDOwner;
			bool ret = SteamRemoteStorage.GetUGCDetails(m_UGCHandle, out AppID, out Name, out m_FileSizeInBytes, out SteamIDOwner);
			GUILayout.Label("GetUGCDetails(m_UGCHandle, out AppID, Name, out FileSizeInBytes, out SteamIDOwner) : " + ret + " -- " + AppID + " -- " + Name + " -- " + m_FileSizeInBytes + " -- " + SteamIDOwner);
		}
		else {
			GUILayout.Label("GetUGCDetails(m_UGCHandle, out AppID, Name, out FileSizeInBytes, out SteamIDOwner) : ");
		}

		if (GUILayout.Button("UGCRead(m_UGCHandle, Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close)")) {
			byte[] Data = new byte[m_FileSizeInBytes];
			int ret = SteamRemoteStorage.UGCRead(m_UGCHandle, Data, m_FileSizeInBytes, 0, EUGCReadAction.k_EUGCRead_Close);
			print("SteamRemoteStorage.UGCRead(" + m_UGCHandle + ", " + Data + ", " + m_FileSizeInBytes + ", " + 0 + ", " + EUGCReadAction.k_EUGCRead_Close + ") : " + ret);
		}

		GUILayout.Label("GetCachedUGCCount() : " + SteamRemoteStorage.GetCachedUGCCount());

		GUILayout.Label("GetCachedUGCHandle(0) : " + SteamRemoteStorage.GetCachedUGCHandle(0));

		//SteamRemoteStorage.GetFileListFromServer() // PS3 Only.

		//SteamRemoteStorage.FileFetch() // PS3 Only.

		//SteamRemoteStorage.FilePersist() // PS3 Only.

		//SteamRemoteStorage.SynchronizeToClient() // PS3 Only.

		//SteamRemoteStorage.SynchronizeToServer() // PS3 Only.

		//SteamRemoteStorage.ResetFileRequestState() // PS3 Only.

		if (GUILayout.Button("PublishWorkshopFile(MESSAGE_FILE_NAME, null, SteamUtils.GetAppID(), \"Title!\", \"Description!\", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, Tags, EWorkshopFileType.k_EWorkshopFileTypeCommunity)")) {
			string[] Tags = { "Test1", "Test2", "Test3" };
			SteamAPICall_t handle = SteamRemoteStorage.PublishWorkshopFile(MESSAGE_FILE_NAME, null, SteamUtils.GetAppID(), "Title!", "Description!", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, Tags, EWorkshopFileType.k_EWorkshopFileTypeCommunity);
			OnRemoteStoragePublishFileProgressCallResult.Set(handle);
			print("SteamRemoteStorage.PublishWorkshopFile(" + MESSAGE_FILE_NAME + ", " + null + ", " + SteamUtils.GetAppID() + ", " + "\"Title!\"" + ", " + "\"Description!\"" + ", " + ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic + ", " + Tags + ", " + EWorkshopFileType.k_EWorkshopFileTypeCommunity + ") : " + handle);
		}

		if (GUILayout.Button("CreatePublishedFileUpdateRequest(m_PublishedFileId)")) {
			m_PublishedFileUpdateHandle = SteamRemoteStorage.CreatePublishedFileUpdateRequest(m_PublishedFileId);
			print("SteamRemoteStorage.CreatePublishedFileUpdateRequest(" + m_PublishedFileId + ") : " + m_PublishedFileUpdateHandle);
		}

		if (GUILayout.Button("UpdatePublishedFileFile(m_PublishedFileUpdateHandle, MESSAGE_FILE_NAME)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileFile(m_PublishedFileUpdateHandle, MESSAGE_FILE_NAME);
			print("SteamRemoteStorage.UpdatePublishedFileFile(" + m_PublishedFileUpdateHandle + ", " + MESSAGE_FILE_NAME + ") : " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFilePreviewFile(m_PublishedFileUpdateHandle, null);
			print("SteamRemoteStorage.UpdatePublishedFilePreviewFile(" + m_PublishedFileUpdateHandle + ", " + null + ") : " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, \"New Title\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileTitle(m_PublishedFileUpdateHandle, "New Title");
			print("SteamRemoteStorage.UpdatePublishedFileTitle(" + m_PublishedFileUpdateHandle + ", " + "\"New Title\"" + ") : " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, \"New Description\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileDescription(m_PublishedFileUpdateHandle, "New Description");
			print("SteamRemoteStorage.UpdatePublishedFileDescription(" + m_PublishedFileUpdateHandle + ", " + "\"New Description\"" + ") : " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic)")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileVisibility(m_PublishedFileUpdateHandle, ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic);
			print("SteamRemoteStorage.UpdatePublishedFileVisibility(" + m_PublishedFileUpdateHandle + ", " + ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic + ") : " + ret);
		}

		if (GUILayout.Button("UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[] {\"First\", \"Second\", \"Third\"})")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileTags(m_PublishedFileUpdateHandle, new string[] {"First", "Second", "Third"});
			print("SteamRemoteStorage.UpdatePublishedFileTags(" + m_PublishedFileUpdateHandle + ", " + new string[] {"First", "Second", "Third"} + ") : " + ret);
		}

		if (GUILayout.Button("CommitPublishedFileUpdate(m_PublishedFileUpdateHandle)")) {
			SteamAPICall_t handle = SteamRemoteStorage.CommitPublishedFileUpdate(m_PublishedFileUpdateHandle);
			OnRemoteStorageUpdatePublishedFileResultCallResult.Set(handle);
			print("SteamRemoteStorage.CommitPublishedFileUpdate(" + m_PublishedFileUpdateHandle + ") : " + handle);
		}

		if (GUILayout.Button("GetPublishedFileDetails(m_PublishedFileId, 0)")) {
			SteamAPICall_t handle = SteamRemoteStorage.GetPublishedFileDetails(m_PublishedFileId, 0);
			OnRemoteStorageGetPublishedFileDetailsResultCallResult.Set(handle);
			print("SteamRemoteStorage.GetPublishedFileDetails(" + m_PublishedFileId + ", " + 0 + ") : " + handle);
		}

		if (GUILayout.Button("DeletePublishedFile(m_PublishedFileId)")) {
			SteamAPICall_t handle = SteamRemoteStorage.DeletePublishedFile(m_PublishedFileId);
			OnRemoteStorageDeletePublishedFileResultCallResult.Set(handle);
			print("SteamRemoteStorage.DeletePublishedFile(" + m_PublishedFileId + ") : " + handle);
		}

		if (GUILayout.Button("EnumerateUserPublishedFiles(0)")) {
			SteamAPICall_t handle = SteamRemoteStorage.EnumerateUserPublishedFiles(0);
			OnRemoteStorageEnumerateUserPublishedFilesResultCallResult.Set(handle);
			print("SteamRemoteStorage.EnumerateUserPublishedFiles(" + 0 + ") : " + handle);
		}

		if (GUILayout.Button("SubscribePublishedFile(m_PublishedFileId)")) {
			SteamAPICall_t handle = SteamRemoteStorage.SubscribePublishedFile(m_PublishedFileId);
			OnRemoteStorageSubscribePublishedFileResultCallResult.Set(handle);
			print("SteamRemoteStorage.SubscribePublishedFile(" + m_PublishedFileId + ") : " + handle);
		}

		if (GUILayout.Button("EnumerateUserSubscribedFiles(0)")) {
			SteamAPICall_t handle = SteamRemoteStorage.EnumerateUserSubscribedFiles(0);
			OnRemoteStorageEnumerateUserSubscribedFilesResultCallResult.Set(handle);
			print("SteamRemoteStorage.EnumerateUserSubscribedFiles(" + 0 + ") : " + handle);
		}

		if (GUILayout.Button("UnsubscribePublishedFile(m_PublishedFileId)")) {
			SteamAPICall_t handle = SteamRemoteStorage.UnsubscribePublishedFile(m_PublishedFileId);
			OnRemoteStorageUnsubscribePublishedFileResultCallResult.Set(handle);
			print("SteamRemoteStorage.UnsubscribePublishedFile(" + m_PublishedFileId + ") : " + handle);
		}

		if (GUILayout.Button("UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, \"Changelog!\")")) {
			bool ret = SteamRemoteStorage.UpdatePublishedFileSetChangeDescription(m_PublishedFileUpdateHandle, "Changelog!");
			print("SteamRemoteStorage.UpdatePublishedFileSetChangeDescription(" + m_PublishedFileUpdateHandle + ", " + "\"Changelog!\"" + ") : " + ret);
		}

		if (GUILayout.Button("GetPublishedItemVoteDetails(m_PublishedFileId)")) {
			SteamAPICall_t handle = SteamRemoteStorage.GetPublishedItemVoteDetails(m_PublishedFileId);
			OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult.Set(handle);
			print("SteamRemoteStorage.GetPublishedItemVoteDetails(" + m_PublishedFileId + ") : " + handle);
		}

		if (GUILayout.Button("UpdateUserPublishedItemVote(m_PublishedFileId, true)")) {
			SteamAPICall_t handle = SteamRemoteStorage.UpdateUserPublishedItemVote(m_PublishedFileId, true);
			OnRemoteStorageUpdateUserPublishedItemVoteResultCallResult.Set(handle);
			print("SteamRemoteStorage.UpdateUserPublishedItemVote(" + m_PublishedFileId + ", " + true + ") : " + handle);
		}

		if (GUILayout.Button("GetUserPublishedItemVoteDetails(m_PublishedFileId)")) {
			SteamAPICall_t handle = SteamRemoteStorage.GetUserPublishedItemVoteDetails(m_PublishedFileId);
			OnRemoteStorageGetPublishedItemVoteDetailsResultCallResult.Set(handle);
			print("SteamRemoteStorage.GetUserPublishedItemVoteDetails(" + m_PublishedFileId + ") : " + handle);
		}

		if (GUILayout.Button("EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0, null, null)")) {
			SteamAPICall_t handle = SteamRemoteStorage.EnumerateUserSharedWorkshopFiles(SteamUser.GetSteamID(), 0, null, null);
			OnRemoteStorageEnumerateUserPublishedFilesResultCallResult.Set(handle);
			print("SteamRemoteStorage.EnumerateUserSharedWorkshopFiles(" + SteamUser.GetSteamID() + ", " + 0 + ", " + null + ", " + null + ") : " + handle);
		}

		if (GUILayout.Button("PublishVideo(EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube, \"William Hunter\", \"Rmvb4Hktv7U\", null, SteamUtils.GetAppID(), \"Test Video\", \"Desc\", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, null)")) {
			SteamAPICall_t handle = SteamRemoteStorage.PublishVideo(EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube, "William Hunter", "Rmvb4Hktv7U", null, SteamUtils.GetAppID(), "Test Video", "Desc", ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic, null);
			OnRemoteStoragePublishFileProgressCallResult.Set(handle);
			print("SteamRemoteStorage.PublishVideo(" + EWorkshopVideoProvider.k_EWorkshopVideoProviderYoutube + ", " + "\"William Hunter\"" + ", " + "\"Rmvb4Hktv7U\"" + ", " + null + ", " + SteamUtils.GetAppID() + ", " + "\"Test Video\"" + ", " + "\"Desc\"" + ", " + ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic + ", " + null + ") : " + handle);
		}

		if (GUILayout.Button("SetUserPublishedFileAction(m_PublishedFileId, EWorkshopFileAction.k_EWorkshopFileActionPlayed)")) {
			SteamAPICall_t handle = SteamRemoteStorage.SetUserPublishedFileAction(m_PublishedFileId, EWorkshopFileAction.k_EWorkshopFileActionPlayed);
			OnRemoteStorageSetUserPublishedFileActionResultCallResult.Set(handle);
			print("SteamRemoteStorage.SetUserPublishedFileAction(" + m_PublishedFileId + ", " + EWorkshopFileAction.k_EWorkshopFileActionPlayed + ") : " + handle);
		}

		if (GUILayout.Button("EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0)")) {
			SteamAPICall_t handle = SteamRemoteStorage.EnumeratePublishedFilesByUserAction(EWorkshopFileAction.k_EWorkshopFileActionPlayed, 0);
			OnRemoteStorageEnumeratePublishedFilesByUserActionResultCallResult.Set(handle);
			print("SteamRemoteStorage.EnumeratePublishedFilesByUserAction(" + EWorkshopFileAction.k_EWorkshopFileActionPlayed + ", " + 0 + ") : " + handle);
		}

		if (GUILayout.Button("EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, null, null)")) {
			SteamAPICall_t handle = SteamRemoteStorage.EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote, 0, 3, 0, null, null);
			OnRemoteStorageEnumerateWorkshopFilesResultCallResult.Set(handle);
			print("SteamRemoteStorage.EnumeratePublishedWorkshopFiles(" + EWorkshopEnumerationType.k_EWorkshopEnumerationTypeRankedByVote + ", " + 0 + ", " + 3 + ", " + 0 + ", " + null + ", " + null + ") : " + handle);
		}

		//SteamRemoteStorage.UGCDownloadToLocation() // There is absolutely no documentation on how to use this function
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

	void OnRemoteStorageFileShareResult(RemoteStorageFileShareResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageFileShareResult_t.k_iCallback + " - RemoteStorageFileShareResult] - " + pCallback.m_eResult + " -- " + pCallback.m_hFile + " -- " + pCallback.m_rgchFilename);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	void OnRemoteStoragePublishFileResult(RemoteStoragePublishFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStoragePublishFileResult_t.k_iCallback + " - RemoteStoragePublishFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_PublishedFileId = pCallback.m_nPublishedFileId;
		}
	}

	void OnRemoteStorageDeletePublishedFileResult(RemoteStorageDeletePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageDeletePublishedFileResult_t.k_iCallback + " - RemoteStorageDeletePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageEnumerateUserPublishedFilesResult(RemoteStorageEnumerateUserPublishedFilesResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageEnumerateUserPublishedFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserPublishedFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
	}

	void OnRemoteStorageSubscribePublishedFileResult(RemoteStorageSubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageSubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageSubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageEnumerateUserSubscribedFilesResult(RemoteStorageEnumerateUserSubscribedFilesResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageEnumerateUserSubscribedFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserSubscribedFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgRTimeSubscribed);
	}

	void OnRemoteStorageUnsubscribePublishedFileResult(RemoteStorageUnsubscribePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUnsubscribePublishedFileResult_t.k_iCallback + " - RemoteStorageUnsubscribePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageUpdatePublishedFileResult(RemoteStorageUpdatePublishedFileResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUpdatePublishedFileResult_t.k_iCallback + " - RemoteStorageUpdatePublishedFileResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
	}

	void OnRemoteStorageDownloadUGCResult(RemoteStorageDownloadUGCResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageDownloadUGCResult_t.k_iCallback + " - RemoteStorageDownloadUGCResult] - " + pCallback.m_eResult + " -- " + pCallback.m_hFile + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_nSizeInBytes + " -- " + pCallback.m_pchFileName + " -- " + pCallback.m_ulSteamIDOwner);
	}

	void OnRemoteStorageGetPublishedFileDetailsResult(RemoteStorageGetPublishedFileDetailsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageGetPublishedFileDetailsResult_t.k_iCallback + " - RemoteStorageGetPublishedFileDetailsResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nCreatorAppID + " -- " + pCallback.m_nConsumerAppID + " -- " + pCallback.m_rgchTitle + " -- " + pCallback.m_rgchDescription + " -- " + pCallback.m_hFile + " -- " + pCallback.m_hPreviewFile + " -- " + pCallback.m_ulSteamIDOwner + " -- " + pCallback.m_rtimeCreated + " -- " + pCallback.m_rtimeUpdated + " -- " + pCallback.m_eVisibility + " -- " + pCallback.m_bBanned + " -- " + pCallback.m_rgchTags + " -- " + pCallback.m_bTagsTruncated + " -- " + pCallback.m_pchFileName + " -- " + pCallback.m_nFileSize + " -- " + pCallback.m_nPreviewFileSize + " -- " + pCallback.m_rgchURL + " -- " + pCallback.m_eFileType + " -- " + pCallback.m_bAcceptedForUse);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			m_UGCHandle = pCallback.m_hFile;
		}
	}

	void OnRemoteStorageEnumerateWorkshopFilesResult(RemoteStorageEnumerateWorkshopFilesResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageEnumerateWorkshopFilesResult_t.k_iCallback + " - RemoteStorageEnumerateWorkshopFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgScore + " -- " + pCallback.m_nAppId + " -- " + pCallback.m_unStartIndex);

		for (int i = 0; i < pCallback.m_nResultsReturned; ++i) {
			print(i + ": " + pCallback.m_rgPublishedFileId[i]);
		}

		if(pCallback.m_nResultsReturned >= 1) {
			m_PublishedFileId = pCallback.m_rgPublishedFileId[0];
		}
	}

	void OnRemoteStorageGetPublishedItemVoteDetailsResult(RemoteStorageGetPublishedItemVoteDetailsResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageGetPublishedItemVoteDetailsResult_t.k_iCallback + " - RemoteStorageGetPublishedItemVoteDetailsResult] - " + pCallback.m_eResult + " -- " + pCallback.m_unPublishedFileId + " -- " + pCallback.m_nVotesFor + " -- " + pCallback.m_nVotesAgainst + " -- " + pCallback.m_nReports + " -- " + pCallback.m_fScore);
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

	void OnRemoteStorageUpdateUserPublishedItemVoteResult(RemoteStorageUpdateUserPublishedItemVoteResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUpdateUserPublishedItemVoteResult_t.k_iCallback + " - RemoteStorageUpdateUserPublishedItemVoteResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId);
	}

	void OnRemoteStorageUserVoteDetails(RemoteStorageUserVoteDetails_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageUserVoteDetails_t.k_iCallback + " - RemoteStorageUserVoteDetails] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eVote);
	}

	void OnRemoteStorageEnumerateUserSharedWorkshopFilesResult(RemoteStorageEnumerateUserSharedWorkshopFilesResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageEnumerateUserSharedWorkshopFilesResult_t.k_iCallback + " - RemoteStorageEnumerateUserSharedWorkshopFilesResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId);
	}

	void OnRemoteStorageSetUserPublishedFileActionResult(RemoteStorageSetUserPublishedFileActionResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageSetUserPublishedFileActionResult_t.k_iCallback + " - RemoteStorageSetUserPublishedFileActionResult] - " + pCallback.m_eResult + " -- " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_eAction);
	}

	void OnRemoteStorageEnumeratePublishedFilesByUserActionResult(RemoteStorageEnumeratePublishedFilesByUserActionResult_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageEnumeratePublishedFilesByUserActionResult_t.k_iCallback + " - RemoteStorageEnumeratePublishedFilesByUserActionResult] - " + pCallback.m_eResult + " -- " + pCallback.m_eAction + " -- " + pCallback.m_nResultsReturned + " -- " + pCallback.m_nTotalResultCount + " -- " + pCallback.m_rgPublishedFileId + " -- " + pCallback.m_rgRTimeUpdated);
	}

	void OnRemoteStoragePublishFileProgress(RemoteStoragePublishFileProgress_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStoragePublishFileProgress_t.k_iCallback + " - RemoteStoragePublishFileProgress] - " + pCallback.m_dPercentFile + " -- " + pCallback.m_bPreview);
	}

	void OnRemoteStoragePublishedFileUpdated(RemoteStoragePublishedFileUpdated_t pCallback) {
		Debug.Log("[" + RemoteStoragePublishedFileUpdated_t.k_iCallback + " - RemoteStoragePublishedFileUpdated] - " + pCallback.m_nPublishedFileId + " -- " + pCallback.m_nAppID + " -- " + pCallback.m_ulUnused);
	}

	void OnRemoteStorageFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageFileWriteAsyncComplete_t.k_iCallback + " - RemoteStorageFileWriteAsyncComplete] - " + pCallback.m_eResult);
	}

	void OnRemoteStorageFileReadAsyncComplete(RemoteStorageFileReadAsyncComplete_t pCallback, bool bIOFailure) {
		Debug.Log("[" + RemoteStorageFileReadAsyncComplete_t.k_iCallback + " - RemoteStorageFileReadAsyncComplete] - " + pCallback.m_hFileReadAsync + " -- " + pCallback.m_eResult + " -- " + pCallback.m_nOffset + " -- " + pCallback.m_cubRead);

		if (pCallback.m_eResult == EResult.k_EResultOK) {
			byte[] Data = new byte[40];
			bool ret = SteamRemoteStorage.FileReadAsyncComplete(pCallback.m_hFileReadAsync, Data, pCallback.m_cubRead);
			print("FileReadAsyncComplete(m_FileReadAsyncHandle, Data, pCallback.m_cubRead) : " + ret);
			if (ret) {
				m_Message = System.Text.Encoding.UTF8.GetString(Data, (int)pCallback.m_nOffset, (int)pCallback.m_cubRead);
			}
		}
	}
}