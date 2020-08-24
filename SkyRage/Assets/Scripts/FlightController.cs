using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightController : playerPlane {

    public bool on = true;
    public float power = 10;
    public float rollSens = 0.5f;
    public float yawSens = 1f;
    public float pitchSens = 1f;
    public float takeOffVel = 30;
    public float allignFac = 0.4f;
    Vector3 tempTorque = Vector3.zero;
    int gear = 0;
    public AudioSource DashSound;
    public Image dashRefill;
    public Slider gearSlider;
    public Joystick joystick;
    public Transform thruster1;
    public Transform thruster2;
    public ParticleSystem thrustParticle;
    public Rigidbody Rb;
    public Animator anim;
    public SkinnedMeshRenderer rend;
    public WheelCollider[] wheels;
    int pitchHash = Animator.StringToHash("Pitch");
    int rollHash = Animator.StringToHash("Roll");
    Vector2 touchRot = Vector2.zero;
    float dashfillRate = 0.75f;
    // Use this for initialization
    void Start () {

        //upgrades
        dashfillRate = Upgrades.qDrone[0] ? 0.4f : 0.2f;
        maxHealth = Upgrades.qDrone[2] ? 150 : 100;
        this.health = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (!on||dead) return;
        //PC
        //Move(1, Input.GetAxis("Vertical"),0,Input.GetAxis("Horizontal"));

        if (dashRefill.fillAmount < 1) dashRefill.fillAmount += Time.deltaTime * dashfillRate;

        Vector2 rot = Vector2.zero;
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x > Screen.width / 2)
                {
                    rot = touch.deltaPosition;
                }
            }
        }
        touchRot = Vector2.Lerp(touchRot, rot, Time.deltaTime * 8);
        
        //Mobile
        Move(gear, 6*touchRot.y*touchSens*Time.deltaTime, 3*touchRot.x*touchSens*Time.deltaTime, Input.acceleration.x,joystick.Vertical,joystick.Horizontal);
        Debug.Log(Input.acceleration.x);
    }

    void Move(int gear,float pitchInput,float yawInput,float rollInput,float groundForward,float groundTurn)
    {
        wheels[1].motorTorque = 200 * groundForward;
        wheels[2].motorTorque = wheels[1].motorTorque;
        wheels[0].steerAngle = 30 * groundTurn;
        //Forward thrust
        Rb.AddForce(transform.forward*Rb.mass * gear * power);

        //Roll
        Rb.rotation = Quaternion.Slerp(Rb.rotation,Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Clamp(-rollInput * 120,-500,500)),Time.deltaTime*rollSens);

        //Yaw
        yawInput = Mathf.Clamp(yawInput,-1, 1);
        if(Rb.velocity.sqrMagnitude>25)
            Rb.AddTorque(30*transform.up * Rb.mass * yawSens*yawInput);

        //pitch
        if (Rb.velocity.sqrMagnitude > takeOffVel * takeOffVel)
        {
            Rb.AddForce(-0.5f*Rb.mass * Physics.gravity*Vector3.Dot(transform.up,Vector3.up));
            
            tempTorque = -10 * transform.right * Rb.mass * pitchSens * pitchInput;
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

    public void PressDash()
    {
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;
        dashRefill.fillAmount = 0;
        
            //thrustR.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.2f)
            {
                Rb.AddTorque(-Rb.mass * transform.up * 50000 * Time.deltaTime);
                //thrustR.startColor = new Color(thrustR.startColor.r, thrustR.startColor.g, thrustR.startColor.b, (startTime + 0.2f - Time.time) / 0.2f);
                yield return null;
            }
        
    }
}
