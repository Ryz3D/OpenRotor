using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class RateProfile : Serializable {
    public float rollRC, rollExpo, rollSuper;
    public float pitchRC, pitchExpo, pitchSuper;
    public float yawRC, yawExpo, yawSuper;

    public float ApplyRoll(float input) {
        return ApplyRates(input, rollRC, rollExpo, rollSuper);
    }

    public float ApplyPitch(float input) {
        return ApplyRates(input, pitchRC, pitchExpo, pitchSuper);
    }

    public float ApplyYaw(float input) {
        return ApplyRates(input, yawRC, yawExpo, yawSuper);
    }

    private float ApplyRates(float input, float rc, float expo, float super) {
		// from https://github.com/cleanflight/cleanflight/blob/83ed5df868e428ee530059565b47a8146c7a4484/src/main/fc/fc_rc.c#L117

		rc *= 100.0f;
		expo *= 100.0f;
		super *= 100.0f;

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

		return angleRate * 3.0f;
	}

    private XElement WriteValue(string name, float value) {
        return new XElement(
            name,
            new XAttribute(
                "value",
                value
            )
        );
    }

    private float ReadValue(XElement xml, string name) {
        return float.Parse(xml.Element(name).Attribute("value").Value);
    }

    public XElement Serialize() {
        List<XElement> elements = new List<XElement>();

        elements.Add(WriteValue("rollRC", rollRC));
        elements.Add(WriteValue("rollExpo", rollExpo));
        elements.Add(WriteValue("rollSuper", rollSuper));

        elements.Add(WriteValue("pitchRC", pitchRC));
        elements.Add(WriteValue("pitchExpo", pitchExpo));
        elements.Add(WriteValue("pitchSuper", pitchSuper));

        elements.Add(WriteValue("yawRC", yawRC));
        elements.Add(WriteValue("yawExpo", yawExpo));
        elements.Add(WriteValue("yawSuper", yawSuper));

        return new XElement(
            "rateProfile",
            elements
        );
    }

    public void Deserialize(XElement xml) {
        rollRC = ReadValue(xml, "rollRC");
        rollExpo = ReadValue(xml, "rollExpo");
        rollSuper = ReadValue(xml, "rollSuper");

        pitchRC = ReadValue(xml, "pitchRC");
        pitchExpo = ReadValue(xml, "pitchExpo");
        pitchSuper = ReadValue(xml, "pitchSuper");

        yawRC = ReadValue(xml, "yawRC");
        yawExpo = ReadValue(xml, "yawExpo");
        yawSuper = ReadValue(xml, "yawSuper");
    }
}
