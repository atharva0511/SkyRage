using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : Destructible {

    public Transform aimer;
    public int difficulty = 2;
    public Transform target = null;
    public float turnSpeed = 1;
    float fireTime = 0;
    public float reloadTime = 2f;
    public AudioSource audioSource;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null && !dead)
        {
            aimer.rotation = Quaternion.Slerp(aimer.rotation, Quaternion.LookRotation(target.position - aimer.position),turnSpeed*Time.deltaTime);
            if (Time.time > fireTime + reloadTime + 5*(1 / difficulty))
            {
                StartCoroutine(SerialFire());
            }
        }
	}

    IEnumerator SerialFire()
    {
        audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
        yield return new WaitForSeconds(0.5f);
        audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
    }
}
