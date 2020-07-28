using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	public float range = 200;
	public float turnSpeed = 1;
	public Transform target = null;
	public Transform turret;


	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		

		if (target != null)
		{
			Quaternion rot = Quaternion.LookRotation(target.position - turret.position);
			turret.rotation = Quaternion.Slerp(turret.rotation, rot, Time.deltaTime * turnSpeed);

	    }
	}
}
