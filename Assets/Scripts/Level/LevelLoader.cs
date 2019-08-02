using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class LevelLoader : MonoBehaviour {
    public Material testMaterial;

    public Level level;

    void Awake() {
        if (SceneParam.selectedLevel == "") {
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
                level.LoadLevel(testMaterial);
                LevelElement spawn = level.elements.Find(l => l.name == "spawn");
                if (spawn != null) {
                    GameObject.FindGameObjectWithTag("copter").transform.position = spawn.position;
                    GameObject.FindGameObjectWithTag("copter").transform.rotation = spawn.rotation;
                }
                Debug.Log("level loaded: '" + level.name + "' (" + SceneParam.selectedLevel + ")");
            }
        }
    }
}
