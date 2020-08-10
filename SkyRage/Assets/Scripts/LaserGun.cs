using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : Weapons {

    public float fireDuration = 3;
    public float chargeDuration = 4;
    public float damageRate = 0.5f;
    float fireTime = 0;
    public Transform cam;
    public GameObject impact;
    public GameObject beamObject;
    public AudioSource audioSource;
    GameObject beamInstance = null;
    public float charge = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time>fireTime+chargeDuration)
        {
            charge = 1;
        }
        if ((slot == 1 && fire1) || (slot == 2 && fire2))
        {
            if (charge>0)
            {
                charge -= Time.deltaTime / fireDuration;
                Fire1();
            }
            else if (beamInstance != null)
            {
                Destroy(beamInstance);
                impact.GetComponent<ParticleSystem>().emissionRate = 0;
                audioSource.Stop();
            }
        }
        else if (beamInstance != null)
        {
            Destroy(beamInstance);
            impact.GetComponent<ParticleSystem>().emissionRate = 0;
            audioSource.Stop();
        }
    }

    public void Fire1()
    {
        if (beamInstance == null) {
            beamInstance = Instantiate(beamObject);
            beamInstance.GetComponent<LineRenderer>().SetColors(new Color(0.6f, 0.6f, 1f), Color.blue);
            impact.GetComponent<ParticleSystem>().emissionRate = 15;
            beamInstance.GetComponent<LineRenderer>().SetWidth(5, 5);
            audioSource.Play();
        }
        fireTime = Time.time;
        RaycastHit hit;
        Vector3 dir = cam.forward;
        if (Physics.Raycast(cam.position, dir, out hit, 300f, ~(1 << 2 | 1 << 9)))
        {
            Destructible ob = hit.transform.GetComponentInParent<Destructible>();
            if (ob != null)
            {
                 ob.TakeDamage(damageRate, owner, 0);
            }        
            Rigidbody Rb = hit.transform.GetComponent<Rigidbody>();
            if (Rb != null)
            {
                Rb.AddForce(dir * damageRate * 60);
            }
            SetBeam(hit.point - transform.position);
            impact.transform.position = hit.point;
        }
        else
        {
            impact.transform.position = transform.position;
            SetBeam(400 * dir);
        }
    }

    void SetBeam(Vector3 target)
    {
        beamInstance.transform.position = transform.position;
        beamInstance.transform.rotation = Quaternion.LookRotation(target);
        beamInstance.transform.localScale = new Vector3(1, 1, target.magnitude);
        
    }
}
