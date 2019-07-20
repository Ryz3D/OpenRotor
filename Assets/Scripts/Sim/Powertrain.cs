using UnityEngine;
using System.Collections.Generic;

public class Powertrain : MonoBehaviour {
    public List<float> XThrottleYCurrent;
    public List<float> XThrottleYThrust;

    public float getCurrent(float throttle) {
        int index = (int)(throttle * XThrottleYCurrent.Count);
        return 0.0f;
    }

    public float getThrust(float throttle) {
        int index = (int)(throttle * XThrottleYCurrent.Count);
        return 0.0f;
    }
}
