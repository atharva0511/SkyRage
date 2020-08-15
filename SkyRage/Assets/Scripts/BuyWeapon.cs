using UnityEngine.UI;

public class BuyWeapon : BuyPanel {

	public byte weaponIndex = 1;

    public override void OnPressedBuy()
    {
        switch (vehicleIndex)
        {
            case 0:Upgrades.qDroneWeapons[weaponIndex] = true;break;
            case 1:Upgrades.hodWeapons[weaponIndex] = true;break;
            case 2:Upgrades.wDroneWeapons[weaponIndex] = true;break;
            case 3:Upgrades.slayerXWeapons[weaponIndex] = true;break;
        }
        PlayerData.coins-=this.price;
        Refresh();
        base.OnPressedBuy();
    }

    public override void RefreshPanel()
    {
        if (price > PlayerData.coins || CheckWeapon())
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
        if (CheckWeapon())
        {
            detailsText.text = "Owned";
        }
        else detailsText.text = " ";
        buyButton.transform.GetChild(0).GetComponent<Text>().text = "Pay " + price.ToString();
    }

    bool CheckWeapon()
    {
        switch (vehicleIndex)
        {
            case 0: return Upgrades.qDroneWeapons[weaponIndex];
            case 1: return Upgrades.hodWeapons[weaponIndex];
            case 2: return Upgrades.wDroneWeapons[weaponIndex];
            case 3: return Upgrades.slayerXWeapons[weaponIndex];
            default: return false;
        }
    }
}
