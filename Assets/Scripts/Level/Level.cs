using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;

//TODO: TRACKS

public class Level : Serializable {
    public string name;

    public Sprite preview;

    public List<LevelElement> elements;

    private GameObject go;

    public void LoadLevel() {
        go = new GameObject("level");
        foreach (LevelElement e in elements) {
            e.Load(go);
        }
    }

    public void UnloadLevel() {
        GameObject.Destroy(go);
    }

    public XElement Serialize() {
        return new XElement("");
    }

    public void Deserialize(XElement xml) {
        name = xml.Element("name").Attribute("value").Value;
        string previewPath = xml.Element("preview").Attribute("value").Value;
        previewPath = ConfigManager.basePath + "level\\" + previewPath;
        Texture2D tex = new Texture2D(768, 160);
        tex.LoadImage(StaticDataAccess.config.fs.ReadB(previewPath));
        tex.Apply();
        preview = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }
}
