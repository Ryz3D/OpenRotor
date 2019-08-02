using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class DebugLevelSaver : MonoBehaviour {
    public string lvlName;
    public string previewPath;

    void Start() {
        Level lvl = new Level();
        lvl.name = lvlName;
        lvl.previewPath = previewPath;
        lvl.elements = new List<LevelElement>();
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            LevelElement e = new LevelElement();
            e.name = child.name;
            e.position = child.transform.localPosition;
            e.rotation = child.transform.localRotation;
            e.scale = child.transform.localScale;
            e.collider = child.GetComponent<MeshCollider>() == null ? ColliderType.None : ColliderType.Mesh;
            if (child.GetComponent<MeshFilter>() != null) {
                e.mesh = child.GetComponent<MeshFilter>().mesh;
                e.materials = child.GetComponent<MeshRenderer>().materials.ToList();
                e.materialNames = new List<string>();
                foreach (Material m in e.materials) {
                    e.materialNames.Add(m.name);
                }
            }
            lvl.elements.Add(e);
        }
        XElement xml = lvl.Serialize();
        XDocument doc = new XDocument(xml);
        string path = ConfigManager.basePath + "level\\" + lvlName + ".xml";
        doc.Save(path);
    }
}
