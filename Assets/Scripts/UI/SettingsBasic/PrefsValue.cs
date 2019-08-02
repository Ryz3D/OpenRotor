using UnityEngine;

[RequireComponent(typeof(ValueField))]
public class PrefsValue : MonoBehaviour {
    public string key;

    private ValueField field;

    public void Apply() {
        PlayerPrefs.SetFloat(key, field.value);
    }

    void Start() {
        field = GetComponent<ValueField>();
        field.value = PlayerPrefs.GetFloat(key);
    }

    void Update() {
    }
}
