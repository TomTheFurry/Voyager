using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public TechTree.TechData techData;
    public int stars;

    private static PlayerData activeData;

    public static void Load()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            activeData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        }
        else
        {
            activeData = new PlayerData();
            activeData.techData = new TechTree.TechData();
            activeData.stars = 0;
        }
    }

    public static void Save()
    {
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(activeData));
    }
    public static void Reset()
    {
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
