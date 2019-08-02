using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputListBuilder : MonoBehaviour {
    public float normalMargin = 0.0f;
    public float hoverMargin = 0.1f;
    public float yOffset = 0.2f;
    public float height = 30;
    public Sprite btnSprite;
    public Font font;
    public Color colorActive;
    public Color colorInactive;

    public void Rebuild() {
        List<string> paths = new List<string>();
        List<string> inputNames = new List<string>();
        foreach (string p in StaticDataAccess.config.fs.ListFiles(ConfigManager.basePath + "input")) {
            if (p.EndsWith(".xml")) {
                paths.Add(p);
                string file = p.Split('\\').Last();
                inputNames.Add(file.Substring(0, file.Length - 4));
            }
        }

        RectTransform ownRect = GetComponent<RectTransform>();
        ownRect.sizeDelta = new Vector2(ownRect.sizeDelta.x, height * inputNames.Count);
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < inputNames.Count; i++) {
            // this will probably need some cleanup

            GameObject go = new GameObject("uiElement");
            RectTransform rect = go.AddComponent<RectTransform>();
            Image img = go.AddComponent<Image>();
            Button btn = go.AddComponent<Button>();
            EventTrigger trigger = go.AddComponent<EventTrigger>();
            ButtonExtend btnExtend = go.AddComponent<ButtonExtend>();

            GameObject goText = new GameObject("uiElement");
            RectTransform rectText = goText.AddComponent<RectTransform>();
            Text text = goText.AddComponent<Text>();

            go.transform.parent = transform;
            goText.transform.parent = go.transform;

            rect.anchorMin = new Vector2(normalMargin, 1.0f - yOffset);
            rect.anchorMax = new Vector2(1.0f - normalMargin, 1.0f - yOffset);
            rect.sizeDelta = new Vector2(0.0f, height);
            rect.anchoredPosition = new Vector2(0.0f, -i * height);

            rectText.anchorMin = new Vector2(0.0f, 0.0f);
            rectText.anchorMax = new Vector2(1.0f, 1.0f);
            rectText.sizeDelta = Vector2.zero;
            rectText.anchoredPosition = Vector2.zero;

            img.sprite = btnSprite;
            img.type = Image.Type.Sliced;
            img.color = PlayerPrefs.GetString("inputConfig") == paths[i] ? colorActive : colorInactive;
            btn.onClick = new Button.ButtonClickedEvent();
            // i will be incremented at onClick call
            string pathBuf = paths[i];
            btn.onClick.AddListener(() => {
                PlayerPrefs.SetString("inputConfig", pathBuf);
                SceneManager.UnloadSceneAsync(gameObject.scene);
            });

            btnExtend.speed = 10;
            btnExtend.retractedMin = new Vector2(normalMargin, 1.0f - yOffset);
            btnExtend.retractedMax = new Vector2(1.0f - normalMargin, 1.0f - yOffset);
            btnExtend.extendedMin = new Vector2(hoverMargin, 1.0f - yOffset);
            btnExtend.extendedMax = new Vector2(1.0f - hoverMargin, 1.0f - yOffset);

            EventTrigger.Entry onEnter = new EventTrigger.Entry();
            onEnter.eventID = EventTriggerType.PointerEnter;
            onEnter.callback.AddListener((eventData) => btnExtend.OnFocus());
            trigger.triggers.Add(onEnter);
            EventTrigger.Entry onExit = new EventTrigger.Entry();
            onExit.eventID = EventTriggerType.PointerExit;
            onExit.callback.AddListener((eventData) => btnExtend.OnFocusLost());
            trigger.triggers.Add(onExit);

            text.font = font;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = inputNames[i];
        }
    }

    void Start() {
        Rebuild();
    }

    void Update() {

    }
}
