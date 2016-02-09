using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdvancedOptionsMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject options;
	public Text ResolutionText;
	public Text FullscreenText;
	public Text VSyncText;
	public Text PresetText;
	public Text TextureScale;
	int ResolutionIndex = 0;
	int MasterTextureLimit;
	bool fullscreen;
	bool vSync;
	// Use this for initialization
	void Start () {
		ResolutionIndex = 0;
		for (int i = 0; i < Screen.resolutions.Length; i++){
			if (Screen.currentResolution.width == Screen.resolutions[i].width && Screen.currentResolution.height == Screen.resolutions[i].height) {
				ResolutionIndex = i;
			}
		}
		fullscreen = Screen.fullScreen;
		updateButtons ();
		QualitySettings.vSyncCount = vSync ? 1 : 0;
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void Preset(){
		if (QualitySettings.GetQualityLevel () < QualitySettings.names.Length-1) {
			QualitySettings.IncreaseLevel (true);
		} else {
			QualitySettings.SetQualityLevel(0, true);
		}
		updateButtons ();
	}
	public void VSync(){
		vSync = !vSync;
		QualitySettings.vSyncCount = vSync ? 1 : 0;
		updateButtons ();
	}
	public void Fullscreen(){
		fullscreen = !fullscreen;
		Resolution res = Screen.resolutions[ResolutionIndex];
		Screen.SetResolution (res.width, res.height, fullscreen, res.refreshRate);
		updateButtons ();
	}
	public void changeResolution() {
		ResolutionIndex++;
		if (ResolutionIndex >= Screen.resolutions.Length) {
			ResolutionIndex = 0;
		}
		Resolution res = Screen.resolutions[ResolutionIndex];
		Screen.SetResolution(res.width, res.height, Screen.fullScreen, res.refreshRate);
		updateButtons ();
	}
	public void BackButton(){
		options.SetActive (false);
		mainMenu.SetActive (true);
	}

	public void TextureScaleButton(){
		MasterTextureLimit++;
		if (MasterTextureLimit > 2) {
			MasterTextureLimit=0; 
		}
		QualitySettings.masterTextureLimit = MasterTextureLimit;
		updateButtons (); 
	}
	
	public void updateButtons(){
		MasterTextureLimit = QualitySettings.masterTextureLimit;
		string TextureSizeName = "High";
		if (MasterTextureLimit == 1) {
			TextureSizeName = "Medium";
		} else if (MasterTextureLimit == 2) {
			TextureSizeName = "Low";
		}
		TextureScale.text = "Texture Scale: "+ TextureSizeName;
		vSync = QualitySettings.vSyncCount >= 1;
		VSyncText.text = "VSync: " + (vSync ? "On" : "Off");
		Resolution res = Screen.currentResolution;
		ResolutionText.text = "Resolution: " + res.width + "x" + res.height + "@" + res.refreshRate + "Hz";
		int Index = QualitySettings.GetQualityLevel ();
		PresetText.text = "Preset: " + QualitySettings.names [Index];
		FullscreenText.text = "Fullscreen: " + (fullscreen ? "On" : "Off");
	}
}
