using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destructible {

    public GameObject Explosion;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Die()
    {
        base.Die();
        Explode();
    }

    public void Explode()
    {
        Instantiate(Explosion, transform.position+transform.up, Quaternion.identity);
        Destroy(this.gameObject, 0.2f);
    }
}
