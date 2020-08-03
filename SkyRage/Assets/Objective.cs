using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour {

    public enum ObjectiveType {Collect,Checkpoints,Destroy};
    public ObjectiveType objectiveType = ObjectiveType.Collect;
    public int ObjectiveIndex = 0;
    public bool active = false;
    public int XP = 20;
    public bool stayOnComplete = false;
    public GameObject[] DestroyOnCompletion;
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
        eventSettings.NextObjective();
        foreach(GameObject ob in DestroyOnCompletion)
        {
            Destroy(ob);
        }
    }

    public void Activate()
    {
        eventSettings.SetDesc(ObjectiveDescription);
        
    }
    
}
