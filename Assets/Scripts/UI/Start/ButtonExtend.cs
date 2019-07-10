using UnityEngine;
using UnityEngine.UI;

public class ButtonExtend : MonoBehaviour {
    public float speed = 1.0f;
    public Vector2 retractedMin = new Vector2(0.3f, 0.0f);
    public Vector2 retractedMax = new Vector2(1.0f, 1.0f);
    public Vector2 extendedMin = new Vector2(0.0f, 0.0f);
    public Vector2 extendedMax = new Vector2(1.0f, 1.0f);

    private float position;
    private float target;
    private Button btn;
    private RectTransform rect;

    void Start() {
        btn = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        if (target != position) {
            position = Mathf.MoveTowards(position, target, speed * Time.deltaTime);
            rect.anchorMin = Vector2.Lerp(retractedMin, extendedMin, position);
            rect.anchorMax = Vector2.Lerp(retractedMax, extendedMax, position);
            rect.sizeDelta = new Vector2(0, rect.sizeDelta.y);
        }
    }

    public void OnFocus() {
        target = 1.0f;
    }

    public void OnFocusLost() {
        target = 0.0f;
    }
}
