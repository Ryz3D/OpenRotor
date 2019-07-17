using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;

public class Level : Serializable {
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
    }
}
