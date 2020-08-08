using UnityEngine;

public class FloatParser {
    public static bool useComma {
        get {
            return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
        }
    }

    public static string ftos(float f) {
        return f.ToString().Replace(',', '.');
    }

    public static float stof(string s) {
        return float.Parse(useComma ? s.Replace('.', ',') : s.Replace(',', '.'));
    }
}
