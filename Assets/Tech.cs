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

    public struct Attribute
    {
        public float attribute;
        public bool isPercentage;
    }

    public Dictionary<string, Attribute> equipAttribute = new Dictionary<string, Attribute>();

    public bool defaultUnlock = false;
    public string identifier;
    public string techName;
    public Tech[] prerequisites;
    public int starCost;
    public Sprite icon;

    public Material[] shippartMaterial;

    public void Start()
    {
        if (identifier == null || identifier.Length == 0) identifier = gameObject.name;

        initTech();
    }

    private void initTech()
    {
        // equipAttribute
        if (health != 0)
            equipAttribute.Add("Health", new Attribute()
            {
                attribute = health,
                isPercentage = healthPercentage
            });
        if (fuelCapacity != 0)
            equipAttribute.Add("Fuel Capacity", new Attribute()
            {
                attribute = fuelCapacity,
                isPercentage = fuelCapacityPercentage
            });
        if (fuelConsumption != 0)
            equipAttribute.Add("Fuel Consuption", new Attribute()
            {
                attribute = fuelConsumption,
                isPercentage = fuelConsumptionPercentage
            });
        if (speed != 0)
            equipAttribute.Add("Speed", new Attribute()
            {
                attribute = speed,
                isPercentage = speedPercentage
            });
        if (spinningSpeed != 0)
            equipAttribute.Add("Spinning speed", new Attribute()
            {
                attribute = spinningSpeed,
                isPercentage = spinningSpeedPercentage
            });
        if (collisionDamage != 0)
            equipAttribute.Add("Collision damage", new Attribute()
            {
                attribute = collisionDamage,
                isPercentage = collisionDamagePercentage
            });
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
        Dictionary<string, Attribute> equipAttributes = getAttribute();
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

        foreach (KeyValuePair<string, Attribute> equipAttribute in equipAttributes)
        {
            Attribute attribute = equipAttribute.Value;
            desc.Add(equipAttribute.Key + " " + getAttributeDescString(attribute.attribute, attribute.isPercentage));
        }

        if (desc.Count == 0)
            desc.Add("Default");

        return desc.ToArray();
    }

    public Dictionary<string, Attribute> getAttribute()
    {
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

    public static bool isValueIsPercentage(float num)
    {
        return num > 0 && num < 10;
    }
}
