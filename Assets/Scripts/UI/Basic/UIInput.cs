using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInput : MonoBehaviour {
	private StandaloneInputModule input;

	void Start() {
		input = GetComponent<StandaloneInputModule>();
	}

    /*
    0: Throttle
    1: Roll
    2: Pitch
    3: Yaw
    4: Tilt
    5: FoV
    6: Exit
    7: Submit
    8: Reset
    9: Flip
    */

	void Update() {
		input.cancelButton = "empty";
		input.submitButton = "empty";
		input.horizontalAxis = "empty";
		input.verticalAxis = "empty";

		if (StaticDataAccess.config == null) {
			return;
		}
		if (StaticDataAccess.config.input == null) {
			return;
		}
	
		string cancelStr = "empty";
		string submitStr = "empty";
		string horizontalStr = "empty";
		string verticalStr = "empty";

		CustomInput inp = (CustomInput)StaticDataAccess.config.input;
		CustomInputAxis cancel = inp.axis.Length > 6 ? inp.axis[6] : null;
		CustomInputAxis submit = inp.axis.Length > 7 ? inp.axis[7] : null;
		CustomInputAxis horizontal = inp.axis.Length > 1 ? inp.axis[1] : null;
		CustomInputAxis vertical = inp.axis.Length > 2 ? inp.axis[2] : null;

		if (cancel != null) {
			if (cancel is ButtonInputAxis) {
				cancelStr = ((ButtonInputAxis)cancel).btnHigh;
			}
			if (cancel is SimpleInputAxis) {
				cancelStr = ((SimpleInputAxis)cancel).axisName;
			}
		}
		if (submit != null) {
			if (submit is ButtonInputAxis) {
				submitStr = ((ButtonInputAxis)submit).btnHigh;
			}
			if (submit is SimpleInputAxis) {
				submitStr = ((SimpleInputAxis)submit).axisName;
			}
		}
		if (horizontal != null) {
			if (horizontal is SimpleInputAxis) {
				horizontalStr = ((SimpleInputAxis)horizontal).axisName;
			}
			if (vertical is SimpleInputAxis) {
				verticalStr = ((SimpleInputAxis)vertical).axisName;
			}
		}

		input.cancelButton = cancelStr;
		input.submitButton = submitStr;
		input.horizontalAxis = horizontalStr;
		input.verticalAxis = verticalStr;
	}
}
