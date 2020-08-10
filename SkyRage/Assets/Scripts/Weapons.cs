﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour {

    [HideInInspector]
    public bool fire1 = false;
    [HideInInspector]
    public bool fire2 = false;
    public byte slot = 1;

    public Transform owner = null;
	// Use this for initialization
	void Start() { 

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void PressedFire1()
    {
        fire1 = true;
    }
    public virtual void ReleaseFire1()
    {
        fire1 = false;
    }
    public virtual void PressedFire2()
    {
        fire2 = true;
    }
    public virtual void ReleaseFire2()
    {
        fire2 = false;
    }
}
