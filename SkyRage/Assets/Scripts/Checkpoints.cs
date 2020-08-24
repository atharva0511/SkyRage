using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : Objective {

    public GameObject arrow;
    public GameObject targetMarker;
    GameObject arrowTemp = null;
    GameObject markerTemp = null;
    public int xp = 10;
    public int finishedXP = 50;
    

	// Use this for initialization
	void Start () {
        NextCheckpoint(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void NextCheckpoint(int index)
    {
        if (index > transform.childCount-1)
        {
            return;
        }
        transform.GetChild(index).GetComponent<BoxCollider>().enabled = true;
        if(arrowTemp!=null)Destroy(arrowTemp);
        if (markerTemp != null) Destroy(markerTemp);
        if(index<transform.childCount-1)arrowTemp = Instantiate(arrow, transform.GetChild(index).position, Quaternion.LookRotation(transform.GetChild(index + 1).position - transform.GetChild(index).position), transform.GetChild(index));
        markerTemp = Instantiate(targetMarker, transform.GetChild(index).position, Quaternion.LookRotation(Vector3.up), transform.GetChild(index));
        Renderer renderer = transform.GetChild(index).GetComponent<Renderer>();
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1);
        RadarCamera.SetScreenMarker(transform.GetChild(index));
    }
}
