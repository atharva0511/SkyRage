using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCamera : MonoBehaviour {

    public Transform playerPlane;
    public Transform playerMark;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerPlane.position.x,950, playerPlane.position.z);
        //transform.rotation = Quaternion.LookRotation(-Vector3.up);
        playerMark.position = transform.position - 500 * Vector3.up;
        playerMark.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(playerPlane.forward, Vector3.up));
	}
}
