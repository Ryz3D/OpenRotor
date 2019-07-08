using UnityEngine;
using UnityEngine.UI;

public class ButtonExtend : MonoBehaviour {
    public float speed = 1.0f;
    public float retracted = 0.3f;
    public float extended = 0.0f;

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
            rect.anchorMin = new Vector2(Mathf.Lerp(retracted, extended, position), rect.anchorMin.y);
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
