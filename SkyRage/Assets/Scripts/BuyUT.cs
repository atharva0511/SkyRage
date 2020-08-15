
using UnityEngine.UI;

public class BuyUT : BuyPanel {


    public override void OnPressedBuy()
    {
        base.OnPressedBuy();
        PlayerData.upgradeTokens += 1;
        PlayerData.coins -= price;
        Refresh();
        base.OnPressedBuy();
    }

    public override void RefreshPanel()
    {
        base.RefreshPanel();
        detailsText.text = PlayerData.upgradeTokens.ToString();
        buyButton.transform.GetChild(0).GetComponent<Text>().text = "Pay " + price.ToString();
    }
}
