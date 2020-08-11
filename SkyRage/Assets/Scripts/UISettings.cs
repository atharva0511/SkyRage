﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISettings : MonoBehaviour {

    public GameObject PausePanel;
    public AudioSource audioSource;
    public GameObject SettingsPanel;
    public GameObject warnPanel;
    public GameObject restartWarnPanel;
    public GameObject LoadingPanel;
    public GameObject FinishPanel;
    public GameObject DeadPanel;
    public Image loadingProgress;
    public Text ObjectiveDesc;
    public GameObject DisplayObj;
    public EventSettings evSettings;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public void ClickSound()
    {
        audioSource.Play();
    }


    public void PressedResume()
    {
        Resume();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        AudioSource[] pauseAudio = evSettings.player.GetComponent<playerPlane>().pauseAudio;
        foreach (AudioSource aud in pauseAudio)
        {
            aud.UnPause();
        }
        PausePanel.SetActive(false);
    }
    
    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ClickedSettings()
    {
        warnPanel.SetActive(false);
        PausePanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void ClickedQuit()
    {
        warnPanel.SetActive(true);
    }

    public void CancelQuit()
    {
        restartWarnPanel.SetActive(false);
        warnPanel.SetActive(false);
        PausePanel.SetActive(true);
    }

    public void ClickedRestart()
    {
        restartWarnPanel.SetActive(true);
    }

    public void Restart()
    {
        DeadPanel.SetActive(false);
        FinishPanel.SetActive(false);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }
    
    IEnumerator LoadLevel(string sceneName)
    {
        PausePanel.SetActive(false);
        restartWarnPanel.SetActive(false);
        LoadingPanel.SetActive(true);
        loadingProgress.fillAmount = 0.05f;
        yield return new WaitForSecondsRealtime(3f);

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            loadingProgress.fillAmount = async.progress / 0.9f;
            yield return null;
        }
    }

    public void OnFinish(int coins)
    {
        Debug.Log("Mission Completed");
        Time.timeScale = 0;
        FinishPanel.SetActive(true);
        FinishPanel.GetComponent<FinishPanel>().SetText(coins);
    }

    public void SetObDescription(string desc)
    {
        ObjectiveDesc.text = desc;
        StartCoroutine(ShowObj(desc));
    }

    //shows objective on top right
    IEnumerator ShowObj(string desc)
    {
        yield return new WaitForSeconds(1f);
        string s = " ";
        DisplayObj.SetActive(true);
        Text descDisp = DisplayObj.transform.GetChild(0).GetComponent<Text>();
        foreach(char c in desc)
        {
            s = s + c;
            descDisp.text = s;
            // play type audio here

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        DisplayObj.SetActive(false);
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        DeadPanel.SetActive(true);
    }

    public void PauseAudio()
    {
        foreach(AudioSource aud in evSettings.player.GetComponent<playerPlane>().pauseAudio)
        {
            aud.Pause();
        }
    }
}
