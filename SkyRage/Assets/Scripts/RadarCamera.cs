using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCamera : MonoBehaviour {

    public Transform playerPlane;
    public Transform playerMark;
    public static Transform currentObjective;
    public RectTransform screenMarker;
    public Transform activeCheckpoint;
	// Use this for initialization
	void Awake () {
        currentObjective = null;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(playerPlane.position.x,950, playerPlane.position.z);
        //transform.rotation = Quaternion.LookRotation(-Vector3.up);
        playerMark.position = transform.position - 500 * Vector3.up;
        playerMark.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(playerPlane.forward, Vector3.up));

        // On Screen Marker
        int border = 50;
        bool show = currentObjective != null;
        if (!show) { screenMarker.gameObject.SetActive(false); return; }
        screenMarker.gameObject.SetActive(true);
        Vector3 camCord = Camera.main.WorldToScreenPoint(currentObjective.transform.position);
        bool onScreen = camCord.x >= 0+border && camCord.x <= Screen.width-border && camCord.y >= 0+border && camCord.y <= Screen.height-border;
        Vector3 markerPos = camCord;
        if (Vector3.Dot(Camera.main.transform.forward, currentObjective.transform.position- Camera.main.transform.position)<0)
        {
            if (onScreen)
            {
                markerPos = new Vector3(camCord.x - Screen.width / 2, camCord.y - Screen.height / 2, 0);
                markerPos = 2 * Screen.width * markerPos.normalized;
            }
            if (markerPos.x <= 0 + border) markerPos.x = 0 + border;
            if (markerPos.x >= Screen.width - border) markerPos.x = Screen.width - border;
            if (markerPos.y <= 0 + border) markerPos.y = 0 + border;
            if (markerPos.y >= Screen.height - border) markerPos.y = Screen.height - border;
            markerPos.x = Screen.width - markerPos.x;
        }
        else
        {
            if (!onScreen)
            {
                if (markerPos.x <= 0 + border) markerPos.x = 0 + border;
                if (markerPos.x >= Screen.width - border) markerPos.x = Screen.width - border;
                if (markerPos.y <= 0 + border) markerPos.y = 0 + border;
                if (markerPos.y >= Screen.height - border) markerPos.y = Screen.height - border;
            }
        }
        markerPos.z = 0;
        screenMarker.position = markerPos;
    }

    public static void SetScreenMarker(Objective ob)
    {
        currentObjective = ob.transform;
    }
    public static void SetScreenMarker(Transform pos)
    {
        currentObjective = pos;
    }
}
