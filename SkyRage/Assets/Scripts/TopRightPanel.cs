using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopRightPanel : MonoBehaviour {

    Transform playerPlane;
    public Image altitudeDisplay;
    float maxAltitude = 1000;
    public Text altitudeValue; 
	// Use this for initialization
	void Start () {
        playerPlane = GetComponentInParent<Destructible>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        altitudeDisplay.fillAmount = (playerPlane.position.y + 500) / maxAltitude;
        altitudeValue.text = ((int)(playerPlane.position.y + 5)).ToString() + " m";
	}
}
