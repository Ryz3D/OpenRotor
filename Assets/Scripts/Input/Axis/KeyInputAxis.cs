using System;
using System.Xml.Linq;
using UnityEngine;

public class KeyInputAxis : CustomInputAxis
{
    public KeyCode keyLow = KeyCode.None;
    public KeyCode keyHigh = KeyCode.None;

    public float GetValue() {
        if (keyLow == KeyCode.None)
            return Input.GetKey(keyHigh) ? 1 : 0;
        else
            return (Input.GetKey(keyHigh) ? 1 : 0) - (Input.GetKey(keyLow) ? 1 : 0);
    }

    public XElement Serialize() {
        return new XElement(
            "keyAxis",
            new XAttribute(
                "keyLow", keyLow.ToString()
            ),
            new XAttribute(
                "keyHigh", keyHigh.ToString()
            )
        );
    }

    public void Deserialize(XElement xml) {
        XAttribute aKeyLow = xml.Attribute("keyLow");
        keyLow = aKeyLow == null ? KeyCode.None : (KeyCode)Enum.Parse(typeof(KeyCode), aKeyLow.Value);
        XAttribute aKeyHigh = xml.Attribute("keyHigh");
        keyHigh = aKeyHigh == null ? KeyCode.None : (KeyCode)Enum.Parse(typeof(KeyCode), aKeyHigh.Value);
    }
}
