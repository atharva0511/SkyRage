using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class PlayerData{

    public static double coins = 0;
    public static double xp = 0;
    public static int lives = 0;
    public static int upgradeTokens = 0;
    public static bool[] unlockedVehicles = new bool[] { true, false,false,false };
    public static int[] levelProgression = new int[] { 0, 0, 0, 0 };
    
    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        GameData data = new GameData();
        data.SaveValues();
        formatter.Serialize(file, data);
        Debug.Log("Saved player data");
        file.Close();
        Stats.SaveData();
    }

    public static void ResetData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        GameData data = new GameData();
        formatter.Serialize(file, data);
        Debug.Log("Data reset");
        file.Close();
        Stats.ResetData();
    }

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/playerData.amg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(file) as GameData;
            data.LoadValues();
            file.Close();
        }
        else
        {
            Debug.Log("Save Files not found");
        }
        Stats.LoadData();
    }
}

[System.Serializable]
public class GameData
{
    public double coins = 0;
    public double xp = 0;
    public int lives = 0;
    public int upgradeTokens = 0;
    public bool[] unlockedVehicles = new bool[] { true, false, false, false };
    public int[] levelProgression = new int[] { 0, 0, 0, 0 };

    public void SaveValues()
    {
        this.coins = PlayerData.coins;
        this.xp = PlayerData.xp;
        this.lives = PlayerData.lives;
        this.upgradeTokens = PlayerData.upgradeTokens;
        this.unlockedVehicles = PlayerData.unlockedVehicles;
        this.levelProgression = PlayerData.levelProgression;
    }

    public void LoadValues()
    {
        PlayerData.coins = this.coins;
        PlayerData.xp = this.xp;
        PlayerData.lives = this.lives;
        PlayerData.upgradeTokens = this.upgradeTokens;
        PlayerData.unlockedVehicles = this.unlockedVehicles;
        PlayerData.levelProgression = this.levelProgression;
    }
}
