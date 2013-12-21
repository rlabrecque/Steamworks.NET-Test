using UnityEngine;
using System.Collections;
using Steamworks;


class SteamUserTest : MonoBehaviour {
	byte[] m_Ticket;
	uint m_pcbTicket;
	uint m_HAuthTicket;
	GameObject m_VoiceLoopback;

	CallResult<EncryptedAppTicketResponse_t> OnEncryptedAppTicketResponseCallResult;

	public void Awake() {
		new Callback<SteamServersConnected_t>(OnSteamServersConnected);
		new Callback<SteamServerConnectFailure_t>(OnSteamServerConnectFailure);
		new Callback<SteamServersDisconnected_t>(OnSteamServersDisconnected);
		new Callback<ClientGameServerDeny_t>(OnClientGameServerDeny);
		new Callback<IPCFailure_t>(OnIPCFailure);
		new Callback<ValidateAuthTicketResponse_t>(OnValidateAuthTicketResponse);
		new Callback<MicroTxnAuthorizationResponse_t>(OnMicroTxnAuthorizationResponse);
		OnEncryptedAppTicketResponseCallResult = new CallResult<EncryptedAppTicketResponse_t>(OnEncryptedAppTicketResponse);
		new Callback<GetAuthSessionTicketResponse_t>(OnGetAuthSessionTicketResponse);
		new Callback<GameWebCallback_t>(OnGameWebCallback);
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("m_HAuthTicket: " + m_HAuthTicket);
		GUILayout.Label("m_pcbTicket: " + m_pcbTicket);
		GUILayout.EndArea();

		GUILayout.Label("SteamUser.GetHSteamUser() : " + SteamUser.GetHSteamUser());
		GUILayout.Label("SteamUser.BLoggedOn() : " + SteamUser.BLoggedOn());
		GUILayout.Label("SteamUser.GetSteamID() : " + SteamUser.GetSteamID());

		//GUILayout.Label("SteamUser.InitiateGameConnection() : " + SteamUser.InitiateGameConnection()); // N/A - Too Hard to test like this.
		//GUILayout.Label("SteamUser.TerminateGameConnection() : " + SteamUser.TerminateGameConnection()); // N/A
		//GUILayout.Label("SteamUser.TrackAppUsageEvent() : " + SteamUser.TrackAppUsageEvent()); // TODO - Bugged codegen, missing the third arg

		{
			string Buffer;
			bool ret = SteamUser.GetUserDataFolder(out Buffer, 260);
			GUILayout.Label("SteamUser.GetUserDataFolder(out Buffer, 260) : " + ret + " -- " + Buffer);
		}

		if (GUILayout.Button("SteamUser.StartVoiceRecording()")) {
			SteamUser.StartVoiceRecording();
			print("SteamUser.StartVoiceRecording()");
		}

		if (GUILayout.Button("SteamUser.StopVoiceRecording()")) {
			SteamUser.StopVoiceRecording();
			print("SteamUser.StopVoiceRecording()");
		}

		{
			uint Compressed;
			uint Uncompressed;
			EVoiceResult ret = SteamUser.GetAvailableVoice(out Compressed, out Uncompressed, 11025);
			GUILayout.Label("SteamUser.GetAvailableVoice(out Compressed, out Uncompressed, 11025) : " + ret + " -- " + Compressed + " -- " + Uncompressed);

			if (ret == EVoiceResult.k_EVoiceResultOK && Compressed > 0) {
				byte[] DestBuffer = new byte[1024];
				byte[] UncompressedDestBuffer = new byte[1024];
				uint BytesWritten;
				uint UncompressedBytesWritten;
				ret = SteamUser.GetVoice(true, DestBuffer, 1024, out BytesWritten, true, UncompressedDestBuffer, 1024, out UncompressedBytesWritten, 11025);
				//print("SteamUser.GetVoice(true, DestBuffer, 1024, out BytesWritten, true, UncompressedDestBuffer, 1024, out UncompressedBytesWritten, 11025) : " + ret + " -- " + BytesWritten + " -- " + UncompressedBytesWritten);

				if (ret == EVoiceResult.k_EVoiceResultOK && BytesWritten > 0) {
					byte[] DestBuffer2 = new byte[11025 * 2];
					uint BytesWritten2;
					ret = SteamUser.DecompressVoice(DestBuffer, BytesWritten, DestBuffer2, 11025 * 2, out BytesWritten2, 11025);
					//print("SteamUser.DecompressVoice(DestBuffer, BytesWritten, DestBuffer2, 11025 * 2, out BytesWritten2, 11025) - " + ret + " -- " + BytesWritten2);

					if (ret == EVoiceResult.k_EVoiceResultOK && BytesWritten2 > 0) {
						AudioSource source;
						if (!m_VoiceLoopback) {
							m_VoiceLoopback = new GameObject("Voice Loopback");
							source = m_VoiceLoopback.AddComponent<AudioSource>();
							source.clip = AudioClip.Create("Testing!", 11025, 1, 11025, false, false);
						}
						else {
							source = m_VoiceLoopback.GetComponent<AudioSource>();
						}

						float[] test = new float[11025];
						for (int i = 0; i < test.Length; ++i) {
							test[i] = (short)(DestBuffer2[i * 2] | DestBuffer2[i * 2 + 1] << 8) / 32768.0f;
						}
						source.clip.SetData(test, 0);
						source.Play();
					}
				}
			}
		}

		GUILayout.Label("SteamUser.GetVoiceOptimalSampleRate() : " + SteamUser.GetVoiceOptimalSampleRate());

		{
			if (GUILayout.Button("SteamUser.GetAuthSessionTicket(Ticket, 1024, out pcbTicket)")) {
				m_Ticket = new byte[1024];
				m_HAuthTicket = SteamUser.GetAuthSessionTicket(m_Ticket, 1024, out m_pcbTicket);
				print("SteamUser.GetAuthSessionTicket(Ticket, 1024, out pcbTicket) - " + m_HAuthTicket + " -- " + m_pcbTicket);
			}

			if (GUILayout.Button("SteamUser.BeginAuthSession(m_Ticket, (int)m_pcbTicket, SteamUser.GetSteamID())")) {
				if (m_HAuthTicket != 0 && m_pcbTicket != 0) {
					EBeginAuthSessionResult ret = SteamUser.BeginAuthSession(m_Ticket, (int)m_pcbTicket, SteamUser.GetSteamID());
					print("SteamUser.BeginAuthSession(m_Ticket, " + (int)m_pcbTicket + ", " + SteamUser.GetSteamID() + ") - " + ret);
				}
				else {
					print("Call GetAuthSessionTicket first!");
				}
			}
		}

		if (GUILayout.Button("SteamUser.EndAuthSession(SteamUser.GetSteamID())")) {
			SteamUser.EndAuthSession(SteamUser.GetSteamID());
			print("SteamUser.EndAuthSession(" + SteamUser.GetSteamID() + ")");
		}

		if (GUILayout.Button("SteamUser.CancelAuthTicket(m_HAuthTicket)")) {
			SteamUser.CancelAuthTicket(m_HAuthTicket);
			print("SteamUser.CancelAuthTicket(" + m_HAuthTicket + ")");
		}

		GUILayout.Label("SteamUser.UserHasLicenseForApp(SteamUser.GetSteamID(), 480) : " + SteamUser.UserHasLicenseForApp(SteamUser.GetSteamID(), 480));
		GUILayout.Label("SteamUser.BIsBehindNAT() : " + SteamUser.BIsBehindNAT());

		if (GUILayout.Button("SteamUser.AdvertiseGame(2, 127.0.0.1, 27015)")) {
			SteamUser.AdvertiseGame(2, 2130706433, 27015);
			print("SteamUser.AdvertiseGame(2, 2130706433, 27015)");
		}

		if(GUILayout.Button("SteamUser.RequestEncryptedAppTicket()")) {
			byte[] k_unSecretData = System.BitConverter.GetBytes(0x5444);
			ulong handle = SteamUser.RequestEncryptedAppTicket(k_unSecretData, sizeof(uint));
			OnEncryptedAppTicketResponseCallResult.SetAPICallHandle(handle);
			print("SteamUser.RequestEncryptedAppTicket(ref k_unSecretData, " + sizeof(uint) + ") - " + handle + " -- " + k_unSecretData);
		}

		if (GUILayout.Button("SteamUser.GetEncryptedAppTicket()")) {
			byte[] rgubTicket = new byte[1024];
			uint cubTicket;
			bool ret = SteamUser.GetEncryptedAppTicket(rgubTicket, 1024, out cubTicket);
			print("SteamUser.GetEncryptedAppTicket() - " + ret + " -- " + cubTicket);
		}

		GUILayout.Label("SteamUser.GetGameBadgeLevel(1, false) : " + SteamUser.GetGameBadgeLevel(1, false));
		GUILayout.Label("SteamUser.GetPlayerSteamLevel() : " + SteamUser.GetPlayerSteamLevel());
#if _PS3
		//GUILayout.Label("SteamUser.LogOn() : " + SteamUser.LogOn());
		//GUILayout.Label("SteamUser.LogOnAndLinkSteamAccountToPSN : " + SteamUser.LogOnAndLinkSteamAccountToPSN());
		//GUILayout.Label("SteamUser.LogOnAndCreateNewSteamAccountIfNeeded : " + SteamUser.LogOnAndCreateNewSteamAccountIfNeeded());
		//GUILayout.Label("SteamUser.GetConsoleSteamID : " + SteamUser.GetConsoleSteamID());
#endif
	}
	
