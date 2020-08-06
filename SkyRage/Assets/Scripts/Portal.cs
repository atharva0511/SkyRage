using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Transform destination;
    public bool setRot = true;
    public bool uniDir = true;
    public bool objective = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        Destructible ob = col.GetComponentInParent<Destructible>();
        if (ob != null)
        {
            OnEnter(ob.transform); 
        }
        GetComponent<Objective>().Completed();
    }

    public virtual void OnEnter(Transform ob)
    {
        ob.position = destination.position;
        ob.rotation = destination.rotation;
        Rigidbody Rb = ob.GetComponent<Rigidbody>();
        if (Rb != null)
        {
            Rb.rotation = destination.rotation;
        }
    }
}
