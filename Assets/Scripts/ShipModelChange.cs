using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipModelChange : MonoBehaviour
{
    static List<ShipModelChange> instances = new List<ShipModelChange>();
    
    public Renderer[] externalLayer;
    public Transform engine;
    public UnityEvent<Tech> onModelUpdate; 

    private void Start()
    {
        TechStorage.instance.onTechEquipChanging.AddListener(updateShipModel);
        if (!instances.Contains(this))
            instances.Add(this);
        updateModel();
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    public void updateModel()
    {
        Tech equipExternalMat = TechStorage.instance.getEquip("ExternalMaterial");
        ShipModelMaterialData.ShipMaterial material = ShipModelMaterialData.instance.getMaterial(equipExternalMat);

        for (int i = 0; i < externalLayer.Length; ++i)
        {
            externalLayer[i].material = material.getMat(externalLayer[i].gameObject.name.ToLower());
        }

        Tech equipEngine = TechStorage.instance.getEquip("Engine");
        ShipModelMaterialData.shipEngine techEngine = ShipModelMaterialData.instance.getEngine(equipEngine);

        for (int i = 0; i < engine.childCount; ++i)
        {
            Destroy(engine.GetChild(i).gameObject);
        }
        Instantiate(techEngine.getEngine(), engine);
        onModelUpdate.Invoke(equipEngine);
    }

    public static void updateShipModel()
    {
        foreach (ShipModelChange ship in instances)
        {
            ship.updateModel();
        }
    }
}