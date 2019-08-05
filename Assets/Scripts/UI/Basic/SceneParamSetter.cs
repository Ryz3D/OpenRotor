using UnityEngine;

public class SceneParamSetter : MonoBehaviour {
    public void SetLast() {
        SceneParam.lastScene = gameObject.scene.name;
    }

    public void SetSelectType(string str) {
        SceneParam.selectType = (LevelSelectType)System.Enum.Parse(typeof(LevelSelectType), str);
    }
}
