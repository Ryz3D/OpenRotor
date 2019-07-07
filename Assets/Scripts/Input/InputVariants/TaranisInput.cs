using UnityEngine;

class TaranisInput : RCInput {
    public override float GetAxisFOV() {
        return 0.0f;
    }

    public override float GetAxisTilt() {
        return 0.0f;
    }

    public override float GetAxisPitch() {
        return Input.GetAxisRaw("axis2");
    }

    public override float GetAxisRoll() {
        return Input.GetAxisRaw("axis1");
    }

    public override float GetAxisThrottle() {
        return Input.GetAxisRaw("axis0");
    }

    public override float GetAxisYaw() {
        return Input.GetAxisRaw("axis3");
    }

    public override bool GetBtnExit() {
        return false;
    }

    public override bool GetBtnFlip() {
        return Input.GetAxisRaw("axis4") < 0;
    }

    public override bool GetBtnReset() {
        return Input.GetAxisRaw("axis5") > 0;
    }

    public override bool GetBtnSubmit() {
        return false;
    }
}
