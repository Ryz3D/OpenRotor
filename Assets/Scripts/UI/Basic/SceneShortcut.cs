using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShortcut : MonoBehaviour {
	public KeyCode key;
	public string scene;
	public LoadSceneMode mode;

	void Update() {
		if (Input.GetKeyDown(key)) {
			SceneManager.LoadScene(scene, mode);
		}
	}
}
