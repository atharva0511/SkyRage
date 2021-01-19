using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishPanel : MonoBehaviour {

    public Text coinText;
    public Text utText;
    public Text lifeText;
    public UISettings uiSettings;
    public GameObject errorPanel;
    public GameObject rewardPanel;
    public Text rewardText;
    public Button coinAdButton;
    public Button lifeAdButton;
    public Button utAdButton;
    public bool arcade = false;
	// Use this for initialization
	void Start () {
        uiSettings.PauseAudio();
        if (arcade)
        {
            SetText(FindObjectOfType<playerPlane>().coins);
            ScoreToLeaderBoard(int.Parse(coinText.text));
        }
        switch (SceneManager.GetActiveScene().name)
        {
            case "Mission_A_1": UnlockAchievement(GPGSIds.achievement_now_i_know_it); AdditionalReward(400); break;
            case "Mission_A_5": UnlockAchievement(GPGSIds.achievement_sky_monarch); CampaignReward(0); break;
            case "Mission_B_5": UnlockAchievement(GPGSIds.achievement_wrath_god); CampaignReward(1); break;
            case "Mission_C_5": UnlockAchievement(GPGSIds.achievement_angel_of_destruction); CampaignReward(2); break;
            case "Mission_D_5": UnlockAchievement(GPGSIds.achievement_lone_legend); CampaignReward(3); break;
            default: break;
        }
        CheckAchievements();
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

    public void ProceedToMenu()
    {
        PlayerPrefs.SetInt("SetMenu", 1);
        if (!arcade)
        {
            string[] mission = SceneManager.GetActiveScene().name.Split('_');
            int i = 0;
            switch (mission[1])
            {
                case "A": i = 0; break;
                case "B": i = 1; break;
                case "C": i = 2; break;
                case "D": i = 3; break;
                default: i = 0; break;
            }
            if (int.Parse(mission[2]) > PlayerData.levelProgression[i])
            {
                PlayerData.levelProgression[i] = int.Parse(mission[2]);
            }
        }
        PlayerData.coins += int.Parse(coinText.text);
        PlayerData.SaveData();
        Upgrades.Save();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveAndRestart()
    {
        if (!arcade)
        {
            string[] mission = SceneManager.GetActiveScene().name.Split('_');
            int i = 0;
            switch (mission[1])
            {
                case "A": i = 0; break;
                case "B": i = 1; break;
                case "C": i = 2; break;
                case "D": i = 3; break;
                default: i = 0; break;
            }
            if (int.Parse(mission[2]) > PlayerData.levelProgression[i])
            {
                PlayerData.levelProgression[i] = int.Parse(mission[2]);
            }
            
        }
        PlayerData.coins += int.Parse(coinText.text);
        PlayerData.SaveData();
        Time.timeScale = 1;
        uiSettings.Restart();
    }

    public void RecieveReward(double amount,string type)
    {
        switch (PlayerPrefs.GetInt("RewardType"))
        {
            case 0:
                coinAdButton.interactable = false;
                coinText.text = (int.Parse(coinText.text) + 200).ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received " + "200" + " rage coins";
                break;
            case 2:
                lifeAdButton.interactable = false;
                PlayerData.lives += 1;
                lifeText.text = PlayerData.lives.ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received +" + "1" + " bonus life";
                break;
            case 1:
                utAdButton.interactable = false;
                PlayerData.upgradeTokens += 2;
                utText.text = PlayerData.upgradeTokens.ToString();
                rewardPanel.SetActive(true);
                rewardText.text = "You received " + "2" + " upgrade token";
                break;
        }
        coinAdButton.interactable = false;
        lifeAdButton.interactable = false;
        utAdButton.interactable = false;
    }

    public void ShowError()
    {
        errorPanel.SetActive(true);
    }

    public void OnShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            ShowError();
        }
    }

    public void ScoreToLeaderBoard(int score)
    {
        string[] ids = new string[] { GPGSIds.leaderboard_quadrone_arcade,GPGSIds.leaderboard_harbinger_of_death_arcade,GPGSIds.leaderboard_wingeddrone_arcade,GPGSIds.leaderboard_slayerx_arcade};
        int index = 0;
        if (PlayerPrefs.HasKey("vehicleIndex"))
        {
            index = PlayerPrefs.GetInt("vehicleIndex");
        }
        if (Stats.data[15 + index] < score) {
            Stats.data[15 + index] = score;
        }
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, ids[index],(bool success)=> {
                if (success)
                {
                    Debug.Log("score uploaded to leaderboard");
                }
                else
                {
                    Debug.Log("Error uploading score");
                }
            });
        }
    }

    public static void UnlockAchievement(string id)
    {
        if(Social.localUser.authenticated)
            Social.ReportProgress(id, 100, success => { });
    }

    public static void IncrementAchievement(string id,int incrementBy)
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(id, incrementBy, success => { });
        }
    }

    public void RateApp()
    {
        #if UNITY_ANDROID
            Application.OpenURL("market://details?id=com.ExperienceInfinity.SkyRage");
        #endif
    }


    public static void CheckAchievements()
    {
        StatsData stat1 = LoadData();
        int[] gains = new int[stat1.data.Length];
        for (int i = 0; i < stat1.data.Length; i++)
        {
            gains[i] = Stats.data[i] - stat1.data[i];
        }
        #region enemyKills
        IncrementAchievement(GPGSIds.achievement_kaboom, gains[0]);

        IncrementAchievement(GPGSIds.achievement_minibot_dominator, gains[1]);
        IncrementAchievement(GPGSIds.achievement_minibot_annihilator, gains[1]);
        IncrementAchievement(GPGSIds.achievement_minibot_dominator, gains[1]);

        IncrementAchievement(GPGSIds.achievement_turret_buster, gains[2]);
        IncrementAchievement(GPGSIds.achievement_turret_annihilator, gains[2]);
        IncrementAchievement(GPGSIds.achievement_turret_slayer, gains[2]);

        IncrementAchievement(GPGSIds.achievement_missile_turret_eliminator, gains[3]);
        IncrementAchievement(GPGSIds.achievement_missile_turret_annihilator, gains[3]);
        IncrementAchievement(GPGSIds.achievement_missile_turret_slayer, gains[3]);

        IncrementAchievement(GPGSIds.achievement_knight_drone_annihilator, gains[4]);
        IncrementAchievement(GPGSIds.achievement_knight_drone_dominator, gains[4]);
        IncrementAchievement(GPGSIds.achievement_knight_drone_slayer, gains[4]);

        IncrementAchievement(GPGSIds.achievement_enforcer_dominator, gains[5]);
        IncrementAchievement(GPGSIds.achievement_enforcer_drone_annihilator, gains[5]);
        IncrementAchievement(GPGSIds.achievement_enforcer_slayer, gains[5]);
        #endregion
        #region weaponKills
        IncrementAchievement(GPGSIds.achievement_gunslinger, gains[6]);
        IncrementAchievement(GPGSIds.achievement_gunslinger_ii, gains[6]);
        IncrementAchievement(GPGSIds.achievement_gunslinger_iii, gains[6]);

        IncrementAchievement(GPGSIds.achievement_missile_hunter, gains[7]);
        IncrementAchievement(GPGSIds.achievement_missile_hunter_ii, gains[7]);
        IncrementAchievement(GPGSIds.achievement_missile_hunter_iii, gains[7]);

        IncrementAchievement(GPGSIds.achievement_burn_it_down, gains[9]);
        IncrementAchievement(GPGSIds.achievement_burn_it_down_ii, gains[9]);
        IncrementAchievement(GPGSIds.achievement_burn_it_down_iii, gains[9]);

        IncrementAchievement(GPGSIds.achievement_stun_master, gains[8]);
        IncrementAchievement(GPGSIds.achievement_stun_master_ii, gains[8]);
        IncrementAchievement(GPGSIds.achievement_stun_master_iii, gains[8]);
        #endregion
        #region exploration
        IncrementAchievement(GPGSIds.achievement_handy_tokens, gains[20]);
        IncrementAchievement(GPGSIds.achievement_handy_tokens_ii, gains[20]);
        IncrementAchievement(GPGSIds.achievement_handy_tokens_iii, gains[20]);

        IncrementAchievement(GPGSIds.achievement_treasure_hunter, gains[19]);
        IncrementAchievement(GPGSIds.achievement_treasure_hunter_ii, gains[19]);
        IncrementAchievement(GPGSIds.achievement_treasure_hunt_iii, gains[19]);

        IncrementAchievement(GPGSIds.achievement_elixir_collector, gains[21]);
        #endregion
        if(gains[12]>0)
            UnlockAchievement(GPGSIds.achievement_near_death_experience);
    }

    static StatsData LoadData()
    {
        string path = Application.persistentDataPath + "/StatsData.amg";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Open);
        StatsData data = formatter.Deserialize(file) as StatsData;
        file.Close();
        return data;
    }

    void CampaignReward(int droneIndex)
    {
        if (PlayerData.levelProgression[droneIndex] < 5)
        {
            PlayerData.coins += (droneIndex * 200 + 600);
            PlayerData.xp += (500 + 500 * droneIndex);
        }
    }

    void AdditionalReward(int coins)
    {
        if (PlayerData.levelProgression[0] < 1)
        {
            PlayerData.coins += coins;
        }
    }
}
