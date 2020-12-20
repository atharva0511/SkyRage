using System.Collections;
using UnityEngine.Audio;
using UnityEngine;

public class EventSettings : MonoBehaviour {

    public static bool inCombat = false;
    public static int loadObjective = 0;
    public static int currentCoins = 0;
    bool end = false; // finished or died

    public enum GameMode {campaign,arcade};
    public GameMode gameMode = GameMode.campaign;

    public int currentObjective = 0;
    public Objective[] objectives;

    public static Transform currentPlayer;
    public Transform player;
    playerPlane character;
    public UISettings uiSettings;

    [Header("Music")]
    public bool constMusic = false;
    public AudioMixer audioMixer;
    public AudioClip finishMusic;

    [Header("Arcade")]
    public ArcadeManager arcadeManager;
    // Use this for initialization
    void Awake()
    {
        currentPlayer = player;
        currentObjective = 0;
        if (PlayerPrefs.GetInt("LoadCheckpoint") == 1)
        {
            LoadObjective();
        }
    }

	void Start () {
        Time.timeScale = 1;
        Application.targetFrameRate = 60;
        objectives[currentObjective].Activate();
        if(objectives[currentObjective].objectiveType != Objective.ObjectiveType.Checkpoints)
            RadarCamera.SetScreenMarker(objectives[currentObjective]);
        character = player.GetComponent<playerPlane>();
        //StartCoroutine(SetMusic(false));
        if (!constMusic)
        {
            inCombat = false;
            audioMixer.SetFloat("CombatVolume", -80);
            StartCoroutine(CheckCombatStat());
        }
        else
        {
            audioMixer.SetFloat("AmbientVolume", 0);
            audioMixer.SetFloat("CombatVolume", -80);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Finished()
    {
        end = true;
        inCombat = false;
        if (arcadeManager == null)
        {
            transform.GetChild(0).GetComponent<AudioSource>().clip = finishMusic;
            //StartCoroutine(SetMusic(false));
            audioMixer.SetFloat("AmbientVolume", 0);
            audioMixer.SetFloat("CombatVolume", -80);
            transform.GetChild(0).GetComponent<AudioSource>().volume = 0.4f;
            transform.GetChild(0).GetComponent<AudioSource>().Play();
        }
        uiSettings.OnFinish(character.coins);
    }

    public void NextObjective()
    {
        currentObjective += 1;
        if (currentObjective < objectives.Length)
        {
            //if (objectives[currentObjective] == null)
            //{
            //    NextObjective();
            //    return;
            //}
            objectives[currentObjective].gameObject.SetActive(true);
            objectives[currentObjective].Activate();
            if (objectives[currentObjective].completed)
            {
                NextObjective();
            }
            RadarCamera.SetScreenMarker(objectives[currentObjective]);
            //if (objectives[currentObjective].objectiveType == Objective.ObjectiveType.Destroy)
            //{
            //    StartCoroutine(SetMusic(true));
            //}
            //else
            //    StartCoroutine(SetMusic(false));
        }
        else
        {
            if(gameMode == GameMode.campaign)
                Finished();
            else if(gameMode == GameMode.arcade)
            {
                arcadeManager.NextPreset();
                currentObjective = 0;
            }
        }
        loadObjective = currentObjective;
    }
    
    public void PlayerDead()
    {
        end = true;
        uiSettings.PlayerDead();
        //StartCoroutine(SetMusic(false));
        if (arcadeManager == null)
        {
            audioMixer.SetFloat("AmbientVolume", -80);
            audioMixer.SetFloat("CombatVolume", -80);
        }
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
                
                yield return null;
            }
        }
    }

    public void SetTurnSens(float s)
    {
        character.SetTurnSensitivity(s);
        character.SetJoystickType();
    }

    IEnumerator lerpVolume(AudioSource music)
    {
        float startTime = Time.time;
        while (Time.time < startTime + 3)
        {
            music.volume = Mathf.Lerp(0, 1, (Time.time - startTime) / 3); 
            yield return null;
        }
    }
    
    IEnumerator CheckCombatStat()
    {
        bool lastStat = false;
        while (!end)
        {
            Debug.Log(inCombat);
            if (inCombat != lastStat)
            {
                StartCoroutine(SetMusic(inCombat));
            }
            lastStat = inCombat;
            inCombat = false;
            yield return new WaitForSeconds(5);
        }
    }

    public void LoadObjective()
    {
        player.GetComponent<playerPlane>().coins = currentCoins;
        player.GetComponent<playerPlane>().SetCoinDisplay();
        currentCoins = 0;
        for(int i=0;i<loadObjective;i++)
        {
            foreach(GameObject go in objectives[i].DestroyOnCompletion)
            {
                if(go!=null)
                    Destroy(go);
            }
            foreach(GameObject go in objectives[i].ActivateOnCompletion)
            {
                if (go != null)
                    go.SetActive(true);
            }
            if (i == loadObjective - 1)
            {
                if (objectives[i >= 0 ? i : 0].GetComponent<Portal>() == null)
                    player.position = objectives[i >= 0 ? i : 0].transform.position + Vector3.up * 5;
                else
                    player.position = objectives[i >= 0 ? i : 0].GetComponent<Portal>().destination.position;
            }
            Destroy(objectives[i].gameObject);
        }
        currentObjective = loadObjective;
        objectives[currentObjective].gameObject.SetActive(true);
    }
}
