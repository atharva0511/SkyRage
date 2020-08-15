using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour {

    public int botsLeft = 0;
    public bool IsObjective = false;
    public Objective objective;
    public int zoneRadius = 100;
    public int appearDistance = 350;
    public int disappearDistance = 450;
    public GameObject Bots;
    float measureTime = 0;
    Transform player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<EventSettings>().player;
        CheckRemaining();
	}
	
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);
    }

	// Update is called once per frame
	void Update () {
        if (Time.time > measureTime + 0.5f)
        {
            measureTime = Time.time;
            float dist = (player.position - transform.position).sqrMagnitude;
            if (dist>disappearDistance*disappearDistance)
            {
                Bots.SetActive(false);
            }
            else if(dist < appearDistance*appearDistance)
            {
                Bots.SetActive(true);
            }
        }
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
