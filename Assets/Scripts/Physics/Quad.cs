using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Quad : MonoBehaviour {
	public float prRate;
	public float prExpo;
	public float yRate;
	public float yExpo;
	public float thrust;
	public float Cd;
	public float areaTop;
	public float areaFront;
	public float defaultRotDrag;
	public float idleRotDrag;

	public ForceMode forceMode;
	public ForceMode torqueMode;

	private Rigidbody rb;
	private ConfigDataManager config;
	private Vector3 startPos;
	private Quaternion startRot;

	void Start() {
		rb = GetComponent<Rigidbody>();
		GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}

		startPos = transform.position;
		startRot = transform.rotation;
	}

	void FixedUpdate() {
		if (config.input.GetBtnReset()) {
			transform.position = startPos;
			transform.rotation = startRot;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		float throttle = config.input.GetAxisThrottle();
		float yaw = config.input.GetAxisYaw();
		float pitch = config.input.GetAxisPitch();
		float roll = config.input.GetAxisRoll();

		//throttle = throttle * 0.5f + 0.5f;
		yaw = ApplyExpo(yaw, yExpo) * yRate;
		pitch = ApplyExpo(pitch, prExpo) * prRate;
		roll = ApplyExpo(roll, prExpo) * prRate;

		Vector3 force = Vector3.up * throttle * thrust;
		Vector3 torque = new Vector3(pitch, yaw, -roll);

		float aoaSine = Vector3.Dot(transform.forward, rb.velocity.normalized);
		float drag = Mathf.Lerp(areaFront, areaTop, Mathf.Abs(aoaSine)) * rb.velocity.sqrMagnitude * Cd;
		rb.drag = drag;
		if (torque.magnitude > 1.0f) {
			rb.angularDrag = defaultRotDrag / torque.sqrMagnitude;
		}
		else if (torque.magnitude < 0.1f) {
			rb.angularDrag = idleRotDrag;
		}
		else {
			rb.angularDrag = defaultRotDrag;
		}

		rb.AddRelativeForce(force, forceMode);
		rb.AddRelativeTorque(torque, torqueMode);
	}

	private float ApplyExpo(float v, float e) {
		return Mathf.Pow(v, 3) * e + v - v * e;
	}
}
