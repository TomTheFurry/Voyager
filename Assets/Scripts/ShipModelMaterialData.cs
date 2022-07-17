using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipModelMaterialData : MonoBehaviour
{
    public static ShipModelMaterialData instance;

    public ShipMaterial[] techMaterials;
    public TechEquip externalMaterial;
    public shipEngine[] techEngines;
    public TechEquip engine;
    public GameObject defaultEngine;
    public SolarBattery[] numSolarBatterys;

    [Serializable]
    public struct ShipMaterial
    {
        [Serializable]
        public struct MaterialNode
        {
            public string identifier;
            public Material mat;
        }

        public Tech tech;
        public MaterialNode[] materials;

        public Material getMat(string identifier)
        {
            foreach (MaterialNode material in materials)
                if (identifier.Equals(material.identifier.ToLower()))
                    return material.mat;

            return new Material(Shader.Find("Specular"));
        }
    }

    [Serializable]
    public struct shipEngine
    {
        public Tech tech;
        public GameObject engine;

        public GameObject getEngine()
        {
            if (engine != null)
                return engine;
            else
                return instance.defaultEngine;
        }
    }

    [Serializable]
    public struct SolarBattery
    {
        public int num;
        public GameObject battery;

        public GameObject getBattery()
        {
            return battery;
        }
    }

    //material
    public ShipMaterial getMaterial(string identifier)
    {
        if (identifier == "default")
            return getMaterial(externalMaterial.defaultEquip);
        return getMaterial(TechStorage.instance.getTechByIdentifier(identifier));
    }

    public ShipMaterial getMaterial(Tech tech)
    {
        foreach (ShipMaterial material in techMaterials)
        {
            if (material.tech == tech)
                return material;
        }
        return getMaterial(externalMaterial.defaultEquip);
    }

    //engine
    public shipEngine getEngine(string identifier)
    {
        if (identifier == "default")
            return getEngine(engine.defaultEquip);
        return getEngine(TechStorage.instance.getTechByIdentifier(identifier));
    }

    public shipEngine getEngine(Tech tech)
    {
        foreach (shipEngine techEngine in techEngines)
        {
            if (techEngine.tech == tech)
                return techEngine;
        }
        return getEngine(engine.defaultEquip);
    }

    //SolarBattery
    public GameObject getSolarBattery(int num = 0)
    {
        foreach (SolarBattery battery in numSolarBatterys)
        {
            if (battery.num == num)
                return battery.getBattery();
        }
        return null;
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("TechTree: multiple instances of TechTree");
            Destroy(this);
            return;
        }
    }
}
