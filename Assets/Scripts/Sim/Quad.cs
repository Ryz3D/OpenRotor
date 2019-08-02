using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Quad : MonoBehaviour, Serializable {
	public float idleThrottle;
	public float thrust;
	public float Cd;
	public float areaTop;
	public float areaFront;
	public float rotDrag;
	public float propwashFactor;
	public PIDProfile pidProfile;
	public RateProfile rateProfile;

	private PID pidRoll;
	private PID pidPitch;
	private PID pidYaw;
	private Rigidbody rb;
	private Lipo lipo;
	private Powertrain powertrain;
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

		rb.maxAngularVelocity = 35;

		quadMesh = transform.GetChild(0);
		startPos = transform.position;
		startRot = transform.rotation;

		pidRoll = new PID();
		pidPitch = new PID();
		pidYaw = new PID();

		XDocument thrcur = XDocument.Parse(StaticDataAccess.config.fs.Read(ConfigManager.basePath + "/thrcur.xml"));
		XDocument curthr = XDocument.Parse(StaticDataAccess.config.fs.Read(ConfigManager.basePath + "/curthr.xml"));
		powertrain = new Powertrain();
		powertrain.throttleCurrentCurve.Deserialize(thrcur.Element("dataCurve"));
		powertrain.currentThrustCurve.Deserialize(curthr.Element("dataCurve"));
	}

	void FixedUpdate() {
		if (StaticDataAccess.config == null) {
			return;
		}
		if (StaticDataAccess.config.input == null) {
			return;
		}

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
		float roll = StaticDataAccess.config.input.GetAxisRoll();
		float pitch = StaticDataAccess.config.input.GetAxisPitch();
		float yaw = StaticDataAccess.config.input.GetAxisYaw();

		throttle = Mathf.Clamp01(throttle + idleThrottle);
		roll = rateProfile.ApplyRoll(roll) * Mathf.Deg2Rad;
		pitch = rateProfile.ApplyPitch(pitch) * Mathf.Deg2Rad;
		yaw = rateProfile.ApplyYaw(yaw) * Mathf.Deg2Rad;

		pidRoll.ApplyProfile(pidProfile, PIDAxis.Roll);
		pidPitch.ApplyProfile(pidProfile, PIDAxis.Pitch);
		pidYaw.ApplyProfile(pidProfile, PIDAxis.Yaw);

		Vector3 localRot = transform.InverseTransformVector(rb.angularVelocity);
		float rollOut = pidRoll.Calculate(-localRot.z, roll);
		float pitchOut = pidPitch.Calculate(localRot.x, pitch);
		float yawOut = pidYaw.Calculate(localRot.y, yaw);

		Vector3 force = Vector3.zero;
		Vector3 torque = Vector3.zero;
		if (lipo == null) {
			force = Vector3.up * throttle * thrust;
			torque = new Vector3(pitch, yaw, -roll);
		}
		else {
			float thrCurrent = 0.0f;
			float rotCurrent = 0.0f;
			if (powertrain == null) {
				thrCurrent = Mathf.Abs(throttle * 200);
				rotCurrent = new Vector3(pitch, yaw, -roll).magnitude * 20;
			}
			else {
				thrCurrent = powertrain.throttleCurrentCurve.Evaluate(throttle);
				// might need to tweak the scale
				rotCurrent = powertrain.throttleCurrentCurve.Evaluate(new Vector3(pitch, yaw, -roll).magnitude / 30.0f);
			}
			lipo.expectedCurrent = thrCurrent + rotCurrent;
			if (lipo.expectedCurrent > 0) {
				float thrFraction = thrCurrent / lipo.expectedCurrent;
				float rotFraction = rotCurrent / lipo.expectedCurrent;
				float power = lipo.actualCurrent * lipo.totalVoltage;

				if (powertrain == null) {
					force = 0.017f * Vector3.up * power * thrFraction;
					torque = 0.01f * new Vector3(pitchOut, yawOut, -rollOut) * power * rotFraction;
				}
				else {
					force = 0.025f * Vector3.up * power * thrFraction;
					torque = 0.07f * new Vector3(pitchOut, yawOut, -rollOut) * power * rotFraction;
				}
			}
		}

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

		float aoaSine = Vector3.Dot(transform.forward, rb.velocity.normalized);
		float drag = Mathf.Lerp(areaFront, areaTop, Mathf.Abs(aoaSine)) * rb.velocity.sqrMagnitude * Cd;
		rb.drag = drag;
		if (torque.magnitude < 0.1f) {
			rb.angularDrag = rotDrag;
		}
		else {
			rb.angularDrag = rotDrag / 5.0f / Mathf.Max(1.0f, torque.magnitude);
		}

		rb.AddRelativeForce(force);
		rb.AddRelativeTorque(torque);
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
        // bloody hell clean this up
        // look into marshaling or something
        return new XElement(
            "quad",
            WriteValue("idleThrottle", idleThrottle),
            WriteValue("thrust", thrust),
            WriteValue("Cd", Cd),
            WriteValue("areaTop", areaTop),
            WriteValue("areaFront", areaFront),
            WriteValue("rotDrag", rotDrag),
            WriteValue("propwashFactor", propwashFactor)
        );
    }
}
