using UnityEngine.UI;
using UnityEngine;

public class AccessCustomization : MonoBehaviour
{
    public Button useUT;
    public StatsPanel statsPanel;
    public int requiredUT = 3;

    // Start is called before the first frame update
    public void OnEnable()
    {
        if (PlayerData.upgradeTokens >= requiredUT)
        {
            useUT.interactable = true;
        }
        else
            useUT.interactable = false;
    }

    public void PayUT()
    {
        PlayerData.upgradeTokens -= requiredUT;
        statsPanel.Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
