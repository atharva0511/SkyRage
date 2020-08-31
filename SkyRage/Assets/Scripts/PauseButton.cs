using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    public GameObject pausePanel;
    public AudioSource audioSource;
    public EventSettings evSettings;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PressedPause();
            }
        }
	}

    public void PressedPause()
    {
        Time.timeScale = 0;
        audioSource.Play();
        AudioSource[] pauseAudio = evSettings.player.GetComponent<playerPlane>().pauseAudio;
        foreach(AudioSource aud in pauseAudio)
        {
            aud.Pause();
        }
        pausePanel.SetActive(true);
    }
}
