using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigun : Weapons {

    float spread = 0;
    public Transform cam;
    public ParticleSystem muzzle;
    public AudioSource audioSource;
    public AudioClip fireAudio;
    public GameObject impact;
    public GameObject beamObject;
    public float stability = 0.5f;
    public float damage = 10;
    public float maxSpread = 1.5f;
    public float fireRate = 5;
    float fireTime = 0;
    //GameObject beamOb;
    //public LineRenderer beam;
    public RawImage crossHair;
    

    void Update()
    {
        if (spread > 0.05f)
        {
            spread = Mathf.Lerp(spread, 0, 2 * Time.deltaTime * stability);
            //AdjustCrosshair();
        }
        if (fire1)
        {
            if (Time.time > fireTime + (1 / fireRate))
            {
                fireTime = Time.time;
                Fire1();
            }
        }
    }
    
    public void Fire1()
    {
        
        muzzle.Play();
        PlayAudio(fireAudio, 2f, 0.5f);
        //Listen Fire Audio
        /*Collider[] listeners = Physics.OverlapSphere(transform.position, 30f, 1 << 9);
        foreach (Collider listener in listeners)
        {
            if (listener.GetComponent<Character>() != null)
                listener.GetComponent<Character>().Listen(transform.position, true, transform);
        }*/

        Vector3 dir = cam.forward;
        dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, Vector3.up) * dir;
        dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, transform.right) * dir;
        RaycastHit hit;
        if (Physics.Raycast(cam.position, dir, out hit, 300f, ~(1 << 2 | 1 << 9)))
        {
            StartCoroutine(Beam(hit.point - transform.GetChild(0).position));
            GameObject blast;
            switch (hit.transform.gameObject.layer)
            {
                //case 4: blast = Instantiate(waterSplash, hit.point, Quaternion.LookRotation(hit.normal), hit.transform); break;
                case 0:
                    {
                        blast = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                        Destructible ob = hit.transform.GetComponentInParent<Destructible>();
                        if ( ob!= null)
                        {
                            ob.TakeDamage(damage, 10);
                        }
                        break;
                    }
                default: blast = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));Debug.Log("hit"); break;
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

        spread += spread < maxSpread ? 0.15f : 0;
    }

    IEnumerator Beam(Vector3 target)
    {
        Debug.Log("Beam");
        float startTime = Time.time;
        GameObject beamOb = Instantiate(beamObject);
        beamOb.transform.position = transform.GetChild(0).position;
        beamOb.transform.rotation = Quaternion.LookRotation(target);
        LineRenderer beam = beamOb.GetComponent<LineRenderer>();
        //beam.enabled = true;
        beam.transform.localScale = new Vector3(1, 1, target.magnitude);
        beam.startColor = new Color(0.5f, 0.4f, 0.2f, 0.6f);
        beam.endColor = new Color(0.3f,0.3f,0.3f, 0.8f);
        //Destroy(beamOb, 0.5f);
        while (Time.time < startTime + 0.5f)
        {
            beam.startColor = new Color(0.5f, 0.4f, 0.2f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.35f));
            beam.endColor = new Color(0.3f, 0.3f, 0.3f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.5f));
            yield return null;
        }
        //beam.enabled = false;
    }

    void AdjustCrosshair()
    {
        float size = 0.6f * spread + 1f;
        crossHair.GetComponent<RectTransform>().localScale = new Vector3(size, size, 0);
    }

    public void PlayAudio(AudioClip clip, float pitch = 1, float randomize = 0.2f)
    {
        audioSource.clip = clip;
        audioSource.pitch = pitch + randomize * Random.value;
        audioSource.Play();
    }
}
