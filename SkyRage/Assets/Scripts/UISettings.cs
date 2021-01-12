using System.Collections;
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
    public GameObject finishPanel;
    public GameObject DeadPanel;
    public Image loadingProgress;
    public Text ObjectiveDesc;
    public GameObject DisplayObj;
    public EventSettings evSettings;
	

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
        EventSettings.inCombat = false;
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
        finishPanel.SetActive(false);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public void LeaveArcade()
    {
        Debug.Log("Mission Completed");
        Time.timeScale = 0;
        finishPanel.SetActive(true);
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
        finishPanel.SetActive(true);
        finishPanel.GetComponent<FinishPanel>().SetText(coins);
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
            DisplayObj.SetActive(true);
            // play type audio here

            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        DisplayObj.SetActive(false);
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        FinishPanel.UnlockAchievement(GPGSIds.achievement_after_life);
        
        AdManager.AppodealInterstitialStatic();
        if (evSettings.gameMode == EventSettings.GameMode.arcade)
        {
            finishPanel.SetActive(true);
        } 
        else
        {
            FinishPanel.CheckAchievements();
            DeadPanel.SetActive(true);
        }
    }

    public void PauseAudio()
    {
        foreach(AudioSource aud in evSettings.player.GetComponent<playerPlane>().pauseAudio)
        {
            aud.Pause();
        }
    }

    public void OnLoadCheckpoint()
    {
        PlayerPrefs.SetInt("LoadCheckpoint", 1);
        EventSettings.currentCoins = int.Parse(EventSettings.currentPlayer.GetComponent<playerPlane>().coinDisp.text);
        DeadPanel.SetActive(false);
        finishPanel.SetActive(false);
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }
}
