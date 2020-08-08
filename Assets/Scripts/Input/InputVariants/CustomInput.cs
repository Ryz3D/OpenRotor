using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class CustomInput : RCInput, Serializable {
    public CustomInputAxis[] axis;
    /*
    0: Throttle
    1: Roll
    2: Pitch
    3: Yaw
    4: Tilt
    5: FoV
    6: Exit
    7: Submit
    8: Reset
    9: Flip
    */

    private List<string> axisKeys = new List<string>() {
        "throttle",
        "roll",
        "pitch",
        "yaw",
        "tilt",
        "fov",
        "exit",
        "submit",
        "reset",
        "flip"
    };

    public static CustomInput defaultInput {
        get {
            CustomInput input = new CustomInput();
            input.axis[0] = new KeyInputAxis() {
                keyLow = KeyCode.S,
                keyHigh = KeyCode.W
            };
            input.axis[1] = new KeyInputAxis() {
                keyLow = KeyCode.Keypad4,
                keyHigh = KeyCode.Keypad6
            };
            input.axis[2] = new KeyInputAxis() {
                keyLow = KeyCode.Keypad2,
                keyHigh = KeyCode.Keypad8
            };
            input.axis[3] = new KeyInputAxis() {
                keyLow = KeyCode.A,
                keyHigh = KeyCode.D
            };
            input.axis[4] = new KeyInputAxis() {
                keyLow = KeyCode.DownArrow,
                keyHigh = KeyCode.UpArrow
            };
            input.axis[5] = new KeyInputAxis() {
                keyLow = KeyCode.LeftArrow,
                keyHigh = KeyCode.RightArrow
            };
            input.axis[6] = new KeyInputAxis() {
                keyHigh = KeyCode.Escape
            };
            input.axis[7] = new KeyInputAxis() {
                keyHigh = KeyCode.Return
            };
            input.axis[8] = new KeyInputAxis() {
                keyHigh = KeyCode.R
            };
            input.axis[9] = new KeyInputAxis() {
                keyHigh = KeyCode.F
            };
            return input;
        }
    }

    public CustomInput() {
        axis = new CustomInputAxis[10];
        for (int i = 0; i < axis.Length; i++) {
            axis[i] = new EmptyAxis();
        }
    }

    public override float GetAxisFOV() {
        return axis[5].GetValue();
    }

    public override float GetAxisTilt() {
        return axis[4].GetValue();
    }

    public override float GetAxisPitch() {
        return axis[2].GetValue();
    }

    public override float GetAxisRoll() {
        return axis[1].GetValue();
    }

    public override float GetAxisThrottle() {
        return axis[0].GetValue();
    }

    public override float GetAxisYaw() {
        return axis[3].GetValue();
    }

    public override bool GetBtnExit() {
        return axis[6].GetValue() >= 0.5f;
    }

    public override bool GetBtnFlip() {
        return axis[9].GetValue() >= 0.5f;
    }

    public override bool GetBtnReset() {
        return axis[8].GetValue() >= 0.5f;
    }

    public override bool GetBtnSubmit() {
        return axis[7].GetValue() >= 0.5f;
    }

    public XElement Serialize()
    {
        List<XElement> children = new List<XElement>();
        for (int i = 0; i < axis.Length; i++) {
            children.Add(new XElement(
                "axis",
                new XAttribute(
                    "key", axisKeys[i]
                ),
                axis[i].Serialize()
            ));
        }

        return new XElement(
            "customInput",
            children.ToArray()
        );
    }

    private CustomInputAxis ParseAxis(XElement xml) {
        CustomInputAxis axis = new EmptyAxis();
        switch (xml.Name.LocalName) {
            case "buttonAxis":
                axis = new ButtonInputAxis();
                break;
            case "emptyAxis":
                axis = new EmptyAxis();
                break;
            case "keyAxis":
                axis = new KeyInputAxis();
                break;
            case "simpleAxis":
                axis = new SimpleInputAxis();
                break;
            case "staticAxis":
                axis = new StaticInputAxis();
                break;
        }
        axis.Deserialize(xml);
        return axis;
    }

    public void Deserialize(XElement xml) {
        if (xml.Attribute("mobile") != null) {
            if (xml.Attribute("mobile").Value == "true") {
                // SetActive didn't work. wtf.
                GameObject go = GameObject.Find("mobileInput");
                if (go != null) {
                    for (int i = 0; i < go.transform.childCount; i++) {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
        List<XElement> axisElem = new List<XElement>(xml.Elements());
        for (int i = 0; i < axisElem.Count; i++) {
            string axisKey = axisElem[i].Attribute("key").Value;
            int axisIndex = axisKeys.FindIndex((string s) => s == axisKey);
            if (axisIndex != -1)
                axis[axisIndex] = ParseAxis(axisElem[i].Elements().First());
        }
    }
}
