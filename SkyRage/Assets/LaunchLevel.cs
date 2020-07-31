using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchLevel : MonoBehaviour {

    public MainMenu menu;
    public GameObject GameModePanel;
    public string LaunchScene;
    public GameObject check;
    public GameObject lockImage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickedLaunch()
    {
        GameModePanel.SetActive(false);
        menu.StartGame(LaunchScene);
    }
}
