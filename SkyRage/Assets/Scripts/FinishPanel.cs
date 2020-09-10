﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishPanel : MonoBehaviour {

    public Text coinText;
    public Text utText;
    public Text lifeText;
    public UISettings uiSettings;
    public GameObject errorPanel;
    public GameObject rewardPanel;
    public Text rewardText;
    public Button coinAdButton;
    public Button lifeAdButton;
    public Button utAdButton;
    public bool arcade = false;
	// Use this for initialization
	void Start () {
        uiSettings.PauseAudio();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(int coins)
    {
        coinText.text = coins.ToString();
        utText.text = PlayerData.upgradeTokens.ToString();
        lifeText.text = PlayerData.lives.ToString();
    }

    public void ProceedToMenu()
    {
        PlayerPrefs.SetInt("SetMenu", 1);
        if (!arcade)
        {
            string[] mission = SceneManager.GetActiveScene().name.Split('_');
            int i = 0;
            switch (mission[1])
            {
                case "A": i = 0; break;
                case "B": i = 1; break;
                case "C": i = 2; break;
                case "D": i = 3; break;
                default: i = 0; break;
            }
            if (int.Parse(mission[2]) > PlayerData.levelProgression[i])
            {
                PlayerData.levelProgression[i] = int.Parse(mission[2]);
            }
        }
        PlayerData.coins += int.Parse(coinText.text);
        PlayerData.SaveData();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveAndRestart()
    {
        if (!arcade)
        {
            string[] mission = SceneManager.GetActiveScene().name.Split('_');
            int i = 0;
            switch (mission[1])
            {
                case "A": i = 0; break;
                case "B": i = 1; break;
                case "C": i = 2; break;
                case "D": i = 3; break;
                default: i = 0; break;
            }
            if (int.Parse(mission[2]) > PlayerData.levelProgression[i])
            {
                PlayerData.levelProgression[i] = int.Parse(mission[2]);
            }
        }
        PlayerData.coins += int.Parse(coinText.text);
        PlayerData.SaveData();
        Time.timeScale = 1;
        uiSettings.Restart();
    }

    public void RecieveReward(double amount,string type)
    {
        switch (type)
        {
            case "coins":
                coinAdButton.interactable = false;
                coinText.text = (int.Parse(coinText.text) + amount).ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received " + amount.ToString() + " rage coins";
                break;
            case "life":
                lifeAdButton.interactable = false;
                PlayerData.lives += (int)amount;
                lifeText.text = PlayerData.lives.ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received +" + amount.ToString() + " bonus life";
                break;
            case "UpgradeToken":
                utAdButton.interactable = false;
                PlayerData.upgradeTokens += (int)amount;
                utText.text = PlayerData.upgradeTokens.ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received " + amount.ToString() + " upgrade token";
                break;
        }
    }

    public void ShowError()
    {
        errorPanel.SetActive(true);
    }
}
