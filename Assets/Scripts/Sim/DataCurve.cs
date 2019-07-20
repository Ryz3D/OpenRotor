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
    private bool cached;

    public DataCurve() {
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
            int upper = -1;
            for (int i = 0; i < points.Count; i++) {
                if (keys[i] > x) {
                    upper = i;
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

            cached = true;
        }

        fraction = (x - lowerBounds) / (upperBounds - lowerBounds);
        if (fraction < 0.0f || fraction > 1.0f) {
            cached = false;
            return Evaluate(x);
        }
        return Mathf.Lerp(lowerBounds, upperBounds, fraction);
    }

    public void Deserialize(XElement xml) {
        if (points == null) {
            points = new Dictionary<float, float>();
        }
        points.Clear();
        foreach (XElement elem in xml.Elements()) {
            float x = float.Parse(elem.Attribute("x").Value);
            float y = float.Parse(elem.Attribute("y").Value);
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
                    entry.Key.ToString()
                ),
                new XAttribute(
                    "y",
                    entry.Value.ToString()
                )
            ));
        }

        return new XElement(
            "dataCurve",
            dataPoints.ToArray()
        );
    }
}
