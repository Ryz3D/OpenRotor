using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Quad : MonoBehaviour, Serializable {
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
		if (StaticDataAccess.config.input.GetBtnReset()) {
			transform.position = startPos;
			transform.rotation = startRot;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

			if (lipo != null) {
				lipo.ChargeTo(4.2f);
			}
		}

		if (StaticDataAccess.config.input.GetBtnFlip()) {
			RaycastHit rayHit;
			if (Physics.Raycast(transform.position, Vector3.down, out rayHit)) {
				transform.position = rayHit.point + Vector3.up * 3.0f;
			}
			else {
				transform.position += Vector3.up * 5.0f;
			}
			transform.rotation = Quaternion.identity;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		float throttle = StaticDataAccess.config.input.GetAxisThrottle();
		float yaw = StaticDataAccess.config.input.GetAxisYaw();
		float pitch = StaticDataAccess.config.input.GetAxisPitch();
		float roll = StaticDataAccess.config.input.GetAxisRoll();

		//Debug.Log(ApplyRates(roll, 1.0f, 0.0f, 0.0f));

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
            UnityEngine.Random.Range(-1.0f, 1.0f),
			UnityEngine.Random.Range(-1.0f, 1.0f),
			UnityEngine.Random.Range(-1.0f, 1.0f)
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

	private float ReadValue(XElement xml, string name) {
        XElement value = xml.Element(name);
        if (value == null) {
            Debug.LogError("couldn't find '" + name + "' in quad config");
            return 0.0f;
        }
        else {
            try {
                return float.Parse(value.Attribute("value").Value);
            }
            catch (FormatException) {
                Debug.LogError("couldn't parse '" + name + "' in quad config");
                return 0.0f;
            }
        }
    }

    public void Deserialize(XElement xml) {
        idleThrottle = ReadValue(xml, "idleThrottle");
        prRate = ReadValue(xml, "prRate");
        prExpo = ReadValue(xml, "prExpo");
        yRate = ReadValue(xml, "yRate");
        yExpo = ReadValue(xml, "yExpo");
        thrust = ReadValue(xml, "thrust");
        Cd = ReadValue(xml, "Cd");
        areaTop = ReadValue(xml, "areaTop");
        areaFront = ReadValue(xml, "areaFront");
        rotDrag = ReadValue(xml, "rotDrag");
        propwashFactor = ReadValue(xml, "propwashFactor");
    }

    private XElement WriteValue(string name, float value) {
        return new XElement(
            name,
            new XAttribute(
                "value", value.ToString()
            )
        );
    }

    public XElement Serialize() {
        List<XElement> children = new List<XElement>();
        // bloody hell clean this up
        // look into marshaling or something
        return new XElement(
            "quad",
            WriteValue("idleThrottle", idleThrottle),
            WriteValue("prRate", prRate),
            WriteValue("prExpo", prExpo),
            WriteValue("yRate", yRate),
            WriteValue("yExpo", yExpo),
            WriteValue("thrust", thrust),
            WriteValue("Cd", Cd),
            WriteValue("areaTop", areaTop),
            WriteValue("areaFront", areaFront),
            WriteValue("rotDrag", rotDrag),
            WriteValue("propwashFactor", propwashFactor)
        );
    }
}
