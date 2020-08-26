using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : Destructible {

    public GameObject blast;
    public GameObject[] pickups;
    public Renderer[] rends;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Die()
    {
        base.Die();
        GetComponent<Collider>().enabled = false;
        foreach(Renderer rend in rends)
        {
            rend.enabled = false;
        }
        Instantiate(blast, transform.position, Quaternion.identity);
        StartCoroutine(SpawnPickups());
        Destroy(this.gameObject, 4);
    }

    public IEnumerator SpawnPickups()
    {
        Vector3[] dirs = new Vector3[pickups.Length];
        GameObject[] picks = new GameObject[pickups.Length];
        for(int i = 0; i < pickups.Length; i++)
        {
            dirs[i] = new Vector3(Random.value, Random.value, Random.value);
            dirs[i] = transform.position+20*(2*Random.value-1)*dirs[i].normalized;
            picks[i] = Instantiate(pickups[i], transform.position, Quaternion.identity);
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
}
