using UnityEngine;
using UnityEngine.UI;

public class OSDDistance : OSDElement {
    private Vector3 startPos;

    private RectTransform rect;
    private Text text;
    private Transform tf;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.AddComponent<Text>();
        GameObject[] copters = GameObject.FindGameObjectsWithTag("copter");
        if (copters.Length > 0) {
            tf = copters[0].transform;
        }

        startPos = tf.position;
        rect.anchorMin = new Vector2(0.02f, 0.7f);
        rect.anchorMax = new Vector2(0.2f, 0.8f);
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
            text.text = "DST  " + (Mathf.Round(Vector3.Distance(startPos, tf.position) / 1000.0f * 100.0f) / 100.0f).ToString() + "km";
        }
    }
}
