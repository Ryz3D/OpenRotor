using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ConfigDataManager : MonoBehaviour {
    public string osdConfigOverride;

    private string[] subFolders = {
        "",
        "input"
    };

    public FileSystem fs {
        get;
        private set;
    }

    public CustomInput input {
        get;
        private set;
    }

    public List<string> osdElements = new List<string>();
    public bool uiRebuild;

    void Awake() {
        StaticDataAccess.config = this;
        Reload();
    }

    public void Reload() {
        if (fs == null) {
            fs = new FSWindows();
        }

        // deprecated, due to settings page
        /*
        if (inputConfigOverride != "") {
            input = null;
            PlayerPrefs.SetString("inputConfig", inputConfigOverride);
        }
        */
        if (osdConfigOverride != "") {
            osdElements = new List<string>(); // make sure it loads
            PlayerPrefs.SetString("osdConfig", osdConfigOverride);
        }

        foreach (string f in subFolders) {
            if (fs.WhatIs(ConfigManager.basePath + f) != FileType.Directory) {
                fs.MakeDir(ConfigManager.basePath + f);
            }
        }

        if (!PlayerPrefs.HasKey("inputConfig")) {
            string path = ConfigManager.basePath + "input\\default.xml";
            if (fs.WhatIs(path) == FileType.Nonexistent) {
                XDocument doc = new XDocument(CustomInput.defaultInput.Serialize());
                fs.Write(path, doc.ToString());
            }
            input = CustomInput.defaultInput;
            PlayerPrefs.SetString("inputConfig", path);
        }
        else {
            string path = PlayerPrefs.GetString("inputConfig");
            if (fs.WhatIs(path) == FileType.Nonexistent) {
                Debug.LogError("couldn't find inputConfig '" + path + "'");
            }
            else {
                XDocument doc = XDocument.Parse(fs.Read(path));
                XElement elem = doc.Element("customInput");
                if (elem == null) {
                    Debug.LogError("inputConfig seems broken '" + path + "'");
                }
                else {
                    if (input == null)
                        input = new CustomInput();
                    input.Deserialize(elem);
                }
            }
        }

        if (osdElements.Count == 0) {
            if (!PlayerPrefs.HasKey("osdConfig")) {
                PlayerPrefs.SetString("osdConfig", "input,");
            }
            osdElements = PlayerPrefs.GetString("osdConfig").Split(',').ToList();
            uiRebuild = true;
        }
        else {
            string str = "";
            osdElements.ForEach((string s) => str += s + ",");
            PlayerPrefs.SetString("osdConfig", str);
        }

        if (!PlayerPrefs.HasKey("focalLength")) {
            PlayerPrefs.SetFloat("focalLength", 2.1f);
        }
        if (!PlayerPrefs.HasKey("camTilt")) {
            PlayerPrefs.SetFloat("camTilt", 20.0f);
        }

        if (gameObject.scene.name == "Sim") {
            Camera.main.focalLength = PlayerPrefs.GetFloat("focalLength");
            Camera.main.transform.eulerAngles = new Vector3(-PlayerPrefs.GetFloat("camTilt"), 0.0f, 0.0f);
        }
    }
}
