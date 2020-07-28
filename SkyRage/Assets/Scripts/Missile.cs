using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

	public ParticleSystem emission;
	public Rigidbody rb;
	public float missileTimer = 15;
	public float speed = 150;
	public float turnRate = 15;
	bool launched = false;
	Transform target;
	public float accl= 25;
	public Transform testTarget;



	// Use this for initialization
	public void Start () {

		LaunchMissile(testTarget);




		
	}
	
	// Update is called once per frame
	void Update () {

		if (launched) 
        {
			this.transform.rotation = Quaternion.LookRotation(target.position-transform.position);
			if (rb.velocity.sqrMagnitude < speed * speed)
            {
				rb.AddForce(this.transform.forward * rb.mass * accl);
				
			}
        }
		
	}



	public void LaunchMissile(Transform target)
    {
		launched = true;
		this.target = target;
		
    }

}


