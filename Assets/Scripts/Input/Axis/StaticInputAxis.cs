using System.Xml.Linq;
using UnityEngine;

public class StaticInputAxis : CustomInputAxis
{
    public string axisName;
    public bool invert;

    public float GetValue() {
        return StaticInputData.GetAxis(axisName) * (invert ? -1 : 1);
    }

    public XElement Serialize() {
        return new XElement(
            "staticAxis",
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
