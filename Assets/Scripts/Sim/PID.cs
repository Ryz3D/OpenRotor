using UnityEngine;

public enum PIDAxis {
    Unknown,
    Roll,
    Pitch,
    Yaw
}

public class PID {
    public float Kp, Ki, Kd;

    private float lastErr;
    private float errSum;

    public void ApplyProfile(PIDProfile profile, PIDAxis axis) {
        switch (axis) {
            case PIDAxis.Roll:
                Kp = profile.rollP;
                Ki = profile.rollI;
                Kd = profile.rollD;
                break;
            case PIDAxis.Pitch:
                Kp = profile.pitchP;
                Ki = profile.pitchI;
                Kd = profile.pitchD;
                break;
            case PIDAxis.Yaw:
                Kp = profile.yawP;
                Ki = profile.yawI;
                Kd = profile.yawD;
                break;
        }
    }

    public void Reset() {
        lastErr = 0.0f;
        errSum = 0.0f;
    }

    public float Calculate(float pv, float sp) {
        float err = sp - pv;
        errSum += err * Time.fixedDeltaTime;
        float deriv = (err - lastErr) / Time.fixedDeltaTime;
        lastErr = err;
        return Kp * err + Ki * errSum + Kd * deriv;
    }
}
