using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechEquip : MonoBehaviour
{
    public string identifier;
    public Tech defaultEquip;
    public List<Tech> teches = new List<Tech>();
    public Tech equip;

    private void Start()
    {
        identifier = name;
        equip = defaultEquip;
    }
}
