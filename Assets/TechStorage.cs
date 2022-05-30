using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TechStorage : MonoBehaviour
{
    public static TechStorage instance;
    public static List<Tech> teches = new List<Tech>();

    public UnityEvent onTechStatusChanged;

    [Serializable]
    public struct TechState
    {
        public bool isUnlocked;
    }
    [Serializable]
    public struct TechData
    {
        [Serializable]
        public struct EntryPair
        {
            public string identifier;
            public TechState state;
        }
        public EntryPair[] entries;
    }

    Dictionary<Tech,TechState> techTable; // Hashtable<Tech,TechState>

    public TechData collectTechData()
    {
        TechData techData = new TechData
        {
            entries = new TechData.EntryPair[techTable.Count]
        };
        int i = 0;
        foreach (KeyValuePair<Tech, TechState> pair in techTable)
        {
            TechData.EntryPair ePair = new TechData.EntryPair();

            ePair.identifier = pair.Key.identifier;
            ePair.state = pair.Value;
            techData.entries[i++] = ePair;
        }
        return techData;
    }

    public void initTechs()
    {
        teches = new List<Tech>();
        techTable = new Dictionary<Tech, TechState>();

        teches.AddRange(transform.GetComponentsInChildren<Tech>());

        foreach (Tech tech in teches)
        {
            techTable.Add(tech, new TechState
            {
                isUnlocked = false
            });
        }
    }

    public Tech getTechByIdentifier(string identifier)
    {
        return teches.Find((t) => t.identifier == identifier);
    }

    public void importTechData(TechData techData)
    {
        if (techData.entries == null) {
            Debug.Log("No player tech data detected.");
            return;
        }
        foreach (TechData.EntryPair pair in techData.entries)
        {
            Tech tech = getTechByIdentifier(pair.identifier);
            if (tech == null)
            {
                Debug.LogWarning("TechTree: importTechData: tech not found: " + pair.identifier);
                continue;
            }
            techTable[tech] = pair.state;
            Debug.Log("Loaded " + pair.identifier + ": " + pair.state);
        }
        Debug.Log("Loaded " + techData.entries.Length + " tech data entries.");
    }

    void Start()
    {
        Debug.Log("TechTree creacted: " + this);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("TechTree: multiple instances of TechTree");
            Destroy(this);
            return;
        }
        initTechs();
        importTechData(PlayerData.GetData().techData);
        onTechStatusChanged.AddListener(() =>
        {
            PlayerData.GetData().techData = collectTechData();
            PlayerData.Save();
        });
    }

    void OnDestroy()
    {
        PlayerData.GetData().techData = collectTechData();
        PlayerData.Save();
        if (instance == this)
        {
            instance = null;
        }
    }

    public TechState getTechState(Tech tech)
    {
        return (TechState)techTable[tech];
    }

    public bool isTechUnlocked(Tech tech)
    {
        return getTechState(tech).isUnlocked;
    }
    public bool isTechsUnlocked(Tech[] techs)
    {
        foreach (Tech tech in techs)
            if (!instance.isTechUnlocked(tech))
                return false;

        return true;
    }

    // ONLY return true if tech is not unlocked, but can be unlocked
    public bool canTechBeUnlocked(Tech tech)
    {
        if (isTechUnlocked(tech)) return false;
        foreach (Tech prereq in tech.prerequisites)
        {
            if (!isTechUnlocked(prereq))
            {
                return false;
            }
        }
        return true;
    }

    public bool unlockTech(Tech tech)
    {
        if (isTechUnlocked(tech))
        {
            return false;
        }
        if (!canTechBeUnlocked(tech))
        {
            return false;
        }
        TechState techState = getTechState(tech);
        techState.isUnlocked = true;
        techTable[tech] = techState;
        onTechStatusChanged.Invoke();
        return true;
    }
}
