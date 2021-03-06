using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TechStorage : MonoBehaviour
{
    public static TechStorage instance;
    public static List<Tech> teches = new List<Tech>();
    public static List<TechEquip> techEquips = new List<TechEquip>();

    public UnityEvent onTechStatusChanged;
    public UnityEvent onTechEquipChanging;

    public int techNumCanBeUnlocked = 0;

    [Serializable]
    public struct TechState
    {
        public bool isUnlocked;
    }
    [Serializable]
    public struct EquipState
    {
        public string equipIdentifier;
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
        [Serializable]
        public struct EquipPair
        {
            public string identifier;
            public EquipState state;
        }
        public EntryPair[] entries;
        public EquipPair[] equips;
    }

    Dictionary<Tech,TechState> techTable; // Hashtable<Tech,TechState>
    Dictionary<TechEquip, EquipState> equipTable; // Hashtable<TechEquip,EquipState>

    public Dictionary<string, TotalAttribute> equipAttribute; // Hashtable<AttributeName, TotalAttribute>
    public struct TotalAttribute
    {
        public float atteributeAdd;
        public float atteributePercentage;
    }

    public TechData collectTechData()
    {
        TechData techData = new TechData
        {
            entries = new TechData.EntryPair[techTable.Count],
            equips = new TechData.EquipPair[equipTable.Count],
        };
        int i = 0;
        foreach (KeyValuePair<Tech, TechState> pair in techTable)
        {
            TechData.EntryPair ePair = new TechData.EntryPair();

            ePair.identifier = pair.Key.identifier;
            ePair.state = pair.Value;
            techData.entries[i++] = ePair;
        }
        //equip
        i = 0;
        foreach (KeyValuePair<TechEquip, EquipState> pair in equipTable)
        {
            TechData.EquipPair ePair = new TechData.EquipPair();

            ePair.identifier = pair.Key.identifier;
            ePair.state = pair.Value;
            techData.equips[i++] = ePair;
        }
        return techData;
    }

    public void initTechs()
    {
        teches = new List<Tech>();
        techTable = new Dictionary<Tech, TechState>();
        equipAttribute = new Dictionary<string, TotalAttribute>();

        teches.AddRange(transform.GetComponentsInChildren<Tech>());

        foreach (Tech tech in teches)
        {
            techTable.Add(tech, new TechState
            {
                isUnlocked = tech.defaultUnlock
            });
        }

        //equip
        techEquips = new List<TechEquip>();
        equipTable = new Dictionary<TechEquip, EquipState>();

        techEquips.AddRange(transform.GetComponentsInChildren<TechEquip>());

        foreach (TechEquip equip in techEquips)
        {
            equipTable.Add(equip, new EquipState
            {
                equipIdentifier = equip.defaultEquip == null ? null : equip.defaultEquip.name
            });
        }
    }

    public Tech getTechByIdentifier(string identifier)
    {
        return teches.Find((t) => t.identifier == identifier);
    }

    public List<Tech> getTechesIsUnlocked()
    {
        List<Tech> techIsUnlocked = new List<Tech>();
        foreach (Tech tech in teches)
        {
            if (!isTechUnlocked(tech))
                continue;
            techIsUnlocked.Add(tech);
        }
        return techIsUnlocked;
    }

    public void importTechData(TechData techData)
    {
        // Tech
        if (techData.entries == null) techData.entries = new TechData.EntryPair[0];
        foreach (TechData.EntryPair pair in techData.entries)
        {
            Tech tech = getTechByIdentifier(pair.identifier);
            if (tech == null)
            {
                Debug.LogWarning("TechTree: importTechData: tech not found: " + pair.identifier);
                continue;
            }
            techTable[tech] = pair.state;
            //Debug.Log("Loaded " + pair.identifier + ": " + pair.state);
        }
        // Equip
        if (techData.equips == null) techData.equips = new TechData.EquipPair[0];
        foreach (TechData.EquipPair pair in techData.equips)
        {
            TechEquip techEquip = getEquipByIdentifier(pair.identifier);
            if (techEquip == null)
            {
                Debug.LogWarning("TechTree: importTechData: techEquip not found: " + pair.identifier);
                continue;
            }
            equipTable[techEquip] = pair.state;
            this.techEquip(getTechByIdentifier(pair.state.equipIdentifier), techEquip);
            //Debug.Log("Loaded Equip " + pair.identifier + ": " + pair.state.equipIdentifier);
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
        collectTechNumCanBeUnlocked();
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

    public Tech getNullTech()
    {
        return getTechByIdentifier("EmptyTech");
    }

    public bool isTechIsNull(Tech tech)
    {
        return tech == getNullTech();
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
        return PlayerData.GetData().stars >= tech.starCost;
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
        PlayerData.GetData().stars -= tech.starCost;
        onTechStatusChanged.Invoke();
        return true;
    }

    // equip
    public TechEquip getEquipByIdentifier(string identifier)
    {
        return techEquips.Find((e) => e.identifier == identifier);
    }

    public TechEquip getEquipByTech(Tech tech)
    {
        return techEquips.Find((e) => e.equip == tech);
    }

    public Tech getEquip(string equipType)
    {
        return getEquipByIdentifier(equipType).equip;
    }

    public EquipState getEquipState(TechEquip techEquip)
    {
        return (EquipState)equipTable[techEquip];
    }

    public bool techEquip(Tech tech, TechEquip equip, bool updateAttribute = true)
    {
        if (tech == null)
            tech = equip.defaultEquip;
        equip.equip = tech;

        if (updateAttribute)
            updateTotalAttribute();

        return true;
    }
    public bool techEquip(Tech tech, string equipType, bool updateAttribute = true)
    {
        TechEquip equip = getEquipByIdentifier(equipType);
        return techEquip(tech, equip, updateAttribute);
    }
    // tech equip
    public bool isEquipedLaser()
    {
        if (getEquip("ExternalDevice1") == getTechByIdentifier("Laser"))
            return true;
        if (getEquip("ExternalDevice2") == getTechByIdentifier("Laser"))
            return true;
        return false;
    }
    public int getSolarCellNum()
    {
        int num = 0;
        if (getEquip("ExternalDevice1") == getTechByIdentifier("SolarBattery"))
            ++num;
        if (getEquip("ExternalDevice2") == getTechByIdentifier("SolarBattery"))
            ++num;
        return num;
    }
    public bool isEquipedServicing()
    {
        if (getEquip("ExternalDevice1") == getTechByIdentifier("MaintenanceSystem"))
            return true;
        if (getEquip("ExternalDevice2") == getTechByIdentifier("MaintenanceSystem"))
            return true;
        return false;
    }
    // tech equip end

    public bool saveEquipChange()
    {
        foreach (TechEquip techEquip in techEquips)
        {
            EquipState state = getEquipState(techEquip);
            Tech tech = techEquip.equip;
            state.equipIdentifier = tech == null ? null : tech.identifier;
            equipTable[techEquip] = state;
        }
        return true;
    }

    public bool loadEquip(string equipType)
    {
        TechEquip equip = getEquipByIdentifier(equipType);
        techEquip(getTechByIdentifier(getEquipState(equip).equipIdentifier), equip, false);
        updateTotalAttribute();
        return true;
    }

    public List<Tech> techCanEquip(string equipType, bool CanEmpty)
    {
        List<Tech> techCanEquip = new List<Tech>();
        TechEquip techEquip = getEquipByIdentifier(equipType);
        foreach (Tech tech in techEquip.teches)
        {
            bool canRepeat = tech.canRepeat;
            if (!isTechUnlocked(tech))
                continue;
            if (!canRepeat && getEquipByTech(tech) != null)
                continue;

            techCanEquip.Add(tech);
        }
        if (CanEmpty && techCanEquip.Count > 0)
            techCanEquip.Insert(0, getTechByIdentifier("EmptyTech"));
        return techCanEquip;
    }

    // update total attribute
    public Dictionary<string, TotalAttribute> updateTotalAttribute()
    {
        equipAttribute.Clear();
        foreach (TechEquip equip in equipTable.Keys)
        {
            Dictionary<string, Tech.Attribute> attributes = equip.equip.getAttribute();
            foreach (KeyValuePair<string, Tech.Attribute> attribute in attributes)
            {
                float value = attribute.Value.attribute;
                bool isPercentage = attribute.Value.isPercentage;
                string attributeType = attribute.Key;

                if (!equipAttribute.ContainsKey(attributeType))
                {
                    equipAttribute.Add(attributeType, new TotalAttribute()
                    {
                        atteributeAdd = isPercentage ? 0f : value,
                        atteributePercentage = isPercentage ? (value / 100f) + 1f : 1f
                    });
                }
                else
                {
                    if (isPercentage)
                    {
                        TotalAttribute temp = equipAttribute[attributeType];
                        temp.atteributePercentage += (value / 100f);
                        equipAttribute[attributeType] = temp;
                    }
                    else
                    {
                        TotalAttribute temp = equipAttribute[attributeType];
                        temp.atteributeAdd += value;
                        equipAttribute[attributeType] = temp;
                    }
                        
                }
            }
        }

        onTechEquipChanging.Invoke();

        return equipAttribute;
    }

    public TotalAttribute getTotalAttribute(string identify)
    {
        if (!equipAttribute.ContainsKey(identify))
            return new TotalAttribute()
            {
                atteributeAdd = 0,
                atteributePercentage = 1
            };
        else
            return equipAttribute[identify];
    }

    // number of teches which can be unlocked
    public void collectTechNumCanBeUnlocked()
    {
        techNumCanBeUnlocked = 0;
        foreach (Tech tech in techTable.Keys)
        {
            if (canTechBeUnlocked(tech))
                techNumCanBeUnlocked++;
        }
    }

    public static void ResetData()
    {
        if (instance != null)
        {
            instance.techTable.Clear();
            instance.equipTable.Clear();
            instance.equipAttribute.Clear();
            instance.techNumCanBeUnlocked = 0;
            instance.hasSetup = false;
            instance = null;
        }
        
    }
    
}