	void OnSteamServersConnected(SteamServersConnected_t pCallback) {
		Debug.Log("[" + SteamServersConnected_t.k_iCallback + " - SteamServersConnected]");
	}

	void OnSteamServerConnectFailure(SteamServerConnectFailure_t pCallback) {
		Debug.Log("[" + SteamServerConnectFailure_t.k_iCallback + " - SteamServerConnectFailure] - " + pCallback.m_eResult);
	}

	void OnSteamServersDisconnected(SteamServersDisconnected_t pCallback) {
		Debug.Log("[" + SteamServersDisconnected_t.k_iCallback + " - SteamServersDisconnected] - " + pCallback.m_eResult);
	}

	void OnClientGameServerDeny(ClientGameServerDeny_t pCallback) {
		Debug.Log("[" + ClientGameServerDeny_t.k_iCallback + " - ClientGameServerDeny] - " + pCallback.m_uAppID + " -- " + pCallback.m_unGameServerIP + " -- " + pCallback.m_usGameServerPort + " -- " + pCallback.m_bSecure + " -- " + pCallback.m_uReason);
	}

	void OnIPCFailure(IPCFailure_t pCallback) {
		Debug.Log("[" + IPCFailure_t.k_iCallback + " - IPCFailure] - " + pCallback.m_eFailureType);
	}

