using System.Collections.Generic;
using UnityEngine;

public abstract class OSDElement {
    public GameObject gameObject;
    protected List<Sprite> sprites;

    public void Build(List<Sprite> sprites) {
        this.sprites = sprites;
        GameObject canvas = GameObject.Find("canvas");
        gameObject = new GameObject("osdElement");
        gameObject.transform.parent = canvas.transform;
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        Start();
    }
    protected abstract void Start();
    public abstract void Update();
}
