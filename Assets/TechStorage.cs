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
        public bool isEquip;
        public string equipType;
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
    Dictionary<string, Tech> techEquips;

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
        techEquips = new Dictionary<string, Tech>();

        teches.AddRange(transform.GetComponentsInChildren<Tech>());

        foreach (Tech tech in teches)
        {
            techTable.Add(tech, new TechState
            {
                isUnlocked = false,
                isEquip = false,
                equipType = null
            });
        }

        List<TechEquip> equips = new List<TechEquip>();
        equips.AddRange(transform.GetComponentsInChildren<TechEquip>());
        foreach (TechEquip equip in equips)
        {
            techEquips.Add(equip.name, null);
        }
    }

    public Tech getTechByIdentifier(string identifier)
    {
        return teches.Find((t) => t.identifier == identifier);
    }

    public List<Tech> getTechesIsUnlocked(string type)
    {
        List<Tech> techIsUnlocked = new List<Tech>();
        foreach (Tech tech in teches)
        {
            if (!isTechUnlocked(tech))
                continue;
            if (type.Length != 0 && !techTypeIsMatch(tech, type))
                continue;
            techIsUnlocked.Add(tech);
        }
        return techIsUnlocked;
    }
    public List<Tech> getTechesIsUnlocked()
    {
        return getTechesIsUnlocked("");
    }
    public bool techTypeIsMatch(Tech tech, string type)
    {
        return tech.type.ToLower().Replace(" ", "").Equals(type.ToLower());
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

            // load equip
            if (pair.state.isEquip)
            {
                techEquips[pair.state.equipType] = tech;
            }
        }
        Debug.Log("Loaded " + techData.entries.Length + " tech data entries.");
    }

    private bool hasSetup = false;

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
        hasSetup = true;
    }

    void OnDestroy()
    {
        if (!hasSetup) return;
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

    public void techEquipSave()
    {
        techUnequip();
        foreach (KeyValuePair<string, Tech> equip in techEquips)
        {
            Tech tech = equip.Value;
            if (tech == null)
                continue;
            TechState ts = getTechState(tech);
            ts.isEquip = true;
            ts.equipType = equip.Key;
            techTable[tech] = ts;
        }
    }

    public void techEquipLoad()
    {
        foreach (Tech tech in teches)
        {
            TechState ts = getTechState(tech);
            if (ts.isEquip)
            {
                techEquips[ts.equipType] = tech;
            }
        }
    }

    public bool techEquip(Tech tech, string equipType)
    {
        techEquips[equipType] = tech;
        return true;
    }

    public bool techUnequip()
    {
        foreach (Tech tech in teches)
        {
            TechState ts = getTechState(tech);
            ts.isEquip = false;
            ts.equipType = null;
            techTable[tech] = ts;
        }
        return true;
    }

    public Tech getEquip(string equipType)
    {
        return (Tech)techEquips[equipType];
    }
}
