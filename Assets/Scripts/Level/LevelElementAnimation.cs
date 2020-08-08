using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;

public class LevelElementAnimation : Serializable {
    public AnimationCurve[] animCurves = new AnimationCurve[6]; // pos xyz, rot xyz

    public XElement Serialize() {
        List<XElement> keys = new List<XElement>();
        for (int i = 0; i < animCurves[0].keys.Length; i++) {
            keys.Add(new XElement(
                "key",
                new XAttribute("t", FloatParser.ftos(animCurves[0].keys[i].time)),
                new XAttribute("px", FloatParser.ftos(animCurves[0].keys[i].value)),
                new XAttribute("py", FloatParser.ftos(animCurves[1].keys[i].value)),
                new XAttribute("pz", FloatParser.ftos(animCurves[2].keys[i].value)),
                new XAttribute("rx", FloatParser.ftos(animCurves[3].keys[i].value)),
                new XAttribute("ry", FloatParser.ftos(animCurves[4].keys[i].value)),
                new XAttribute("rz", FloatParser.ftos(animCurves[5].keys[i].value))
            ));
        }
        return new XElement("anim", keys.ToArray());
    }

    public void Deserialize(XElement xml) {
        for (int i = 0; i < 6; i++) {
            animCurves[i] = new AnimationCurve();
            animCurves[i].postWrapMode = WrapMode.Loop;
        }
        foreach (XElement x in xml.Elements("key")) {
            animCurves[0].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("px").Value));
            animCurves[1].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("py").Value));
            animCurves[2].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("pz").Value));
            animCurves[3].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("rx").Value));
            animCurves[4].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("ry").Value));
            animCurves[5].AddKey(FloatParser.stof(x.Attribute("t").Value), FloatParser.stof(x.Attribute("rz").Value));
        }
    }
}