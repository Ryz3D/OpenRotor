using UnityEngine;
using UnityEngine.UI;

public class FPSText : MonoBehaviour {
    public int precision;

    private Text text;
    private float factor;

    void Start() {
        text = GetComponent<Text>();
        factor = Mathf.Pow(10, precision);
    }

    void Update() {
        text.text = (Mathf.Round(factor * 1.0f / Time.deltaTime) / factor).ToString() + "fps";
    }
}
