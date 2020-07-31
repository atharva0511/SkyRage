using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData{

    public static int coins = 0;
    public static int xp = 0;
    public static int xpLevel = 0;
    public static bool[] unlockedVehicles = new bool[] { true, false };

    
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(file, new PlayerData());
        Debug.Log("Saved player data");
        file.Close();
    }

    public static PlayerData LoadData()
    {
        string path = Application.persistentDataPath + "/playerData.amg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(file) as PlayerData;
            
            file.Close();
            return data;
        }
        else
        {
            Debug.Log("Save Files not found");
            return null;
        }
    }
}
