using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour {

    public Text coinText;
    public Text UTText;
    public Text lifeText;
    public Text levelText;
    public Image levelProgression;

    public static double[] xpProgress = new double[] {0,100,300,700,1500,2500,5000,7500,10000,12500,15000 };

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
        levelText.text = EvaluateLevel(PlayerData.xp).ToString();
    }

    int EvaluateLevel(double xp)
    {
        int level = 0;
        for (int i = 0; i < xpProgress.Length; i++)
        {
            if (xp >= xpProgress[i])
            {
                level = i;
            }
            else
            {
                levelProgression.fillAmount = ((int)(xp - xpProgress[i - 1])) / ((int)(xpProgress[i] - xpProgress[i - 1])) ;
                break;
            }
        }
        if (level != xpProgress.Length - 1) 
            levelProgression.fillAmount = float.Parse(((xp - xpProgress[level])/(xpProgress[level+1]-xpProgress[level])).ToString());
        return level;
    }

    public void AddCoins()
    {
        PlayerData.coins += 500;
        Refresh();
    }

    public void AddXP()
    {
        PlayerData.xp += 500;
        Refresh();
    }
}
