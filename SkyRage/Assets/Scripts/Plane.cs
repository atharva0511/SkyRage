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

    public Joystick joystick;

    [HideInInspector]
    public Rigidbody Rb;
    
	// Use this for initialization
	public virtual void Start () {
        Rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
        //if(playerPilot)
            //PlayerPilot();
	}

    void PlayerPilot()
    {
        if(Rb.velocity.sqrMagnitude<maxSpeed*maxSpeed)
            Rb.AddForce(Rb.mass*acceleration*transform.forward);

        //Rb.AddForce(Rb.mass*Vector3.Dot(Rb.velocity, transform.forward)*0.5f * transform.up*acceleration*joystick.Vertical);
        Rb.AddTorque(-30*transform.right * joystick.Vertical * Rb.mass);

        Rb.AddTorque(5*transform.up * Rb.mass * joystick.Horizontal);
        Rb.AddTorque(-10*Rb.mass*transform.forward * Input.acceleration.x);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Checkpoint"))
        {
            col.GetComponent<AudioSource>().Play();
            col.GetComponentInParent<Checkpoints>().NextCheckpoint(col.transform.GetSiblingIndex()+1);
            Destroy(col.transform.GetChild(0).gameObject);
            Destroy(col.transform.gameObject, 1);
        }
    }
}
