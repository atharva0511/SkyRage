
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;
using System;

public class AdManager : MonoBehaviour {
    /* PlayerPrefs -- RewardType:
     * 0.coin
     * 1.UT
     * 2.Life
     * 3.Customize*/
    public FinishPanel finishPanel = null;
    public GamePanel gamePanel = null;
    RewardedAd lifeAd;
    RewardedAd UTAd;
    RewardedAd coinAd;
    RewardedAd customAd;

    InterstitialAd interstitial;

    string lifeAdID = "ca-app-pub-9623151168717383/1339739541";
    string UTAdID = "ca-app-pub-9623151168717383/1152871437";
    string coinAdID = "ca-app-pub-9623151168717383/4951040866";
    string customAdID = "ca-app-pub-9623151168717383/5759999153";

    string testVideoAdID = "ca-app-pub-3940256099942544/5224354917";
    string testInterstitialID = "ca-app-pub-3940256099942544/1033173712";

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

        this.interstitial = new InterstitialAd(testInterstitialID);

        //Appodeal.setRewardedVideoCallbacks(this);
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
        //PlayerPrefs.SetInt("RewardType", 2);
        //if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        //    Appodeal.show(Appodeal.REWARDED_VIDEO);
        //else
        //{
        //    if (finishPanel != null)
        //        finishPanel.ShowError();
        //    else
        //        gamePanel.ShowError();
        //}
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
        //PlayerPrefs.SetInt("RewardType", 1);
        //if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        //    Appodeal.show(Appodeal.REWARDED_VIDEO);
        //else
        //{
        //    if (finishPanel != null)
        //        finishPanel.ShowError();
        //    else
        //        gamePanel.ShowError();
        //}
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
        //PlayerPrefs.SetInt("RewardType", 0);
        //if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        //    Appodeal.show(Appodeal.REWARDED_VIDEO);
        //else
        //{
        //    if (finishPanel != null)
        //        finishPanel.ShowError();
        //    else
        //        gamePanel.ShowError();
        //}
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
        //PlayerPrefs.SetInt("RewardType", 0);
        //if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
        //    Appodeal.show(Appodeal.REWARDED_VIDEO);
        //else
        //{
        //    if (finishPanel != null)
        //        finishPanel.ShowError();
        //    else
        //        gamePanel.ShowError();
        //}
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
        Stats.data[11] += 1;
        FinishPanel.UnlockAchievement(GPGSIds.achievement_first_reward);
        FinishPanel.IncrementAchievement(GPGSIds.achievement_happy_watching, 1);
        FinishPanel.IncrementAchievement(GPGSIds.achievement_ad_lover, 1);
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

    public void AppodealInterstitial()
    {
        //Appodeal.show(Appodeal.INTERSTITIAL);
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }

    public static void AppodealInterstitialStatic()
    {
        //Appodeal.show(Appodeal.INTERSTITIAL);
    }

    public void AppodealVideo()
    {
        //if(Appodeal.isLoaded(Appodeal.NON_SKIPPABLE_VIDEO))
        //    Appodeal.show(Appodeal.NON_SKIPPABLE_VIDEO);
    }

//######################################################## Appodeal Rewarded video ################################
    //public void onRewardedVideoLoaded(bool precache)
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoFailedToLoad()
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoShowFailed()
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoShown()
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoFinished(double amount, string name)
    //{
    //    //Reward
    //    int type = PlayerPrefs.GetInt("RewardType");
    //    //double amount = args.Amount;
    //    Stats.data[11] += 1;
    //    FinishPanel.UnlockAchievement(GPGSIds.achievement_first_reward);
    //    FinishPanel.IncrementAchievement(GPGSIds.achievement_happy_watching, 1);
    //    FinishPanel.IncrementAchievement(GPGSIds.achievement_ad_lover, 1);
    //    MonoBehaviour.print(
    //        "HandleRewardedAdRewarded event received for "
    //                    + amount.ToString() + " " + type);
    //    if (finishPanel != null)
    //        finishPanel.RecieveReward(amount, type);
    //    else
    //    {
    //        gamePanel.PressCustomize();
    //    }
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoClosed(bool finished)
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoExpired()
    //{
    //    throw new NotImplementedException();
    //}

    //public void onRewardedVideoClicked()
    //{
    //    throw new NotImplementedException();
    //}

//######################################################################################################
}
