using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunPickup : MonoBehaviour {

    public AudioClip clip;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        WeaponManager wm = col.GetComponentInParent<WeaponManager>();
        if(wm != null){
            switch (wm.vehicleIndex)
            {
                case 0:Upgrades.qDroneWeapons[0] = true;break;
                case 1:Upgrades.hodWeapons[0] = true;break;
                case 2:Upgrades.wDroneWeapons[0] = true;break;
                case 3:Upgrades.slayerXWeapons[0] = true;break;
            }
            wm.GetComponent<playerPlane>().pickupAudio.clip = clip;
            wm.GetComponent<playerPlane>().pickupAudio.Play();
            wm.SetPossessions();
            Upgrades.Save();
            if (GetComponent<Objective>() != null)
            {
                GetComponent<Objective>().Completed();
            }
            Destroy(this.gameObject);
        }
    }
}
