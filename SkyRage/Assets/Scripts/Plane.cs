using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public float maxHealth = 100;
    public float health = 100;
    public bool god = false;

    // Use this for initialization
    public virtual void Start () {

	}
	
	// Update is called once per frame
	public void Update () {

	}

    public void TakeDamage(float amount, float randomize = 2f)
    {
        if (!god)
        {
            health -= (amount + Random.Range(0, randomize));
        }
        Damaged();
        if (this.health <= 0) Die();
    }

    public virtual void Die()
    {

    }

    public virtual void Damaged()
    {

    }
}
