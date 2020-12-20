using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : Destructible {

    public GameObject Explosion;
    public bool isMine;
    public GameObject Flare;
	// Use this for initialization
	void Start () {
		
	}
	
    void OnEnable()
    {
        if (isMine)
        {
            Instantiate(Flare, transform.position, Quaternion.identity);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    public override void Die()
    {
        base.Die();
        if (isMine)
        {
            if (GetComponentInParent<BotSpawner>() != null)
            {
                GetComponentInParent<BotSpawner>().Died();
            }
        }
        Explode();
    }

    public void Explode()
    {
        Instantiate(Explosion, transform.position+transform.up, Quaternion.identity);
        Destroy(this.gameObject, 0.2f);
    }

    public override void DeathBlow(Transform hitter, WeaponName weapon)
    {
        if (EventSettings.currentPlayer == null) return;
        if (hitter == EventSettings.currentPlayer && !isMine)
        {
            Stats.data[0] += 1;
        }
    }
}
