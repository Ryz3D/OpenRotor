using UnityEngine;

public class RateProfileDebug : MonoBehaviour {
    public float rollRC, rollExpo, rollSuper;
    public float pitchRC, pitchExpo, pitchSuper;
    public float yawRC, yawExpo, yawSuper;

    private Quad quad;

    void Start() {
        quad = GetComponent<Quad>();
        quad.rateProfile = new RateProfile();
    }

    void Update() {
        if (quad == null) {
            return;
        }
        if (quad.rateProfile == null) {
            quad.rateProfile = new RateProfile();
        }

        quad.rateProfile.rollRC = rollRC;
        quad.rateProfile.rollExpo = rollExpo;
        quad.rateProfile.rollSuper = rollSuper;

        quad.rateProfile.pitchRC = pitchRC;
        quad.rateProfile.pitchExpo = pitchExpo;
        quad.rateProfile.pitchSuper = pitchSuper;

        quad.rateProfile.yawRC = yawRC;
        quad.rateProfile.yawExpo = yawExpo;
        quad.rateProfile.yawSuper = yawSuper;
    }
}
