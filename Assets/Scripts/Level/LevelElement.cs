using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System;

public enum ColliderType {
    Unkown,
    None,
    Mesh
}

public class LevelElement : Serializable {
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public ColliderType collider;
    public Mesh mesh;
    public List<LevelMat> materials;
    public LevelElementAnimation animation;

    public LevelElement() {
        name = "";
        position = Vector3.zero;
        rotation = Quaternion.identity;
        scale = Vector3.one;
        collider = ColliderType.Unkown;
        mesh = new Mesh();
        materials = new List<LevelMat>();
    }

    public void Load(GameObject parent) {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        go.transform.localPosition = position;
        go.transform.localRotation = rotation;
        go.transform.localScale = scale;
        go.layer = LayerMask.NameToLayer("Terrain");

        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        filter.mesh = mesh;
        for (int i = 0; i < materials.Count; i++) {
            materials[i].BuildMaterial(renderer);
        }

        switch (collider) {
            case ColliderType.Mesh:
                go.AddComponent<MeshCollider>();
                break;
            default:
                break;
        }

        if (animation != null) {
            go.AddComponent<LevelElementAnimator>().animCurves = animation.animCurves;
        }
    }

    private XElement SerializeValue(string key, string value) {
        return new XElement(
            key,
            new XAttribute(
                "value",
                value
            )
        );
    }

    private string VecStr(Vector3 vec) {
        return string.Format("{0},{1},{2}", FloatParser.ftos(vec.x), FloatParser.ftos(vec.y), FloatParser.ftos(vec.z));
    }

    private string QuatStr(Quaternion quat) {
        return string.Format("{0},{1},{2},{3}", FloatParser.ftos(quat.x), FloatParser.ftos(quat.y), FloatParser.ftos(quat.z), FloatParser.ftos(quat.w));
    }

    private XElement MeshXml(Mesh mesh) {
        string vValue = "";
        foreach (Vector3 v in mesh.vertices) {
            vValue += VecStr(v) + " ";
        }
        string tValue = "";
        foreach (int t in mesh.triangles) {
            tValue += t + " ";
        }
        string uValue = "";
        foreach (Vector2 u in mesh.uv) {
            uValue += VecStr(u) + " ";
        }
        string nValue = "";
        foreach (Vector3 n in mesh.normals) {
            nValue += VecStr(n) + " ";
        }

        return new XElement(
            "mesh",
            SerializeValue("v", vValue),
            SerializeValue("t", tValue),
            SerializeValue("u", uValue),
            SerializeValue("n", nValue)
        );
    }

    public XElement Serialize() {
        List<XElement> xMat = new List<XElement>();
        foreach (LevelMat mat in materials) {
            xMat.Add(mat.Serialize());
        }
        List<XElement> content = new List<XElement>() {
            SerializeValue("name", name),
            SerializeValue("pos", VecStr(position)),
            SerializeValue("rot", QuatStr(rotation)),
            SerializeValue("scl", VecStr(scale)),
            SerializeValue("collider", collider.ToString()),
            MeshXml(mesh),
            new XElement(
                "mat",
                xMat
            )
        };
        if (animation != null) {
            content.Add(animation.Serialize());
        }
        return new XElement(
            "lel", // lel = level element
            content.ToArray()
        );
    }

    private string DeserializeValue(XElement xml, string key) {
        XElement e = xml.Element(key);
        if (e == null) {
            Debug.LogError("LevelElement.DeserializeValue failed, i didn't find '" + key + "'");
            return "";
        }
        return e.Attribute("value").Value;
    }

    private Vector3 StrVec(string str) {
        string[] split = str.Split(',');
        if (split.Length < 2 || split.Length > 3) {
            Debug.LogError("LevelElement.StrVec value count: '" + str + "'");
            return Vector3.zero;
        }
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < split.Length; i++) {
            try {
                vec[i] = FloatParser.stof(split[i]);
            }
            catch (FormatException) {
                Debug.LogError("LevelElement.StrVec just received proper bullshit: '" + str + "'");
                return vec;
            }
        }
        return vec;
    }

    private Quaternion StrQuat(string str) {
        string[] split = str.Split(',');
        if (split.Length != 4) {
            Debug.LogError("LevelElement.StrQuat value count: '" + str + "'");
            return Quaternion.identity;
        }
        Quaternion quat = Quaternion.identity;
        for (int i = 0; i < split.Length; i++) {
            try {
                quat[i] = FloatParser.stof(split[i]);
            }
            catch (FormatException) {
                Debug.LogError("LevelElement.StrQuat just received proper bullshit: '" + str + "'");
                return quat;
            }
        }
        return quat;
    }

    private Mesh XmlMesh(XElement xml) {
        string vValue = DeserializeValue(xml, "v");
        string[] vSplit = vValue.Split(' ');
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < vSplit.Length; i++) {
            if (vSplit[i] != "") {
                vertices.Add(StrVec(vSplit[i]));
            }
        }

        string tValue = DeserializeValue(xml, "t");
        string[] tSplit = tValue.Split(' ');
        List<int> triangles = new List<int>();
        for (int i = 0; i < tSplit.Length; i++) {
            try {
                if (tSplit[i] != "") {
                    triangles.Add(int.Parse(tSplit[i]));
                }
            }
            catch (FormatException) {
                Debug.LogError("LevelElement.XmlMesh broken tri [" + i +  "]: '" + tSplit[i] + "'");
            }
        }

        string uValue = DeserializeValue(xml, "u");
        string[] uSplit = uValue.Split(' ');
        List<Vector2> uv = new List<Vector2>();
        for (int i = 0; i < uSplit.Length; i++) {
            if (uSplit[i] != "") {
                uv.Add(StrVec(uSplit[i]));
            }
        }

        string nValue = DeserializeValue(xml, "n");
        string[] nSplit = nValue.Split(' ');
        List<Vector3> normals = new List<Vector3>();
        for (int i = 0; i < nSplit.Length; i++) {
            if (nSplit[i] != "") {
                normals.Add(StrVec(nSplit[i]));
            }
        }

        return new Mesh() {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uv.ToArray(),
            normals = normals.ToArray()
        };
    }

    public void Deserialize(XElement xml) {
        name = DeserializeValue(xml, "name");
        position = StrVec(DeserializeValue(xml, "pos"));
        rotation = StrQuat(DeserializeValue(xml, "rot"));
        scale = StrVec(DeserializeValue(xml, "scl"));
        collider = (ColliderType)Enum.Parse(typeof(ColliderType), DeserializeValue(xml, "collider"));
        mesh = XmlMesh(xml.Element("mesh"));
        List<XElement> xMat = xml.Element("mat").Elements().ToList();
        if (materials == null) {
            materials = new List<LevelMat>();
        }
        materials.Clear();
        foreach (XElement x in xMat) {
            materials.Add(new LevelMat());
            materials.Last().Deserialize(x);
        }
        if (xml.Element("anim") != null) {
            animation = new LevelElementAnimation();
            animation.Deserialize(xml.Element("anim"));
        }
    }
}
