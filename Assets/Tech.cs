using System;
using System.Collections.Generic;
using UnityEngine;

public class Tech : MonoBehaviour, IComparable
{
    /*  Attribute
        ex.
        health = -500;
        healthPercentage = false;
        hp = hp + (-500);

        speed = 5f;
        speedPercentage = true;
        speed = speed * (1 + (5 / 100) );
        */
    public float health;
    public bool healthPercentage;
    public float fuelCapacity;
    public bool fuelCapacityPercentage;
    public float fuelConsumption;
    public bool fuelConsumptionPercentage;
    public float speed;
    public bool speedPercentage;
    public float spinningSpeed;
    public bool spinningSpeedPercentage;
    public float collisionDamage;
    public bool collisionDamagePercentage;

    public bool defaultUnlock = false;
    public string identifier;
    public Tech[] prerequisites;
    public int starCost;
    public Sprite icon;

    public void Start()
    {
        if (identifier == null || identifier.Length == 0) identifier = gameObject.name;
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

    public string[] getAttributeDescription()
    {
        List<string> desc = new List<string>();
        Dictionary<string, float> equipAttribute = getAttribute();
        //if (health != 0)
        //    desc.Add("Health " + getAttributeDescString(health, healthPercentage));
        //if (fuelCapacity != 0)
        //    desc.Add("Fuel Capacity " + getAttributeDescString(fuelCapacity, fuelCapacityPercentage));
        //if (fuelConsumption != 0)
        //    desc.Add("Fuel Consuption " + getAttributeDescString(fuelConsumption, fuelConsumptionPercentage));
        //if (speed != 0)
        //    desc.Add("Speed " + getAttributeDescString(speed, speedPercentage));
        //if (spinningSpeed != 0)
        //    desc.Add("Spinning speed " + getAttributeDescString(spinningSpeed, spinningSpeedPercentage));
        //if (collisionDamage != 0)
        //    desc.Add("Collision damage " + getAttributeDescString(collisionDamage, collisionDamagePercentage));

        foreach (KeyValuePair<string, float> attribute in equipAttribute)
        {
            desc.Add(attribute.Key + " " + getAttributeDescString(attribute.Value));
        }

        return desc.ToArray();
    }

    public Dictionary<string, float> getAttribute()
    {
        Dictionary<string, float> equipAttribute = new Dictionary<string, float>();
        if (health != 0)
            equipAttribute.Add("Health", health);
        if (fuelCapacity != 0)
            equipAttribute.Add("Fuel Capacity", fuelCapacity);
        if (fuelConsumption != 0)
            equipAttribute.Add("Fuel Consuption", fuelConsumption);
        if (speed != 0)
            equipAttribute.Add("Speed" ,speed);
        if (spinningSpeed != 0)
            equipAttribute.Add("Spinning speed", spinningSpeed);
        if (collisionDamage != 0)
            equipAttribute.Add("Collision damage" ,collisionDamage);
        return equipAttribute;
    }

    private string getAttributeDescString(float num, bool isPercentage)
    {
        string desc = "";
        if (num < 0)
            desc += num.ToString();
        else
            desc += "+" + num.ToString();

        return desc + (isPercentage ? "%" : "");
    }

    private string getAttributeDescString(float num)
    {
        string desc = "";

        bool isPercentage = isValueIsPercentage(num);

        if (num < 0 || (isPercentage && num < 1))
            desc += num.ToString();
        else
            desc += "+" + num.ToString();

        return desc + (isPercentage ? "%" : "");
    }

    public static bool isValueIsPercentage(float num)
    {
        return num > 0 && num < 10;
    }
}
