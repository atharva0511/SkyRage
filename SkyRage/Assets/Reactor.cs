using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Reactor : Destructible {

    public Transform displayPos;
    public Renderer[] rends;
    public GameObject laser;
    public GameObject explosion;
    public GameObject healthCanvas;
    public GameObject[] deleteOnDestruct;
    GameObject canvas;
    float lastHit = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Damaged()
    {
        base.Damaged();
        lastHit = Time.time;
        if (canvas == null && !dead)
        {
            StartCoroutine(DisplayHealth());
        }
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

    public override void Die()
    {
        base.Die();
        if (GetComponentInParent<BotSpawner>() != null)
        {
            GetComponentInParent<BotSpawner>().Died();
        }
        foreach (Renderer rend in rends)
        {
            Material[] mats = rend.materials;
            foreach (Material mat in mats) mat.color = new Color(0f, 0f, 0f);
            rend.materials = mats;
        }
        laser.SetActive(false);
        foreach (GameObject ob in deleteOnDestruct)
        {
            Destroy(ob);
        }
        Instantiate(explosion, transform.position + 3 * Vector3.up, Quaternion.identity);
    }
}
