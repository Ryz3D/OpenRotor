using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour {
	public Dropdown ddnRes;
	public Toggle tglFull;
	public Dropdown ddnShader;
	public Dropdown ddnTexscale;
	public Dropdown ddnShadow;
	public Dropdown ddnLOD;

	public void Apply() {
		PlayerPrefs.SetInt("videoRes", ddnRes.value);
		PlayerPrefs.SetInt("videoFull", tglFull.isOn ? 1 : 0);
		PlayerPrefs.SetInt("videoShader", ddnShader.value);
		PlayerPrefs.SetInt("videoTex", ddnTexscale.value);
		PlayerPrefs.SetInt("videoShadow", ddnShadow.value);
		PlayerPrefs.SetInt("videoLOD", ddnLOD.value);

		Dictionary<int, Resolution> dictResolution = new Dictionary<int, Resolution>() {
			{0, new Resolution() {width = 1920, height = 1080}},
			{1, new Resolution() {width = 1280, height = 720}},
			{2, new Resolution() {width = 960, height = 540}},
			{3, new Resolution() {width = 640, height = 360}},
		};
		Resolution res = dictResolution[ddnRes.value];
		Screen.SetResolution(res.width, res.height, tglFull.isOn);

		Dictionary<int, ShadowResolution> dictShadowRes = new Dictionary<int, ShadowResolution>() {
			{0, ShadowResolution.VeryHigh},
			{1, ShadowResolution.Medium},
			{2, ShadowResolution.Low},
		};
		Dictionary<int, float> dictShadowDist = new Dictionary<int, float>() {
			{0, 100.0f},
			{1, 50.0f},
			{2, 0.0f},
		};
		Dictionary<int, ShadowQuality> dictShadowQual = new Dictionary<int, ShadowQuality>() {
			{0, ShadowQuality.All},
			{1, ShadowQuality.HardOnly},
			{2, ShadowQuality.Disable},
		};
		QualitySettings.shadowResolution = dictShadowRes[ddnShadow.value];
		QualitySettings.shadowDistance = dictShadowDist[ddnShadow.value];
		QualitySettings.shadows = dictShadowQual[ddnShadow.value];
	}

	void Start() {
		ddnRes.value = PlayerPrefs.HasKey("videoRes") ? PlayerPrefs.GetInt("videoRes") : 0;
		tglFull.isOn = PlayerPrefs.HasKey("videoFull") ? PlayerPrefs.GetInt("videoFull") == 1 : false;
		ddnShader.value = PlayerPrefs.HasKey("videoShader") ? PlayerPrefs.GetInt("videoShader") : 1;
		ddnTexscale.value = PlayerPrefs.HasKey("videoTex") ? PlayerPrefs.GetInt("videoTex") : 1;
		ddnShadow.value = PlayerPrefs.HasKey("videoShadow") ? PlayerPrefs.GetInt("videoShadow") : 1;
		ddnLOD.value = PlayerPrefs.HasKey("videoLOD") ? PlayerPrefs.GetInt("videoLOD") : 1;
	}

	void Update() {
		
	}
}
