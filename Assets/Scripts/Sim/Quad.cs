using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Quad : MonoBehaviour {
	public float idleThrottle;
	public float prRate;
	public float prExpo;
	public float yRate;
	public float yExpo;
	public float thrust;
	public float Cd;
	public float areaTop;
	public float areaFront;
	public float rotDrag;
	public float propwashFactor;

	private Rigidbody rb;
	private Lipo lipo;
	//private Powertrain powertrain;
	private ConfigDataManager config;
	private Transform quadMesh;
	private Vector3 startPos;
	private Quaternion startRot;
	private Vector2 propwashTorque;

	void Start() {
		rb = GetComponent<Rigidbody>();
		lipo = GetComponent<Lipo>();
		if (lipo == null) {
			lipo = GetComponentInChildren<Lipo>();
		}
		GameObject go = GameObject.Find("dataManager");
		if (go == null) {
			Debug.LogError("FATAL: dataManager object not found!");
		}
		else {
			config = go.GetComponent<ConfigDataManager>();
			config.Reload();
		}

		quadMesh = transform.GetChild(0);
		startPos = transform.position;
		startRot = transform.rotation;
	}

	float ApplyRates(float input, float rc, float expo, float super) {
		// from https://github.com/cleanflight/cleanflight/blob/83ed5df868e428ee530059565b47a8146c7a4484/src/main/fc/fc_rc.c#L117

		float rcRateIncremental = 14.54f;
		if (expo > 0.0f) {
			float expof = expo / 100.0f;
			input = input * Mathf.Pow(Mathf.Abs(input), 3) * expof + input * (1 - expof);
		}

		float rcRate = rc / 100.0f;
		if (rcRate > 2.0f) {
			rcRate += rcRateIncremental * (rcRate - 2.0f);
		}
		float angleRate = 200.0f * rcRate * input;
		if (super > 0.0f) {
			float rcSuperfactor = 1.0f / (Mathf.Clamp(1.0f - (Mathf.Abs(input) * (super / 100.0f)), 0.01f, 1.00f));
			angleRate *= rcSuperfactor;
		}

		Debug.Log(angleRate);

		return angleRate;
	}

	void FixedUpdate() {
		if (config.input.GetBtnReset()) {
			transform.position = startPos;
			transform.rotation = startRot;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

			if (lipo != null) {
				lipo.ChargeTo(4.2f);
			}
		}

		float throttle = config.input.GetAxisThrottle();
		float yaw = config.input.GetAxisYaw();
		float pitch = config.input.GetAxisPitch();
		float roll = config.input.GetAxisRoll();

		Debug.Log(ApplyRates(roll, 1.0f, 0.0f, 0.0f));

		throttle += idleThrottle;
		yaw = ApplyExpo(yaw, yExpo) * yRate;
		pitch = ApplyExpo(pitch, prExpo) * prRate;
		roll = ApplyExpo(roll, prExpo) * prRate;

		Vector3 force = Vector3.zero;
		Vector3 torque = Vector3.zero;
		if (lipo == null) {
			force = Vector3.up * throttle * thrust;
			torque = new Vector3(pitch, yaw, -roll);
		}
		else {
			float thrCurrent = Mathf.Abs(throttle * thrust * 4); // powertrain eval
			float rotCurrent = new Vector3(pitch, yaw, -roll).magnitude * 20; // powertrain eval
			lipo.expectedCurrent = thrCurrent + rotCurrent;
			if (lipo.expectedCurrent > 0) {
				float thrFraction = thrCurrent / lipo.expectedCurrent;
				float rotFraction = rotCurrent / lipo.expectedCurrent;
				float power = lipo.actualCurrent * lipo.totalVoltage;

				force = 0.017f * Vector3.up * power * thrFraction; // powertrain eval
				torque = 0.01f * new Vector3(pitch, yaw, -roll) * power * rotFraction; // powertrain eval
			}
		}

		float aoaSine = Vector3.Dot(transform.forward, rb.velocity.normalized);
		Vector2 forw2d = new Vector2(transform.forward.x, transform.forward.z);
		Vector2 vel2d = new Vector2(rb.velocity.x, rb.velocity.z);
		float propwash = force.magnitude * propwashFactor * (1 - Vector2.Dot(forw2d.normalized, vel2d.normalized)) / (0.1f * vel2d.magnitude + 1.5f);
		if (vel2d.magnitude < 2.0f) {
			// fixes too much propwash while hovering
			propwash *= vel2d.magnitude + 0.05f;
		}
		quadMesh.localEulerAngles = new Vector3(
			Random.Range(-1.0f, 1.0f),
			Random.Range(-1.0f, 1.0f),
			Random.Range(-1.0f, 1.0f)
		) * propwash;

		float drag = Mathf.Lerp(areaFront, areaTop, Mathf.Abs(aoaSine)) * rb.velocity.sqrMagnitude * Cd;
		rb.drag = drag;
		if (torque.magnitude < 0.1f) {
			rb.angularDrag = rotDrag;
		}
		else {
			rb.angularDrag = rotDrag / 2.0f / Mathf.Max(1.0f, torque.magnitude);
		}

		rb.AddRelativeForce(force);
		rb.AddRelativeTorque(torque);
	}

	private float ApplyExpo(float v, float e) {
		return Mathf.Pow(v, 3) * e + v - v * e;
	}
}
