using UnityEngine;
using UnityEngine.SceneManagement;

class LastSceneSetter : MonoBehaviour {
    void Awake() {
        SceneParam.lastScene = SceneManager.GetActiveScene().name;
    }
}
