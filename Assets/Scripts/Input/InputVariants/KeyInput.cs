using UnityEngine;

class KeyInput : RCInput {
    public override float GetAxisFOV() {
        return (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
    }

    public override float GetAxisTilt() {
        return (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
    }

    public override float GetAxisPitch() {
        return (Input.GetKey(KeyCode.Keypad8) ? 1 : 0) - (Input.GetKey(KeyCode.Keypad2) ? 1 : 0);
    }

    public override float GetAxisRoll() {
        return (Input.GetKey(KeyCode.Keypad6) ? 1 : 0) - (Input.GetKey(KeyCode.Keypad4) ? 1 : 0);
    }

    public override float GetAxisThrottle() {
        return (Input.GetKey("w") ? 1 : 0) - (Input.GetKey("s") ? 1 : 0);
    }

    public override float GetAxisYaw() {
        return (Input.GetKey("d") ? 1 : 0) - (Input.GetKey("a") ? 1 : 0);
    }

    public override bool GetBtnExit() {
        return Input.GetKey(KeyCode.Escape);
    }

    public override bool GetBtnFlip() {
        return Input.GetKey("f");
    }

    public override bool GetBtnReset() {
        return Input.GetKey("r");
    }

    public override bool GetBtnSubmit() {
        return Input.GetKey(KeyCode.Return);
    }
}
