using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class QuadLoader : MonoBehaviour, Serializable {
    private Quad quad;

    void Awake() {
        quad = GetComponent<Quad>();
        if (quad == null) {
            quad = GetComponentInChildren<Quad>();
        }

        Deserialize(Serialize());
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
        quad.idleThrottle = ReadValue(xml, "idleThrottle");
        quad.prRate = ReadValue(xml, "prRate");
        quad.prExpo = ReadValue(xml, "prExpo");
        quad.yRate = ReadValue(xml, "yRate");
        quad.yExpo = ReadValue(xml, "yExpo");
        quad.thrust = ReadValue(xml, "thrust");
        quad.Cd = ReadValue(xml, "Cd");
        quad.areaTop = ReadValue(xml, "areaTop");
        quad.areaFront = ReadValue(xml, "areaFront");
        quad.rotDrag = ReadValue(xml, "rotDrag");
        quad.propwashFactor = ReadValue(xml, "propwashFactor");
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
            WriteValue("idleThrottle", quad.idleThrottle),
            WriteValue("prRate", quad.prRate),
            WriteValue("prExpo", quad.prExpo),
            WriteValue("yRate", quad.yRate),
            WriteValue("yExpo", quad.yExpo),
            WriteValue("thrust", quad.thrust),
            WriteValue("Cd", quad.Cd),
            WriteValue("areaTop", quad.areaTop),
            WriteValue("areaFront", quad.areaFront),
            WriteValue("rotDrag", quad.rotDrag),
            WriteValue("propwashFactor", quad.propwashFactor)
        );
    }
}
