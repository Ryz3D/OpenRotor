using System.Collections.Generic;
using UnityEngine;

public class OSDManager : MonoBehaviour {
    public List<OSDElement> elements = new List<OSDElement>();

    void Start() {
    }

    void Update() {
        foreach (OSDElement elem in elements) {
            elem.Update();
        }
    }
}
