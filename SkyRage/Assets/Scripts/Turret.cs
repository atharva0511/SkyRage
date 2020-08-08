using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Destructible {

	public float range = 300;
	public float turnSpeed = 1;
    public float damage = 5f;
    public GameObject beamObject;
    public GameObject impact;
    public ParticleSystem muzzle;
    public AudioSource audioSource;
    public Transform displayPos;
    float difficulty = 2;
	public Transform turret;
    public Transform shootPos;
    float lastFired = 0;
    float rand = 2;
    public float spread = 10f;
    // display health
    public GameObject healthCanvas;
    public bool displayHealth = true;
    GameObject canvas = null;
    float lastHit = 0;

    //death
    public GameObject explosion;
    public GameObject radarMarker;
    public Renderer[] rends;
    // Use this for initialization
    void Start() {
        target = EventSettings.currentPlayer;
    }

	// Update is called once per frame
	void Update() {
        if (dead) return;
        if (stunned) return;
		if (target != null)
		{
			Quaternion rot = Quaternion.LookRotation(target.position - turret.position);
			turret.rotation = Quaternion.Slerp(turret.rotation, rot, Time.deltaTime * turnSpeed);
	    }
        if (Time.time > lastFired +2+ rand)
        {
            rand = 3 * Random.value;
            RaycastHit hit;
            if (Physics.Raycast(shootPos.position, target.position - shootPos.position, out hit, 300, ~(1 << 2)))
            {
                if (hit.transform == target)
                {
                    lastFired = Time.time;
                    StartCoroutine(SerialFire());
                }
            }
        }
	}

    IEnumerator SerialFire()
    {
        for (int i = 0; i < Random.Range(6, 10); i++)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Fire()
    {
        muzzle.Play();
        PlayAudio( 2f, 0.5f);
        Vector3 dir = turret.forward;
        dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, Vector3.up) * dir;
        dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, transform.right) * dir;
        RaycastHit hit;
        if (Physics.Raycast(turret.position, dir, out hit, 300f, ~(1 << 2 | 1 << 9)))
        {
            StartCoroutine(Beam(hit.point - transform.GetChild(0).position));
            GameObject blast;

            blast = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
            Destructible ob = hit.transform.GetComponentInParent<Destructible>();
            if (ob != null)
            {
                ob.TakeDamage(damage+this.difficulty*2, this.transform, 0);
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

    public override void Damaged()
    {
        base.Damaged();
        lastHit = Time.time;
        if (canvas == null && displayHealth && !dead)
        {
            StartCoroutine(DisplayHealth());
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
        beam.startColor = new Color(0.5f, 0.4f, 0.2f, 0.6f);
        beam.endColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        //Destroy(beamOb, 0.5f);
        while (Time.time < startTime + 0.5f)
        {
            beam.startColor = new Color(0.5f, 0.4f, 0.2f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.35f));
            beam.endColor = new Color(0.3f, 0.3f, 0.3f, Mathf.Lerp(beam.startColor.a, 0, (Time.time - startTime) / 0.5f));
            yield return null;
        }
        //beam.enabled = false;
    }

    public void PlayAudio(float pitch = 1, float randomize = 0.2f)
    {
        audioSource.pitch = pitch + randomize * Random.value;
        audioSource.Play();
    }

    public IEnumerator DisplayHealth()
    {
        this.canvas = Instantiate(healthCanvas);
        canvas.transform.localPosition = displayPos.position;
        canvas.transform.LookAt(Camera.main.transform.position);
        Image healthBar = canvas.GetComponent<RectTransform>().GetChild(0).GetChild(0).GetComponent<Image>();

        healthBar.fillAmount = health / maxHealth;
        while (Time.time < lastHit + 3f)
        {
            if (dead)
            {
                canvas.GetComponent<RectTransform>().GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            canvas.transform.localPosition = displayPos.position;
            canvas.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward);
            //canvas.transform.LookAt(Camera.main.transform.position);
            healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, 5 * Time.deltaTime);
            healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
            yield return null;
        }
        Destroy(canvas);
        canvas = null;
    }

    public override void Die()
    {
        base.Die();
        radarMarker.SetActive(false);
        StopCoroutine(SerialFire());
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        foreach (Renderer rend in rends)
        {
            Material[] mats = rend.materials;
            foreach (Material mat in mats) mat.color = new Color(0.1f, 0.1f, 0f);
            rend.materials = mats;
        }
        Destroy(this.gameObject, 10f);
    }
}
