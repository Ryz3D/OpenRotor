using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsLoader : MonoBehaviour {
	public GameObject fps;
	public Lipo lipo;

	void Start() {
		if (PlayerPrefs.HasKey("gameFPS")) {
			fps.SetActive(PlayerPrefs.GetInt("gameFPS") == 1);
		}
		if (PlayerPrefs.HasKey("gameLipo")) {
			lipo.doesDischarge = PlayerPrefs.GetInt("gameLipo") == 1;
		}
	}

	void Update() {
		
	}
}
