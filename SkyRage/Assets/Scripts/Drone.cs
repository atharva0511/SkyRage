using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Drone : Destructible {

    public bool On = true;
    public float maxThrust = 40;
    public float fanThrust = 10;
    public float hoverHeight = 1;
    public float turnSens = 1;
    float thrust = 0;
    float leanAngle = 0;
    float dashRefill = 0;
    public Slider thrustSlider;
    public Joystick joystick;
    public Image healthBar;
    public LineRenderer thrustR;
    public LineRenderer thrustL;
    public LineRenderer thrustB;
    public AudioSource PropellerSound;
    public AudioSource EngineSound;
    public AudioSource DashSound;
    public ParticleSystem thrustParticle;
    public Transform[] fans;
    public Weapons[] weapons;
    Rigidbody Rb;
    Animator anim;
    public GameObject xpCanvas;
    GameObject xpInstance;
    public Transform cam;
    public Transform DisplayPos;
    int turnHash = Animator.StringToHash("Turn");
    int moveHash = Animator.StringToHash("Forward");

	// Use this for initialization
	void Start () {
        Rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        DisplayHealth();
	}
	
	void FixedUpdate()
    {
        if (!On) return;
        //refill dash
        if (dashRefill < 1) dashRefill += Time.deltaTime*0.75f;
        //pass inputs
        //MoveDrone(thrust,joystick.Horizontal, joystick.Vertical,Input.acceleration.x,Input.acceleration.z);

        //PC Input
        if (Input.GetKeyDown(KeyCode.LeftShift)) PressDash(false);
        MoveDrone(thrust, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        foreach(Transform fan in fans)
        {
            fan.Rotate(0, 0, 120);
        }
    }

    public void ThrustInput()
    {
        thrust = thrustSlider.normalizedValue * maxThrust;
        thrustB.transform.localScale = new Vector3(1, 1, 1 + thrustSlider.normalizedValue * 5);
        thrustParticle.emissionRate = thrustSlider.normalizedValue * 10;
    }

    void MoveDrone(float thrust,float hor,float ver,float turn,float lean)
    {
        //Hover above surface
        if (Physics.Raycast(transform.position, -transform.up, 0.5f+hoverHeight))
        {
            Rb.AddForce(transform.up * Rb.mass * 10);
        }
        Rb.AddForce(-Rb.mass * Physics.gravity);
        //Apply thrust
        Rb.AddForce(Rb.mass*transform.forward * (thrust+fanThrust*ver));
        Rb.AddForce(Rb.mass * transform.right * fanThrust * hor);

        //Turn
        Rb.AddTorque(Vector3.up * turn * 200*Rb.mass*turnSens);

        //Lean
        //leanAngle = Mathf.Clamp(-Mathf.Rad2Deg*Mathf.Asin(lean),-70,70);
        leanAngle = -60 * lean;
        //Debug.Log(leanAngle);
        Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.Euler(leanAngle, transform.eulerAngles.y, 0),Time.deltaTime*2);
        //Rb.AddTorque(transform.right * lean * 80 * Rb.mass);

        UpdateAnimator(turn, ver);
        UpdateAudio(hor, ver, thrust);
    }

    void UpdateAnimator(float turn,float forward)
    {
        anim.SetFloat(turnHash, turn);
        anim.SetFloat(moveHash, forward);
    }

    void UpdateAudio(float hor,float ver,float thrust)
    {
        PropellerSound.pitch = 1.5f + 0.5f * (hor * hor + ver * ver);
        EngineSound.volume = Mathf.Lerp(EngineSound.volume,thrust,Time.deltaTime);
    }

    public void PressDash(bool mobileInput = true)
    {
        if (dashRefill < 1) return;
        if (mobileInput)
        {
            if (joystick.Horizontal < 0.1f) StartCoroutine(Dash(true));
            else if (joystick.Horizontal > 0.1f) StartCoroutine(Dash(false));
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0.1f) StartCoroutine(Dash(true));
            else if (Input.GetAxis("Horizontal") > 0.1f) StartCoroutine(Dash(false));
        }
    }
    IEnumerator Dash(bool leftSide)
    {
        float startTime = Time.time;
        dashRefill = 0;
        if (leftSide)
        {
            thrustR.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.2f)
            {
                Rb.AddForce(-Rb.mass * transform.right * 15000*Time.deltaTime);
                thrustR.startColor = new Color(thrustR.startColor.r, thrustR.startColor.g, thrustR.startColor.b, (startTime + 0.2f - Time.time) / 0.2f);
                yield return null;
            }
            thrustR.enabled = false;
            yield return new WaitForSeconds(0.2f);
            thrustL.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.6f)
            {
                Rb.AddForce(Rb.mass * transform.right * 13000*Time.deltaTime);
                thrustL.startColor = new Color(thrustL.startColor.r, thrustL.startColor.g, thrustL.startColor.b, (startTime + 0.6f - Time.time) / 0.2f);
                yield return null;
            }
            thrustL.enabled = false;
        }
        else
        {
            thrustL.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.2f)
            {
                Rb.AddForce(Rb.mass * transform.right * 15000*Time.deltaTime);
                thrustL.startColor = new Color(thrustL.startColor.r, thrustL.startColor.g, thrustL.startColor.b, (startTime + 0.2f - Time.time) / 0.2f);
                yield return null;
            }
            thrustL.enabled = false;
            yield return new WaitForSeconds(0.2f);
            thrustR.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.6f)
            {
                Rb.AddForce(-Rb.mass * transform.right * 13000*Time.deltaTime);
                thrustR.startColor = new Color(thrustR.startColor.r, thrustR.startColor.g, thrustR.startColor.b, (startTime + 0.6f - Time.time) / 0.2f);
                yield return null;
            }
            thrustR.enabled = false;
        }
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
                StartCoroutine(AddXP(cpoints.finishedXP));
            }
            Destroy(col.transform.GetChild(0).gameObject);
            Destroy(col.transform.gameObject, 1);
        }
    }

    IEnumerator AddXP(int amount)
    {
        float startTime = Time.time;
        xpInstance = Instantiate(xpCanvas, DisplayPos.position, Quaternion.LookRotation(DisplayPos.position - cam.position, cam.up), DisplayPos);
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
        foreach(Weapons w in weapons)
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
}
