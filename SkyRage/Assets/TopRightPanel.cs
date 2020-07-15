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
        playerPlane = GetComponentInParent<Plane>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        altitudeDisplay.fillAmount = playerPlane.position.y / maxAltitude;
        altitudeValue.text = ((int)(playerPlane.position.y*2f + 5)).ToString() + " m";
	}
}
