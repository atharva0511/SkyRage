using UnityEngine.UI;
using UnityEngine;

public class ColorPreset : MonoBehaviour {

    public ColorPanel colorPanel;
    public Button img;

    public void ClickedPreset()
    {
        img = GetComponent<Button>();
        colorPanel = GetComponentInParent<ColorPanel>();
        colorPanel.FromPreset(img.colors.normalColor);
    }
}