	void OnValidateAuthTicketResponse(ValidateAuthTicketResponse_t pCallback) {
		Debug.Log("[" + ValidateAuthTicketResponse_t.k_iCallback + " - ValidateAuthTicketResponse] - " + pCallback.m_SteamID + " -- " + pCallback.m_eAuthSessionResponse + " -- " + pCallback.m_OwnerSteamID);
	}

	void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback) {
		Debug.Log("[" + MicroTxnAuthorizationResponse_t.k_iCallback + " - MicroTxnAuthorizationResponse] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulOrderID + " -- " + pCallback.m_bAuthorized);
	}

	void OnEncryptedAppTicketResponse(ulong handle, EncryptedAppTicketResponse_t pCallback) {
		Debug.Log("[" + EncryptedAppTicketResponse_t.k_iCallback + " - EncryptedAppTicketResponse] - " + pCallback.m_eResult);

		// This code is taken directly from SteamworksExample/SpaceWar
		if (pCallback.m_eResult == EResult.k_EResultOK) {
			byte[] rgubTicket = new byte[1024];
			uint cubTicket;
			SteamUser.GetEncryptedAppTicket(rgubTicket, 1024, out cubTicket);

			// normally at this point you transmit the encrypted ticket to the service that knows the decryption key
			// this code is just to demonstrate the ticket cracking library

			// included is the "secret" key for spacewar. normally this is secret
			byte[] rgubKey = new byte[32] { 0xed, 0x93, 0x86, 0x07, 0x36, 0x47, 0xce, 0xa5, 0x8b, 0x77, 0x21, 0x49, 0x0d, 0x59, 0xed, 0x44, 0x57, 0x23, 0xf0, 0xf6, 0x6e, 0x74, 0x14, 0xe1, 0x53, 0x3b, 0xa3, 0x3c, 0xd8, 0x03, 0xbd, 0xbd };		

			byte[] rgubDecrypted = new byte[1024];
			uint cubDecrypted = 1024;
			if (!SteamEncryptedAppTicket.BDecryptTicket(rgubTicket, cubTicket, rgubDecrypted, out cubDecrypted, rgubKey, rgubKey.Length)) {
				Debug.Log("Ticket failed to decrypt");
				return;
			}

			if (!SteamEncryptedAppTicket.BIsTicketForApp(rgubDecrypted, cubDecrypted, SteamUtils.GetAppID())) {
				Debug.Log("Ticket for wrong app id");
			}

			ulong steamIDFromTicket;
			SteamEncryptedAppTicket.GetTicketSteamID(rgubDecrypted, cubDecrypted, out steamIDFromTicket);
			if (steamIDFromTicket != SteamUser.GetSteamID()) {
				Debug.Log("Ticket for wrong user");
			}

			uint cubData;
			byte[] punSecretData = SteamEncryptedAppTicket.GetUserVariableData(rgubDecrypted, cubDecrypted, out cubData);
			if(cubData != sizeof(uint)) {
				Debug.Log("Secret data size is wrong.");
			}
			Debug.Log(punSecretData.Length);
			Debug.Log(System.BitConverter.ToUInt32(punSecretData, 0));
			if (System.BitConverter.ToUInt32(punSecretData, 0) != 0x5444) {
				Debug.Log("Failed to retrieve secret data");
				return;
			}

			Debug.Log("Successfully retrieved Encrypted App Ticket");
		}
	}

	void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback) {
		Debug.Log("[" + GetAuthSessionTicketResponse_t.k_iCallback + " - GetAuthSessionTicketResponse] - " + pCallback.m_hAuthTicket + " -- " + pCallback.m_eResult);
	}

	void OnGameWebCallback(GameWebCallback_t pCallback) {
		Debug.Log("[" + GameWebCallback_t.k_iCallback + " - GameWebCallback] - " + pCallback.m_szURL);
	}

}