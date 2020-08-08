using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Xml.Linq;

public class InputWizard : MonoBehaviour {
    public InputField txtName;
    public Slider[] sldPreview;
    public Button[] btnAxisType;
    public Button[] btnFind;

    private CustomInput input;
    private int findAxisIndex;
    private string findAxisType = "";
    private string lastFindAxisType;
    private bool firstFindRun;
    private float[] lastAxisInputs;

    public void SaveConfig() {
        string name = txtName.text.Replace(".", "").Replace(",", "").Replace(Path.DirectorySeparatorChar.ToString(), "");
        if (name.Replace(" ", "") == "") {
            name = "input" + Random.Range(0, 100000);
        }
        string path = ConfigManager.basePath + "input" + Path.DirectorySeparatorChar + name + ".xml";
        if (StaticDataAccess.config.fs.WhatIs(path) != FileType.Nonexistent) {
            Debug.Log("Overwriting input '" + path + "'");
        }

        XElement xml = input.Serialize();
        XDocument doc = new XDocument(xml);
        doc.Save(path);

        PlayerPrefs.SetString("inputConfig", path);
    }

    void Start() {
        input = new CustomInput();
    }

    public void SwitchAxisType(int index) {
    }

    public void FindAxis(int index) {
        if (findAxisType == "") {
            findAxisIndex = index;
            findAxisType = "simpleAxis";
        }
    }

    void Update() {
        for (int i = 0; i < 10; i++) {
            sldPreview[i].value = input.axis[i].GetValue();
            btnAxisType[i].interactable = findAxisType == "";
            btnFind[i].interactable = findAxisType == "";
        }

        switch (findAxisType) {
            case "simpleAxis":
                if (lastFindAxisType != findAxisType) {
                    lastAxisInputs = new float[32];
                    firstFindRun = true;
                }
                else if (lastFindAxisType == "simpleAxis") {
                    for (int i = 0; i < 32; i++) {
                        float delta = Input.GetAxis(i < 16 ? "axis" + i : "btn" + (i - 16)) - lastAxisInputs[i];
                        if (!firstFindRun && Mathf.Abs(delta) > 0.1f) {
                            input.axis[findAxisIndex] = new SimpleInputAxis() {
                                axisName = i < 16 ? "axis" + i : "btn" + (i - 16),
                                invert = delta < 0,
                            };
                            findAxisType = "";
                            break;
                        }
                        lastAxisInputs[i] = Input.GetAxis(i < 16 ? "axis" + i : "btn" + (i - 16));
                    }
                    firstFindRun = false;
                }
                break;
            default:
                if (lastFindAxisType != "") {
                    input.axis[findAxisIndex] = new EmptyAxis();
                }
                break;
        }
        lastFindAxisType = findAxisType;
    }
}
