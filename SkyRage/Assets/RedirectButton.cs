using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectButton : MonoBehaviour
{
    public string redirectURL;
    
    public void OnClick()
    {
        Application.OpenURL(redirectURL);
    }
}
