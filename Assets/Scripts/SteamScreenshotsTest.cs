using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamScreenshotsTest : MonoBehaviour {
	private Vector2 m_ScrollPos;
	private ScreenshotHandle m_ScreenshotHandle;
	private bool m_Hooked;

	protected Callback<ScreenshotReady_t> m_ScreenshotReady;
	protected Callback<ScreenshotRequested_t> m_ScreenshotRequested;

	public void OnEnable() {
		m_ScreenshotReady = Callback<ScreenshotReady_t>.Create(OnScreenshotReady);
		m_ScreenshotRequested = Callback<ScreenshotRequested_t>.Create(OnScreenshotRequested);
	}

	IEnumerator WriteScreenshot() {
		yield return new WaitForEndOfFrame();

		Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
		texture.Apply(false);

		Color[] color = texture.GetPixels();
		byte[] RGB = new byte[color.Length * 3];

		for(int i = 0, c = 0; i < RGB.Length; i += 3, ++c) {
			RGB[i] = (byte)(color[c].r * 255.0f);
			RGB[i + 1] = (byte)(color[c].g * 255.0f);
			RGB[i + 2] = (byte)(color[c].b * 255.0f);
		}

		Destroy(texture);

		// TODO: The image is upside down! "@ares_p: in Unity all texture data starts from "bottom" (OpenGL convention)"
		m_ScreenshotHandle = SteamScreenshots.WriteScreenshot(RGB, (uint)RGB.Length, Screen.width, Screen.height);
		print("SteamScreenshots.WriteScreenshot(" + RGB + ", " + (uint)RGB.Length + ", " + Screen.width + ", " + Screen.height + ") : " + m_ScreenshotHandle);
	}

	IEnumerator AddScreenshotToLibrary() {
		while (true) {
			if (System.IO.File.Exists(Application.dataPath + "/screenshot.png")) {
				m_ScreenshotHandle = SteamScreenshots.AddScreenshotToLibrary(Application.dataPath + "/screenshot.png", "", Screen.width, Screen.height);
				print("SteamScreenshots.AddScreenshotToLibrary(\"screenshot.png\", \"\", " + Screen.width + ", " + Screen.height + ") : " + m_ScreenshotHandle);
				yield break;
			}

			yield return null;
		}
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_ScreenshotHandle: " + m_ScreenshotHandle);
		GUILayout.Label("m_Hooked: " + m_Hooked);
		GUILayout.EndArea();

		GUILayout.BeginVertical("box");
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(Screen.width - 215), GUILayout.Height(Screen.height - 33));

		if (GUILayout.Button("WriteScreenshot(RGB, (uint)RGB.Length, Screen.width, Screen.height)")) {
			// We start a Coroutine with the actual implementation of this test because Texture2D.ReadPixels() has to be called at the end of the frame.
			StartCoroutine(WriteScreenshot());
		}

		if (GUILayout.Button("AddScreenshotToLibrary(Application.dataPath + \"/screenshot.png\", \"\", Screen.width, Screen.height)")) {
			ScreenCapture.CaptureScreenshot("screenshot.png");
			// Application.CaptureScreenshot is asyncronous, therefore we have to wait until the screenshot is created.
			StartCoroutine(AddScreenshotToLibrary());
		}

		if (GUILayout.Button("TriggerScreenshot()")) {
			SteamScreenshots.TriggerScreenshot();
			print("SteamScreenshots.TriggerScreenshot()");
		}

		if (GUILayout.Button("HookScreenshots(!m_Hooked)")) {
			SteamScreenshots.HookScreenshots(!m_Hooked);
			print("SteamScreenshots.HookScreenshots(" + !m_Hooked + ")");
			m_Hooked = !m_Hooked;
		}

		if (GUILayout.Button("SetLocation(m_ScreenshotHandle, \"LocationTest\")")) {
			bool ret = SteamScreenshots.SetLocation(m_ScreenshotHandle, "LocationTest");
			print("SteamScreenshots.SetLocation(" + m_ScreenshotHandle + ", " + "\"LocationTest\"" + ") : " + ret);
		}

		if (GUILayout.Button("TagUser(m_ScreenshotHandle, TestConstants.Instance.k_SteamId_rlabrecque)")) {
			bool ret = SteamScreenshots.TagUser(m_ScreenshotHandle, TestConstants.Instance.k_SteamId_rlabrecque);
			print("SteamScreenshots.TagUser(" + m_ScreenshotHandle + ", " + TestConstants.Instance.k_SteamId_rlabrecque + ") : " + ret);
		}

		if (GUILayout.Button("TagPublishedFile(m_ScreenshotHandle, PublishedFileId_t.Invalid)")) {
			bool ret = SteamScreenshots.TagPublishedFile(m_ScreenshotHandle, PublishedFileId_t.Invalid);
			print("SteamScreenshots.TagPublishedFile(" + m_ScreenshotHandle + ", " + PublishedFileId_t.Invalid + ") : " + ret);
		}

		GUILayout.Label("IsScreenshotsHooked() : " + SteamScreenshots.IsScreenshotsHooked());

		if (GUILayout.Button("AddVRScreenshotToLibrary(EVRScreenshotType.k_EVRScreenshotType_None, null, null)")) {
			ScreenshotHandle ret = SteamScreenshots.AddVRScreenshotToLibrary(EVRScreenshotType.k_EVRScreenshotType_None, null, null);
			print("SteamScreenshots.AddVRScreenshotToLibrary(" + EVRScreenshotType.k_EVRScreenshotType_None + ", " + null + ", " + null + ") : " + ret);
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	void OnScreenshotReady(ScreenshotReady_t pCallback) {
		Debug.Log("[" + ScreenshotReady_t.k_iCallback + " - ScreenshotReady] - " + pCallback.m_hLocal + " -- " + pCallback.m_eResult);
	}

	void OnScreenshotRequested(ScreenshotRequested_t pCallback) {
		Debug.Log("[" + ScreenshotRequested_t.k_iCallback + " - ScreenshotRequested]");
	}
}