using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {

    public bool playerPilot = false;
    public float health = 100;
    public float damageMultiplier = 1;
    public float acceleration = 5;
    public float maxSpeed = 40;
    public float turbulance = 0.6f;

    public Rigidbody Rb;
	// Use this for initialization
	public virtual void Start () {
        Rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
        if(playerPilot)
            PlayerPilot();
	}

    void PlayerPilot()
    {
        if(Rb.velocity.sqrMagnitude<maxSpeed*maxSpeed)
            Rb.AddForce(Rb.mass*acceleration*transform.forward);

        //Rb.AddTorque(*Rb.mass*transform.right * Input.acceleration.y);
    }
}
