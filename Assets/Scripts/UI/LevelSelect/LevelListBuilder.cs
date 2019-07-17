using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelListBuilder : MonoBehaviour {
    public float yOffset = 0.2f;
    public float height = 150;
    public Sprite btnSprite;
    public Font font;

    private ConfigDataManager config;

    public void Rebuild() {
        List<string> paths = new List<string>();
        List<string> levelNames = new List<string>();
        foreach (string p in config.fs.ListFiles(ConfigManager.basePath + "level")) {
            if (p.EndsWith(".xml")) {
                paths.Add(p);
                string file = p.Split('\\').Last();
                levelNames.Add(file.Substring(0, file.Length - 4));
            }
        }

        RectTransform parentRect = transform.parent.GetComponent<RectTransform>();
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, height * levelNames.Count);
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < levelNames.Count; i++) {
            // this will probably need some cleanup

            GameObject go = new GameObject("uiElement");
            go.transform.parent = transform;
            RectTransform rect = go.AddComponent<RectTransform>();
            Image img = go.AddComponent<Image>();
            Button btn = go.AddComponent<Button>();

            GameObject goText = new GameObject("uiElement");
            goText.transform.parent = go.transform;
            RectTransform rectText = goText.AddComponent<RectTransform>();
            Text text = goText.AddComponent<Text>();

            rect.anchorMin = new Vector2(0.0f, 1.0f - yOffset);
            rect.anchorMax = new Vector2(1.0f, 1.0f - yOffset);
            rect.sizeDelta = new Vector2(0.0f, height);
            rect.anchoredPosition = new Vector2(0.0f, -i * height);
            img.sprite = btnSprite;
            img.type = Image.Type.Sliced;
            btn.onClick = new Button.ButtonClickedEvent();
            // i will be incremented at onClick call
            string pathBuf = paths[i];
            btn.onClick.AddListener(() => {
                SceneParam.selectedLevel = pathBuf;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Sim");
            });

            rectText.anchorMin = new Vector2(0.0f, 0.0f);
            rectText.anchorMax = new Vector2(1.0f, 1.0f);
            rectText.sizeDelta = Vector2.zero;
            rectText.anchoredPosition = Vector2.zero;
            text.font = font;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = levelNames[i];
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
