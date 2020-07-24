using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour {

    public GameObject PausePanel;
    public AudioSource audioSource;
    public Dropdown graphicsQuality;
    public Slider resolScale;
    public Slider overallVolume;
    public GameObject applyButton;
    bool warnChanges = false;
    public GameObject ChangeWarnPanel;
    public GameObject QuitWarnPanel;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Apply()
    {
        QualitySettings.SetQualityLevel(graphicsQuality.value);
        QualitySettings.resolutionScalingFixedDPIFactor = 0.5f + resolScale.normalizedValue * 0.5f;
        AudioListener.volume = overallVolume.normalizedValue;
        warnChanges = false;
    }

    public void ClickSound()
    {
        audioSource.Play();
    }

    public void OnChanged()
    {
        warnChanges = true;
    }

    public void PressedResume()
    {
        if (warnChanges) ChangeWarnPanel.SetActive(true);
        else Resume();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        ChangeWarnPanel.SetActive(false);
        QuitWarnPanel.SetActive(false);
        PausePanel.SetActive(false);
    }

    public void ToPauseMenu()
    {
        ChangeWarnPanel.SetActive(false);
        QuitWarnPanel.SetActive(false);
    }

    public void ToMainMenu()
    {

    }
}
