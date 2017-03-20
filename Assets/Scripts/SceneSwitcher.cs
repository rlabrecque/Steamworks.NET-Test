using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is to test if Steamworks.NET behaves well when switching between scenes.
/// </summary>
public class SceneSwitcher : MonoBehaviour {
	void Update() {
		if (Input.GetKeyDown(KeyCode.F5)) {
			if (SceneManager.GetActiveScene().buildIndex == 0) {
				SceneManager.LoadScene(1);
			}
			else {
				SceneManager.LoadScene(0);
			}
		}
	}
}
