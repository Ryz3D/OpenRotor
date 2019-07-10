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
	public float defaultRotDrag;
	public float idleRotDrag;
	public float propwashFactor;
	public float hoverPropwash;

	public ForceMode forceMode;
	public ForceMode torqueMode;

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
			propwash *= vel2d.magnitude + hoverPropwash;
		}
		quadMesh.localEulerAngles = new Vector3(
			Random.Range(-1.0f, 1.0f),
			Random.Range(-1.0f, 1.0f),
			Random.Range(-1.0f, 1.0f)
		) * propwash;

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
