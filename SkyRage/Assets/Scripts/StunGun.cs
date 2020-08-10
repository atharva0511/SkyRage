using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : Weapons {

    float fireTime = 0;
    public float fireRate = 0.25f;
    public float stunTime = 5f;
    public Transform cam;
    public AudioSource audioSource;
    public AudioClip fireAudio;
    public GameObject impact;
    public GameObject beamObject;
    public float damage = 1;
    // Use this for initialization
    void Start () {
        this.owner = GetComponentInParent<Destructible>().transform;
    }
	
	// Update is called once per frame
	void Update () {
		if((slot==1 && fire1) ||(slot == 2 && fire2))
        {
            if (Time.time > fireTime + (1 / fireRate))
            {
                fireTime = Time.time;
                Fire2();
            }
        }
	}

    public void Fire2()
    {
        PlayAudio(fireAudio);
        RaycastHit hit;
        Vector3 dir = cam.forward;
        if (Physics.Raycast(cam.position, dir, out hit, 300f, ~(1 << 2 | 1 << 9)))
        {
            StartCoroutine(Beam(hit.point - transform.GetChild(0).position));
            GameObject blast;
            
            blast = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            Destructible ob = hit.transform.GetComponentInParent<Destructible>();
            if (ob != null)
            {
                            ob.TakeDamage(damage, owner, 1);
                            StartCoroutine(ob.Stun(stunTime));
            }
            Rigidbody Rb = hit.transform.GetComponent<Rigidbody>();
            if (Rb != null)
            {
                Rb.AddForce(dir * damage * 60);
            }

            Destroy(blast, 0.5f);
        }
        else
        {
            StartCoroutine(Beam(400 * dir));
        }
    }

    IEnumerator Beam(Vector3 target)
    {
        float startTime = Time.time;
        GameObject beamOb = Instantiate(beamObject);
        beamOb.transform.position = transform.GetChild(0).position;
        beamOb.transform.rotation = Quaternion.LookRotation(target);
        LineRenderer beam = beamOb.GetComponent<LineRenderer>();
        //beam.enabled = true;
        beam.transform.localScale = new Vector3(1, 1, target.magnitude);
        beam.startColor = new Color(0.2f, 0.4f, 0.6f, 0.6f);
        beam.endColor = new Color(0.3f, 0.3f, 0.6f, 0.8f);
        //Destroy(beamOb, 0.5f);
        while (Time.time < startTime + 0.5f)
        {
            beam.startColor = new Color(0.2f, 0.4f, 0.6f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.35f));
            beam.endColor = new Color(0.3f, 0.3f, 0.6f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.5f));
            yield return null;
        }
        //beam.enabled = false;
    }

    public void PlayAudio(AudioClip clip, float pitch = 1, float randomize = 0.2f)
    {
        audioSource.clip = clip;
        audioSource.pitch = pitch + randomize * Random.value;
        audioSource.Play();
    }
}
