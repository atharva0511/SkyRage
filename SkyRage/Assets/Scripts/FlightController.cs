using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightController : Destructible {

    public bool on = true;
    public float power = 10;
    public float rollSens = 0.5f;
    public float yawSens = 1f;
    public float pitchSens = 1f;
    public float takeOffVel = 30;
    public float allignFac = 0.4f;
    Vector3 tempTorque = Vector3.zero;
    int gear = 0;
    public Slider gearSlider;
    public Joystick joystick;
    public Transform thruster1;
    public Transform thruster2;
    public ParticleSystem thrustParticle;
    public Transform DisplayPos;
    public Transform cam;
    public GameObject xpCanvas;
    GameObject xpInstance;
    public Rigidbody Rb;
    public Animator anim;
    public SkinnedMeshRenderer rend;
    public Weapons[] weapons;
    int pitchHash = Animator.StringToHash("Pitch");
    int rollHash = Animator.StringToHash("Roll");
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (!on) return; 
        //PC
        //Move(1, Input.GetAxis("Vertical"),0,Input.GetAxis("Horizontal"));

        //Mobile
        Move(gear, -joystick.Vertical, joystick.Horizontal, Input.acceleration.x);
        Debug.Log(Input.acceleration.x);
    }

    void Move(int gear,float pitchInput,float yawInput,float rollInput)
    {
        //Forward thrust
        Rb.AddForce(transform.forward*Rb.mass * gear * power);

        //Roll
        Rb.rotation = Quaternion.Slerp(Rb.rotation,Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Clamp(-rollInput * 120,-500,500)),Time.deltaTime*rollSens);

        //Yaw
        Rb.AddTorque(15*transform.up * Rb.mass * yawSens*yawInput);

        //pitch
        if (Rb.velocity.sqrMagnitude > takeOffVel * takeOffVel)
        {
            Rb.AddForce(-0.5f*Rb.mass * Physics.gravity*Vector3.Dot(transform.up,Vector3.up));
            
            tempTorque = 30 * transform.right * Rb.mass * pitchSens * pitchInput;
            Rb.AddTorque(tempTorque);
            //Allign to vel
            Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.LookRotation(Rb.velocity), Time.deltaTime * allignFac);
            Rb.velocity = Vector3.Lerp(Rb.velocity,Vector3.Dot(Rb.velocity, transform.forward) * transform.forward,Time.deltaTime*0.5f);
        }

        UpdateAnimator(pitchInput, rollInput,yawInput);
    }

    void UpdateAnimator(float pitch,float roll,float yaw)
    {
        anim.SetFloat(pitchHash, pitch);
        anim.SetFloat(rollHash, roll);
        rend.SetBlendShapeWeight(0, -yaw * 100);
        rend.SetBlendShapeWeight(1, yaw * 100);
    }

    public void ChangeGear()
    {
        gear = (int)gearSlider.value;
        if (gear > 0)
        {
            thruster1.localScale = new Vector3(1, 1, 4 * gear);
            thruster2.localScale = new Vector3(1, 1, 4 * gear);
            thrustParticle.emissionRate = gear * 10;
        }
        else
        {
            thruster1.localScale = Vector3.zero;
            thruster2.localScale = Vector3.zero;
            thrustParticle.emissionRate = 0;
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
}
