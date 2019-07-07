using UnityEngine;

class GamepadInput : RCInput {
    public override float GetAxisFOV() {
        return Input.GetAxisRaw("axis5");
    }

    public override float GetAxisTilt() {
        return Input.GetAxisRaw("axis6");
    }

    public override float GetAxisPitch() {
        return -Input.GetAxisRaw("axis4");
    }

    public override float GetAxisRoll() {
        return Input.GetAxisRaw("axis3");
    }

    public override float GetAxisThrottle() {
        return -Input.GetAxisRaw("axis1");
    }

    public override float GetAxisYaw() {
        return Input.GetAxisRaw("axis0");
    }

    public override bool GetBtnExit() {
        return Input.GetButton("btn7");
    }

    public override bool GetBtnFlip() {
        return Input.GetButton("btn4");
    }

    public override bool GetBtnReset() {
        return Input.GetButton("btn5");
    }

    public override bool GetBtnSubmit() {
        return Input.GetButton("btn0");
    }
}
