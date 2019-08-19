using System.Xml.Linq;
using UnityEngine;

public class LevelMat : Serializable {
    public string shader = "Standard";
    public Color color;
    public string texture;

    public Material BuildMaterial() {
        Material mat = new Material(Shader.Find(shader));
        mat.SetColor("_Color", color);
        Texture2D tex = (Texture2D)Resources.Load("Textures/Level/" + texture);
        mat.SetTexture("_MainTex", tex);
        return mat;
    }

    public void Deserialize(XElement xml) {
        shader = xml.Attribute("shader").Value;
        color.r = float.Parse(xml.Attribute("colorR").Value);
        color.g = float.Parse(xml.Attribute("colorG").Value);
        color.b = float.Parse(xml.Attribute("colorB").Value);
        texture = xml.Attribute("texture").Value;
    }

    public XElement Serialize() {
        return new XElement(
            "mat",
            new XAttribute(
                "shader",
                shader
            ),
            new XAttribute(
                "colorR",
                color.r
            ),
            new XAttribute(
                "colorG",
                color.g
            ),
            new XAttribute(
                "colorB",
                color.b
            ),
            new XAttribute(
                "texture",
                texture
            )
        );
    }
}
