using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OSDManager : MonoBehaviour {
    public List<OSDElement> elements = new List<OSDElement>();

    private ConfigDataManager config;

    void Start() {
        GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}
    }

    void Rebuild() {
        foreach (OSDElement elem in elements) {
            Destroy(elem.gameObject);
        }

        elements.Clear();
        foreach (string s in config.osdElements) {
            switch (s) {
                case "input":
                    elements.Add(new OSDInput());
                    elements.Last().Build();
                    break;
            }
        }
    }

    void Update() {
        if (config.uiRebuild) {
            Rebuild();
            config.uiRebuild = false;
        }

        foreach (OSDElement elem in elements) {
            elem.Update();
        }
    }
}
