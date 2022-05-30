using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public TechStorage.TechData techData;
    public int stars;

    private static PlayerData activeData;

    public static void Load()
    {
        Debug.Log("Loading...");
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            Debug.Log("Data detected. Reading...");
            activeData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        }
        else
        {
            Debug.Log("No data detected. Reseting...");
            activeData = new PlayerData();
            activeData.techData = new TechStorage.TechData();
            activeData.stars = 0;
        }
    }

    public static void Save()
    {
        Debug.Log("Saving...");
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(activeData));
    }
    public static void Reset()
    {
        Debug.Log("Reseting all data...");
        PlayerPrefs.DeleteKey("PlayerData");
    }

    public static PlayerData GetData()
    {
        if (activeData == null)
        {
            Load();
        }
        return activeData;
    }


}
