using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour {

    public Text coinText;
    public Text utText;
    public Text lifeText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(int coins)
    {
        coinText.text = coins.ToString();
        utText.text = PlayerData.upgradeTokens.ToString();
        lifeText.text = PlayerData.lives.ToString();
    }
}
