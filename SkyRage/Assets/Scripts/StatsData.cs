using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class Stats
{
    public static int count = 30;
    public static int[] data = new int[count];

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/StatsData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        StatsData data = new StatsData();
        data.SaveValues();
        formatter.Serialize(file, data);
        Debug.Log("Saved stats data");
        file.Close();
    }

    public static void ResetData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/StatsData.amg";
        FileStream file = new FileStream(path, FileMode.Create);
        StatsData data = new StatsData();
        formatter.Serialize(file, data);
        Debug.Log("Stats reset");
        file.Close();
    }

    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/StatsData.amg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            StatsData data = formatter.Deserialize(file) as StatsData;
            data.LoadValues();
            file.Close();
        }
        else
        {
            Debug.Log("Save Files not found");
            data = new int[count];
        }
    }
}

[System.Serializable]
public class StatsData
{
    public int count = 30;
    public int[] data;

    public void SaveValues()
    {
        this.count = Stats.count;
        this.data = Stats.data;
    }

    public void LoadValues()
    {
        Stats.data = this.data;
    }
}

/* #########################################################################
 * 
 * 0.  Barrels destroyed
 * 1.  Minibots destroyed
 * 2.  Turrets destroyed
 * 3.  Missile Turrets destroyed
 * 4.  Knight drones destroyed
 * 5.  Enforcers destroyed
 * 
 * 6.  Minigun destructions
 * 7.  Missile destructions
 * 8.  Enemies Stunned
 * 9.  Laser destructions
 * 
 * 10. no. of times customized.
 * 11. no. of ads watched.
 * 12. no. of times revived.
 * 13. player drone destructions
 * 14. total coins collected
 * 
 * ......... Max coins in rage mode..........
 * 15. Quadrone
 * 16. HOD 
 * 17. Winged
 * 18. Slayer
 * 
 * 19. Blimps popped
 * 20. UTs collected
 * 21. lives collected
 * 
 * 22. upgrades unlocked
 #########################################################################*/

