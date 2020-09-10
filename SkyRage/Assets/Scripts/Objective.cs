using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    public enum ObjectiveType {Collect,Checkpoints,Destroy};
    public ObjectiveType objectiveType = ObjectiveType.Collect;
    public int ObjectiveIndex = 0;
    public bool active = false;
    public int XP = 20;
    public bool completed = false;
    public GameObject[] DestroyOnCompletion;
    public GameObject[] ActivateOnCompletion;
    public string ObjectiveDescription = "Proceed to the marked location";
    public EventSettings eventSettings;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Completed()
    {
        StartCoroutine(eventSettings.player.GetComponent<playerPlane>().AddXP(XP));
        eventSettings.NextObjective();
        completed = true;
        foreach(GameObject ob in DestroyOnCompletion)
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
    
}
