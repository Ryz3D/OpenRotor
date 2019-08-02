using UnityEngine;

public class PIDProfileDebug : MonoBehaviour {
    public float rollP, rollI, rollD;
    public float pitchP, pitchI, pitchD;
    public float yawP, yawI, yawD;

    private Quad quad;

    void Start() {
        quad = GetComponent<Quad>();
        quad.pidProfile = new PIDProfile();
    }

    void Update() {
        if (quad == null) {
            return;
        }
        if (quad.pidProfile == null) {
            quad.pidProfile = new PIDProfile();
        }

        quad.pidProfile.rollP = rollP;
        quad.pidProfile.rollI = rollI;
        quad.pidProfile.rollD = rollD;

        quad.pidProfile.pitchP = pitchP;
        quad.pidProfile.pitchI = pitchI;
        quad.pidProfile.pitchD = pitchD;

        quad.pidProfile.yawP = yawP;
        quad.pidProfile.yawI = yawI;
        quad.pidProfile.yawD = yawD;
    }
}
