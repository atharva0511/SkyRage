using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public float maxHealth = 100;
    public float health = 100;
    public bool god = false;
    public int team = 0; //Single player character should have team =1
    [HideInInspector]
    public bool dead = false;

    // Use this for initialization
    public virtual void Start () {

	}
	
	

    public void TakeDamage(float amount, float randomize = 2f)
    {
        if (!god)
        {
            health -= (amount + Random.Range(0, randomize));
        }
        Damaged();
        if (this.health <= 0 && !dead) Die();
    }

    public void TakeDamage(float amount, Transform hitter,float randomize = 2f)
    {
        if (!god)
        {
            health -= (amount + Random.Range(0, randomize));
        }
        Damaged();
        if (this.health <= 0 && !dead) Die();
    }

    public void Heal(float amount)
    {
        if (!dead && this.health<this.maxHealth)
        {
            this.health += amount;
            this.health = this.health > this.maxHealth ? this.maxHealth : this.health;
        }
    }

    public virtual void Die()
    {
        dead = true;
    }

    public virtual void Damaged()
    {
        // is "death blow" if health is -ve but dead is false
    }
}
