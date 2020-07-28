using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour {

    public Renderer vehicleRenderer;
    public int materialIndex;

    public Slider selectR;
    public Slider selectG;
    public Slider selectB;
    public Image previewColor;
    public Color finalColor;
	// Use this for initialization
	void Start () {
        ReadColor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // called at start
    public void ReadColor()
    {
        finalColor = vehicleRenderer.materials[materialIndex].color;
        SetRGBUI(finalColor);
    }

    void SetRGBUI(Color color)
    {
        selectR.normalizedValue = color.r;
        selectG.normalizedValue = color.g;
        selectB.normalizedValue = color.b;
        SetPreviewColor(color);
    }

    //called from rgb sliders
    public void OnColorChanged()
    {
        finalColor = new Color(selectR.normalizedValue, selectG.normalizedValue, selectB.normalizedValue);
        SetPreviewColor(finalColor);
        SetRenderer(selectR.normalizedValue, selectG.normalizedValue, selectB.normalizedValue);
    }

    //this func is called by PresetColor buttons 
    public void FromPreset(Color color)
    {
        finalColor = color;
        SetRGBUI(color);
        SetRenderer(color.r, color.g, color.b);
    }

    void SetRenderer(float r,float g,float b)
    {
        Material[] mats = vehicleRenderer.materials;
        mats[materialIndex].color = new Color(r, g, b);
        vehicleRenderer.materials = mats;
    }

    void SetPreviewColor(Color color)
    {
        previewColor.color = color;
    }

    public Color GetColor()
    {
        return finalColor;
    }
}
