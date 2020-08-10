using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomingLauncher : Weapons {

    public GameObject LockCanvas;
    public GameObject missileProjectile;
    GameObject lockInstance;
    Collider tempCol =null;
    public float lockDuration = 2;
    float lockTime = 3;
    public float damage = 150;
    Collider targetCol = null;
    public SkinnedMeshRenderer rend1;
    public SkinnedMeshRenderer rend2;
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    float blendValue = 0;
    float blendTime = 0;
    Transform target;
    public LayerMask layerMask;
    bool side = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if ((slot == 1 && fire1) || (slot == 2 && fire2))
        {
            Collider[] cols = Physics.OverlapSphere(transform.position+transform.forward * 150, 140, layerMask);
            float dot = 0;
            if (cols.Length == 0) targetCol = null;
            Debug.Log(cols.Length);
            foreach (Collider col in cols)
            {
                if (Vector3.Dot((col.transform.position - transform.position).normalized, transform.forward) > dot)
                {
                    if (targetCol != col)
                    {
                        targetCol = col;
                    }
                }
            }
            if (targetCol != null)
            {
                if (targetCol != tempCol)
                {
                    lockTime = Time.time;
                    tempCol = targetCol;
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
                Debug.Log(targetCol.name);
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
        }
    }

    public override void ReleaseFire1()
    {
        base.ReleaseFire1();
        if (slot != 1) return;
        blendTime = Time.time;
        blendValue = 0;
        Destroy(lockInstance);
        Fire();
    }

    public override void PressedFire2()
    {
        base.PressedFire2();
        if (slot == 2)
        {
            blendTime = Time.time;
            lockTime = Time.time;
            blendValue = 100;
        }
    }

    public override void ReleaseFire2()
    {
        base.ReleaseFire2();
        if (slot != 2) return;
        blendTime = Time.time;
        blendValue = 0;
        Destroy(lockInstance);
        Fire();
    }

    public IEnumerator StartLockOn()
    {
        while((slot == 1 && fire1) || (slot == 2 && fire2))
        {
            
            yield return null;
        }
    }

    public void Fire()
    {
        lockTime = Time.time;
        Projectile p = Instantiate(missileProjectile, transform.position+transform.right*(side?2.2f:-2.2f), Quaternion.LookRotation(transform.forward)).GetComponent<Projectile>();
        if (side) ps1.Play();
        else ps2.Play();
        side = !side;
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
        p.speed = 120;
        p.turnRate = 60;
        p.damage = this.damage;
        target = null;
    }
}
