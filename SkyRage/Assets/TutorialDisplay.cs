using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialDisplay : MonoBehaviour {

    public bool shown = false;
    public GameObject TutorialPanel;

	// Use this for initialization
	void Awake () {
        int i = 0;
        switch (SceneManager.GetActiveScene().name.Split('_')[1])
        {
            case "A": i = 0; break;
            case "B": i = 1; break;
            case "C": i = 2; break;
            case "D": i = 3; break;
            default: i = 0; break;
        }
        if(PlayerData.levelProgression[i] >= int.Parse(SceneManager.GetActiveScene().name.Split('_')[2]))
        {
            shown = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        if(col.GetComponentInParent<playerPlane>() != null)
        {
            ShowTutorial();
        }
    }

    public void ShowTutorial()
    {
        if (shown) return;
        TutorialPanel.SetActive(true);
        Time.timeScale = 0;
        shown = true;
    }

    public void Continue()
    {
        TutorialPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
