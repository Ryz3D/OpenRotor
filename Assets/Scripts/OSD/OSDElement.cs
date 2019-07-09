using System.Collections.Generic;
using UnityEngine;

public abstract class OSDElement {
    public GameObject gameObject;
    protected List<Sprite> sprites;
    protected Font font;

    public void Build(List<Sprite> sprites, Font font) {
        this.sprites = sprites;
        this.font = font;
        GameObject canvas = GameObject.Find("canvas");
        gameObject = new GameObject("osdElement");
        gameObject.transform.parent = canvas.transform;
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        Start();
    }
    protected abstract void Start();
    public abstract void Update();
}
