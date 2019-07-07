using System.Xml.Linq;
using UnityEngine;

public class SimpleInputAxis : CustomInputAxis
{
    public string axisName;
    public bool invert;

    public float GetValue() {
        return Input.GetAxisRaw(axisName) * (invert ? -1 : 1);
    }

    public XElement Serialize() {
        return new XElement(
            "simpleAxis",
            new XAttribute(
                "axisName", axisName
            ),
            new XAttribute(
                "invert", invert ? "1" : "0"
            )
        );
    }

    public void Deserialize(XElement xml) {
        XAttribute aAxisName = xml.Attribute("axisName");
        axisName = aAxisName == null ? "" : aAxisName.Value;
        XAttribute aInvert = xml.Attribute("invert");
        invert = aInvert == null ? false : aInvert.Value == "1";
    }
}
