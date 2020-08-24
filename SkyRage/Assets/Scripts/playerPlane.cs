using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerPlane : Destructible {

    public GameObject xpCanvas;
    [HideInInspector]
    public int coins = 0;
    protected GameObject xpInstance;
    protected GameObject displayInstance;
    public GameObject spark;
    public Transform cam;
    public Transform DisplayPos;
    public AudioSource[] pauseAudio;
    public WeaponManager wm;
    public Image healthBar;
    public ParticleSystem DamageSmoke;
    public Image thrustFill;
    public AudioSource pickupAudio;
    public AudioClip lifeAudio;
    public Text coinDisp;
    public Text lifeDisp;
    public Text utDisp;
    public Image deathVignette;
    IEnumerator routine = null;
    public bool explodeOnDeath = false;
    public GameObject explosion;
    public float touchSens = 1f;

    // Use this for initialization
    void Awake () {
        SetLifeDisp();
        SetUpgradeTokenDisp();
	}
	
	// Update is called once per frame
	void Update () {
        // 3 ,22
        RaycastHit hit;
        if (Physics.Raycast(DisplayPos.position + 0.75f * transform.up, -transform.forward, out hit, 16, ~(1 << 2)))
        {
            cam.position = hit.point + transform.forward;
        }
        else
            cam.position = DisplayPos.position + 0.75f * transform.up - 16 * transform.forward;
	}


    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Checkpoint"))
        {
            col.enabled = false;
            Checkpoints cpoints = col.GetComponentInParent<Checkpoints>();
            col.GetComponent<AudioSource>().Play();
            if (col.transform.GetSiblingIndex() != cpoints.transform.childCount - 1)
            {
                cpoints.NextCheckpoint(col.transform.GetSiblingIndex() + 1);
                StartCoroutine(AddXP(cpoints.xp));
            }
            else
            {
                cpoints.Completed();
                StartCoroutine(AddXP(cpoints.finishedXP));
            }
            Destroy(col.transform.GetChild(0).gameObject);
            Destroy(col.transform.gameObject, 1);
        }
        // pickups
        if (col.transform.CompareTag("Pickup"))
        {
            col.enabled = false;
            Pickup pickup = col.GetComponent<Pickup>();
            pickup.Collect();
            if(pickup.type == Pickup.pickupType.health)
            {
                if (health >= maxHealth) return;
                if(routine!=null)StopCoroutine(routine);
                if(displayInstance!=null)Destroy(displayInstance);
                routine = DisplayText("+ " + Mathf.Ceil(pickup.health>maxHealth-health?maxHealth-health:pickup.health).ToString() + " Health", Color.green);
                this.health = Mathf.Clamp(this.health + pickup.health, 0, maxHealth);
                DisplayHealth();
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if (pickup.type == Pickup.pickupType.coin)
            {
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+ " + pickup.coins.ToString() + " Coins", Color.yellow);
                coins += pickup.coins;
                SetCoinDisplay();
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if (pickup.type == Pickup.pickupType.life)
            {
                if (PlayerData.lives == 5) return;
                PlayerData.lives += 1;
                SetLifeDisp();
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+1 Life", Color.green);
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if(pickup.type == Pickup.pickupType.upgradeToken)
            {
                PlayerData.upgradeTokens += 1;
                SetUpgradeTokenDisp();
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+1 Upgrade Token", Color.blue);
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if(pickup.type == Pickup.pickupType.Nitro)
            {
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("Nitro Refilled", Color.red);
                thrustFill.fillAmount = 1;
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if(pickup.type == Pickup.pickupType.ammo)
            {
                if (wm.rockets >= wm.maxRockets) return;
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+ " + Mathf.Ceil(pickup.rockets > wm.maxRockets - wm.rockets ? wm.maxRockets - wm.rockets : pickup.rockets).ToString() + " Missile", Color.cyan);
                wm.rockets = Mathf.Clamp(wm.rockets + pickup.rockets, 0, wm.maxRockets);
                wm.UpdateDisp();
                StartCoroutine(routine);
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            else if(pickup.type == Pickup.pickupType.Objective)
            {
                if(pickup.xp!=0) StartCoroutine(AddXP(pickup.xp));
                pickupAudio.clip = pickup.collectSound;
                pickupAudio.Play();
                Destroy(col.gameObject);
            }
            if (pickup.isObjective) pickup.Collect();
        }
    }

    public IEnumerator AddXP(int amount)
    {
        float startTime = Time.time;
        PlayerData.xp += amount;
        xpInstance = Instantiate(xpCanvas, DisplayPos.position, Quaternion.LookRotation(DisplayPos.position-cam.position,cam.up), DisplayPos);
        Text xpText = xpInstance.transform.GetChild(0).GetComponent<Text>();
        xpText.text = "+ " + amount.ToString() + " xp";
        xpText.CrossFadeAlpha(0, 1, false);
        while (Time.time < startTime + 1)
        {
            xpInstance.transform.rotation = Quaternion.LookRotation(DisplayPos.position - cam.position, cam.up);
            yield return null;
        }
        Destroy(xpInstance);
    }

    public void DisplayHealth()
    {
        healthBar.fillAmount = health / maxHealth;
        healthBar.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        if (health < maxHealth/2) DamageSmoke.emissionRate = 20 * ((maxHealth / 2) - health)/(maxHealth/2);
        else DamageSmoke.emissionRate = 0;
    }
    public override void Damaged()
    {
        base.Damaged();
        DisplayHealth();
    }
    public override void Heal(float amount)
    {
        base.Heal(amount);
        DisplayHealth();
    }

    
    
    IEnumerator DisplayText(string dispText,Color dispColor)
    {
        float startTime = Time.time;
        displayInstance = Instantiate(xpCanvas, DisplayPos.position, Quaternion.LookRotation(DisplayPos.position - cam.position, cam.up), DisplayPos);
        Text dText = displayInstance.transform.GetChild(0).GetComponent<Text>();
        dText.text = dispText;
        dText.color = dispColor;
        dText.CrossFadeAlpha(0, 1, false);
        while (Time.time < startTime + 1)
        {
            displayInstance.transform.rotation = Quaternion.LookRotation(DisplayPos.position - cam.position, cam.up);
            yield return null;
        }
        Destroy(displayInstance);
    }

    public void SetCoinDisplay()
    {
        coinDisp.text = this.coins.ToString();
    }
    public void SetLifeDisp()
    {
        Mathf.Clamp(PlayerData.lives,0, 5);
        lifeDisp.text = PlayerData.lives.ToString();
    }
    public void SetUpgradeTokenDisp()
    {
        utDisp.text = PlayerData.upgradeTokens.ToString();
    }


    public override void Die()
    {
        if (PlayerData.lives > 0)
        {
            StartCoroutine(UseLife());
        }
        else
        {
            dead = true;
            wm.dead = true;
            StartCoroutine(DeathSequence());
        }
    }

    public IEnumerator UseLife()
    {
        dead = false;
        this.health = this.maxHealth;
        DisplayHealth();
        float startTime = Time.time;
        PlayerData.lives -= 1;
        SetLifeDisp();
        pickupAudio.clip = lifeAudio;
        pickupAudio.Play();
        deathVignette.gameObject.SetActive(true);
        deathVignette.color = new Color(0.7f, 0.77f, 0.65f, 1);
        deathVignette.CrossFadeAlpha(0, 4,false);
        this.god = true;
        yield return new WaitForSeconds(4);
        this.god = false;
        deathVignette.gameObject.SetActive(false);
    }

    

    public IEnumerator DeathSequence()
    {
        foreach(Weapons w in wm.weapons)
        {
            w.enabled = false;
        }
        foreach (AudioSource aud in pauseAudio)
        {
            aud.Pause();
        }
        //radarMarker.SetActive(false);
        deathVignette.gameObject.SetActive(true);
        deathVignette.color = new Color(1f, 0.2f, 0, 1);
        deathVignette.CrossFadeAlpha(0, 3, false);
        Instantiate(spark, transform.position, Quaternion.identity, this.transform);
        if(explodeOnDeath) Instantiate(explosion, transform.position, Quaternion.identity, this.transform);
        Renderer[] rends = GetComponent<Customizable>().rends;
        foreach (Renderer rend in rends)
        {
            Material[] mats = rend.materials;
            foreach (Material mat in mats) mat.color = new Color(0.1f, 0.1f, 0f);
            rend.materials = mats;
        }
        yield return new WaitForSeconds(3f);
        FindObjectOfType<EventSettings>().PlayerDead();
    }
}
