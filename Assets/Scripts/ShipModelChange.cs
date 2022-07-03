using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipModelChange : MonoBehaviour
{
    static List<ShipModelChange> instances = new List<ShipModelChange>();
    
    public Renderer[] externalLayer;

    private void Start()
    {
        TechStorage.instance.onTechEquipChanging.AddListener(updateShipModel);
        if (!instances.Contains(this))
            instances.Add(this);
        updateMaterial();
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    public void updateMaterial()
    {
        Tech equip = TechStorage.instance.getEquip("ExternalMaterial");
        ShipModelMaterialData.ShipMaterial material = ShipModelMaterialData.instance.getMaterial(equip);

        externalLayer[0].material = material.apex;
        externalLayer[1].material = material.body;
        externalLayer[2].material = material.door;
        externalLayer[3].material = material.ring1;
        externalLayer[4].material = material.ring2;
        externalLayer[5].material = material.tank1;
        externalLayer[6].material = material.tank2;
        externalLayer[7].material = material.threeQuarterRing;
        externalLayer[8].material = material.window;
    }

    public static void updateShipModel()
    {
        foreach (ShipModelChange ship in instances)
        {
            ship.updateMaterial();
        }
    }
}