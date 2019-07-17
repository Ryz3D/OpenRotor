using UnityEngine;
using System.Collections.Generic;

public class LevelElement {
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Mesh mesh;
    public List<Material> materials;

    public void Load(GameObject parent) {
        GameObject go = new GameObject(name);
        go.transform.parent = parent.transform;
        go.transform.position = position;
        go.transform.rotation = rotation;
        go.transform.localScale = scale;

        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        filter.mesh = mesh;
        renderer.materials = materials.ToArray();
    }
}
