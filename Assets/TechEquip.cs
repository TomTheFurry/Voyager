using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechEquip : MonoBehaviour
{
    public string identifier;
    public GameObject[] equipType;
    public List<Tech> teches = new List<Tech>();
    public Tech equip;

    private void Start()
    {
        identifier = name;
        foreach (GameObject instance in equipType)
        {
            teches.Add(instance.GetComponent<Tech>());
        }
        System.Array.Clear(equipType, 0, equipType.Length);
    }
}
