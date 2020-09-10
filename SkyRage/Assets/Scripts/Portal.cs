using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Transform destination;
    public bool setRot = true;
    public bool uniDir = true;
    public bool objective = false;
    public GameObject Flare;
	// Use this for initialization
	void Start () {
        Objective ob = GetComponent<Objective>();
        if (ob != null)
        {
            if(ob.eventSettings.gameMode == EventSettings.GameMode.arcade){
                destination.position = new Vector3(0, 100, 0);
                destination.rotation = Quaternion.LookRotation(Vector3.forward);
            }
        }
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
            if(ob.GetComponentInParent<playerPlane>() != null)
            {
                Instantiate(Flare, destination.position, Quaternion.identity);
                if (objective)
                    GetComponent<Objective>().Completed();
            }
            
        }
        
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
