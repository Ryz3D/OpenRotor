using System.Xml.Linq;
using UnityEngine;

public class ButtonInputAxis : CustomInputAxis
{
    public string btnLow;
    public string btnHigh;

    public float GetValue() {
        if (btnLow == "")
            return Input.GetButton(btnHigh) ? 1 : 0;
        else
            return (Input.GetButton(btnHigh) ? 1 : 0) - (Input.GetButton(btnLow) ? 1 : 0);
    }

    public XElement Serialize() {
        return new XElement(
            "buttonAxis",
            new XAttribute(
                "btnLow", btnLow
            ),
            new XAttribute(
                "btnHigh", btnHigh
            )
        );
    }

    public void Deserialize(XElement xml) {
        XAttribute aBtnLow = xml.Attribute("btnLow");
        btnLow = aBtnLow == null ? "" : aBtnLow.Value;
        XAttribute aBtnHigh = xml.Attribute("btnHigh");
        btnHigh = aBtnHigh == null ? "" : aBtnHigh.Value;
    }
}
