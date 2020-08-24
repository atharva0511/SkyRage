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
    public GameObject UpgradePanel0;
    public GameObject LevelPanel0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    public void PressCustomize()
    {
        SinglePlayerPanel.SetActive(false);
        GameMenu.SetActive(false);
        CustmPanel.SetActive(true);
    }

    public void PressPlay()
    {
        // use switch for different vehicles;
        LevelPanel0.SetActive(true);
        GameMenu.SetActive(false);
        SinglePlayerPanel.SetActive(false);
    }

    public void PressUpgrade()
    {
        UpgradePanel0.SetActive(true);
        GameMenu.SetActive(false);
    }

    public void BackToPlayPanel()
    {
        SinglePlayerPanel.SetActive(true);
        UpgradePanel0.SetActive(false);
        LevelPanel0.SetActive(false);
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
}
