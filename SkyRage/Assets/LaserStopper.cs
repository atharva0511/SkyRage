using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStopper : MonoBehaviour {

    public float damage = 40;
    public Collider burnZone;
    public float timeSpan = 3;
    public GameObject[] beams;
    public AudioSource laserSound;
    float lastBurn;
    float lastDamaged = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > lastBurn + timeSpan)
        {
            lastBurn = Time.time;
            ChangeStat();
        }
	}

    public void OnTriggerStay(Collider col)
    {
        
        Destructible ob = col.GetComponentInParent<Destructible>();
        if (ob!=null)
        {
            
            if (Time.time > lastDamaged + timeSpan)
            {
                Debug.Log("damaged");
                ob.TakeDamage(60, 5);
                lastDamaged = Time.time;
            }
        }
    }

    void ChangeStat()
    {
        if (burnZone.enabled)
        {
            burnZone.enabled = false;
            foreach(GameObject beam in beams)
            {
                beam.SetActive(false);
            }
        }
        else
        {
            burnZone.enabled = true;
            foreach (GameObject beam in beams)
            {
                beam.SetActive(true);
            }
            laserSound.Play();
        }
    }
}
