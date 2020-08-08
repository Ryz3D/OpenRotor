using System.Collections.Generic;

class StaticInputData {
    public static Dictionary<string, float> data = new Dictionary<string, float>();

    public static float GetAxis(string axis) {
        if (!data.ContainsKey(axis)) {
            UnityEngine.Debug.LogWarning("Could not find static input '" + axis + "'");
            return 0.0f;
        }
        else {
            return data[axis];
        }
    }

    public static void SetAxis(string axis, float value) {
        if (!data.ContainsKey(axis)) {
            data.Add(axis, value);
        }
        else {
            data[axis] = value;
        }
    }
}
