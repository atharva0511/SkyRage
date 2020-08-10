
using UnityEngine;
using UnityEngine.UI;

public class LaunchLevel : MonoBehaviour {

    public MainMenu menu;
    public GameObject GameModePanel;
    public int LevelIndex = 1;
    public string LaunchScene;
    public GameObject check;
    public GameObject lockImage;

	// Use this for initialization
	void Start () {
        int i = 0;
        if (PlayerPrefs.HasKey("vehicleIndex")) i = PlayerPrefs.GetInt("vehicleIndex");
        if (LevelIndex <= PlayerData.levelProgression[i] + 1)
        {
            GetComponent<Button>().interactable = true;
            lockImage.SetActive(false);
            if (LevelIndex == PlayerData.levelProgression[i] + 1)
                check.SetActive(false);
            else
                check.SetActive(true);
        }
        else
        {
            lockImage.SetActive(true);
            GetComponent<Button>().interactable = false;
            check.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ClickedLaunch()
    {
        GameModePanel.SetActive(false);
        PlayerPrefs.Save();
        menu.StartGame(LaunchScene);
    }
}
