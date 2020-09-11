using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeManager : MonoBehaviour {

    public RadarCamera radarCamera;
    public EventSettings eventSettings;
    public GameObject[] presets;
    public int currentZone = 0;
    [HideInInspector]
    public GameObject currentPreset;
    int presetIndex = 0;
    bool spawnedUT = true;
    bool spawnedLife = true;
	// Use this for initialization
	void Awake () {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            PlayerPrefs.SetInt("Difficulty", 2);
        }
        int veh = 0;
        if (PlayerPrefs.HasKey("vehicleIndex"))
        {
            veh = PlayerPrefs.GetInt("vehicleIndex");
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == veh)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        radarCamera.playerPlane = transform.GetChild(veh);
        eventSettings.player = transform.GetChild(veh);
        EventSettings.currentPlayer = transform.GetChild(veh);

        NextPreset();
	}
	

    public void NextPreset()
    {
        currentZone += 1;
        ChoosePreset();
        int choice = ChoosePreset();
        while(currentPreset == presets[choice])
        {
            choice = ChoosePreset();
        }
        Destroy(currentPreset);
        currentPreset = Instantiate(presets[choice]);
        ArcadePreset ap = currentPreset.GetComponent<ArcadePreset>();
        eventSettings.objectives = ap.objectives;
        if (Random.value>0.5f)
        {
            if (!spawnedUT)
            {
                spawnedUT = true;
                ap.SpawnUT();
            }
            else if (!spawnedLife)
            {
                spawnedLife = true;
                ap.SpawnLife();
            }
        }
    }

    public int ChoosePreset()
    {
        int maxDraw;
        if (currentZone < 3)
        {
            maxDraw = 4;
        }
        else
            maxDraw = presets.Length;

        if(currentZone>1 && currentZone % 5 == 0)
        {
            spawnedLife = false;
            spawnedUT = false;
        }

        if (currentZone==3)
        {
            spawnedLife = false;
            spawnedUT = false;
        }

        return Random.Range(1, maxDraw);
    }
}
