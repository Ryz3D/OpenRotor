using UnityEngine;
using UnityEngine.UI;

class TouchJoystick : MonoBehaviour {
    public float minX, maxX;
    public float radius;
    public string axisX, axisY;
    public bool resetX, resetY;
    public float fadeSpeed;

    private bool stickActive;
    private Vector2 startPos;
    private Vector2 stickDelta;
    private int lastTouchCount;
    private RectTransform rectTransform;
    private Image imgParent;
    private RectTransform stickTransform;
    private Image imgStick;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        imgParent = GetComponent<Image>();
        stickTransform = transform.GetChild(0).GetComponent<RectTransform>();
        imgStick = transform.GetChild(0).GetComponent<Image>();
    }

    void Update() {
        if (stickActive) {
            bool touchFound = false;
            foreach (Touch t in Input.touches) {
                if (t.position.x >= Screen.currentResolution.width * minX && t.position.x <= Screen.currentResolution.width * maxX) {
                    stickDelta = (t.position - startPos) / radius;
                    if (stickDelta.magnitude > 1) {
                        stickDelta.Normalize();
                    }

                    touchFound = true;
                }
            }
            if (!touchFound) {
                stickActive = false;
                if (resetX) {
                    stickDelta.x = 0.0f;
                }
                if (resetY) {
                    stickDelta.y = 0.0f;
                }
            }
        }
        else if (Input.touchCount > lastTouchCount) {
            foreach (Touch t in Input.touches) {
                if (t.position.x >= Screen.currentResolution.width * minX && t.position.x <= Screen.currentResolution.width * maxX) {
                    startPos = t.position;
                    stickActive = true;
                    rectTransform.position = startPos;
                }
            }
        }

        imgParent.color = imgStick.color = Color.Lerp(imgParent.color, stickActive ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0), Time.deltaTime * fadeSpeed);

        stickTransform.position = startPos + stickDelta * radius;
        StaticInputData.SetAxis(axisX, stickDelta.x);
        StaticInputData.SetAxis(axisY, stickDelta.y);

        lastTouchCount = Input.touchCount;
    }
}
