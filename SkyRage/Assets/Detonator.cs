using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Detonator : MonoBehaviour {

    public Text timerText;
    public int duration = 30;
	// Use this for initialization
	void Start () {
		
	}
	
    public void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<playerPlane>() != null)
        {
            StartCoroutine(DisplayTimer(duration));
            GetComponent<Collider>().enabled = false;
        }
        
    }

	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator DisplayTimer(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            //timerText.text = 
            yield return new WaitForSeconds(1);
        }
    }
}
