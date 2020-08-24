using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enforcer : Destructible {

    public float difficulty = 2;
    public Vector3 zoneCentre;
    public int zoneRadius = 100;
    public GameObject healthCanvas;
    public GameObject Explosion;
    public GameObject Flare;
    //public GameObject thrusterL;
    //public GameObject thrusterR;
    //public GameObject thruster;
    public GameObject spark;
    public bool displayHealth = true;
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform displayPos;
    public GameObject radarMarker;
    public Renderer[] rends;
    float lastHit = 0;
    GameObject canvas = null;
    public Rigidbody Rb;
    public AudioSource fireAudio;
    public GameObject laserBeam;
    public GameObject homingMissile;
    public int baseDamage = 3;
    int state = 0;
    public float turnSpeed = 1;
    float lastShot = 0;
    float lastDashed = 0;
    float lastDisappeared = 0;
    float rand = 1;
    bool side = true;
    // Use this for initialization
    void Start()
    {
        target = EventSettings.currentPlayer;
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            this.difficulty = PlayerPrefs.GetInt("Difficulty");
        }
        radarMarker.gameObject.SetActive(true);
        lastDashed = Time.time;
        lastHit = Time.time;
        lastDisappeared = Time.time;
        if (GetComponentInParent<BotSpawner>() != null)
        {
            zoneCentre = GetComponentInParent<BotSpawner>().transform.position;
            zoneRadius = GetComponentInParent<BotSpawner>().zoneRadius;
        }
        else
            zoneCentre = transform.position;
    }

    void OnEnable()
    {
        Instantiate(Flare, transform.position, Quaternion.identity);
    }

    void OnDisable()
    {
        Instantiate(Flare, transform.position, Quaternion.identity);
    }

    void OnDestroy()
    {
        Instantiate(Flare, transform.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        if (dead) return;
        if (stunned) return;
        if (target != null)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 5 * turnSpeed * Time.deltaTime);
            if (Time.time > lastShot + 4 - difficulty + rand)
            {
                rand = 2 * Random.value;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, target.position - transform.position, out hit, 300, ~(1 << 2)))
                {
                    if (hit.transform == target)
                    {
                        lastShot = Time.time;
                        if (rand > 0.5f) StartCoroutine(SerialFire());
                        else FireMissile();
                    }
                }
            }
            if (Time.time > lastDashed + 5 + rand)
            {
                lastDashed = Time.time;
                if ((transform.position - zoneCentre).sqrMagnitude > zoneRadius * zoneRadius)
                {
                    bool dir = Vector3.Dot(zoneCentre - transform.position, transform.right) < 0;
                    StartCoroutine(Dash(dir));
                }
                else
                    StartCoroutine(Dash(rand > 1));
            }
            if (Time.time > lastDisappeared + 10+2*rand)
            {
                StartCoroutine(Disappear());
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
            this.TakeDamage(Vector3.Dot(ob.velocity, transform.position - ob.position) * ob.mass / 10000, ob.transform);
        }
    }

    public override void Die()
    {
        base.Die();
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Instantiate(spark, transform.position, Quaternion.identity, this.transform);
        Rb.drag = 0;
        radarMarker.SetActive(false);
        foreach (Renderer rend in rends)
        {
            Material[] mats = rend.materials;
            foreach (Material mat in mats) mat.color = new Color(0.1f, 0.1f, 0f);
            rend.materials = mats;
        }
        Rb.useGravity = true;
        //thruster.SetActive(false);
        if (GetComponentInParent<BotSpawner>() != null)
        {
            GetComponentInParent<BotSpawner>().Died();
        }
        if (canvas != null) Destroy(canvas, 4f);
        Destroy(this.gameObject, 5f);
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
        if (canvas != null)
            Destroy(canvas);
        canvas = null;
    }

    public IEnumerator SerialFire()
    {
        for (int i = 0; i < Random.Range(3, 6); i++)
        {
            Fire();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void Fire()
    {
        fireAudio.Play();
        GameObject ms = Instantiate(laserBeam, (side ? pos1 : pos2).position, Quaternion.LookRotation(target.position - (side ? pos1 : pos2).position));
        Projectile p = ms.GetComponent<Projectile>();
        p.speed = 140 + 20 * difficulty;
        p.damage = baseDamage + 2f * difficulty;
        p.Shooter = this.transform;
        side = !side;
    }

    public IEnumerator Dash(bool isLeft)
    {
        Rb.AddForce((isLeft ? -transform.right : transform.right) * Rb.mass * 80, ForceMode.Impulse);
        //thrusterL.SetActive(!isLeft);
        //thrusterR.SetActive(isLeft);
        yield return new WaitForSeconds(0.4f);
        //thrusterR.SetActive(!isLeft);
        //thrusterL.SetActive(isLeft);
        Rb.AddForce((isLeft ? transform.right : -transform.right) * Rb.mass * 60, ForceMode.Impulse);
        yield return new WaitForSeconds(0.4f);
        //thrusterL.SetActive(false);
        //thrusterR.SetActive(false);
    }

    public IEnumerator Disappear()
    {
        lastDisappeared = Time.time;
        Instantiate(Flare, transform.position, Quaternion.identity);
        foreach (Renderer rend in rends)
        {
            rend.enabled = false;
        }
        yield return new WaitForSeconds(8);
        Instantiate(Flare, transform.position, Quaternion.identity);
        foreach (Renderer rend in rends)
        {

            rend.enabled = true;
        }
        lastDisappeared = Time.time;
    }

    public void FireMissile()
    {
        Projectile p = Instantiate(homingMissile, pos3.position, Quaternion.LookRotation(target.position - pos3.position)).GetComponent<Projectile>();
        p.damage = 5 + 2 * difficulty;
        p.target = this.target;
        p.turnRate = 40;
        p.speed = 50 + 20 * difficulty;
    }
}
