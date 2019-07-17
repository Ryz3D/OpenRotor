using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public Level level;

    void Awake() {
        if (SceneParam.selectedLevel == null) {
            Debug.LogWarning("no level :/");
        }
        else {
            XDocument doc = XDocument.Parse(StaticDataAccess.config.fs.Read(SceneParam.selectedLevel));
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
}
