using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

class ConfigDataManager : MonoBehaviour {
    public string inputConfigOverride;
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
        Reload();
    }

    public void Reload() {
        if (fs == null) {
            fs = new FSWindows();
        }

        if (inputConfigOverride != "") {
            input = null;
            PlayerPrefs.SetString("inputConfig", inputConfigOverride);
        }
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
            Debug.Log("using input config '" + path + "'");
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
    }
}
