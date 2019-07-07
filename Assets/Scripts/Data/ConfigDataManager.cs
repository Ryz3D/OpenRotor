using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

class ConfigDataManager : MonoBehaviour {
    public string inputConfigOverride;

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

    public List<string> osdElements {
        get;
        private set;
    }

    void Awake() {
        Reload();
    }

    public void Reload() {
        if (fs == null) {
            fs = new FSWindows();
        }

        if (inputConfigOverride != "") {
            PlayerPrefs.SetString("inputConfig", inputConfigOverride);
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
    }
}
