using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

//TODO: TRACKS

public class Level : Serializable {
    public string name;
    public string previewPath;
    public Sprite preview;
    public List<LevelElement> elements;

    private GameObject go;

    public void LoadLevel() {
        go = new GameObject("level");
        foreach (LevelElement e in elements) {
            e.Load(go);
        }
        StaticDataAccess.level = this;
    }

    public void UnloadLevel() {
        GameObject.Destroy(go);
        StaticDataAccess.level = null;
    }

    public void LoadPreview() {
        string totalPath = ConfigManager.basePath + "level\\" + previewPath;
        if (StaticDataAccess.config.fs.WhatIs(totalPath) == FileType.File) {
            Texture2D tex = new Texture2D(768, 160);
            tex.LoadImage(StaticDataAccess.config.fs.ReadB(totalPath));
            tex.Apply();
            preview = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }
        else {
            Debug.LogWarning("Couldn't load preview for level '" + name + "' (" + totalPath + ")");
        }
    }

    public XElement Serialize() {
        XElement xName = new XElement(
            "name",
            new XAttribute(
                "value",
                name
            )
        );
        XElement xPreview = new XElement(
            "preview",
            new XAttribute(
                "value",
                previewPath
            )
        );
        List<XElement> xElements = new List<XElement>();
        if (elements != null) {
            foreach (LevelElement elem in elements) {
                xElements.Add(elem.Serialize());
            }
        }

        return new XElement(
            "level",
            xName,
            xPreview,
            xElements.ToArray()
        );
    }

    private void DeserializePreview(XElement xml) {
        previewPath = xml.Element("preview").Attribute("value").Value;
        LoadPreview();
    }

    private void DeserializeElements(XElement xml) {
        if (elements == null) {
            elements = new List<LevelElement>();
        }
        else {
            elements.Clear();
        }
        foreach (XElement elem in xml.Elements("lel")) {
            elements.Add(new LevelElement());
            elements.Last().Deserialize(elem);
        }
    }

    public void PreDeserialize(XElement xml) {
        name = xml.Element("name").Attribute("value").Value;
        DeserializePreview(xml);
    }

    public void Deserialize(XElement xml) {
        name = xml.Element("name").Attribute("value").Value;
        DeserializePreview(xml);
        DeserializeElements(xml);
    }
}
