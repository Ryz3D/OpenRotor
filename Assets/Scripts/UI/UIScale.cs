using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIBehaviour {
	None,
	RelativeHeight,
	RelativeWidth
}

public class UIScale : MonoBehaviour {
	public UIBehaviour behaviour;

	private RectTransform rect;

	private Vector2 initSize;
	private float aspect;

	void Start() {
		rect = GetComponent<RectTransform>();
		initSize = rect.sizeDelta;
		aspect = rect.rect.width / rect.rect.height;
	}

	void Update() {
		switch (behaviour) {
			case UIBehaviour.None:
				break;
			case UIBehaviour.RelativeHeight:
				rect.sizeDelta = new Vector2(rect.rect.height * aspect, rect.sizeDelta.y);
				break;
			case UIBehaviour.RelativeWidth:
				break;
		}
	}
}
