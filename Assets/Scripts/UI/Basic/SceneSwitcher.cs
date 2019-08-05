using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public void SwitchLast() {
        SceneManager.LoadScene(SceneParam.lastScene);
    }

    public void SwitchString(string str) {
        SceneManager.LoadScene(str, LoadSceneMode.Single);
    }

    public void AddString(string str) {
        SceneManager.LoadScene(str, LoadSceneMode.Additive);
    }

    public void UnloadString(string str) {
        SceneManager.UnloadSceneAsync(str);
    }
}
