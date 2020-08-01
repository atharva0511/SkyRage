using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MissileTurret : Destructible {

    public Transform aimer;
    public Transform pos1;
    public Transform pos2;
    public Transform displayPos;
    public GameObject radarMarker;
    public GameObject missile;
    public GameObject explosion;
    public GameObject healthCanvas;
    public bool displayHealth = true;
    GameObject canvas = null;
    float lastHit = 0;
    public float baseDamage = 5;
    public int difficulty = 2;
    public float turnSpeed = 1;
    float fireTime = 0;
    public float reloadTime = 2f;
    public AudioSource audioSource;
    public Renderer[] rends;
    // Use this for initialization
    void Start () {
        turnSpeed += (3 / difficulty);
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null && !dead)
        {
            aimer.rotation = Quaternion.Slerp(aimer.rotation, Quaternion.LookRotation(target.position - aimer.position),turnSpeed*Time.deltaTime);
            if (Time.time > fireTime + reloadTime + 5*(1 / difficulty))
            {
                //audioSource.Play();
                
                RaycastHit hit;
                if (Physics.Raycast(aimer.position, target.position - aimer.position,out hit, 300, ~(1 << 2)))
                {
                    if (hit.transform == target)
                    {
                        fireTime = Time.time;
                        StartCoroutine(SerialFire());
                    }
                }
            }
        }
	}

    IEnumerator SerialFire()
    {
        Debug.Log("Firing");
        LaunchMissile(pos1);
        yield return new WaitForSeconds(0.25f);
        LaunchMissile(pos1);
        yield return new WaitForSeconds(0.25f);
        LaunchMissile(pos1);
        yield return new WaitForSeconds(0.5f);
        LaunchMissile(pos2);
        yield return new WaitForSeconds(0.25f);
        LaunchMissile(pos2);
        yield return new WaitForSeconds(0.25f);
        LaunchMissile(pos2);
    }

    public void LaunchMissile(Transform pos)
    {
        audioSource.Play();
        GameObject ms = Instantiate(missile, pos.position, Quaternion.LookRotation(aimer.forward));
        Projectile p = ms.GetComponent<Projectile>();
        p.speed = 140 + 20 * difficulty;
        p.damage = baseDamage + 2.5f * difficulty;
        p.Shooter = this.transform;
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

    public override void Die()
    {
        base.Die();
        radarMarker.SetActive(false);
        StopCoroutine(SerialFire());
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        foreach(Renderer rend in rends)
        {
            Material[] mats = rend.materials;
            foreach (Material mat in mats) mat.color = new Color(0.1f, 0.1f, 0f);
            rend.materials = mats;
        }
        Destroy(this.gameObject, 10f);
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
}
