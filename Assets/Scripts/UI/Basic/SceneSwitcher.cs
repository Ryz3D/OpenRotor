using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public void SwitchString(string str) {
        SceneManager.LoadScene(str, LoadSceneMode.Single);
    }

    public void UnloadString(string str) {
        SceneManager.UnloadSceneAsync(str);
    }
}
