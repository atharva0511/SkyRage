using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerPlane : Destructible {

    public GameObject xpCanvas;
    protected int coins = 0;
    protected GameObject xpInstance;
    protected GameObject displayInstance;
    public Transform cam;
    public Transform DisplayPos;
    public Weapons[] weapons;
    public Image healthBar;
    public ParticleSystem DamageSmoke;
    public AudioSource pickupAudio;
    public Text coinDisp;
    IEnumerator routine = null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
            if(pickup.type == Pickup.pickupType.health)
            {
                if(routine!=null)StopCoroutine(routine);
                if(displayInstance!=null)Destroy(displayInstance);
                routine = DisplayText("+ " +  (pickup.health>maxHealth-health?maxHealth-health:pickup.health).ToString() + " Health", Color.green);
                this.health = Mathf.Clamp(this.health + pickup.health, 0, maxHealth);
                DisplayHealth();
                StartCoroutine(routine);
            }
            else if (pickup.type == Pickup.pickupType.coin)
            {
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+ " + pickup.coins.ToString() + " Coins", Color.yellow);
                coins += pickup.coins;
                SetCoinDisplay();
                StartCoroutine(routine);
            }
            else if (pickup.type == Pickup.pickupType.life)
            {
                if (routine != null) StopCoroutine(routine);
                if (displayInstance != null) Destroy(displayInstance);
                routine = DisplayText("+1 Life", Color.green);
                StartCoroutine(routine);
            }
            if (pickup.isObjective) pickup.Collect();
            pickupAudio.clip = pickup.collectSound;
            pickupAudio.Play();
            Destroy(col.gameObject);
        }
    }

    IEnumerator AddXP(int amount)
    {
        float startTime = Time.time;
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
        if (health < maxHealth) DamageSmoke.emissionRate = 5 + 15 * ((maxHealth / 2) - health);
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

    public void PressedFire1()
    {
        foreach (Weapons w in weapons)
        {
            w.PressedFire1();
        }
    }
    public void ReleaseFire1()
    {
        foreach (Weapons w in weapons)
        {
            w.ReleaseFire1();
        }
    }
    public void PressedFire2()
    {
        foreach (Weapons w in weapons)
        {
            w.PressedFire2();
        }
    }
    public void ReleaseFire2()
    {
        foreach (Weapons w in weapons)
        {
            w.ReleaseFire1();
        }
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
}
