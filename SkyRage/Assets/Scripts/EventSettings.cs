using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSettings : MonoBehaviour {

    public int currentObjective = 0;
    public Objective[] objectives;
    


    public Transform player;

    public UISettings uiSettings;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        if (PlayerPrefs.HasKey("Objective"))
        {
            currentObjective = PlayerPrefs.GetInt("Objective");
            LoadObjective(currentObjective);
        }
        objectives[0].Activate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Finished()
    {
        PlayerPrefs.SetInt("Objective", 0);
        uiSettings.OnFinish();
    }

    public void NextObjective()
    {
        Debug.Log("Next Objective");
        currentObjective += 1;
        if (currentObjective < objectives.Length)
        {
            objectives[currentObjective].gameObject.SetActive(true);
            objectives[currentObjective].Activate();
        }
        else
        {
            Finished();
        }
    }
    

    void LoadObjective(int index)
    {
        for(int i = 0; i < objectives.Length; i++)
        {
            if (i < index)
            {
                objectives[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetDesc(string desc)
    {
        uiSettings.SetObDescription(desc);
    }
}
