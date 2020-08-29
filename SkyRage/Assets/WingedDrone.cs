using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WingedDrone : playerPlane {

    public bool On = true;
    public float maxThrust = 40;
    public float fanThrust = 10;
    public float hoverHeight = 1;
    public float turnSens = 1;
    public float lives;
    bool thrust = false;
    public Image dashRefill;
    public float thrustRechargeRate = 0.1f;

    public Joystick joystick;
    public LineRenderer thrustR;
    public LineRenderer thrustL;
    public LineRenderer thrustB;
    public AudioSource PropellerSound;
    public AudioSource EngineSound;
    public AudioSource DashSound;
    public ParticleSystem thrustParticle;
    public Transform[] fans;
    Rigidbody Rb;
    Animator anim;
    Vector2 touchRot = Vector2.zero;
    //public Transform cam;
    //public Transform DisplayPos;
    int turnHash = Animator.StringToHash("Turn");
    int moveHash = Animator.StringToHash("Forward");

    float dashfillRate = 0.75f;

    // Use this for initialization
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lives = PlayerData.lives;
        DisplayHealth();

        // upgrades
        dashfillRate = Upgrades.wDrone[0] ? 1 : 0.75f;
        thrustRechargeRate = Upgrades.wDrone[1] ? 0.05f : 0.04f;
        maxHealth = Upgrades.wDrone[2] ? 160 : 120;
        this.health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!On || dead) return;
        //refill dash
        if (dashRefill.fillAmount < 1) dashRefill.fillAmount += Time.deltaTime * 0.75f;
        if (thrustFill.fillAmount < 1) thrustFill.fillAmount += Time.deltaTime * thrustRechargeRate;
        if (thrust && thrustFill.fillAmount < 0.05f)
        {
            thrust = false;
            thrustB.transform.localScale = new Vector3(1, 1, 1 + (thrust ? 4 : 0));
        }
        else if (thrust) thrustFill.fillAmount -= Time.deltaTime * 0.15f;

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

        //pass inputs
        MoveDrone(thrust, joystick.Horizontal, joystick.Vertical, 6*Time.deltaTime * touchSens * touchRot.x, touchSens * 3*Time.deltaTime * touchRot.y, CrossPlatformInputManager.GetAxis("UpDown"));

        //PC Input
        //if (Input.GetKeyDown(KeyCode.LeftShift)) PressDash(false);
        //MoveDrone(thrust, Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),CrossPlatformInputManager.GetAxis("UpDown"));

        foreach (Transform fan in fans)
        {
            fan.Rotate(0, 0, 120);
        }
    }

    public void ThrustInput()
    {
        thrust = !thrust;
        if (thrustFill.fillAmount < 0.05f) thrust = false;
        thrustB.transform.localScale = new Vector3(1, 1, 1 + (thrust ? 4 : 0));
        thrustParticle.emissionRate = thrust ? 8 : 0;
    }

    void MoveDrone(bool thrust, float hor, float ver, float turn, float lean, float upDown)
    {
        //Hover above surface
        if (Physics.Raycast(transform.position, -transform.up, 0.5f + hoverHeight))
        {
            Rb.AddForce(transform.up * Rb.mass * 10);
        }
        Rb.AddForce(-Rb.mass * Physics.gravity);
        //Apply thrust
        Rb.AddForce(Rb.mass * transform.forward * ((thrust ? 40 : 0) + fanThrust * ver));
        Rb.AddForce(Rb.mass * transform.right * fanThrust * hor);

        //Turn
        //Rb.AddTorque(Vector3.up * turn * 200*Rb.mass*turnSens);

        //Lean
        //leanAngle = Mathf.Clamp(-Mathf.Rad2Deg*Mathf.Asin(lean),-70,70);
        //leanAngle = -60 * lean;
        //Debug.Log(leanAngle);
        //Rb.rotation = Quaternion.Slerp(Rb.rotation, Quaternion.Euler(leanAngle, transform.eulerAngles.y, 0),Time.deltaTime*2);
        //Rb.AddTorque(transform.right * lean * 80 * Rb.mass);

        //MouseLook
        Vector3 target = Quaternion.AngleAxis(-lean, this.transform.right) * transform.forward;
        transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(turn, Vector3.up) * target);
        Vector3 camRot = transform.eulerAngles;
        if (camRot.x > 60f && camRot.x < 90f) camRot.x = 60f;
        if (camRot.x < 300f && camRot.x > 270f) camRot.x = 300f;
        transform.eulerAngles = new Vector3(camRot.x, camRot.y, 0);

        //UpDown
        Rb.AddForce(Rb.mass * Vector3.up * 250 * upDown);

        UpdateAnimator(turn, ver);
        UpdateAudio(hor, ver, thrust);
    }

    void UpdateAnimator(float turn, float forward)
    {
        anim.SetFloat(turnHash, turn);
        anim.SetFloat(moveHash, forward);
    }

    void UpdateAudio(float hor, float ver, bool thrust)
    {
        PropellerSound.pitch = 1.5f + 0.5f * (hor * hor + ver * ver);
        EngineSound.volume = Mathf.Lerp(EngineSound.volume, thrust ? 0.6f : 0, Time.deltaTime);
    }

    public void PressDash()//bool mobileInput = true)
    {
        if (dashRefill.fillAmount < 1) return;
        if (true)
        {
            if (Mathf.Abs(joystick.Horizontal) < 0.1f) return;
            else if (joystick.Horizontal < 0.1f) StartCoroutine(Dash(true));
            else if (joystick.Horizontal > 0.1f) StartCoroutine(Dash(false));
        }
        else
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f) return;
            else if (Input.GetAxis("Horizontal") < 0.1f) StartCoroutine(Dash(true));
            else if (Input.GetAxis("Horizontal") > 0.1f) StartCoroutine(Dash(false));
        }
    }
    IEnumerator Dash(bool leftSide)
    {
        float startTime = Time.time;
        dashRefill.fillAmount = 0;
        if (leftSide)
        {
            thrustR.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.2f)
            {
                Rb.AddForce(-Rb.mass * transform.right * 15000 * Time.deltaTime);
                thrustR.startColor = new Color(thrustR.startColor.r, thrustR.startColor.g, thrustR.startColor.b, (startTime + 0.2f - Time.time) / 0.2f);
                yield return null;
            }
            thrustR.enabled = false;
            yield return new WaitForSeconds(0.2f);
            thrustL.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.6f)
            {
                Rb.AddForce(Rb.mass * transform.right * 12000 * Time.deltaTime);
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
                Rb.AddForce(Rb.mass * transform.right * 15000 * Time.deltaTime);
                thrustL.startColor = new Color(thrustL.startColor.r, thrustL.startColor.g, thrustL.startColor.b, (startTime + 0.2f - Time.time) / 0.2f);
                yield return null;
            }
            thrustL.enabled = false;
            yield return new WaitForSeconds(0.2f);
            thrustR.enabled = true;
            DashSound.Play();
            while (Time.time < startTime + 0.6f)
            {
                Rb.AddForce(-Rb.mass * transform.right * 12000 * Time.deltaTime);
                thrustR.startColor = new Color(thrustR.startColor.r, thrustR.startColor.g, thrustR.startColor.b, (startTime + 0.6f - Time.time) / 0.2f);
                yield return null;
            }
            thrustR.enabled = false;
        }
    }
    
}
