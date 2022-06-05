using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    private static T[] ExpendArray<T>(T[] source, int newSize)
    {
        if (source == null) return new T[newSize];
        if (source.Length <= newSize) return source;
        T[] newArray = new T[newSize];
        source.CopyTo(newArray, 0);
        return source;
    }
    private static T[] AppendArray<T>(T[] source, T item)
    {
        if (source == null || source.Length==0)
        {
            return new T[] { item };
        }
        source = ExpendArray(source, source.Length + 1);
        source[source.Length - 1] = item;
        return source;
    }


    public TechStorage.TechData techData;
    public int stars;
    public LevelData[] levelData;

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
    private LevelData ResizeAndGetLevelData(int level) {
        levelData = ExpendArray(levelData, level + 1);
        return levelData[level];
    }

    public static void SetLevelData(int level, bool[] aquiredStars, float[] timeRecords) {
        GetData().ResizeAndGetLevelData(level);
        GetData().levelData[level] = new LevelData()
        {
            aquiredBonusStars = aquiredStars,
            timeRecords = timeRecords
        };
    }

    // Return how many new aquired stars
    public static int AddLevelData(int level, bool[] newAquiredStars, float newTime)
    {
        LevelData oldData = GetData().ResizeAndGetLevelData(level);
        oldData.aquiredBonusStars = ExpendArray(oldData.aquiredBonusStars, newAquiredStars.Length);
        int newStars = 0;
        for (int i = 0; i < newAquiredStars.Length; i++) {
            if (!oldData.aquiredBonusStars[i] && newAquiredStars[i]) newStars++;
            oldData.aquiredBonusStars[i] |= newAquiredStars[i];
        }
        oldData.timeRecords = AppendArray(oldData.timeRecords, newTime);
        return newStars;
    }

    public struct LevelData {
        public bool[] aquiredBonusStars;
        public float[] timeRecords;
        public float getBestTime() {
            float minTime = float.MaxValue;
            foreach (float l in timeRecords) if (l < minTime) minTime = l;
            return minTime;
        }
    }
}
