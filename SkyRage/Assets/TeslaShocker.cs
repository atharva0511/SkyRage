using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaShocker : MonoBehaviour {

    public float shockDelay = 10;
    public float shockDuration = 4;
    public float radius = 120;
    public float damage = 5;
    public GameObject lightning;
    public Transform spherePos;
    public float spread = 10;
    float lastShocked = 0;
    public LayerMask layerMask;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > lastShocked + shockDelay)
        {
            lastShocked = Time.time;
            StartCoroutine(Shock());
        }
	}

    IEnumerator Shock()
    {
        float startTime = Time.time;
        List<playerPlane> targets = new List<playerPlane>();
        Collider[] cols = Physics.OverlapSphere(spherePos.position, radius, 1 << 0);
        foreach (Collider col in cols)
        {
            playerPlane player = col.GetComponentInParent<playerPlane>();
            if (player!=null)
            {
                targets.Add(player);
                StartCoroutine(player.Stun(shockDuration));
            }
        }
        GameObject[] strikes = new GameObject[targets.Count];
        for(int i = 0; i < targets.Count; i++)
        {
            strikes[i] = Instantiate(lightning,spherePos.position,Quaternion.identity);
        }
        while (Time.time < startTime + shockDuration)
        {
            for(int i = 0; i < strikes.Length; i++)
            {
                Vector3 dir = targets[i].transform.position - spherePos.position;
                spherePos.rotation = Quaternion.LookRotation(dir);
                dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, spherePos.up) * dir;
                dir = Quaternion.AngleAxis((2 * Random.value - 1) * spread, spherePos.right) * dir;
                RaycastHit hit;
                if (Physics.Raycast(spherePos.position, targets[i].transform.position - spherePos.position, out hit, radius,layerMask))
                {
                    if (hit.transform == targets[i].transform)
                    {
                        strikes[i].SetActive(true);
                        strikes[i].transform.rotation = Quaternion.LookRotation(dir);
                        strikes[i].transform.localScale = new Vector3(strikes[i].transform.lossyScale.x, strikes[i].transform.lossyScale.y, dir.magnitude);
                        targets[i].TakeDamage(Time.deltaTime *4* damage*Random.value,0);
                    }
                    else
                    {
                        strikes[i].SetActive(false);
                    }
                }
                else
                {
                    strikes[i].SetActive(false);
                }
            }
            yield return null;
        }
        foreach(GameObject strike in strikes)
        {
            Destroy(strike);
        }
    }
}
