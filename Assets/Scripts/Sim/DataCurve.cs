using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class DataCurve : Serializable {
    public Dictionary<float, float> points;

    private float fraction;
    private float lowerBounds;
    private float upperBounds;
    private float lowerValue;
    private float upperValue;
    private bool cached;

    public DataCurve() {
        points = new Dictionary<float, float>();
        cached = false;
    }

    public float Evaluate(float x) {
        // check performance

        if (x <= lowerBounds || x >= upperBounds || !cached) {
            float preciseout;
            if (points.TryGetValue(x, out preciseout)) {
                return preciseout;
            }

            float[] keys = new float[points.Count];
            points.Keys.CopyTo(keys, 0);
            float[] values = new float[points.Count];
            points.Values.CopyTo(values, 0);
            int upper = -1;
            for (int i = 0; i < points.Count; i++) {
                if (keys[i] > x) {
                    upper = i;
                    break;
                }
            }
            if (upper == -1) {
                float top;
                if (points.TryGetValue(keys.Last(), out top)) {
                    return top;
                }
                else {
                    return 0;
                }
            }
            lowerBounds = keys[upper - 1];
            upperBounds = keys[upper];
            lowerValue = values[upper - 1];
            upperValue = values[upper];
            cached = true;
        }

        fraction = (x - lowerBounds) / (upperBounds - lowerBounds);
        if (fraction < 0.0f || fraction > 1.0f) {
            cached = false;
            return Evaluate(x);
        }
        return Mathf.Lerp(lowerValue, upperValue, fraction);
    }

    public void Deserialize(XElement xml) {
        if (points == null) {
            points = new Dictionary<float, float>();
        }
        points.Clear();
        foreach (XElement elem in xml.Elements()) {
            float x = FloatParser.stof(elem.Attribute("x").Value);
            float y = FloatParser.stof(elem.Attribute("y").Value);
            points.Add(x, y);
        }
    }

    public XElement Serialize() {
        List<XElement> dataPoints = new List<XElement>();
        foreach (KeyValuePair<float, float> entry in points) {
            dataPoints.Add(new XElement(
                "dataPoint",
                new XAttribute(
                    "x",
                    FloatParser.ftos(entry.Key)
                ),
                new XAttribute(
                    "y",
                    FloatParser.ftos(entry.Value)
                )
            ));
        }

        return new XElement(
            "dataCurve",
            dataPoints.ToArray()
        );
    }
}
