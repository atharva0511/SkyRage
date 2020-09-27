
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour {

    public GameObject PlayerModePanel;
    public GameObject SingleplayerPanel;
    public GameObject MultiplayerPanel;
    public GameObject SPMenu;
    public GameObject SettingsPanel;
    public GameObject CreditsPanel;
    public GameObject PlayPanel;
    public GameObject LoadingPanel;
    public GameObject CustomizePanel;
    public Text levelText;
    AudioSource UIAudio;
    public AudioClip clip1;
    public AudioClip clip2;
    public GameObject colorPanel;
    ColorPanel cpInstance;
    public Transform podium;
    public int selectionIndex = 0;
    public Image loadingProgress;
    public Transform cam;
    public Transform camPos0;
    public Transform camPos1;
    public Transform camPos2;
    public Text vehName;
    public Button PlayButton;
    public GameObject LockPanel;
    public StatsPanel statsPanel;
    public Button UnlockButton;
    public AudioMixer mixer;
    Coroutine camMotion = null;
	// Use this for initialization

    void Awake()
    {
        PlayerData.LoadData();
        Upgrades.Load();
    }
	void Start () {
        Time.timeScale = 1;
        UIAudio = GetComponent<AudioSource>();
        //podium.GetChild(selectionIndex).GetComponent<Customizable>().LoadCustomizations();
        
        ApplySettings();
        if(PlayerPrefs.HasKey("vehicleIndex"))selectionIndex = PlayerPrefs.GetInt("vehicleIndex");
        if (PlayerPrefs.HasKey("SetMenu"))
        {
            if (PlayerPrefs.GetInt("SetMenu") != 0)
            {
                cam.position = camPos1.position;
                SPMenu.SetActive(false);
                PlayPanel.SetActive(true);
                PlayerPrefs.SetInt("SetMenu", 0);
                PlayerPrefs.Save();
            }
        }
        else
        {
            cam.position = camPos0.position;
            PlayPanel.SetActive(false);
            SPMenu.SetActive(true);
        }
        DisplayVehicle();
    }
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.9f);
    }

    public void PlayAudio(bool heavy = false)
    {
        UIAudio.clip = heavy ? clip2 : clip1;
        UIAudio.Play();
    }

    public void SelectSingleplayer()
    {
        PlayerModePanel.SetActive(false);
        SingleplayerPanel.SetActive(true);
    }
    public void SelectMultiplayer()
    {
        PlayerModePanel.SetActive(false);
        MultiplayerPanel.SetActive(true);
    }

    public void StartGame(string sceneName)
    {
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(LoadLevel(sceneName));
    }

    //public void GetColor1()
    //{
    //    cpInstance.GetColor();
    //}

    public void ApplyCustumization()
    {
        Customizable Cu = podium.GetChild(selectionIndex).GetComponent<Customizable>();
        Customizations custms = new Customizations(Cu.rends[0].materials[0].color,Cu.rends[0].materials[1].color);
        Cu.SaveCustomizations(custms);
        CustomizePanel.SetActive(false);
        PlayPanel.SetActive(true);
        SingleplayerPanel.SetActive(true);
    }
    
    public void SelectColor1()
    {
        colorPanel.SetActive(true);
        CustomizePanel.SetActive(false);
        cpInstance = colorPanel.GetComponent<ColorPanel>();
        cpInstance.vehicleRenderer = podium.GetChild(selectionIndex).GetComponent<Customizable>().rends[0];
        cpInstance.materialIndex = podium.GetChild(selectionIndex).GetComponent<Customizable>().priColorIndex;
    }
    public void SelectColor2()
    {
        colorPanel.SetActive(true);
        CustomizePanel.SetActive(false);
        cpInstance = colorPanel.GetComponent<ColorPanel>();
        cpInstance.vehicleRenderer = podium.GetChild(selectionIndex).GetComponent<Customizable>().rends[0];
        cpInstance.materialIndex = podium.GetChild(selectionIndex).GetComponent<Customizable>().secColorIndex;
        cpInstance.neon = podium.GetChild(selectionIndex).GetComponent<Customizable>().NeonLight;
    }
    public void ConfirmColor()
    {
        CustomizePanel.SetActive(true);
        colorPanel.SetActive(false);
    }

    public void CancelColor()
    {
        cpInstance.SetRenderer(cpInstance.initColor.r, cpInstance.initColor.g, cpInstance.initColor.b);
        CustomizePanel.SetActive(true);
        colorPanel.SetActive(false);
    }

    IEnumerator LoadLevel(string sceneName)
    {
        StartCoroutine(ChangeCamPos(camPos2));
        SingleplayerPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        loadingProgress.fillAmount = 0.05f;
        yield return new WaitForSeconds(3f);
        
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            loadingProgress.fillAmount = async.progress / 0.9f;
            yield return null;
        }
    }

    public IEnumerator ChangeCamPos(Transform camPos)
    {
        float startTime = Time.time;
        while (Time.time < startTime + 0.25f)
        {
            cam.position = Vector3.Lerp(cam.position, camPos.position, (Time.time - startTime) / 0.25f);
            yield return null;
        }
        camMotion = null;
    }

    public void ChangeVehicle(bool right)
    {
        int maxVal = podium.childCount;
        if (right)
        {
            selectionIndex += 1;
            if (selectionIndex >= maxVal) selectionIndex = 0;
        }
        else
        {
            selectionIndex -= 1;
            if (selectionIndex < 0) selectionIndex = maxVal - 1;
        }
        PlayerPrefs.SetInt("vehicleIndex", selectionIndex);
        DisplayVehicle();
    }

    public void DisplayVehicle()
    {
        PlayButton.gameObject.SetActive(PlayerData.unlockedVehicles[selectionIndex]);
        UnlockButton.gameObject.SetActive(!PlayerData.unlockedVehicles[selectionIndex]);
        UnlockButton.interactable = (PlayerData.coins>=podium.GetChild(selectionIndex).GetComponent<Customizable>().price) && int.Parse(levelText.text)>=podium.GetChild(selectionIndex).GetComponent<Customizable>().pilotLevel;
        UnlockButton.transform.GetChild(0).GetComponent<Text>().text = podium.GetChild(selectionIndex).GetComponent<Customizable>().price.ToString();
        LockPanel.SetActive(!PlayerData.unlockedVehicles[selectionIndex]);
        if(int.Parse(levelText.text) >= podium.GetChild(selectionIndex).GetComponent<Customizable>().pilotLevel)
        {
            LockPanel.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            LockPanel.transform.GetChild(0).gameObject.SetActive(true);
            LockPanel.transform.GetChild(0).GetComponent<Text>().text = "Player Level "+ podium.GetChild(selectionIndex).GetComponent<Customizable>().pilotLevel.ToString()+ "   Required";
        }
        for (int i = 0; i < podium.childCount; i++)
        {
            podium.GetChild(i).gameObject.SetActive(false);
            if (i == selectionIndex)
            {
                podium.GetChild(i).gameObject.SetActive(true);
            }
        }
        vehName.text = podium.GetChild(selectionIndex).GetComponent<Customizable>().displayName;
        podium.GetChild(selectionIndex).GetComponent<WeaponManager>().SetPossessions();
    }

    public void PurchaseVehicle()
    {
        PlayerData.coins -= podium.GetChild(selectionIndex).GetComponent<Customizable>().price;
        PlayerData.unlockedVehicles[selectionIndex] = true;
        statsPanel.Refresh();
        PlayerData.SaveData();
        DisplayVehicle();
    }

    public void ApplySettings()
    {
        if(PlayerPrefs.HasKey("GQ"))QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("GQ"));
        if (PlayerPrefs.HasKey("Vo")) AudioListener.volume = PlayerPrefs.GetFloat("Vo");
        if(PlayerPrefs.HasKey("MuVo")) mixer.SetFloat("AmbientVolume", PlayerPrefs.GetInt("MuVo"));
    }

    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        SingleplayerPanel.SetActive(false);
    }
    public void OpenCredits()
    {
        CreditsPanel.SetActive(true);
        SingleplayerPanel.SetActive(false);
    }

    public void PressedPlay()
    {
        SPMenu.SetActive(false);
        PlayPanel.SetActive(true);
        if (camMotion != null)
        {
            StopCoroutine(camMotion);
        }
        camMotion = StartCoroutine(ChangeCamPos(camPos1));
    }

    public void ToMainMenu()
    {
        SPMenu.SetActive(true);
        PlayPanel.SetActive(false);
        if (camMotion != null)
        {
            StopCoroutine(camMotion);
        }
        StartCoroutine(ChangeCamPos(camPos0));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
