using System;
using System.Collections.Generic;
using UnityEngine;

public class Tech : MonoBehaviour, IComparable
{
    public string identifier;
    public Tech[] prerequisites;
    private List<Tech> nextTechs = new List<Tech>();
    private List<TechConnecter> nextConnecters = new List<TechConnecter>();
    public int starCost;

    public void Start()
    {
        //if (TechTree.instance == null) throw new Exception();
        TechTree.registorTech(this);
        if (identifier == null || identifier.Length==0) identifier = gameObject.name;
    }

    public void initTech()
    {
        foreach (Tech tech in prerequisites)
        {
            tech.addNext(this);
        }
    }

    public void addNext(Tech tech)
    {
        nextTechs.Add(tech);
    }

    public void updateConnectLine()
    {
        if (nextConnecters.Count == 0) return;
        foreach (TechConnecter connecter in nextConnecters)
        {
            connecter.updateState(nextTechs.ToArray());
        }
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
