using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputListBuilder : MonoBehaviour {
    public float normalMargin = 0.0f;
    public float hoverMargin = 0.1f;
    public float height = 30;
    public Sprite btnSprite;
    public Font font;

    private ConfigDataManager config;

    public void Rebuild() {
        List<string> paths = config.fs.ListFiles(ConfigManager.basePath + "input"); // input\\default.xml
        List<string> inputNames = new List<string>();
        foreach (string p in paths) {
            if (p.EndsWith(".xml")) {
                string file = p.Split('\\').Last();
                inputNames.Add(file.Substring(0, file.Length - 4));
            }
        }

        for (int i = 0; i < inputNames.Count; i++) {
            // this will probably need some cleanup

            GameObject go = new GameObject("uiElement");
            go.transform.parent = transform;
            RectTransform rect = go.AddComponent<RectTransform>();
            Image img = go.AddComponent<Image>();
            Button btn = go.AddComponent<Button>();
            ButtonExtend btnExtend = go.AddComponent<ButtonExtend>();
            EventTrigger trigger = go.AddComponent<EventTrigger>();

            GameObject goText = new GameObject("uiElement");
            goText.transform.parent = go.transform;
            RectTransform rectText = goText.AddComponent<RectTransform>();
            Text text = goText.AddComponent<Text>();

            rect.anchorMin = new Vector2(normalMargin, 1.0f);
            rect.anchorMax = new Vector2(1.0f - normalMargin, 1.0f);
            rect.sizeDelta = new Vector2(0.0f, height);
            rect.anchoredPosition = new Vector2(0.0f, -i * height);
            img.sprite = btnSprite;
            img.type = Image.Type.Sliced;
            btn.onClick = new Button.ButtonClickedEvent();
            // i will be incremented at onClick call
            string pathBuf = paths[i];
            btn.onClick.AddListener(() => {
                PlayerPrefs.SetString("inputConfig", pathBuf);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Sim");
            });
            btnExtend.speed = 10;
            btnExtend.retractedMin = new Vector2(normalMargin, 1.0f);
            btnExtend.retractedMax = new Vector2(1.0f - normalMargin, 1.0f);
            btnExtend.extendedMin = new Vector2(hoverMargin, 1.0f);
            btnExtend.extendedMax = new Vector2(1.0f - hoverMargin, 1.0f);
            EventTrigger.Entry onEnter = new EventTrigger.Entry();
            onEnter.eventID = EventTriggerType.PointerEnter;
            onEnter.callback.AddListener((eventData) => btnExtend.OnFocus());
            trigger.triggers.Add(onEnter);
            EventTrigger.Entry onExit = new EventTrigger.Entry();
            onExit.eventID = EventTriggerType.PointerExit;
            onExit.callback.AddListener((eventData) => btnExtend.OnFocusLost());
            trigger.triggers.Add(onExit);

            rectText.anchorMin = new Vector2(0.0f, 0.0f);
            rectText.anchorMax = new Vector2(1.0f, 1.0f);
            rectText.sizeDelta = Vector2.zero;
            rectText.anchoredPosition = Vector2.zero;
            text.font = font;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = inputNames[i];
        }
    }

    void Start() {
        GameObject go = GameObject.Find("dataManager");
        if (go == null) {
            Debug.LogError("FATAL: dataManager object not found!");
        }
        else {
            config = go.GetComponent<ConfigDataManager>();
            config.Reload();
        }
        Rebuild();
    }

    void Update() {

    }
}
