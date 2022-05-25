using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tech : IComparable
{
    public string identifier;
    [SerializeReference]
    public Tech[] prerequisites;
    public int starCost;

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
