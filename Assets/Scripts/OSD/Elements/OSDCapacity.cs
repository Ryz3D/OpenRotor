using UnityEngine;
using UnityEngine.UI;

public class OSDCapacity : OSDElement {
    private RectTransform rect;
    private Text text;
    private Lipo lipo;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.AddComponent<Text>();
        GameObject[] copters = GameObject.FindGameObjectsWithTag("copter");
        if (copters.Length > 0) {
            lipo = copters[0].GetComponent<Lipo>();
            if (lipo == null) {
                lipo = copters[0].GetComponentInChildren<Lipo>();
            }
        }

        rect.anchorMin = new Vector2(0.82f, 0.6f);
        rect.anchorMax = new Vector2(1.0f, 0.7f);
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
        if (lipo != null) {
            text.text = "CAP  " + Mathf.Round((lipo.capacity - lipo.capacityLeft) * 1000.0f).ToString() + "mAh";
        }
    }
}
