using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public void SwitchString(string str) {
        SceneManager.LoadScene(str, LoadSceneMode.Single);
    }

    public void SwitchInt(int i) {
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }
}
