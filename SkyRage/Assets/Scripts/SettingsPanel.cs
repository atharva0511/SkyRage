
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsPanel : MonoBehaviour {

    public GameObject SPPanel;
    public Dropdown GQSetting;
    public Slider Volume;
    public Slider Music;
    public Slider turnSens;
    public bool credits = false;
    public AudioMixer mixer;
    public EventSettings eventSettings = null;
	// Use this for initialization
	void Start () {
        if (credits) return;
        LoadSettings();
	}
	
    void OnEnable()
    {
        LoadSettings();
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
        PlayerPrefs.SetInt("MuVo", (int)Music.value);
        PlayerPrefs.SetFloat("TurnSens", turnSens.value);
        PlayerPrefs.Save();
    }

    public void OnVolumeChanged()
    {
        AudioListener.volume = Volume.normalizedValue;
    }

    public void OnMusicChanged()
    {
        mixer.SetFloat("AmbientVolume",Music.value);
    }

    public void LoadSettings()
    {
        if(PlayerPrefs.HasKey("GQ")) GQSetting.value = PlayerPrefs.GetInt("GQ");
        if (PlayerPrefs.HasKey("Vo")) Volume.normalizedValue = PlayerPrefs.GetFloat("Vo");
        if (PlayerPrefs.HasKey("MuVo")) Music.value = PlayerPrefs.GetInt("MuVo");
        if (PlayerPrefs.HasKey("TurnSens")) turnSens.value = PlayerPrefs.GetFloat("TurnSens");
    }

    public void Apply()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQ"));
        AudioListener.volume = PlayerPrefs.GetFloat("Vo");
        mixer.SetFloat("AmbientVolume", PlayerPrefs.GetInt("MuVo"));
        if(eventSettings!=null)
            eventSettings.SetTurnSens(PlayerPrefs.GetFloat("TurnSens"));
    }
}
