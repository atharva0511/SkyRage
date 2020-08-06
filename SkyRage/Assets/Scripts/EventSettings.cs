using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSettings : MonoBehaviour {

    public int currentObjective = 0;
    public Objective[] objectives;


    public static Transform currentPlayer;
    public Transform player;
    playerPlane character;
    public UISettings uiSettings;
	// Use this for initialization
    void Awake()
    {
        currentPlayer = player;
    }

	void Start () {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        objectives[0].Activate();
        character = player.GetComponent<playerPlane>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Finished()
    {
        uiSettings.OnFinish(character.coins);
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
