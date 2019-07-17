using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    private Level level;

    private ConfigDataManager config;

    void Awake() {
        GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}
        XDocument doc = XDocument.Parse(config.fs.Read(SceneParam.selectedLevel));
        XElement elem = doc.Element("level");
        if (elem == null) {
            Debug.LogError("level seems broken '" + SceneParam.selectedLevel + "'");
        }
        else {
            if (level == null) {
                level = new Level();
            }
            else {
                level.UnloadLevel();
            }
            level.Deserialize(elem);
        }
    }
}
