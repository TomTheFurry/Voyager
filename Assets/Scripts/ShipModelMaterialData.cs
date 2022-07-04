using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipModelMaterialData : MonoBehaviour
{
    public static ShipModelMaterialData instance;

    public ShipMaterial[] techMaterials;
    public TechEquip externalMaterial;

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
        return getMaterial(externalMaterial.defaultEquip);
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
