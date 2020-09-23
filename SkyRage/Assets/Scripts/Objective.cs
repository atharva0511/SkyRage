using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    public enum ObjectiveType { Collect, Checkpoints, Destroy };
    public ObjectiveType objectiveType = ObjectiveType.Collect;
    public int ObjectiveIndex = 0;
    public bool active = false;
    int XP = 10;
    public float xpMultiplier = 1;
    public bool completed = false;
    public GameObject[] DestroyOnCompletion;
    public GameObject[] ActivateOnCompletion;
    public string ObjectiveDescription = "Proceed to the marked location";
    public EventSettings eventSettings;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Completed()
    {
        if (completed) return;
        XP = (int)(xpMultiplier*(10 + 5 * EvaluateLevel(PlayerData.xp)));
        StartCoroutine(eventSettings.player.GetComponent<playerPlane>().AddXP(XP));
        eventSettings.NextObjective();
        completed = true;
        foreach (GameObject ob in DestroyOnCompletion)
        {
            Destroy(ob);
        }
        foreach (GameObject ob in ActivateOnCompletion)
        {
            ob.SetActive(true);
        }
    }

    public void Activate()
    {
        eventSettings.SetDesc(ObjectiveDescription);
    }

    int EvaluateLevel(double xp)
    {
        int level = 0;
        for (int i = 0; i < StatsPanel.xpProgress.Length; i++)
        {
            if (xp >= StatsPanel.xpProgress[i])
            {
                level = i;
            }
        }
        return level;
    }
}
