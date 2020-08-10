using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {

    public GameObject SPPanel;
    public Dropdown GQSetting;
    public Slider Volume;
    public AudioSource Music;
    public Toggle MenuMusic;
    public bool credits = false;
    
	// Use this for initialization
	void Start () {
        if (credits) return;
        LoadSettings();
        if(Music!=null)MenuMusic.isOn = Music.volume != 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PressBack()
    {
        SaveSettings();
        Apply();
        SPPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
    public void Cancel()
    {
        LoadSettings();
        SPPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Cancel1()
    {
        SPPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("GQ", GQSetting.value);
        PlayerPrefs.SetFloat("Vo", Volume.normalizedValue);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if(PlayerPrefs.HasKey("GQ")) GQSetting.value = PlayerPrefs.GetInt("GQ");
        if (PlayerPrefs.HasKey("Vo")) Volume.normalizedValue = PlayerPrefs.GetFloat("Vo");
    }

    public void Apply()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQ"));
        AudioListener.volume = PlayerPrefs.GetFloat("Vo");
        if(Music!=null)
        Music.volume = MenuMusic.isOn ? 1 : 0;
    }
}
