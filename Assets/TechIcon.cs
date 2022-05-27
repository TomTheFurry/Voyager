using System;
using UnityEngine;

public class TechIcon : MonoBehaviour
{
    public Tech reference;

    public void Start()
    {
        if (TechTree.instance == null) throw new Exception();

        if (reference == null) reference = TechTree.instance.getTechByIdentifier(gameObject.name);
    }
}
