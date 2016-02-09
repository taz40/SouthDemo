using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject options;
	public GameObject advancedOptions;
	public Text ResolutionText;
	public Text FullscreenText;
	public Text VSyncText;
	public Text PresetText;
    int ResolutionIndex = 0;
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
		vSync = QualitySettings.vSyncCount >= 1;
		QualitySettings.vSyncCount = vSync ? 1 : 0;
		updateButtons ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void Preset(){
		if (QualitySettings.GetQualityLevel () < QualitySettings.names.Length-1) {
			QualitySettings.IncreaseLevel (true);
		} else {
			QualitySettings.SetQualityLevel(0);
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
	public void AdvancedOptions(){
		options.SetActive (false);
		advancedOptions.SetActive (true);
	}

	public void updateButtons(){
		VSyncText.text = "VSync: " + (vSync ? "On" : "Off");
		Resolution res = Screen.resolutions[ResolutionIndex];
		ResolutionText.text = "Resolution: " + res.width + "x" + res.height + "@" + res.refreshRate + "Hz";
		int Index = QualitySettings.GetQualityLevel ();
		PresetText.text = "Preset: " + QualitySettings.names [Index];
		FullscreenText.text = "Fullscreen: " + (fullscreen ? "On" : "Off");
	}

}
