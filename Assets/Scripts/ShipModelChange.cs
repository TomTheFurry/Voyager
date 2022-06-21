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
    }

    private void OnDestroy()
    {
        instances.Remove(this);
    }

    public void updateMaterial()
    {
        Tech equip = TechStorage.instance.getEquip("ExternalMaterial");
        foreach (Renderer renderer in externalLayer)
        {
            renderer.material = equip.material;
        }
    }

    public static void updateShipModel()
    {
        foreach (ShipModelChange ship in instances)
        {
            ship.updateMaterial();
        }
    }
}