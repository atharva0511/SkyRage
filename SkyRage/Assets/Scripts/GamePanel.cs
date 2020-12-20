using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour {

    public MainMenu menu;
    public GameObject GameMenu;
    public GameObject SinglePlayerPanel;
    public GameObject SPMenu;
    public GameObject CustmPanel;
    public GameObject errorPanel;
    public GameObject[] UpgradePanel;
    public GameObject[] LevelPanel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    public void PressCustomize()
    {
        Stats.data[10] += 1;
        FinishPanel.UnlockAchievement(GPGSIds.achievement_designer);
        FinishPanel.IncrementAchievement(GPGSIds.achievement_looks_matter, 1);
        FinishPanel.IncrementAchievement(GPGSIds.achievement_i_wanna_look_good, 1);
        SinglePlayerPanel.SetActive(false);
        GameMenu.SetActive(false);
        CustmPanel.SetActive(true);
    }

    public void PressPlay()
    {
        // use switch for different vehicles;
        LevelPanel[menu.selectionIndex].SetActive(true);
        GameMenu.SetActive(false);
        SinglePlayerPanel.SetActive(false);
    }

    public void PressUpgrade()
    {
        UpgradePanel[menu.selectionIndex].SetActive(true);
        GameMenu.SetActive(false);
    }

    public void BackToPlayPanel()
    {
        SinglePlayerPanel.SetActive(true);
        UpgradePanel[menu.selectionIndex].SetActive(false);
        LevelPanel[menu.selectionIndex].SetActive(false);
        GameMenu.SetActive(true);
    }

    public void SaveAndReturn()
    {
        Upgrades.Save();
        PlayerData.SaveData();
        menu.podium.GetChild(menu.selectionIndex).GetComponent<WeaponManager>().SetPossessions();
        BackToPlayPanel();
    }

    public void ShowError()
    {
        errorPanel.SetActive(true);
    }

    public void ResetGameData()
    {
        Upgrades.Reset();
        PlayerData.ResetData();
    }

    public void UnlockLevels()
    {
        for(int i = 0; i < PlayerData.levelProgression.Length; i++)
        {
            PlayerData.levelProgression[i] = 5;
        }
    }
}
