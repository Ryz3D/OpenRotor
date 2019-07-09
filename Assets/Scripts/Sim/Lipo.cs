using System;
using UnityEngine;

public class Lipo : MonoBehaviour {
    public AnimationCurve voltageCurve;
    public AnimationCurve maximumCurrentCurve;

    public bool doesDischarge;
    public bool doesSag;
    public int cellCount;
    public float capacity;
    public float internalResistance;
    public float sagRecovery;
    public float currentDelivery;
    public float cellDeviation;

    public float[] cellCapacities;
    public float expectedCurrent;
    public float actualCurrent;

    public float[] cellVoltages {
        get {
            float[] voltages = new float[cellCount];
            for (int i = 0; i < cellCount; i++) {
                float t = cellCapacities[i] / capacity;
                voltages[i] = voltageCurve.Evaluate(t);
                if (doesSag) {
                    voltages[i] -= cellVoltageSag[i];
                }
            }
            return voltages;
        }
    }
    public float totalVoltage {
        get {
            float sum = 0;
            Array.ForEach(cellVoltages, (f) => sum += f);
            return sum;
        }
    }
    public float averageVoltage {
        get {
            return totalVoltage / cellCount;
        }
    }
    public float capacityLeft {
        get {
            float sum = 0;
            Array.ForEach(cellCapacities, (f) => sum += f);
            return sum / cellCount;
        }
    }

    private float[] cellVoltageSag;

    public void ChargeTo(float cellVoltage) {
        expectedCurrent = 0.0f;
        actualCurrent = 0.0f;
        for (int i = 0; i < cellCount; i++) {
            cellCapacities[i] = capacity;
        }
        for (int i = 0; i < cellCount; i++) {
            cellVoltageSag[i] = 0;
        }
    }

    void Start() {
        cellCapacities = new float[cellCount];
        cellVoltageSag = new float[cellCount];
        ChargeTo(4.2f);
    }

    void Update() {
        actualCurrent = Mathf.Lerp(actualCurrent, expectedCurrent, currentDelivery * Time.deltaTime);

        float deliveredCurrent = 0.0f;
        for (int i = 0; i < cellCount; i++) {
            if (cellCapacities[i] > 0) {
                float maxCellCurrent = maximumCurrentCurve.Evaluate(cellCapacities[i] / capacity);
                float cellCurrent = Mathf.Min(actualCurrent, maxCellCurrent);
                deliveredCurrent += cellCurrent / cellCount;
                float voltageDrop = cellCurrent * internalResistance;
                if (voltageDrop > cellVoltageSag[i]) {
                    cellVoltageSag[i] = voltageDrop;
                }
                else {
                    cellVoltageSag[i] = Mathf.Lerp(cellVoltageSag[i], voltageDrop, sagRecovery * Time.deltaTime);
                }

                if (doesDischarge) {
                    float capacityLoss = actualCurrent * Time.deltaTime / 3600;
                    capacityLoss *= UnityEngine.Random.Range(1.0f - cellDeviation, 1.0f + cellDeviation);
                    cellCapacities[i] -= capacityLoss;
                }
            }
            else {
                cellCapacities[i] = 0;
            }
        }
        actualCurrent = deliveredCurrent;
    }
}
