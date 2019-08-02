using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildrenFontSetter : MonoBehaviour {
	public Font font;

	private void SetFontRecursive(Transform root, Font font) {
		for (int i = 0; i < root.childCount; i++) {
			Transform tf = root.GetChild(i);
			Text txt = tf.GetComponent<Text>();
			if (txt != null) {
				txt.font = font;
			}
			if (tf.childCount > 0) {
				SetFontRecursive(tf, font);
			}
		}
	}

	void Start() {
		SetFontRecursive(transform, font);
	}
}
