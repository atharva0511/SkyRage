using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarker : MonoBehaviour {

    public Transform player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<EventSettings>().player;
    }
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(transform.position.y - player.position.y) < 20)
        {
            SetHeightDisplay(false, false);
        }
        else if (transform.position.y - player.position.y > 0)
        {
            SetHeightDisplay(true, false);
        }
        else
            SetHeightDisplay(false, true);
	}

    void SetHeightDisplay(bool high,bool low)
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(high);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(low);
    }
}
