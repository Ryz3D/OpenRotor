using UnityEngine;
using UnityEngine.UI;

public class OSDSpeed : OSDElement {
    private RectTransform rect;
    private Text text;
    private Rigidbody rb;

    protected override void Start() {
        rect = gameObject.GetComponent<RectTransform>();
        text = gameObject.AddComponent<Text>();
        GameObject[] copters = GameObject.FindGameObjectsWithTag("copter");
        if (copters.Length > 0) {
            rb = copters[0].GetComponent<Rigidbody>();
        }

        rect.anchorMin = new Vector2(0.02f, 0.9f);
        rect.anchorMax = new Vector2(0.2f, 1.0f);
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
        if (rb != null) {
            text.text = "SPD  " + Mathf.Round(rb.velocity.magnitude * 3.6f).ToString() + "km/h";
        }
    }
}
