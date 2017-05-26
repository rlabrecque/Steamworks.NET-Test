using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamHTTPTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private HTTPRequestHandle m_RequestHandle;
	private ulong m_ContextValue;
	private uint m_Offset;
	private uint m_BufferSize;
	private HTTPCookieContainerHandle m_CookieContainer;

	protected Callback<HTTPRequestHeadersReceived_t> m_HTTPRequestHeadersReceived;
	protected Callback<HTTPRequestDataReceived_t> m_HTTPRequestDataReceived;

	private CallResult<HTTPRequestCompleted_t> OnHTTPRequestCompletedCallResult;

	public void OnEnable() {
		m_RequestHandle = HTTPRequestHandle.Invalid;
		m_CookieContainer = HTTPCookieContainerHandle.Invalid;

		m_HTTPRequestHeadersReceived = Callback<HTTPRequestHeadersReceived_t>.Create(OnHTTPRequestHeadersReceived);
		m_HTTPRequestDataReceived = Callback<HTTPRequestDataReceived_t>.Create(OnHTTPRequestDataReceived);

		OnHTTPRequestCompletedCallResult = CallResult<HTTPRequestCompleted_t>.Create(OnHTTPRequestCompleted);
	}

	public void OnDisable() {
		ReleaseCookieContainer();
	}

	void ReleaseCookieContainer() {
		if (m_CookieContainer != HTTPCookieContainerHandle.Invalid) {
			print("SteamHTTP.ReleaseCookieContainer(" + m_CookieContainer + ") - " + SteamHTTP.ReleaseCookieContainer(m_CookieContainer));
			m_CookieContainer = HTTPCookieContainerHandle.Invalid;
		}
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_RequestHandle: " + m_RequestHandle);
		GUILayout.Label("m_ContextValue: " + m_ContextValue);
		GUILayout.Label("m_Offset: " + m_Offset);
		GUILayout.Label("m_BufferSize: " + m_BufferSize);
		GUILayout.Label("m_CookieContainer: " + m_CookieContainer);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		if (GUILayout.Button("CreateHTTPRequest(EHTTPMethod.k_EHTTPMethodGET, \"http://httpbin.org/get\")")) {
			HTTPRequestHandle ret = SteamHTTP.CreateHTTPRequest(EHTTPMethod.k_EHTTPMethodGET, "http://httpbin.org/get");
			m_RequestHandle = ret;
			print("SteamHTTP.CreateHTTPRequest(" + EHTTPMethod.k_EHTTPMethodGET + ", " + "\"http://httpbin.org/get\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestContextValue(m_RequestHandle, 1)")) {
			bool ret = SteamHTTP.SetHTTPRequestContextValue(m_RequestHandle, 1);
			print("SteamHTTP.SetHTTPRequestContextValue(" + m_RequestHandle + ", " + 1 + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestNetworkActivityTimeout(m_RequestHandle, 30)")) {
			bool ret = SteamHTTP.SetHTTPRequestNetworkActivityTimeout(m_RequestHandle, 30);
			print("SteamHTTP.SetHTTPRequestNetworkActivityTimeout(" + m_RequestHandle + ", " + 30 + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestHeaderValue(m_RequestHandle, \"From\", \"support@rileylabrecque.com\")")) {
			bool ret = SteamHTTP.SetHTTPRequestHeaderValue(m_RequestHandle, "From", "support@rileylabrecque.com");
			print("SteamHTTP.SetHTTPRequestHeaderValue(" + m_RequestHandle + ", " + "\"From\"" + ", " + "\"support@rileylabrecque.com\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestGetOrPostParameter(m_RequestHandle, \"testing\", \"Steamworks.NET\")")) {
			bool ret = SteamHTTP.SetHTTPRequestGetOrPostParameter(m_RequestHandle, "testing", "Steamworks.NET");
			print("SteamHTTP.SetHTTPRequestGetOrPostParameter(" + m_RequestHandle + ", " + "\"testing\"" + ", " + "\"Steamworks.NET\"" + ") : " + ret);
		}

		if (GUILayout.Button("SendHTTPRequest(m_RequestHandle, out handle)")) {
			SteamAPICall_t handle;
			bool ret = SteamHTTP.SendHTTPRequest(m_RequestHandle, out handle);
			OnHTTPRequestCompletedCallResult.Set(handle);
			print("SteamHTTP.SendHTTPRequest(" + m_RequestHandle + ", " + "out handle" + ") : " + ret + " -- " + handle);
		}

		if (GUILayout.Button("SendHTTPRequestAndStreamResponse(m_RequestHandle, out handle)")) {
			SteamAPICall_t handle;
			bool ret = SteamHTTP.SendHTTPRequestAndStreamResponse(m_RequestHandle, out handle);
			OnHTTPRequestCompletedCallResult.Set(handle);
			print("SteamHTTP.SendHTTPRequestAndStreamResponse(" + m_RequestHandle + ", " + "out handle" + ") : " + ret + " -- " + handle);
		}

		if (GUILayout.Button("DeferHTTPRequest(m_RequestHandle)")) {
			bool ret = SteamHTTP.DeferHTTPRequest(m_RequestHandle);
			print("SteamHTTP.DeferHTTPRequest(" + m_RequestHandle + ") : " + ret);
		}

		if (GUILayout.Button("PrioritizeHTTPRequest(m_RequestHandle)")) {
			bool ret = SteamHTTP.PrioritizeHTTPRequest(m_RequestHandle);
			print("SteamHTTP.PrioritizeHTTPRequest(" + m_RequestHandle + ") : " + ret);
		}

		if (GUILayout.Button("GetHTTPResponseHeaderSize(m_RequestHandle, \"User-Agent\", out ResponseHeaderSize)")) {
			uint ResponseHeaderSize;
			bool ret = SteamHTTP.GetHTTPResponseHeaderSize(m_RequestHandle, "User-Agent", out ResponseHeaderSize);
			print("SteamHTTP.GetHTTPResponseHeaderSize(" + m_RequestHandle + ", " + "\"User-Agent\"" + ", " + "out ResponseHeaderSize" + ") : " + ret + " -- " + ResponseHeaderSize);
		}

		if (GUILayout.Button("GetHTTPResponseHeaderValue(m_RequestHandle, \"User-Agent\", HeaderValueBuffer, ResponseHeaderSize)")) {
			uint ResponseHeaderSize;
			SteamHTTP.GetHTTPResponseHeaderSize(m_RequestHandle, "User-Agent", out ResponseHeaderSize);

			byte[] HeaderValueBuffer = new byte[ResponseHeaderSize];
			bool ret = SteamHTTP.GetHTTPResponseHeaderValue(m_RequestHandle, "User-Agent", HeaderValueBuffer, ResponseHeaderSize);
			print("SteamHTTP.GetHTTPResponseHeaderValue(" + m_RequestHandle + ", " + "\"User-Agent\"" + ", " + HeaderValueBuffer + ", " + ResponseHeaderSize + ") : " + ret);
			print("HeaderValueBuffer:\n" + System.Text.Encoding.UTF8.GetString(HeaderValueBuffer));
		}

		if (GUILayout.Button("GetHTTPResponseBodySize(m_RequestHandle, out BodySize)")) {
			uint BodySize;
			bool ret = SteamHTTP.GetHTTPResponseBodySize(m_RequestHandle, out BodySize);
			print("SteamHTTP.GetHTTPResponseBodySize(" + m_RequestHandle + ", " + "out BodySize" + ") : " + ret + " -- " + BodySize);
		}

		if (GUILayout.Button("GetHTTPResponseBodyData(m_RequestHandle, BodyDataBuffer, BodySize)")) {
			uint BodySize;
			SteamHTTP.GetHTTPResponseBodySize(m_RequestHandle, out BodySize);

			byte[] BodyDataBuffer = new byte[BodySize];
			bool ret = SteamHTTP.GetHTTPResponseBodyData(m_RequestHandle, BodyDataBuffer, BodySize);
			print("SteamHTTP.GetHTTPResponseBodyData(" + m_RequestHandle + ", " + BodyDataBuffer + ", " + BodySize + ") : " + ret);
			print("BodyDataBuffer:\n" + System.Text.Encoding.UTF8.GetString(BodyDataBuffer));
		}

		if (GUILayout.Button("GetHTTPStreamingResponseBodyData(m_RequestHandle, m_Offset, BodyDataBuffer, m_BufferSize)")) {
			byte[] BodyDataBuffer = new byte[m_BufferSize];
			bool ret = SteamHTTP.GetHTTPStreamingResponseBodyData(m_RequestHandle, m_Offset, BodyDataBuffer, m_BufferSize);
			print("SteamHTTP.GetHTTPStreamingResponseBodyData(" + m_RequestHandle + ", " + m_Offset + ", " + BodyDataBuffer + ", " + m_BufferSize + ") : " + ret);
			print("BodyDataBuffer:\n" + System.Text.Encoding.UTF8.GetString(BodyDataBuffer));
		}

		if (GUILayout.Button("ReleaseHTTPRequest(m_RequestHandle)")) {
			bool ret = SteamHTTP.ReleaseHTTPRequest(m_RequestHandle);
			print("SteamHTTP.ReleaseHTTPRequest(" + m_RequestHandle + ") : " + ret);
		}

		{
			float PercentOut;
			bool ret = SteamHTTP.GetHTTPDownloadProgressPct(m_RequestHandle, out PercentOut);
			GUILayout.Label("GetHTTPDownloadProgressPct(m_RequestHandle, out PercentOut) : " + ret + " -- " + PercentOut);
		}

		if (GUILayout.Button("SetHTTPRequestRawPostBody(m_RequestHandle, \"application/x-www-form-urlencoded\", buffer, (uint)buffer.Length)")) {
			string Body = "parameter=value&also=another";
			byte[] buffer = new byte[System.Text.Encoding.UTF8.GetByteCount(Body) + 1];
			System.Text.Encoding.UTF8.GetBytes(Body, 0, Body.Length, buffer, 0);
			bool ret = SteamHTTP.SetHTTPRequestRawPostBody(m_RequestHandle, "application/x-www-form-urlencoded", buffer, (uint)buffer.Length);
			print("SteamHTTP.SetHTTPRequestRawPostBody(" + m_RequestHandle + ", " + "\"application/x-www-form-urlencoded\"" + ", " + buffer + ", " + (uint)buffer.Length + ") : " + ret);
		}

		if (GUILayout.Button("CreateCookieContainer(true)")) {
			m_CookieContainer = SteamHTTP.CreateCookieContainer(true);
			print("SteamHTTP.CreateCookieContainer(" + true + ") : " + m_CookieContainer);
		}

		if (GUILayout.Button("ReleaseCookieContainer(m_CookieContainer)")) {
			ReleaseCookieContainer();
		}

		if (GUILayout.Button("SetCookie(m_CookieContainer, \"http://httpbin.org\", \"http://httpbin.org/cookies\", \"TestCookie=Testing\")")) {
			bool ret = SteamHTTP.SetCookie(m_CookieContainer, "http://httpbin.org", "http://httpbin.org/cookies", "TestCookie=Testing");
			print("SteamHTTP.SetCookie(" + m_CookieContainer + ", " + "\"http://httpbin.org\"" + ", " + "\"http://httpbin.org/cookies\"" + ", " + "\"TestCookie=Testing\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestCookieContainer(m_RequestHandle, m_CookieContainer)")) {
			bool ret = SteamHTTP.SetHTTPRequestCookieContainer(m_RequestHandle, m_CookieContainer);
			print("SteamHTTP.SetHTTPRequestCookieContainer(" + m_RequestHandle + ", " + m_CookieContainer + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestUserAgentInfo(m_RequestHandle, \"TestUserAgentInfo\")")) {
			bool ret = SteamHTTP.SetHTTPRequestUserAgentInfo(m_RequestHandle, "TestUserAgentInfo");
			print("SteamHTTP.SetHTTPRequestUserAgentInfo(" + m_RequestHandle + ", " + "\"TestUserAgentInfo\"" + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestRequiresVerifiedCertificate(m_RequestHandle, false)")) {
			bool ret = SteamHTTP.SetHTTPRequestRequiresVerifiedCertificate(m_RequestHandle, false);
			print("SteamHTTP.SetHTTPRequestRequiresVerifiedCertificate(" + m_RequestHandle + ", " + false + ") : " + ret);
		}

		if (GUILayout.Button("SetHTTPRequestAbsoluteTimeoutMS(m_RequestHandle, 20000)")) {
			bool ret = SteamHTTP.SetHTTPRequestAbsoluteTimeoutMS(m_RequestHandle, 20000);
			print("SteamHTTP.SetHTTPRequestAbsoluteTimeoutMS(" + m_RequestHandle + ", " + 20000 + ") : " + ret);
		}

		{
			bool WasTimedOut;
			bool ret = SteamHTTP.GetHTTPRequestWasTimedOut(m_RequestHandle, out WasTimedOut);
			GUILayout.Label("GetHTTPRequestWasTimedOut(m_RequestHandle, out WasTimedOut) : " + ret + " -- " + WasTimedOut);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnHTTPRequestCompleted(HTTPRequestCompleted_t pCallback, bool bIOFailure) {
		Debug.Log("[" + HTTPRequestCompleted_t.k_iCallback + " - HTTPRequestCompleted] - " + pCallback.m_hRequest + " -- " + pCallback.m_ulContextValue + " -- " + pCallback.m_bRequestSuccessful + " -- " + pCallback.m_eStatusCode + " -- " + pCallback.m_unBodySize);

		m_ContextValue = pCallback.m_ulContextValue;
	}

	void OnHTTPRequestHeadersReceived(HTTPRequestHeadersReceived_t pCallback) {
		Debug.Log("[" + HTTPRequestHeadersReceived_t.k_iCallback + " - HTTPRequestHeadersReceived] - " + pCallback.m_hRequest + " -- " + pCallback.m_ulContextValue);
	}

	void OnHTTPRequestDataReceived(HTTPRequestDataReceived_t pCallback) {
		Debug.Log("[" + HTTPRequestDataReceived_t.k_iCallback + " - HTTPRequestDataReceived] - " + pCallback.m_hRequest + " -- " + pCallback.m_ulContextValue + " -- " + pCallback.m_cOffset + " -- " + pCallback.m_cBytesReceived);

		m_Offset = pCallback.m_cOffset;
		m_BufferSize = pCallback.m_cBytesReceived;
	}
}