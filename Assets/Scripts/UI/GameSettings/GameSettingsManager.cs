using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour {
    public Toggle tglGate;
    public Toggle tglFPS;
    public Toggle tglProps;
    public Toggle tglCross;
    public Toggle tglLipo;
    public GameObject osdParent;

	private Dictionary<string, Toggle> osd;

    public void Apply() {
        PlayerPrefs.SetInt("gameGate", tglGate.isOn ? 1 : 0);
        PlayerPrefs.SetInt("gameFPS", tglFPS.isOn ? 1 : 0);
        PlayerPrefs.SetInt("gameProps", tglProps.isOn ? 1 : 0);
        PlayerPrefs.SetInt("gameCross", tglCross.isOn ? 1 : 0);
        PlayerPrefs.SetInt("gameLipo", tglLipo.isOn ? 1 : 0);

        string osdString = "";
		foreach (KeyValuePair<string, Toggle> e in osd) {
			if (e.Value.isOn) {
				osdString += e.Key + ",";
			}
		}
		PlayerPrefs.SetString("osdConfig", osdString);
    }
    
    void Start() {
        tglGate.isOn = PlayerPrefs.HasKey("gameGate") ? PlayerPrefs.GetInt("gameGate") == 1 : false;
        tglFPS.isOn = PlayerPrefs.HasKey("gameFPS") ? PlayerPrefs.GetInt("gameFPS") == 1 : false;
        tglProps.isOn = PlayerPrefs.HasKey("gameProps") ? PlayerPrefs.GetInt("gameProps") == 1 : false;
        tglCross.isOn = PlayerPrefs.HasKey("gameCross") ? PlayerPrefs.GetInt("gameCross") == 1 : false;
        tglLipo.isOn = PlayerPrefs.HasKey("gameLipo") ? PlayerPrefs.GetInt("gameLipo") == 1 : false;

        string osdString = PlayerPrefs.HasKey("osdConfig") ? PlayerPrefs.GetString("osdConfig") : "";
		osd = new Dictionary<string, Toggle>();
		for (int i = 0; i < osdParent.transform.childCount; i++) {
			string key = osdParent.transform.GetChild(i).name;
			Toggle value = osdParent.transform.GetChild(i).GetComponentInChildren<Toggle>();
			osd.Add(key, value);

			value.isOn = osdString.Contains(key + ",");
		}
    }

    void Update() {
        
    }
}
