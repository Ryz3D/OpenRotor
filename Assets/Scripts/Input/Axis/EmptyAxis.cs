using System.Xml.Linq;

public class EmptyAxis : CustomInputAxis
{
    public float GetValue() {
        return 0.0f;
    }

    public XElement Serialize() {
        return new XElement(
            "emptyAxis"
        );
    }

    public void Deserialize(XElement xml) {
    }
}
