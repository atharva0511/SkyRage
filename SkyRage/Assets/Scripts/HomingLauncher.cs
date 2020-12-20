
using UnityEngine;
using UnityEngine.UI;

public class HomingLauncher : Weapons {

    public WeaponManager wm;
    public GameObject LockCanvas;
    public GameObject missileProjectile;
    GameObject lockInstance;
    Collider tempCol =null;
    public float lockDuration = 2;
    public float launchDist = 0;
    float lockTime = 3;
    public float damage = 150;
    Collider targetCol = null;
    public SkinnedMeshRenderer rend1;
    public SkinnedMeshRenderer rend2;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public Text rocketDisp;
    float blendValue = 0;
    float blendTime = 0;
    Transform target;
    public LayerMask layerMask;
    bool side = false;
    public int range = 300;
    public int rockets0 = 6;
    public int rockets1 = 12;
    int rcktSpeed = 120;
	// Use this for initialization
	void Start () {
        this.owner = GetComponentInParent<Destructible>().transform;
        rocketDisp.text = wm.rockets.ToString();
        //upgrades
        this.damage = CheckUpgrade(9) ? 100 : 50;
        wm.maxRockets = CheckUpgrade(10) ? rockets1 : rockets0;
        wm.rockets = wm.maxRockets;
        rocketDisp.text = wm.rockets.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!equipped) return;
        if (wm.rockets>0)//(slot == 1 && fire1) || (slot == 2 && fire2))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position+transform.forward * range/2, (range/2)-10, layerMask);
            float dot = 0;
            if (cols.Length == 0) targetCol = null;
            foreach (Collider col in cols)
            {
                
                if (Vector3.Dot((col.transform.position - transform.position).normalized, transform.forward) > dot)
                {
                    dot = Vector3.Dot((col.transform.position - transform.position).normalized, transform.forward);
                    //if (targetCol != col)
                    //{
                    //    targetCol = col;
                    //}
                    targetCol = col;
                }
            }
            if (targetCol != null)
            {
                if (targetCol != tempCol)
                {
                    lockTime = Time.time;
                    tempCol = targetCol;
                    if (lockInstance != null) lockInstance.GetComponent<AudioSource>().Play();
                }
                if (lockInstance == null)
                    lockInstance = Instantiate(LockCanvas, targetCol.transform.position, Quaternion.identity);
                lockInstance.transform.position = targetCol.transform.position;
                lockInstance.transform.LookAt(Camera.main.transform.position);
                lockInstance.GetComponent<Image>().color = Color.Lerp(Color.yellow, Color.red, (Time.time - lockTime) / lockDuration);
                if (Time.time > lockTime + lockDuration)
                {
                    target = targetCol.transform;
                }
            }
            else
            {
                if (lockInstance != null)
                {
                    Destroy(lockInstance);
                    target = null;
                }
            }
        }
        if (Time.time < blendTime + 1)
        {
            float val = Mathf.Lerp(rend1.GetBlendShapeWeight(0), blendValue, Time.time - blendTime);
            rend1.SetBlendShapeWeight(0, val);
            rend2.SetBlendShapeWeight(0, val);
        }
    }

    public override void PressedFire1()
    {
        base.PressedFire1();
        if (slot == 1)
        {
            blendTime = Time.time;
            lockTime = Time.time;
            blendValue = 100;
            Fire();
        }
    }

    public override void ReleaseFire1()
    {
        base.ReleaseFire1();
        if (slot != 1) return;
        blendTime = Time.time;
        blendValue = 0;
        Destroy(lockInstance);
        //Fire();
    }

    public override void PressedFire2()
    {
        base.PressedFire2();
        if (slot == 2)
        {
            blendTime = Time.time;
            lockTime = Time.time;
            blendValue = 100;
            Fire();
        }
    }

    public override void ReleaseFire2()
    {
        base.ReleaseFire2();
        if (slot != 2) return;
        blendTime = Time.time;
        blendValue = 0;
        Destroy(lockInstance);
        //Fire();
    }
    

    public void Fire()
    {
        if (!equipped || wm.rockets<=0) return;
        lockTime = Time.time;
        Projectile p = Instantiate(missileProjectile, transform.position+transform.forward*launchDist+transform.right*(side?2.2f:-2.2f), Quaternion.LookRotation(transform.forward)).GetComponent<Projectile>();
        if (side) ps1.Play();
        else ps2.Play();
        side = !side;
        if(!wm.infiniteRocket)wm.rockets -= 1;
        UpdateDisp();
        if (this.target != null)
        {
            p.homing = true;
            p.target = this.target;
        }
        else
        {
            p.homing = false;
            p.moveLinearly = true;
        }
        p.weapon = Destructible.WeaponName.MissileLauncher;
        p.Shooter = this.owner;
        p.speed = rcktSpeed;
        p.turnRate = 60;
        p.damage = this.damage;
        target = null;
    }

    public void UpdateDisp()
    {
        rocketDisp.text = wm.rockets.ToString();
    }

    public bool CheckUpgrade(int upgradeIndex)
    {
        switch (wm.vehicleIndex)
        {
            case 0: return Upgrades.qDrone[upgradeIndex];
            case 1: return Upgrades.hod[upgradeIndex];
            case 2: return Upgrades.wDrone[upgradeIndex];
            case 3: return Upgrades.slayerX[upgradeIndex];
            default: return false;
        }
    }
}
