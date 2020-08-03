using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour {

    public int botsLeft = 0;
    public bool IsObjective = false;
    public Objective objective;
    public int zoneRadius = 100;
    public Vector3 zoneCenter;
	// Use this for initialization
	void Start () {
        CheckRemaining();
	}
	
    public void OnDrawGizmos()
    {
        zoneCenter = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(zoneCenter, zoneRadius);
    }

	// Update is called once per frame
	void Update () {
		
	}

    //called by bots On death
    public void Died()
    {
        CheckRemaining();
    }
    void CheckRemaining()
    {
        int count = 0;
        foreach(Destructible bot in GetComponentsInChildren<Destructible>())
        {
            if (!bot.dead) count++;
        }
        if (count == 0 && IsObjective)
        {
            objective.Completed();
        }
        botsLeft = count;
    }
}
