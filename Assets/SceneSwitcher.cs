using UnityEngine;
using System.Collections;

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