using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject PlayerModePanel;
    public GameObject SingleplayerPanel;
    public GameObject MultiplayerPanel;
    public GameObject CustomizePanel;
    AudioSource UIAudio;
    public AudioClip clip1;
    public AudioClip clip2;
    public GameObject colorPanel;
    ColorPanel cpInstance;
    public Transform podium;
    public int selectionIndex = 0;
    public Customizations custms;
	// Use this for initialization
	void Start () {
        UIAudio = GetComponent<AudioSource>();
        //podium.GetChild(selectionIndex).GetComponent<Customizable>().LoadCustomizations();

    }
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.6f);
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

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GetColor1()
    {
        cpInstance.GetColor();
    }

    public void ApplyCustumization()
    {
        Customizable Cu = podium.GetChild(selectionIndex).GetComponent<Customizable>();
        Customizations custms = new Customizations(Cu.rends[0].materials[0].color,Cu.rends[0].materials[1].color);
        Cu.SaveCustomizations(custms);
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
    }
    public void ConfirmColor()
    {
        CustomizePanel.SetActive(true);
        colorPanel.SetActive(false);
    }
}
