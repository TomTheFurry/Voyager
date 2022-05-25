using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TechTree : MonoBehaviour
{
    public static TechTree instance;

    public Tech[] teches; // should be sorted by identifier for fast access

    public UnityEvent onTechStatusChanged;
    
    [Serializable]
    public struct TechState {
        public bool isUnlocked;
    }
    [Serializable]
    public struct TechData
    {
        [Serializable]
        public struct EntryPair {
            public string identifier;
            public TechState state;
        }
        public EntryPair[] entries;
    }

    Hashtable techTable; // Hashtable<Tech,TechState>

    public TechData collectTechData()
    {
        TechData techData = new TechData
        {
            entries = new TechData.EntryPair[techTable.Count]
        };
        int i = 0;
        foreach (KeyValuePair<Tech, TechState> pair in techTable)
        {
            techData.entries[i] = new TechData.EntryPair();
            techData.entries[i].identifier = pair.Key.identifier;
            techData.entries[i].state = pair.Value;
        }
        return techData;
    }

    public void initTechs()
    {
        Array.Sort(teches);
        techTable = new Hashtable();
        foreach (Tech tech in teches)
        {
            techTable.Add(tech, new TechState {
                isUnlocked = false
            });
        }
    }

    private Tech getTechByIdentifier(string identifier)
    {
        return teches[Array.BinarySearch(teches, identifier)];
    }

    public void importTechData(TechData techData)
    {
        if (techData.entries == null) return;
        foreach (TechData.EntryPair pair in techData.entries)
        {
            Tech tech = getTechByIdentifier(pair.identifier);
            if (tech == null)
            {
                Debug.LogWarning("TechTree: importTechData: tech not found: " + pair.identifier);
                continue;
            }
            techTable[tech] = pair.state;
        }
    }
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("TechTree: multiple instances of TechTree");
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

    public bool canTechBeUnlocked(Tech tech)
    {
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
