using System;
using UnityEngine;

public class Tech : MonoBehaviour, IComparable
{
    public string identifier;
    public Tech[] prerequisites;
    public int starCost;

    public void Start()
    {
        if (TechTree.instance == null) throw new Exception();
        TechTree.registorTech(this);
        if (identifier == null || identifier.Length==0) identifier = gameObject.name;
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;
        if (obj is Tech)
            return identifier.CompareTo(((Tech)obj).identifier);
        else if (obj is string)
            return identifier.CompareTo((string)obj);
        else
            throw new ArgumentException("Invalid comparison");
    }
}
