using GooglePlayGames;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class GPGManager : MonoBehaviour
{
    public static PlayGamesPlatform platform;
    public RawImage profileImage;
    public GameObject ErrorPanel;
    public Text userName;
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        if (Social.localUser.authenticated || platform == null)
        {
            platform = PlayGamesPlatform.Activate();
            Social.Active.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Debug.Log("Logged In");
                    StartCoroutine(LoadImage());
                }
                else
                {
                    Debug.Log("Login failed");

                }
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        }
        else
        {
            ErrorPanel.SetActive(true);
        }
    }

    public void OnShowAchievement()
    {
        if(Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
        else
        {
            ErrorPanel.SetActive(true);
        }
    }

    public void OnLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public IEnumerator LoadImage()
    {
        string name = Social.localUser.userName;
        Debug.Log(name+" Logged In");
        while (Social.localUser.image == null)
        {
            yield return null;
            Debug.Log("Getting profile image");
        }
        Debug.Log("Profile image loaded");
        userName.text = Social.localUser.userName;
        if (name.Length <= 15)
        {
            userName.fontSize = 20;
        }
        else
        {
            userName.fontSize = 16;
        }
        profileImage.color = new Color(1, 1, 1, 1);
        profileImage.texture = Social.localUser.image as Texture;
    }
}
