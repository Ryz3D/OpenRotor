using UnityEngine;

public abstract class RCInput {
    public abstract float GetAxisThrottle();
    public abstract float GetAxisYaw();
    public abstract float GetAxisPitch();
    public abstract float GetAxisRoll();

    public abstract float GetAxisTilt();
    public abstract float GetAxisFOV();

    public abstract bool GetBtnExit();
    public abstract bool GetBtnSubmit();
    public abstract bool GetBtnReset();
    public abstract bool GetBtnFlip();
}
