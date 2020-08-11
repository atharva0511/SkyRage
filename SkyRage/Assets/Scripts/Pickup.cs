using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public enum pickupType { health,coin,life,ammo,upgradeToken,Objective,Nitro};
    public pickupType type;
    public bool isObjective = false;
    public int health = 0;
    public int coins = 0;
    public int rockets = 0;
    public int homing = 0;

    public bool rotate = false;
    public Vector3 rotateBy = Vector3.zero;
    public AudioClip collectSound;
	// Use this for initialization
	void Start () {
        if (type == pickupType.coin)
        {
            switch (PlayerPrefs.GetInt("Difficulty"))
            {
                case 1: coins = Random.Range(10, 15);break;
                case 2: coins = Random.Range(13, 18); break;
                case 3: coins = Random.Range(16, 22); break;
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!rotate) return;
        transform.Rotate(rotateBy);
	}

    //called when pickup type is objective
    public void Collect()
    {
        GetComponent<Objective>().Completed();
    }
}
