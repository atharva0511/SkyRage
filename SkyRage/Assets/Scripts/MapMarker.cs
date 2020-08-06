using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarker : MonoBehaviour {

    public Transform player;
    public float radarRadius = 280;
    public bool stayOnRadar = true;
    public bool zone = false;
    public int zoneRadius = 0;
    public RectTransform zoneMarker;
    public BotSpawner spawner;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<EventSettings>().player;
        if (zone)
        {
            zoneMarker.gameObject.SetActive(true);
            zoneRadius = spawner.zoneRadius;
            zoneMarker.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 4*zoneRadius);
            zoneMarker.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 4 * zoneRadius);
        }
    }
	
    public void SetZone(int zoneRadius)
    {
        zoneMarker.gameObject.SetActive(true);
        zoneMarker.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2 * zoneRadius);
        zoneMarker.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2 * zoneRadius);
    }
	// Update is called once per frame
	void Update () {
        //Set height icon
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


        if (!stayOnRadar) return;
        Vector3 dir = Vector3.ProjectOnPlane(transform.position - player.position, Vector3.up);
        if(dir.sqrMagnitude > (radarRadius)*(radarRadius))
        {
            transform.GetChild(0).position = player.transform.position + dir.normalized * radarRadius;
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x, transform.position.y, transform.GetChild(0).position.z);
        }
        else
        {
            transform.GetChild(0).localPosition = Vector3.zero;
        }
	}

    void SetHeightDisplay(bool high,bool low)
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(high);
        transform.GetChild(0).GetChild(0).gameObject.SetActive(low);
    }
}
