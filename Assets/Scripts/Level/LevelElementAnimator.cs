using UnityEngine;

public class LevelElementAnimator : MonoBehaviour {
    public AnimationCurve[] animCurves; // pos xyz, rot xyz

    void Start() {
    }

    void Update() {
        if (animCurves != null) {
            transform.position = new Vector3(
                animCurves[0].Evaluate(Time.timeSinceLevelLoad),
                animCurves[1].Evaluate(Time.timeSinceLevelLoad),
                animCurves[2].Evaluate(Time.timeSinceLevelLoad)
            );
            transform.eulerAngles = new Vector3(
                animCurves[3].Evaluate(Time.timeSinceLevelLoad),
                animCurves[4].Evaluate(Time.timeSinceLevelLoad),
                animCurves[5].Evaluate(Time.timeSinceLevelLoad)
            );
        }
    }
}
