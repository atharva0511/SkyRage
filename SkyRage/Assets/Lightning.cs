using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    public LineRenderer line;
	// Use this for initialization
	void Start () {
		
	}
	


	// Update is called once per frame
	void Update () {
            line.startWidth = 3.5f + 5 * Random.value;
            line.endWidth = 3.5f + 5 * Random.value;
    }


}
