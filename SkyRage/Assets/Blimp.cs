using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : Destructible {

    public bool isBalloon = false;
    public GameObject flare;
    public GameObject blast;
    public GameObject[] pickups;
    public Renderer[] rends;
	// Use this for initialization
	void Start () {
        
        if (isBalloon)
        {
            Color[] colors = new Color[] { Color.red, Color.yellow, Color.blue, Color.green };
            Color col = colors[Random.Range(0, 4)];
            foreach (Renderer rend in rends)
            {
                rend.material.color = col;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        if(isBalloon)
            Instantiate(flare, transform.position, Quaternion.identity);
    }

    public override void Die()
    {
        base.Die();
        GetComponent<Collider>().enabled = false;
        foreach(Renderer rend in rends)
        {
            rend.enabled = false;
        }
        if (isBalloon)
        {
            if (GetComponentInParent<BotSpawner>() != null)
            {
                GetComponentInParent<BotSpawner>().Died();
            }
        }
        Instantiate(blast, transform.position, Quaternion.identity);
        StartCoroutine(SpawnPickups());
        GetComponent<Rigidbody>().Sleep();
        GetComponent<Rigidbody>().isKinematic = true;
        //Destroy(this.gameObject, 4);
    }

    public IEnumerator SpawnPickups()
    {
        PlayerPrefs.SetInt("LoadCheckpoint", 0);
        Vector3[] dirs = new Vector3[pickups.Length];
        GameObject[] picks = new GameObject[pickups.Length];
        for(int i = 0; i < pickups.Length; i++)
        {
            dirs[i] = new Vector3(Random.value, Random.value, Random.value);
            dirs[i] = transform.position+20*(2*Random.value-1)*dirs[i].normalized;
            picks[i] = Instantiate(pickups[i], transform.position, Quaternion.identity,this.transform);
        }

        float startTime = Time.time;
        while (Time.time < startTime + 2)
        {
            for(int i = 0; i < pickups.Length; i++)
            {
                if(picks[i]!=null)
                    picks[i].transform.position = Vector3.Lerp(picks[i].transform.position, dirs[i], (Time.time - startTime) / 2);
            }
            yield return null;
        }
    }

    public override void DeathBlow(Transform hitter, WeaponName weapon)
    {
        if (isBalloon) return;
        if (EventSettings.currentPlayer == null) return;
        if (hitter == EventSettings.currentPlayer)
        {
            Stats.data[19] += 1;
        }
    }
}
