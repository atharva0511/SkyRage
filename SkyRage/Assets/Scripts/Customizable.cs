using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Customizable : MonoBehaviour{

    public string vehicleName = "Drone";
    public string displayName = "Drone";
    public int price = 2000;
    public int pilotLevel = 5;
    public int priColorIndex = 0;
    public int secColorIndex = 1;
    public Renderer[] rends;
    public Light NeonLight;
    
    void Start()
    {
        LoadCustomizations();
    }

    public void SaveCustomizations(Customizations custms)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + vehicleName + "-custsms.amg";
        FileStream file = new FileStream(path, FileMode.Create);

        formatter.Serialize(file, custms);
        file.Close();
    }

    public Customizations LoadCustomizations()
    {
        string path = Application.persistentDataPath + "/" + vehicleName + "-custsms.amg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            Customizations custms = formatter.Deserialize(file) as Customizations;
            foreach(Renderer rend in rends)
            {
                Material[] mats = rend.materials;
                mats[priColorIndex].color = new Color(custms.droneColor1[0], custms.droneColor1[1], custms.droneColor1[2]);
                mats[secColorIndex].color = new Color(custms.droneColor2[0], custms.droneColor2[1], custms.droneColor2[2]);
                rend.materials = mats;
            }
            file.Close();
            NeonLight.color = rends[0].materials[secColorIndex].color;
            return custms;
        }
        else
        {
            Debug.Log("Customs not found");
            NeonLight.color = rends[0].materials[secColorIndex].color;
            return null;
        }
        
    }
}
