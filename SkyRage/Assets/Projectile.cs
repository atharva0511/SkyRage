using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    public Transform Shooter = null;
    public GameObject blast;
    public float stayTime = 6f;
    float startTime = 0;
    public float damage = 5;
    public bool moveLinearly = false;
    public bool homing = false;
    public Transform target = null;
    public float turnRate = 1;
    public float speed = 5;
	// Use this for initialization
	void Start () {
        startTime = Time.time;
        if (moveLinearly)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > startTime + stayTime)
        {
            Instantiate(blast, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (!homing) return;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position), turnRate * Time.deltaTime);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision col)
    {
        Destructible ob = col.transform.GetComponentInParent<Destructible>();
        if (ob != null)
        {
            ob.TakeDamage(damage, Shooter);
        }
        Instantiate(blast, col.contacts[0].point,Quaternion.identity);
        Destroy(this.gameObject);
    }
}
