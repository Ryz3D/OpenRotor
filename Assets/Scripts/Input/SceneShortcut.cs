using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShortcut : MonoBehaviour {
	public KeyCode key;
	public string scene;
	public LoadSceneMode mode;
	public int maxScenes;
	public LevelSelectType levelSelectType;

	void Update() {
		if (Input.GetKeyDown(key)) {
			if (mode == LoadSceneMode.Additive) {
				for (int i = 0; i < SceneManager.sceneCount; i++) {
					if (SceneManager.GetSceneAt(i).name == scene) {
						return;
					}
				}
			}

			if (mode != LoadSceneMode.Additive || maxScenes == 0 || SceneManager.sceneCount < maxScenes) {
				if (levelSelectType != LevelSelectType.Unknown) {
					SceneParam.selectType = levelSelectType;
				}
				SceneParam.lastScene = gameObject.scene.name;
				SceneManager.LoadScene(scene, mode);
			}
		}
	}
}
