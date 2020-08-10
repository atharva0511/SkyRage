using UnityEngine.UI;
using UnityEngine;

public class BuyPanel : MonoBehaviour {

    public static Text coinText;
    public static Text UTText;
    public static Text lifeText;
    public Text detailsText;
    public Button buyButton;
    public int price = 100;
    
    public void Start()
    {
        RefreshPanel();
    }
	
    public virtual void OnPressedBuy()
    {

    }

    public virtual void RefreshPanel()
    {
        if (price > PlayerData.coins)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public void Refresh()
    {
        coinText.text = PlayerData.coins.ToString();
        UTText.text = PlayerData.upgradeTokens.ToString();
        lifeText.text = PlayerData.lives.ToString();
    }
}
