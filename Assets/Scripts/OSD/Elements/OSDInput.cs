using UnityEngine;
using UnityEngine.UI;

public class OSDInput : OSDElement {
    private RectTransform rect;
    private Image img;
    private ConfigDataManager config;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        img = gameObject.AddComponent<Image>();

        GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}
    }

    public override void Update() {
        rect.anchorMin = new Vector2(config.input.GetAxisRoll() * 0.5f + 0.5f, 0.5f);
        rect.anchorMax = new Vector2(config.input.GetAxisRoll() * 0.5f + 0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(30, 30);
    }
}
