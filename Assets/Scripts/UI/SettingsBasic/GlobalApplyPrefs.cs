using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalApplyPrefs : MonoBehaviour {
    public void ApplyAll() {
        foreach (PrefsValue val in Resources.FindObjectsOfTypeAll(typeof(PrefsValue))) {
            if (val.gameObject.scene == gameObject.scene) {
                val.Apply();
            }
        }
    }
}
