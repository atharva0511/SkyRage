using UnityEngine.UI;
using UnityEngine;

public class UpgradePanel : MonoBehaviour {

    public byte vehicleIndex;
    public int upgradeIndex;

    public byte tokens = 4;
    public Button upgradeButton;
    public Text btnText;
    public GameObject btnIcon;
	// Use this for initialization
	void Start () {
        RefreshPanel();
	}

    public void OnPressedUpgrade()
    {
        switch (vehicleIndex)
        {
            case 0: Upgrades.qDrone[upgradeIndex] = true; break;
            case 1: Upgrades.hod[upgradeIndex] = true; break;
            case 2: Upgrades.wDrone[upgradeIndex] = true; break;
            case 3: Upgrades.slayerX[upgradeIndex] = true; break;
            default: break;
        }
        PlayerData.upgradeTokens -= tokens;
        foreach (UpgradePanel up in transform.parent.GetComponentsInChildren<UpgradePanel>())
        {
            up.RefreshPanel();
        }
        Refresh();
    }

    void RefreshPanel()
    {
        if (tokens > PlayerData.upgradeTokens || CheckUpgrade())
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }
        if (CheckUpgrade())
        {
            btnText.text = "  Upgraded";
            btnIcon.SetActive(false);
        }
        else
        {
            btnText.text = "  Use  " + tokens.ToString();
            btnIcon.SetActive(true);
        }
    }

    bool CheckUpgrade()
    {
        switch (vehicleIndex)
        {
            case 0: return Upgrades.qDrone[upgradeIndex];
            case 1: return Upgrades.hod[upgradeIndex];
            case 2: return Upgrades.wDrone[upgradeIndex];
            case 3: return Upgrades.slayerX[upgradeIndex];
            default: return false;
        }
    }

    public void Refresh()
    {
        BuyPanel.coinText.text = PlayerData.coins.ToString();
        BuyPanel.UTText.text = PlayerData.upgradeTokens.ToString();
        BuyPanel.lifeText.text = PlayerData.lives.ToString();
    }
}
