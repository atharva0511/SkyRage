using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class EventSettings : MonoBehaviour {

    public int currentObjective = 0;
    public Objective[] objectives;

    public static Transform currentPlayer;
    public Transform player;
    playerPlane character;
    public UISettings uiSettings;

    [Header("Music")]
    public bool constMusic = false;
    public AudioMixer audioMixer;
    // Use this for initialization
    void Awake()
    {
        currentPlayer = player;
    }

	void Start () {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        objectives[0].Activate();
        RadarCamera.SetScreenMarker(objectives[0]);
        character = player.GetComponent<playerPlane>();
        StartCoroutine(SetMusic(false));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Finished()
    {
        uiSettings.OnFinish(character.coins);
    }

    public void NextObjective()
    {
        Debug.Log("Next Objective");
        currentObjective += 1;
        if (currentObjective < objectives.Length)
        {
            if (objectives[currentObjective] == null)
            {
                NextObjective();
                return;
            }
            objectives[currentObjective].gameObject.SetActive(true);
            objectives[currentObjective].Activate();
            if (objectives[currentObjective].completed)
            {
                NextObjective();
            }
            RadarCamera.SetScreenMarker(objectives[currentObjective]);
            if (objectives[currentObjective].objectiveType == Objective.ObjectiveType.Destroy)
            {
                StartCoroutine(SetMusic(true));
            }
            else
                StartCoroutine(SetMusic(false));
        }
        else
        {
            Finished();
        }
        
    }
    
    public void PlayerDead()
    {
        uiSettings.PlayerDead();
    }

    void LoadObjective(int index)
    {
        for(int i = 0; i < objectives.Length; i++)
        {
            if (i < index)
            {
                objectives[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetDesc(string desc)
    {
        uiSettings.SetObDescription(desc);
    }

    public IEnumerator SetMusic(bool combat)
    {
        if (!constMusic)
        {
            float startTime = Time.time;
            float ambVol;
            float combVol;
            audioMixer.GetFloat("AmbientVolume", out ambVol);
            audioMixer.GetFloat("CombatVolume", out combVol);
            while (Time.time < startTime + 5)
            {
                audioMixer.SetFloat("AmbientVolume", Mathf.Lerp(ambVol, combat ? -80 : 0, (Time.time - startTime) / 5));
                audioMixer.SetFloat("CombatVolume", Mathf.Lerp(combVol, combat ? 0 : -80, (Time.time - startTime) / 5));
                if (combat)
                    Debug.Log(combVol);
                yield return null;
            }
        }
    }
}
