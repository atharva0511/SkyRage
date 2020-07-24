using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    public GameObject pausePanel;
    public AudioSource audioSource;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PressedPause()
    {
        Time.timeScale = 0;
        audioSource.Play();
        pausePanel.SetActive(true);
    }
}
