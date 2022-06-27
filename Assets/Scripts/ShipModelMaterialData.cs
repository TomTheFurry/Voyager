using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipModelMaterialData : MonoBehaviour
{
    public static ShipModelMaterialData instance;

    public ShipMaterial[] techMaterials;

    [Serializable]
    public struct ShipMaterial
    {
        public Tech tech;
        public Material apex;
        public Material body;
        public Material door;
        public Material ring1;
        public Material ring2;
        public Material tank1;
        public Material tank2;
        public Material threeQuarterRing;
        public Material window;
    }

    public ShipMaterial getMaterial(string identifier)
    {
        return getMaterial(TechStorage.instance.getTechByIdentifier(identifier));
    }

    public ShipMaterial getMaterial(Tech tech)
    {
        foreach (ShipMaterial material in techMaterials)
        {
            if (material.tech == tech)
                return material;
        }
        return new ShipMaterial();
    }

    private void Start()
    {
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
    }
}
