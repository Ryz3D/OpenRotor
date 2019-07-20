using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PIDProfile : Serializable {
    public float rollP, rollI, rollD;
    public float pitchP, pitchI, pitchD;
    public float yawP, yawI, yawD;

    private XElement WriteValue(string name, float value) {
        return new XElement(
            name,
            new XAttribute(
                "value",
                value
            )
        );
    }

    private float ReadValue(XElement xml, string name) {
        return float.Parse(xml.Element(name).Attribute("value").Value);
    }

    public XElement Serialize() {
        List<XElement> elements = new List<XElement>();

        elements.Add(WriteValue("rollP", rollP));
        elements.Add(WriteValue("rollI", rollI));
        elements.Add(WriteValue("rollD", rollD));

        elements.Add(WriteValue("pitchP", pitchP));
        elements.Add(WriteValue("pitchI", pitchI));
        elements.Add(WriteValue("pitchD", pitchD));

        elements.Add(WriteValue("yawP", yawP));
        elements.Add(WriteValue("yawI", yawI));
        elements.Add(WriteValue("yawD", yawD));

        return new XElement(
            "pidProfile",
            elements
        );
    }

    public void Deserialize(XElement xml) {
        rollP = ReadValue(xml, "rollP");
        rollI = ReadValue(xml, "rollI");
        rollD = ReadValue(xml, "rollD");

        pitchP = ReadValue(xml, "pitchP");
        pitchI = ReadValue(xml, "pitchI");
        pitchD = ReadValue(xml, "pitchD");

        yawP = ReadValue(xml, "yawP");
        yawI = ReadValue(xml, "yawI");
        yawD = ReadValue(xml, "yawD");
    }
}
