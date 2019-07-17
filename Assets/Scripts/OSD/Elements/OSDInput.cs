using UnityEngine;
using UnityEngine.UI;

public class OSDInput : OSDElement {
    private float stickSize = 0.1f;

    private GameObject left;
    private GameObject right;
    private GameObject leftStick;
    private GameObject rightStick;
    private RectTransform rectRoot;
    private RectTransform rectL;
    private RectTransform rectR;
    private RectTransform rectLStick;
    private RectTransform rectRStick;

    protected override void Start() {
        rectRoot = gameObject.GetComponent<RectTransform>();
        left = new GameObject("osdInputLeft");
        right = new GameObject("osdInputRight");
        leftStick = new GameObject("osdInputLeftStick");
        rightStick = new GameObject("osdInputRightStick");
        left.transform.parent = gameObject.transform;
        right.transform.parent = gameObject.transform;
        leftStick.transform.parent = left.transform;
        rightStick.transform.parent = right.transform;
        rectL = left.AddComponent<RectTransform>();
        rectR = right.AddComponent<RectTransform>();
        left.AddComponent<Image>().sprite = sprites[1];
        right.AddComponent<Image>().sprite = sprites[1];
        rectLStick = leftStick.AddComponent<RectTransform>();
        rectRStick = rightStick.AddComponent<RectTransform>();
        leftStick.AddComponent<Image>().sprite = sprites[0];
        rightStick.AddComponent<Image>().sprite = sprites[0];

        rectRoot.anchorMin = new Vector2(0.3f, 0.0f);
        rectRoot.anchorMax = new Vector2(0.7f, 0.2f);
        rectRoot.offsetMin = rectRoot.offsetMax = Vector2.zero;

        rectL.anchorMin = new Vector2(0.25f, 0.0f);
        rectL.anchorMax = new Vector2(0.25f, 1.0f);
        rectL.offsetMin = rectL.offsetMax = Vector2.zero;
        rectL.anchoredPosition = Vector2.zero;
        rectL.sizeDelta = new Vector2(rectL.rect.height, rectL.sizeDelta.y);

        rectR.anchorMin = new Vector2(0.75f, 0.0f);
        rectR.anchorMax = new Vector2(0.75f, 1.0f);
        rectR.offsetMin = rectR.offsetMax = Vector2.zero;
        rectR.anchoredPosition = Vector2.zero;
        rectR.sizeDelta = new Vector2(rectR.rect.height, rectL.sizeDelta.y);
    }

    public override void Update() {
        float y = StaticDataAccess.config.input.GetAxisYaw() * 0.5f + 0.5f;
        float t = StaticDataAccess.config.input.GetAxisThrottle() * 0.5f + 0.5f;
        float r = StaticDataAccess.config.input.GetAxisRoll() * 0.5f + 0.5f;
        float p = StaticDataAccess.config.input.GetAxisPitch() * 0.5f + 0.5f;

        rectLStick.anchorMin = new Vector2(y - stickSize, t - stickSize);
        rectLStick.anchorMax = new Vector2(y + stickSize, t + stickSize);
        rectLStick.offsetMin = rectLStick.offsetMax = Vector2.zero;
        rectLStick.anchoredPosition = Vector2.zero;

        rectRStick.anchorMin = new Vector2(r - stickSize, p - stickSize);
        rectRStick.anchorMax = new Vector2(r + stickSize, p + stickSize);
        rectRStick.offsetMin = rectRStick.offsetMax = Vector2.zero;
        rectRStick.anchoredPosition = Vector2.zero;
    }
}
