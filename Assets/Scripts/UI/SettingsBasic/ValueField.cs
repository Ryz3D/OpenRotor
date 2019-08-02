using UnityEngine;
using UnityEngine.UI;

public class ValueField : MonoBehaviour {
    public string valueKey;
    public float min, max;
    public bool inputField;
    public int precision;
    public float value;
    public float handleSize;
    public Font font;
    public Sprite imgBackground;
    public Sprite imgHandle;
    public Color clrBg;
    public Color clrHandle;

    private float pFactor;
    private bool inpFocusLast;
    private Slider sSlider;
    private Text tValue;
    private InputField iValue;

    private void BuildLabel(RectTransform root) {
        GameObject gText = new GameObject("label");
        RectTransform rText = gText.AddComponent<RectTransform>();
        rText.SetParent(root);
        rText.anchorMin = new Vector2(0.0f, 0.0f);
        rText.anchorMax = new Vector2(0.2f, 1.0f);
        rText.anchoredPosition = Vector2.zero;
        rText.sizeDelta = Vector2.zero;

        Text tText = gText.AddComponent<Text>();
        tText.text = valueKey;
        tText.alignment = TextAnchor.MiddleRight;
        tText.font = font;
        tText.resizeTextForBestFit = true;
        tText.resizeTextMinSize = 10;
        tText.resizeTextMaxSize = 30;
        tText.fontSize = 15;
    }

    private void BuildSlider(RectTransform root) {
        GameObject gSlider = new GameObject("slider");
        RectTransform rSlider = gSlider.AddComponent<RectTransform>();
        rSlider.SetParent(root);
        rSlider.anchorMin = new Vector2(0.2f, 0.5f);
        rSlider.anchorMax = new Vector2(0.8f, 0.5f);
        rSlider.anchoredPosition = Vector2.zero;
        rSlider.sizeDelta = new Vector2(0.0f, handleSize);

        GameObject gSliderBG = new GameObject("bg");
        RectTransform rSliderBG = gSliderBG.AddComponent<RectTransform>();
        rSliderBG.SetParent(rSlider);
        rSliderBG.anchorMin = new Vector2(0.1f, 0.25f);
        rSliderBG.anchorMax = new Vector2(0.9f, 0.75f);
        rSliderBG.anchoredPosition = Vector2.zero;
        rSliderBG.sizeDelta = Vector2.zero;

        GameObject gSliderHandleArea = new GameObject("handlearea");
        RectTransform rSliderHandleArea = gSliderHandleArea.AddComponent<RectTransform>();
        rSliderHandleArea.SetParent(rSlider);
        rSliderHandleArea.anchorMin = new Vector2(0.1f, 0.0f);
        rSliderHandleArea.anchorMax = new Vector2(0.9f, 1.0f);
        rSliderHandleArea.anchoredPosition = Vector2.zero;
        rSliderHandleArea.sizeDelta = Vector2.zero;

        GameObject gSliderHandle = new GameObject("handle");
        RectTransform rSliderHandle = gSliderHandle.AddComponent<RectTransform>();
        rSliderHandle.SetParent(rSliderHandleArea);
        rSliderHandle.anchorMin = new Vector2(0.5f, 0.0f);
        rSliderHandle.anchorMax = new Vector2(0.5f, 1.0f);
        rSliderHandle.anchoredPosition = Vector2.zero;
        rSliderHandle.sizeDelta = new Vector2(handleSize, 0.0f);

        Image bSliderBG = gSliderBG.AddComponent<Image>();
        bSliderBG.sprite = imgBackground;
        bSliderBG.type = Image.Type.Sliced;
        bSliderBG.color = clrBg;

        Image bSliderHandle = gSliderHandle.AddComponent<Image>();
        bSliderHandle.sprite = imgHandle;
        bSliderHandle.color = clrHandle;

        sSlider = gSlider.AddComponent<Slider>();
        sSlider.minValue = min * pFactor;
        sSlider.maxValue = max * pFactor;
        sSlider.value = value * pFactor;
        sSlider.wholeNumbers = true;
        sSlider.targetGraphic = bSliderHandle;
        sSlider.handleRect = rSliderHandle;
    }

    private void BuildValue(RectTransform root) {
        if (inputField) {
            GameObject gInp = new GameObject("value");
            RectTransform rInp = gInp.AddComponent<RectTransform>();
            rInp.SetParent(root);
            rInp.anchorMin = new Vector2(0.8f, 0.0f);
            rInp.anchorMax = new Vector2(1.0f, 1.0f);
            rInp.anchoredPosition = Vector2.zero;
            rInp.sizeDelta = Vector2.zero;

            GameObject gText = new GameObject("value");
            RectTransform rText = gText.AddComponent<RectTransform>();
            rText.SetParent(rInp);
            rText.anchorMin = new Vector2(0.0f, 0.0f);
            rText.anchorMax = new Vector2(1.0f, 1.0f);
            rText.anchoredPosition = Vector2.zero;
            rText.sizeDelta = Vector2.zero;

            tValue = gText.AddComponent<Text>();
            tValue.alignment = TextAnchor.MiddleRight;
            tValue.font = font;
            tValue.resizeTextForBestFit = true;
            tValue.resizeTextMinSize = 10;
            tValue.resizeTextMaxSize = 30;
            tValue.fontSize = 15;

            iValue = gInp.AddComponent<InputField>();
            iValue.transition = Selectable.Transition.None;
            iValue.textComponent = tValue;
            iValue.contentType = InputField.ContentType.DecimalNumber;
            iValue.onEndEdit.AddListener((str) => {
                value = Mathf.Round(float.Parse(str) * pFactor) / pFactor;
                sSlider.value = value * pFactor;
            });
        }
        else {
            GameObject gText = new GameObject("value");
            RectTransform rText = gText.AddComponent<RectTransform>();
            rText.SetParent(root);
            rText.anchorMin = new Vector2(0.8f, 0.0f);
            rText.anchorMax = new Vector2(1.0f, 1.0f);
            rText.anchoredPosition = Vector2.zero;
            rText.sizeDelta = Vector2.zero;

            tValue = gText.AddComponent<Text>();
            tValue.alignment = TextAnchor.MiddleRight;
            tValue.font = font;
            tValue.resizeTextForBestFit = true;
            tValue.resizeTextMinSize = 10;
            tValue.resizeTextMaxSize = 30;
            tValue.fontSize = 15;
        }
    }

    public void Rebuild() {
        RectTransform root = GetComponent<RectTransform>();
        for (int i = 0; i < root.childCount; i++) {
            Destroy(root.GetChild(i));
        }

        BuildLabel(root);
        BuildSlider(root);
        BuildValue(root);
    }

    void Start() {
        pFactor = Mathf.Pow(10, precision);
        Rebuild();
    }

    void Update() {
        value = sSlider.value / pFactor;
        string str = (Mathf.Round(value * pFactor) / pFactor).ToString();
        if (tValue != null) {
            tValue.text = str;
        }
        if (iValue != null) {
            if (!iValue.isFocused && !inpFocusLast) {
                iValue.text = str;
            }
            inpFocusLast = iValue.isFocused;
        }
    }
}
