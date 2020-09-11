using GooglePlayGames;
using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class GPGManager : MonoBehaviour
{
    public static PlayGamesPlatform platform;
    public RawImage profileImage;
    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        if(platform==null)
            platform = PlayGamesPlatform.Activate();

        Social.Active.localUser.Authenticate(success => {
            if (success)
            {
                Debug.Log("Logged In");
                StartCoroutine(LoadImage());
            }
            else
                Debug.Log("Login failed");
        });
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
    }


    public void OnLogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
    }

    public IEnumerator LoadImage()
    {
        while (Social.localUser.image == null)
        {
            yield return null;
        }
        profileImage.color = new Color(1, 1, 1, 1);
        profileImage.texture = Social.localUser.image;
    }
}
