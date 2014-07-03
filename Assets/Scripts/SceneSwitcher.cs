using UnityEngine;
using System.Collections;

/// <summary>
/// This is to test if Steamworks.NET behaves well when switching between scenes.
/// </summary>
public class SceneSwitcher : MonoBehaviour {
	void Update() {
		if (Input.GetKeyDown(KeyCode.F5)) {
			if (Application.loadedLevel == 0) {
				Application.LoadLevel(1);
			}
			else {
				Application.LoadLevel(0);
			}
		}
	}
}
