﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour {

    public bool active = true;
    public int healRate = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider col)
    {
        if (!active) return;
        Destructible ob = col.GetComponentInParent<Destructible>();
        if (ob != null)
        {
            if (ob.health < ob.maxHealth)
            {
                ob.health += Time.deltaTime * healRate;
            }
        }
    }
}