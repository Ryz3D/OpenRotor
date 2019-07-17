using UnityEngine;

public class SceneParamSetter : MonoBehaviour {
    public void SetSelectType(string str) {
        SceneParam.selectType = (LevelSelectType)System.Enum.Parse(typeof(LevelSelectType), str);
    }
}
