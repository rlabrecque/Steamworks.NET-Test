using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamHTMLSurfaceTest : MonoBehaviour {
	const int WidthOffset = 400;
	const int HeightOffset = 100;

	private bool m_Init;
	private HHTMLBrowser m_HHTMLBrowser = HHTMLBrowser.Invalid;
	private string m_URL = "http://steamworks.github.io";
	private Texture2D m_Texture;
	private uint m_Width;
	private uint m_Height;
	private bool m_CanGoBack;
	private bool m_CanGoForward;
	private Rect m_Rect;
	private Vector2 m_LastMousePos;
	private uint m_VerticalScrollMax;
	private uint m_VeritcalScrollCurrent;
	private uint m_HorizontalScrollMax;
	private uint m_HorizontalScrollCurrent;
	private bool m_SetKeyFocus;
	private string m_Find = "Steamworks";
	private bool m_CurrentlyInFind = false;
	private float m_ScaleFactor = 0f;
	
	protected Callback<HTML_NeedsPaint_t> m_HTML_NeedsPaint;
	protected Callback<HTML_StartRequest_t> m_HTML_StartRequest;
	protected Callback<HTML_CloseBrowser_t> m_HTML_CloseBrowser;
	protected Callback<HTML_URLChanged_t> m_HTML_URLChanged;
	protected Callback<HTML_FinishedRequest_t> m_HTML_FinishedRequest;
	protected Callback<HTML_OpenLinkInNewTab_t> m_HTML_OpenLinkInNewTab;
	protected Callback<HTML_ChangedTitle_t> m_HTML_ChangedTitle;
	protected Callback<HTML_SearchResults_t> m_HTML_SearchResults;
	protected Callback<HTML_CanGoBackAndForward_t> m_HTML_CanGoBackAndForward;
	protected Callback<HTML_HorizontalScroll_t> m_HTML_HorizontalScroll;
	protected Callback<HTML_VerticalScroll_t> m_HTML_VerticalScroll;
	protected Callback<HTML_LinkAtPosition_t> m_HTML_LinkAtPosition;
	protected Callback<HTML_JSAlert_t> m_HTML_JSAlert;
	protected Callback<HTML_JSConfirm_t> m_HTML_JSConfirm;
	protected Callback<HTML_FileOpenDialog_t> m_HTML_FileOpenDialog;
	protected Callback<HTML_NewWindow_t> m_HTML_NewWindow;
	protected Callback<HTML_SetCursor_t> m_HTML_SetCursor;
	protected Callback<HTML_StatusText_t> m_HTML_StatusText;
	protected Callback<HTML_ShowToolTip_t> m_HTML_ShowToolTip;
	protected Callback<HTML_UpdateToolTip_t> m_HTML_UpdateToolTip;
	protected Callback<HTML_HideToolTip_t> m_HTML_HideToolTip;

	private CallResult<HTML_BrowserReady_t> m_HTML_BrowserReadyResult;

	public void OnEnable() {
		m_HTML_NeedsPaint = Callback<HTML_NeedsPaint_t>.Create(OnHTML_NeedsPaint);
		m_HTML_StartRequest = Callback<HTML_StartRequest_t>.Create(OnHTML_StartRequest);
		m_HTML_CloseBrowser = Callback<HTML_CloseBrowser_t>.Create(OnHTML_CloseBrowser);
		m_HTML_URLChanged = Callback<HTML_URLChanged_t>.Create(OnHTML_URLChanged);
		m_HTML_FinishedRequest = Callback<HTML_FinishedRequest_t>.Create(OnHTML_FinishedRequest);
		m_HTML_OpenLinkInNewTab = Callback<HTML_OpenLinkInNewTab_t>.Create(OnHTML_OpenLinkInNewTab);
		m_HTML_ChangedTitle = Callback<HTML_ChangedTitle_t>.Create(OnHTML_ChangedTitle);
		m_HTML_SearchResults = Callback<HTML_SearchResults_t>.Create(OnHTML_SearchResults);
		m_HTML_CanGoBackAndForward = Callback<HTML_CanGoBackAndForward_t>.Create(OnHTML_CanGoBackAndForward);
		m_HTML_HorizontalScroll = Callback<HTML_HorizontalScroll_t>.Create(OnHTML_HorizontalScroll);
		m_HTML_VerticalScroll = Callback<HTML_VerticalScroll_t>.Create(OnHTML_VerticalScroll);
		m_HTML_LinkAtPosition = Callback<HTML_LinkAtPosition_t>.Create(OnHTML_LinkAtPosition);
		m_HTML_JSAlert = Callback<HTML_JSAlert_t>.Create(OnHTML_JSAlert);
		m_HTML_JSConfirm = Callback<HTML_JSConfirm_t>.Create(OnHTML_JSConfirm);
		m_HTML_FileOpenDialog = Callback<HTML_FileOpenDialog_t>.Create(OnHTML_FileOpenDialog);
		m_HTML_NewWindow = Callback<HTML_NewWindow_t>.Create(OnHTML_NewWindow);
		m_HTML_SetCursor = Callback<HTML_SetCursor_t>.Create(OnHTML_SetCursor);
		m_HTML_StatusText = Callback<HTML_StatusText_t>.Create(OnHTML_StatusText);
		m_HTML_ShowToolTip = Callback<HTML_ShowToolTip_t>.Create(OnHTML_ShowToolTip);
		m_HTML_UpdateToolTip = Callback<HTML_UpdateToolTip_t>.Create(OnHTML_UpdateToolTip);
		m_HTML_HideToolTip = Callback<HTML_HideToolTip_t>.Create(OnHTML_HideToolTip);

		m_HTML_BrowserReadyResult = CallResult<HTML_BrowserReady_t>.Create(OnHTML_BrowserReady);

		m_Init = SteamHTMLSurface.Init();
		print("SteamHTMLSurface.Init() : " + m_Init);

		m_Texture = null;
	}

	public void OnDisable() {
		RemoveBrowser();
	}

	public void RenderOnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
		GUILayout.Label("Variables:");
		GUILayout.Label("m_Init: " + m_Init);
		GUILayout.Label("m_HHTMLBrowser: " + m_HHTMLBrowser);
		GUILayout.EndArea();

		if (m_Texture) {
			GUI.DrawTexture(m_Rect, m_Texture);
		}

		if (!m_Init) {
			GUILayout.Label("SteamHTMLSurface.Init() returned false");
			return;
		}

		if (GUILayout.Button("CreateBrowser(\"SpaceWars Test\", null)")) {
			RemoveBrowser(); // Remove an old browser if it exists.
			SteamAPICall_t handle = SteamHTMLSurface.CreateBrowser("SpaceWars Test", null);
			m_HTML_BrowserReadyResult.Set(handle);
			print("SteamHTMLSurface.CreateBrowser(\"SpaceWars Test\", null) - " + handle);
		}

		if (GUILayout.Button("RemoveBrowser(m_HHTMLBrowser)")) {
			RemoveBrowser();
		}

		m_URL = GUILayout.TextField(m_URL);
		if (GUILayout.Button("LoadURL(m_HHTMLBrowser, m_URL, null)")) {
			SteamHTMLSurface.LoadURL(m_HHTMLBrowser, m_URL, null);
			print("SteamHTMLSurface.LoadURL(m_HHTMLBrowser," + m_URL + ", null)");
		}

		if (GUILayout.Button("SetSize(m_HHTMLBrowser, m_Width, m_Height)")) {
			m_Width = (uint)Screen.width - WidthOffset;
			m_Height = (uint)Screen.height - HeightOffset;
			m_Rect = new Rect(WidthOffset, m_Height + HeightOffset, m_Width, -m_Height); // This flips the viewport since Unity renders textures upside down.
			m_Texture = null;
			SteamHTMLSurface.SetSize(m_HHTMLBrowser, m_Width, m_Height);
			print("SteamHTMLSurface.SetSize(m_HHTMLBrowser, " + m_Width + ", " + m_Height + ")");
		}

		if (GUILayout.Button("StopLoad(m_HHTMLBrowser)")) {
			SteamHTMLSurface.StopLoad(m_HHTMLBrowser);
			print("SteamHTMLSurface.StopLoad(m_HHTMLBrowser)");
		}

		if (GUILayout.Button("Reload(m_HHTMLBrowser)")) {
			SteamHTMLSurface.Reload(m_HHTMLBrowser);
			print("SteamHTMLSurface.Reload(m_HHTMLBrowser)");
		}

		GUI.enabled = m_CanGoBack;
		if (GUILayout.Button("GoBack(m_HHTMLBrowser)")) {
			SteamHTMLSurface.GoBack(m_HHTMLBrowser);
			print("SteamHTMLSurface.GoBack(m_HHTMLBrowser)");
		}
		GUI.enabled = m_CanGoForward;
		if (GUILayout.Button("GoForward(m_HHTMLBrowser)")) {
			SteamHTMLSurface.GoForward(m_HHTMLBrowser);
			print("SteamHTMLSurface.GoForward(m_HHTMLBrowser)");
		}
		GUI.enabled = true;

		if (GUILayout.Button("AddHeader(m_HHTMLBrowser, \"From\", \"test@test.com\")")) {
			SteamHTMLSurface.AddHeader(m_HHTMLBrowser, "From", "test@test.com");
			print("SteamHTMLSurface.AddHeader(m_HHTMLBrowser, \"From\", \"test@test.com\")");
		}

		if (GUILayout.Button("ExecuteJavascript(m_HHTMLBrowser, \"window.alert('Test');\")")) {
			SteamHTMLSurface.ExecuteJavascript(m_HHTMLBrowser, "window.alert('Test');");
			print("SteamHTMLSurface.ExecuteJavascript(m_HHTMLBrowser, \"window.alert('Test');\")");
		}

		if (GUILayout.Button("SetKeyFocus(m_HHTMLBrowser, " + !m_SetKeyFocus + ")")) {
			SteamHTMLSurface.SetKeyFocus(m_HHTMLBrowser, !m_SetKeyFocus);
			m_SetKeyFocus = !m_SetKeyFocus;
			print("SteamHTMLSurface.SetKeyFocus(m_HHTMLBrowser, " + !m_SetKeyFocus + ")");
		}

		if (GUILayout.Button("ViewSource(m_HHTMLBrowser)")) {
			SteamHTMLSurface.ViewSource(m_HHTMLBrowser);
			print("SteamHTMLSurface.ViewSource(m_HHTMLBrowser)");
		}

		if (GUILayout.Button("CopyToClipboard(m_HHTMLBrowser)")) {
			SteamHTMLSurface.CopyToClipboard(m_HHTMLBrowser);
			print("SteamHTMLSurface.CopyToClipboard(m_HHTMLBrowser)");
		}

		if (GUILayout.Button("PasteFromClipboard(m_HHTMLBrowser)")) {
			SteamHTMLSurface.PasteFromClipboard(m_HHTMLBrowser);
			print("SteamHTMLSurface.PasteFromClipboard(m_HHTMLBrowser)");
		}

		m_Find = GUILayout.TextField(m_Find);
		if (GUILayout.Button("Find(m_HHTMLBrowser, m_Find, m_CurrentlyInFind, false)")) {
			SteamHTMLSurface.Find(m_HHTMLBrowser, m_Find, m_CurrentlyInFind, bReverse: false);
			print("SteamHTMLSurface.Find(m_HHTMLBrowser," + m_Find + ", " + m_CurrentlyInFind + ", false)");
			m_CurrentlyInFind = true;
		}

		if (GUILayout.Button("StopFind(m_HHTMLBrowser)")) {
			SteamHTMLSurface.StopFind(m_HHTMLBrowser);
			print("SteamHTMLSurface.StopFind(m_HHTMLBrowser)");
			m_CurrentlyInFind = false;
		}

		if (GUILayout.Button("GetLinkAtPosition(m_HHTMLBrowser, 500, 120)")) {
			SteamHTMLSurface.GetLinkAtPosition(m_HHTMLBrowser, 500 - WidthOffset, 120 - HeightOffset);
			print("SteamHTMLSurface.GetLinkAtPosition(m_HHTMLBrowser, 500, 120)");
		}

		// Use with http://html-kit.com/tools/cookietester/
		if (GUILayout.Button("SetCookie(\"html-kit.com\", \"testcookiekey\", \"testcookievalue\")")) {
			SteamHTMLSurface.SetCookie("html-kit.com", "testcookiekey", "testcookievalue");
			print("SteamHTMLSurface.SetCookie(\"html-kit.com\", \"testcookiekey\", \"testcookievalue\")");
		}

		m_ScaleFactor = GUILayout.HorizontalScrollbar(m_ScaleFactor, 0.25f, 0f, 2f);
		if (GUILayout.Button("SetPageScaleFactor(m_HHTMLBrowser, " + m_ScaleFactor + ", 0, 0)")) {
			SteamHTMLSurface.SetPageScaleFactor(m_HHTMLBrowser, m_ScaleFactor, 0, 0);
			print("SteamHTMLSurface.SetPageScaleFactor(m_HHTMLBrowser, " + m_ScaleFactor + ", 0, 0)");
		}

		if (GUILayout.Button("SetBackgroundMode(m_HHTMLBrowser, Random.Range(0, 2) != 0)")) {
			SteamHTMLSurface.SetBackgroundMode(m_HHTMLBrowser, Random.Range(0, 2) != 0);
			print("SteamHTMLSurface.SetBackgroundMode(m_HHTMLBrowser, " + (Random.Range(0, 2) != 0));
		}
		
		if (m_HHTMLBrowser == HHTMLBrowser.Invalid) {
			return;
		}

		// We set the moust position before checking for mouse presses just incase the mouse moved in the same OnGUI frame as a mouse press.
		Event e = Event.current;
		if (e.mousePosition != m_LastMousePos) {
			if ((e.mousePosition.x >= WidthOffset && e.mousePosition.x <= m_Width + WidthOffset) && (e.mousePosition.y >= HeightOffset && e.mousePosition.y <= m_Height + HeightOffset)) {
				m_LastMousePos = e.mousePosition;
				SteamHTMLSurface.MouseMove(m_HHTMLBrowser, (int)(e.mousePosition.x - WidthOffset), (int)(e.mousePosition.y - HeightOffset));
			}
		}

		//virtual void MouseDoubleClick( HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton ) = 0; //TODO
		switch(e.type) {
			case EventType.MouseDown:
				SteamHTMLSurface.MouseDown(m_HHTMLBrowser, (EHTMLMouseButton)e.button);
				break;
			case EventType.MouseUp:
				SteamHTMLSurface.MouseUp(m_HHTMLBrowser, (EHTMLMouseButton)e.button);
				break;
			case EventType.ScrollWheel:
				SteamHTMLSurface.MouseWheel(m_HHTMLBrowser, (int)(-e.delta.y * 100));
				break;
			case EventType.KeyDown:
				//print("KeyDown: " + e.keyCode + " - " + (int)e.character + " - " + e.character);
				EHTMLKeyModifiers modifiers = EHTMLKeyModifiers.k_eHTMLKeyModifier_None;
				if (e.alt)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_AltDown;
				if (e.shift)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_ShiftDown;
				if (e.control)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_CtrlDown;

				if (e.keyCode != KeyCode.None)
					SteamHTMLSurface.KeyDown(m_HHTMLBrowser, (uint)e.keyCode, modifiers);
				if (e.character != 0)
					SteamHTMLSurface.KeyChar(m_HHTMLBrowser, (uint)e.character, modifiers);

				if (e.keyCode == KeyCode.DownArrow) {
					m_VeritcalScrollCurrent = System.Math.Min(m_VeritcalScrollCurrent + 100, m_VerticalScrollMax);
					SteamHTMLSurface.SetVerticalScroll(m_HHTMLBrowser, m_VeritcalScrollCurrent);
				}
				else if (e.keyCode == KeyCode.UpArrow) {
					if (m_VeritcalScrollCurrent - 100 > m_VeritcalScrollCurrent) // Underflow
						m_VeritcalScrollCurrent = 0;
					else
						m_VeritcalScrollCurrent -= 100;
					SteamHTMLSurface.SetVerticalScroll(m_HHTMLBrowser, m_VeritcalScrollCurrent);
				}
				else if (e.keyCode == KeyCode.RightArrow) {
					m_HorizontalScrollCurrent = System.Math.Min(m_HorizontalScrollCurrent + 100, m_HorizontalScrollMax);
					SteamHTMLSurface.SetHorizontalScroll(m_HHTMLBrowser, m_HorizontalScrollCurrent);
				}
				else if (e.keyCode == KeyCode.LeftArrow) {
					if (m_HorizontalScrollCurrent - 100 > m_HorizontalScrollCurrent) // Underflow
						m_HorizontalScrollCurrent = 0;
					else
						m_HorizontalScrollCurrent -= 100;
					SteamHTMLSurface.SetHorizontalScroll(m_HHTMLBrowser, m_HorizontalScrollCurrent);
				}
				break;
			case EventType.KeyUp:
				//print("KeyUp: " + e.keyCode + " - " + (int)e.character + " - " + e.character);
				modifiers = EHTMLKeyModifiers.k_eHTMLKeyModifier_None;
				if (e.alt)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_AltDown;
				if (e.shift)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_ShiftDown;
				if (e.control)
					modifiers = modifiers | EHTMLKeyModifiers.k_eHTMLKeyModifier_CtrlDown;

				if (e.keyCode != KeyCode.None)
					SteamHTMLSurface.KeyUp(m_HHTMLBrowser, (uint)e.keyCode, modifiers);
				break;
		}
	}

	void RemoveBrowser() {
		if (m_HHTMLBrowser != HHTMLBrowser.Invalid) {
			print("SteamHTMLSurface.RemoveBrowser(" + m_HHTMLBrowser + ")");
			SteamHTMLSurface.RemoveBrowser(m_HHTMLBrowser);
			m_HHTMLBrowser = HHTMLBrowser.Invalid;
		}
		m_Texture = null;
	}

	void OnHTML_BrowserReady(HTML_BrowserReady_t pCallback, bool bIOFailure) {
		Debug.Log("[" + HTML_BrowserReady_t.k_iCallback + " - HTML_BrowserReady] - " + pCallback.unBrowserHandle);
		m_HHTMLBrowser = pCallback.unBrowserHandle;
	}

	void OnHTML_NeedsPaint(HTML_NeedsPaint_t pCallback) {
		Debug.Log("[" + HTML_NeedsPaint_t.k_iCallback + " - HTML_NeedsPaint] - " + pCallback.unBrowserHandle + " -- " + pCallback.pBGRA + " -- " + pCallback.unWide + " -- " + pCallback.unTall + " -- " + pCallback.unUpdateX + " -- " + pCallback.unUpdateY + " -- " + pCallback.unUpdateWide + " -- " + pCallback.unUpdateTall + " -- " + pCallback.unScrollX + " -- " + pCallback.unScrollY + " -- " + pCallback.flPageScale + " -- " + pCallback.unPageSerial);

		if (m_Texture == null) {
			m_Texture = new Texture2D((int)pCallback.unWide, (int)pCallback.unTall, TextureFormat.BGRA32, false, true);
		}

		int dataSize = (int)(pCallback.unWide * pCallback.unTall * 4);
		byte[] bytes = new byte[dataSize];
		System.Runtime.InteropServices.Marshal.Copy(pCallback.pBGRA, bytes, 0, dataSize);

		m_Texture.LoadRawTextureData(bytes);
		m_Texture.Apply();
	}

	void OnHTML_StartRequest(HTML_StartRequest_t pCallback) {
		Debug.Log("[" + HTML_StartRequest_t.k_iCallback + " - HTML_StartRequest] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchURL + " -- " + pCallback.pchTarget + " -- " + pCallback.pchPostData + " -- " + pCallback.bIsRedirect);

		SteamHTMLSurface.AllowStartRequest(pCallback.unBrowserHandle, true);
		print("SteamHTMLSurface.AllowStartRequest(pCallback.unBrowserHandle, true)");
	}

	void OnHTML_CloseBrowser(HTML_CloseBrowser_t pCallback) {
		Debug.Log("[" + HTML_CloseBrowser_t.k_iCallback + " - HTML_CloseBrowser] - " + pCallback.unBrowserHandle);
		m_HHTMLBrowser = HHTMLBrowser.Invalid;
	}

	void OnHTML_URLChanged(HTML_URLChanged_t pCallback) {
		Debug.Log("[" + HTML_URLChanged_t.k_iCallback + " - HTML_URLChanged] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchURL + " -- " + pCallback.pchPostData + " -- " + pCallback.bIsRedirect + " -- " + pCallback.pchPageTitle + " -- " + pCallback.bNewNavigation);
	}

	void OnHTML_FinishedRequest(HTML_FinishedRequest_t pCallback) {
		Debug.Log("[" + HTML_FinishedRequest_t.k_iCallback + " - HTML_FinishedRequest] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchURL + " -- " + pCallback.pchPageTitle);
	}

	void OnHTML_OpenLinkInNewTab(HTML_OpenLinkInNewTab_t pCallback) {
		Debug.Log("[" + HTML_OpenLinkInNewTab_t.k_iCallback + " - HTML_OpenLinkInNewTab] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchURL);
	}

	void OnHTML_ChangedTitle(HTML_ChangedTitle_t pCallback) {
		Debug.Log("[" + HTML_ChangedTitle_t.k_iCallback + " - HTML_ChangedTitle] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchTitle);
	}

	void OnHTML_SearchResults(HTML_SearchResults_t pCallback) {
		Debug.Log("[" + HTML_SearchResults_t.k_iCallback + " - HTML_SearchResults] - " + pCallback.unBrowserHandle + " -- " + pCallback.unResults + " -- " + pCallback.unCurrentMatch);
	}

	void OnHTML_CanGoBackAndForward(HTML_CanGoBackAndForward_t pCallback) {
		Debug.Log("[" + HTML_CanGoBackAndForward_t.k_iCallback + " - HTML_CanGoBackAndForward] - " + pCallback.unBrowserHandle + " -- " + pCallback.bCanGoBack + " -- " + pCallback.bCanGoForward);
		m_CanGoBack = pCallback.bCanGoBack;
		m_CanGoForward = pCallback.bCanGoForward;
	}

	void OnHTML_HorizontalScroll(HTML_HorizontalScroll_t pCallback) {
		Debug.Log("[" + HTML_HorizontalScroll_t.k_iCallback + " - HTML_HorizontalScroll] - " + pCallback.unBrowserHandle + " -- " + pCallback.unScrollMax + " -- " + pCallback.unScrollCurrent + " -- " + pCallback.flPageScale + " -- " + pCallback.bVisible + " -- " + pCallback.unPageSize);
		m_HorizontalScrollMax = pCallback.unScrollMax;
		m_HorizontalScrollCurrent = pCallback.unScrollCurrent;
	}

	void OnHTML_VerticalScroll(HTML_VerticalScroll_t pCallback) {
		Debug.Log("[" + HTML_VerticalScroll_t.k_iCallback + " - HTML_VerticalScroll] - " + pCallback.unBrowserHandle + " -- " + pCallback.unScrollMax + " -- " + pCallback.unScrollCurrent + " -- " + pCallback.flPageScale + " -- " + pCallback.bVisible + " -- " + pCallback.unPageSize);
		m_VerticalScrollMax = pCallback.unScrollMax;
		m_VeritcalScrollCurrent = pCallback.unScrollCurrent;
	}

	void OnHTML_LinkAtPosition(HTML_LinkAtPosition_t pCallback) {
		Debug.Log("[" + HTML_LinkAtPosition_t.k_iCallback + " - HTML_LinkAtPosition] - " + pCallback.unBrowserHandle + " -- " + pCallback.x + " -- " + pCallback.y + " -- " + pCallback.pchURL + " -- " + pCallback.bInput + " -- " + pCallback.bLiveLink);
	}

	void OnHTML_JSAlert(HTML_JSAlert_t pCallback) {
		Debug.Log("[" + HTML_JSAlert_t.k_iCallback + " - HTML_JSAlert] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchMessage);

		SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true);
		print("SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true)");
	}

	void OnHTML_JSConfirm(HTML_JSConfirm_t pCallback) {
		Debug.Log("[" + HTML_JSConfirm_t.k_iCallback + " - HTML_JSConfirm] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchMessage);

		SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true);
		print("SteamHTMLSurface.JSDialogResponse(pCallback.unBrowserHandle, true)");
	}

	void OnHTML_FileOpenDialog(HTML_FileOpenDialog_t pCallback) {
		Debug.Log("[" + HTML_FileOpenDialog_t.k_iCallback + " - HTML_FileOpenDialog] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchTitle + " -- " + pCallback.pchInitialFile);

		// TODO: Valve has no example usage of this.
		SteamHTMLSurface.FileLoadDialogResponse(pCallback.unBrowserHandle, System.IntPtr.Zero);
	}

	void OnHTML_NewWindow(HTML_NewWindow_t pCallback) {
		Debug.Log("[" + HTML_NewWindow_t.k_iCallback + " - HTML_NewWindow] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchURL + " -- " + pCallback.unX + " -- " + pCallback.unY + " -- " + pCallback.unWide + " -- " + pCallback.unTall + " -- " + pCallback.unNewWindow_BrowserHandle);
	}

	void OnHTML_SetCursor(HTML_SetCursor_t pCallback) {
		Debug.Log("[" + HTML_SetCursor_t.k_iCallback + " - HTML_SetCursor] - " + pCallback.unBrowserHandle + " -- " + pCallback.eMouseCursor);
	}

	void OnHTML_StatusText(HTML_StatusText_t pCallback) {
		Debug.Log("[" + HTML_StatusText_t.k_iCallback + " - HTML_StatusText] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchMsg);
	}

	void OnHTML_ShowToolTip(HTML_ShowToolTip_t pCallback) {
		Debug.Log("[" + HTML_ShowToolTip_t.k_iCallback + " - HTML_ShowToolTip] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchMsg);
	}

	void OnHTML_UpdateToolTip(HTML_UpdateToolTip_t pCallback) {
		Debug.Log("[" + HTML_UpdateToolTip_t.k_iCallback + " - HTML_UpdateToolTip] - " + pCallback.unBrowserHandle + " -- " + pCallback.pchMsg);
	}

	void OnHTML_HideToolTip(HTML_HideToolTip_t pCallback) {
		Debug.Log("[" + HTML_HideToolTip_t.k_iCallback + " - HTML_HideToolTip] - " + pCallback.unBrowserHandle);
	}
}
