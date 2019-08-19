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
            MeshCollider collider = child.GetComponent<MeshCollider>();
            MeshFilter filter = child.GetComponent<MeshFilter>();
            MeshRenderer renderer = child.GetComponent<MeshRenderer>();
            TextureMarker texMarker = child.GetComponent<TextureMarker>();

            LevelElement e = new LevelElement();
            e.name = child.name;
            e.position = child.transform.localPosition;
            e.rotation = child.transform.localRotation;
            e.scale = child.transform.localScale;
            e.collider = collider == null ? ColliderType.None : ColliderType.Mesh;
            if (filter != null) {
                e.mesh = filter.mesh;
                e.materials = new List<LevelMat>();
                for (int m = 0; m < renderer.materials.Length; m++) {
                    LevelMat mat = new LevelMat();
                    mat.shader = renderer.materials[m].shader.name;
                    mat.color = renderer.materials[m].GetColor("_Color");
                    if (texMarker != null) {
                        mat.texture = texMarker.textures[m];
                    }
                    e.materials.Add(mat);
                }
            }
            lvl.elements.Add(e);
        }
        XElement xml = lvl.Serialize();
        XDocument doc = new XDocument(xml);
        string path = ConfigManager.basePath + "level\\" + lvlName + ".xml";
        doc.Save(path);

        Level reload = new Level();
        reload.Deserialize(xml);
        reload.LoadLevel();
        gameObject.SetActive(false);
    }
}
