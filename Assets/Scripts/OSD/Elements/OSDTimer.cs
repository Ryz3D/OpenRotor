using UnityEngine;
using UnityEngine.UI;

public class OSDTimer : OSDElement {
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

        rect.anchorMin = new Vector2(0.02f, 0.6f);
        rect.anchorMax = new Vector2(0.2f, 0.7f);
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
            float t = Time.timeSinceLevelLoad;
            int min = (int)Mathf.Floor(t / 60.0f);
            int sec = (int)Mathf.Floor(t - min * 60.0f);
            string minStr = min.ToString();
            string secStr = sec.ToString();
            while (minStr.Length < 2) {
                minStr = "0" + minStr;
            }
            while (secStr.Length < 2) {
                secStr = "0" + secStr;
            }
            text.text = "TME  " + minStr + ":" + secStr;
        }
    }
}
