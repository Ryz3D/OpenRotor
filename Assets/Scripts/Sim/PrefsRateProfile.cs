using UnityEngine;

public class PrefsRateProfile : MonoBehaviour {
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

        quad.rateProfile.rollRC = PlayerPrefs.GetFloat("rcRate");
        quad.rateProfile.rollExpo = PlayerPrefs.GetFloat("expoRate");
        quad.rateProfile.rollSuper = PlayerPrefs.GetFloat("superRate");

        quad.rateProfile.pitchRC = PlayerPrefs.GetFloat("rcRate");
        quad.rateProfile.pitchExpo = PlayerPrefs.GetFloat("expoRate");
        quad.rateProfile.pitchSuper = PlayerPrefs.GetFloat("superRate");

        quad.rateProfile.yawRC = PlayerPrefs.GetFloat("rcRate");
        quad.rateProfile.yawExpo = PlayerPrefs.GetFloat("expoRate");
        quad.rateProfile.yawSuper = PlayerPrefs.GetFloat("superRate");
    }
}
