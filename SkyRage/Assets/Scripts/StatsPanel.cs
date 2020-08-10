using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

    public Text coinText;
    public Text UTText;
    public Text lifeText;

    void Awake()
    {
        BuyPanel.coinText = this.coinText;
        BuyPanel.UTText = this.UTText;
        BuyPanel.lifeText = this.lifeText;
    }

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        coinText.text = PlayerData.coins.ToString();
        UTText.text = PlayerData.upgradeTokens.ToString();
        lifeText.text = PlayerData.lives.ToString();
    }
}
