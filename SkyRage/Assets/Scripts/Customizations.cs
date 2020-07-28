
using UnityEngine;

[System.Serializable]
public class Customizations {
    public float[] droneColor1;
    public float[] droneColor2;

    public Customizations(Color droneColor1,Color droneColor2)
    {
        this.droneColor1 = new float[] { droneColor1.r, droneColor1.g, droneColor1.b };
        this.droneColor2 = new float[] { droneColor2.r, droneColor2.g, droneColor2.b };
    }
}
