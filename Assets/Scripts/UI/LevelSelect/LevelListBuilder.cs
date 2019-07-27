using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelListBuilder : MonoBehaviour {
    public float yOffset = 0.2f;
    public float height = 150;
    public float margin = 10;
    public Sprite btnSprite;
    public Font font;

    public void Rebuild() {
        List<string> paths = new List<string>();
        List<Level> levels = new List<Level>();
        foreach (string p in StaticDataAccess.config.fs.ListFiles(ConfigManager.basePath + "level")) {
            if (p.EndsWith(".xml")) {
                paths.Add(p);

                XDocument doc = XDocument.Parse(StaticDataAccess.config.fs.Read(p));
                XElement elem = doc.Element("level");
                if (elem == null) {
                    Debug.LogError("level seems broken '" + p + "'");
                }

                levels.Add(new Level());
                levels.Last().PreDeserialize(elem);
            }
        }

        RectTransform ownRect = GetComponent<RectTransform>();
        ownRect.sizeDelta = new Vector2(ownRect.sizeDelta.x, (height + margin) * levels.Count);
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < levels.Count; i++) {
            // this will probably need some cleanup

            GameObject go = new GameObject("uiElement");
            RectTransform rect = go.AddComponent<RectTransform>();
            Image bg = go.AddComponent<Image>();
            Button btn = go.AddComponent<Button>();

            GameObject goImg = new GameObject("uiElement");
            RectTransform rectImg = goImg.AddComponent<RectTransform>();
            Image img = goImg.AddComponent<Image>();

            GameObject goText = new GameObject("uiElement");
            RectTransform rectText = goText.AddComponent<RectTransform>();
            Text text = goText.AddComponent<Text>();

            go.transform.parent = transform;
            goImg.transform.parent = go.transform;
            goText.transform.parent = go.transform;

            rect.anchorMin = new Vector2(0.0f, 1.0f - yOffset);
            rect.anchorMax = new Vector2(1.0f, 1.0f - yOffset);
            rect.sizeDelta = new Vector2(0.0f, height);
            rect.anchoredPosition = new Vector2(0.0f, -i * (height + margin));

            rectImg.anchorMin = new Vector2(0.0f, 0.2f);
            rectImg.anchorMax = new Vector2(1.0f, 1.0f);
            rectImg.sizeDelta = Vector2.zero;
            rectImg.anchoredPosition = Vector2.zero;

            rectText.anchorMin = new Vector2(0.0f, 0.0f);
            rectText.anchorMax = new Vector2(1.0f, 0.2f);
            rectText.sizeDelta = Vector2.zero;
            rectText.anchoredPosition = Vector2.zero;

            bg.color = Color.white;
            img.sprite = levels[i].preview;
            btn.onClick = new Button.ButtonClickedEvent();
            // i will be incremented at onClick call
            string pathBuf = paths[i];
            btn.onClick.AddListener(() => {
                SceneParam.selectedLevel = pathBuf;
                switch (SceneParam.selectType) {
                    case LevelSelectType.Freestyle:
                    case LevelSelectType.Race:
                    case LevelSelectType.Custom:
                    case LevelSelectType.Test:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Sim");
                        break;
                    case LevelSelectType.Leaderboard: // TODO: ADD LEADERBOARD SCENE
                    case LevelSelectType.Unknown:
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
                        break;
                }
            });

            text.font = font;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = levels[i].name;
        }
    }

    void Start() {
        Rebuild();
    }

    void Update() {

    }
}
