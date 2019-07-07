using UnityEngine;

public abstract class OSDElement {
    public GameObject gameObject;

    public void Build() {
        GameObject canvas = GameObject.Find("canvas");
        gameObject = new GameObject("osdElement");
        gameObject.transform.parent = canvas.transform;
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        Start();
    }
    protected abstract void Start();
    public abstract void Update();
}
