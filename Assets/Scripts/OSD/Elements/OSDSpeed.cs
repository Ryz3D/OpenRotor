using UnityEngine;
using UnityEngine.UI;

public class OSDSpeed : OSDElement {
    private RectTransform rect;
    private Text text;
    private Rigidbody rb;

    private ConfigDataManager config;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.AddComponent<Text>();
        GameObject[] copters = GameObject.FindGameObjectsWithTag("copter");
        if (copters.Length > 0) {
            rb = copters[0].GetComponent<Rigidbody>();
        }
        GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}

        rect.anchorMin = new Vector2(0.00f, 0.9f);
        rect.anchorMax = new Vector2(0.125f, 1.0f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        text.resizeTextMinSize = 15;
        text.resizeTextMaxSize = 40;
        text.fontSize = 25;
        text.alignment = TextAnchor.MiddleCenter;
        text.font = font;
        text.raycastTarget = false;
    }

    public override void Update() {
        if (rb != null) {
            text.text = Mathf.Round(rb.velocity.magnitude).ToString() + "km/h";
        }
    }
}
