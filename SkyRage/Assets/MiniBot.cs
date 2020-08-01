using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniBot : Destructible {

    public float difficulty = 2;
    public GameObject healthCanvas;
    public bool displayHealth = true;
    public Transform pos1;
    public Transform pos2;
    public Transform displayPos;
    public GameObject radarMarker;
    float lastHit = 0;
    GameObject canvas = null;
    public Rigidbody Rb;
    public AudioSource fireAudio;
    public GameObject laserBeam;
    public int baseDamage = 3;
    int state = 0;
    public float turnSpeed = 1;
    float lastShot = 0;
    float rand = 1;
    bool side = true;
    // Use this for initialization
    void Start () {
        radarMarker.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 5*turnSpeed * Time.deltaTime);
            if (Time.time > lastShot + 4 - difficulty + rand){
                rand = 2 * Random.value;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 300, ~(1 << 2)))
                {
                    if (hit.transform == target)
                    {
                        lastShot = Time.time;
                        StartCoroutine(SerialFire());
                    }
                }
            }
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

    public void OnCollisionEnter(Collision col)
    {
        Rigidbody ob = col.transform.GetComponent<Rigidbody>();
        if (ob != null)
        {
            this.TakeDamage(Vector3.Dot(ob.velocity, transform.position - ob.position)*ob.mass/5000,ob.transform);
        }
    }

    public override void Die()
    {
        base.Die();
        radarMarker.SetActive(false);
        if(canvas!=null)Destroy(canvas);
        Destroy(this.gameObject, 0.5f);
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
        if(canvas!=null)
            Destroy(canvas);
        canvas = null;
    }

    public IEnumerator SerialFire()
    {
        for(int i = 0; i < Random.Range(3, 6); i++)
        {
            Fire();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Fire()
    {
        fireAudio.Play();
        GameObject ms = Instantiate(laserBeam, (side?pos1:pos2).position, Quaternion.LookRotation(transform.forward));
        Projectile p = ms.GetComponent<Projectile>();
        p.speed = 140 + 20 * difficulty;
        p.damage = baseDamage + 5f * difficulty;
        p.Shooter = this.transform;
        side = !side;
    }
}
