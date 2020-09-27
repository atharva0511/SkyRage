
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour {

    public FinishPanel finishPanel = null;
    public GamePanel gamePanel = null;
    RewardedAd lifeAd;
    RewardedAd UTAd;
    RewardedAd coinAd;
    RewardedAd customAd;

    string lifeAdID = "ca-app-pub-9623151168717383/1339739541";
    string UTAdID = "ca-app-pub-9623151168717383/1152871437";
    string coinAdID = "ca-app-pub-9623151168717383/4951040866";
    string customAdID = "ca-app-pub-9623151168717383/5759999153";

    string testVideoAdID = "ca-app-pub-3940256099942544/5224354917";

    // Use this for initialization
    void Start () {
        //lifeAd = CreateAndLoadRewardedAd(testVideoAdID);
        //UTAd = CreateAndLoadRewardedAd(testVideoAdID);
        //coinAd = CreateAndLoadRewardedAd(testVideoAdID);
        //customAd = CreateAndLoadRewardedAd(testVideoAdID);
        lifeAd = CreateAndLoadRewardedAd(lifeAdID);
        UTAd = CreateAndLoadRewardedAd(UTAdID);
        coinAd = CreateAndLoadRewardedAd(coinAdID);
        if (customAd == null)
        {
            customAd = CreateAndLoadRewardedAd(customAdID);
        }
        else if (!customAd.IsLoaded())
        {
            customAd = CreateAndLoadRewardedAd(customAdID);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    
    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("338305FCF33DCCE2").AddTestDevice("342D62625C6B3CEE").Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
        return rewardedAd;
    }
    
    public void ShowLifeAd()
    {
        if (lifeAd.IsLoaded())
        {
            lifeAd.Show();
        }
        else
        {
            if (finishPanel != null)
                finishPanel.ShowError();
            else
                gamePanel.ShowError();
        }
    }

    public void ShowUTAd()
    {
        if (UTAd.IsLoaded())
        {
            UTAd.Show();
        }
        else
        {
            if (finishPanel != null)
                finishPanel.ShowError();
            else
                gamePanel.ShowError();
        }
    }

    public void ShowCoinAd()
    {
        if (coinAd.IsLoaded())
        {
            coinAd.Show();
        }
        else
        {
            if (finishPanel != null)
                finishPanel.ShowError();
            else
                gamePanel.ShowError();
        }
    }

    public void ShowCustomAd()
    {
        if (customAd.IsLoaded())
        {
            customAd.Show();
        }
        else
        {
            if (finishPanel != null)
                finishPanel.ShowError();
            else
                gamePanel.ShowError();
        }
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
        if (finishPanel != null)
            finishPanel.ShowError();
        else
            gamePanel.ShowError();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        //###################################### load customize add again #######################################
        //lifeAd = CreateAndLoadRewardedAd(testVideoAdID);
        //UTAd = CreateAndLoadRewardedAd(testVideoAdID);
        //coinAd = CreateAndLoadRewardedAd(testVideoAdID);
        if (customAd == null)
        {
            customAd = CreateAndLoadRewardedAd(customAdID);
        }
        else if (!customAd.IsLoaded())
        {
            customAd = CreateAndLoadRewardedAd(customAdID);
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        if(finishPanel!=null)
            finishPanel.RecieveReward(amount, type);
        else
        {
            gamePanel.PressCustomize();
        }
    }
}
