using UnityEngine.UI;
using UnityEngine;

public class BuyLife : BuyPanel {

    public override void OnPressedBuy()
    {
        if (PlayerData.lives < 5)
        {
            PlayerData.lives += 1;
            PlayerData.coins -= price;
        }
        Refresh();
    }

    public override void RefreshPanel()
    {
        base.RefreshPanel();
        if(PlayerData.lives >= 5)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
        detailsText.text = PlayerData.lives.ToString() + " / 5";
        buyButton.transform.GetChild(0).GetComponent<Text>().text = "Pay " + price.ToString();
    }
}
