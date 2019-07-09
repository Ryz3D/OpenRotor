using UnityEngine;
using UnityEngine.UI;

public class OSDAltitude : OSDElement {
    private Transform tf;

    private RectTransform rect;
    private Text text;
    private Rigidbody rb;

    private ConfigDataManager config;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.AddComponent<Text>();
        GameObject[] copters = GameObject.FindGameObjectsWithTag("copter");
        if (copters.Length > 0) {
            tf = copters[0].transform;
        }
        GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}

        rect.anchorMin = new Vector2(0.02f, 0.8f);
        rect.anchorMax = new Vector2(0.2f, 0.9f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        text.resizeTextMinSize = 15;
        text.resizeTextMaxSize = 40;
        text.fontSize = 25;
        text.alignment = TextAnchor.MiddleLeft;
        text.font = font;
        text.raycastTarget = false;
    }

    public override void Update() {
        if (tf != null) {
            text.text = "ALT  " + (Mathf.Round(tf.position.y * 10.0f) / 10.0f).ToString() + "m";
        }
    }
}
