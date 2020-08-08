using System.Xml.Linq;
using UnityEngine;

public class LevelMat : Serializable {
    public string shader = "Standard";
    public Color color;
    public string texture;

    public void BuildMaterial(MeshRenderer rend) {
        rend.material = new Material(Shader.Find(shader));
        rend.material.mainTexture = (Texture2D)Resources.Load("Textures/Level/" + texture);
        rend.material.color = color;
    }

    public void Deserialize(XElement xml) {
        shader = xml.Element("shader").Attribute("value").Value;
        color.r = FloatParser.stof(xml.Element("colorR").Attribute("value").Value);
        color.g = FloatParser.stof(xml.Element("colorG").Attribute("value").Value);
        color.b = FloatParser.stof(xml.Element("colorB").Attribute("value").Value);
        color.a = 1.0f;
        texture = xml.Element("texture").Attribute("value").Value;
    }

    private XElement WriteValue(string s, string v) {
        return new XElement(
            s,
            new XAttribute("value", v)
        );
    }

    private XElement WriteValue(string s, float f) {
        return WriteValue(s, FloatParser.ftos(f));
    }

    public XElement Serialize() {
        return new XElement(
            "mat",
            WriteValue("shader", shader),
            WriteValue("colorR", color.r),
            WriteValue("colorG", color.g),
            WriteValue("colorB", color.b),
            WriteValue("texture", texture)
        );
    }
}
