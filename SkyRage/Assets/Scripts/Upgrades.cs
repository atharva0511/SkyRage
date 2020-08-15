using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Upgrades {

    public static bool[] qDroneWeapons = new bool[] { false, false, false, false };
    public static bool[] qDrone = new bool[] {false,false,false,false,false,false,false,false,false,false,false };

    public static bool[] hodWeapons = new bool[] { false, false, false, false };
    public static bool[] hod = new bool[] { false, false, false, false, false, false, false, false, false, false, false };

    public static bool[] wDroneWeapons = new bool[] { false, false, false, false };
    public static bool[] wDrone = new bool[] { false, false, false, false, false, false, false, false, false, false, false };

    public static bool[] slayerXWeapons = new bool[] { false, false, false, false };
    public static bool[] slayerX = new bool[] { false, false, false, false, false, false, false, false, false, false, false };

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/upgradeData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        UpgradeData data = new UpgradeData();
        formatter.Serialize(file, data);
        Debug.Log("Saved upgrade data");
        file.Close();
    }
    public static void Load()
    {
        string path = Application.persistentDataPath + "/upgradeData.amg";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            UpgradeData data = formatter.Deserialize(file) as UpgradeData;
            qDroneWeapons = data.qDroneWeapons;
            qDrone = data.qDrone;
            hodWeapons = data.hodWeapons;
            hod = data.hod;
            wDroneWeapons = data.wDroneWeapons;
            wDrone = data.wDrone;
            slayerXWeapons = data.slayerXWeapons;
            slayerX = data.slayerX;
            file.Close();
        }
        else
        {
            Debug.Log("Save Files not found");
        }
    }
}

[System.Serializable]
public class UpgradeData
{
    public bool[] qDroneWeapons;
    public bool[] qDrone;

    public bool[] hodWeapons;
    public bool[] hod;

    public bool[] wDroneWeapons;
    public bool[] wDrone;

    public bool[] slayerXWeapons;
    public bool[] slayerX;

    public UpgradeData()
    {
        this.qDroneWeapons = Upgrades.qDroneWeapons;
        this.qDrone = Upgrades.qDrone;
        this.hodWeapons = Upgrades.hodWeapons;
        this.hod = Upgrades.hod;
        this.wDroneWeapons = Upgrades.wDrone;
        this.wDrone = Upgrades.wDrone;
        this.slayerXWeapons = Upgrades.slayerXWeapons;
        this.slayerX = Upgrades.slayerX;
    }
}
