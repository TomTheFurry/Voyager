using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipModelChange : MonoBehaviour
{
    static List<ShipModelChange> instances = new List<ShipModelChange>();
    static bool usingDefault = false;
    
    public Renderer[] externalLayer;
    public Transform engine;
    public UnityEvent<Tech> onModelUpdate; 
    public GameObject laser;
    public Transform solarBattery;
    public GameObject servicing;

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
        // material
        Tech equipExternalMat = TechStorage.instance.getEquip("ExternalMaterial");
        ShipModelMaterialData.ShipMaterial material = ShipModelMaterialData.instance.getMaterial(equipExternalMat);
        if (usingDefault)
            material = ShipModelMaterialData.instance.getMaterial("default");
        for (int i = 0; i < externalLayer.Length; ++i)
        {
            externalLayer[i].material = material.getMat(externalLayer[i].gameObject.name.ToLower());
        }
        // engine
        Tech equipEngine = TechStorage.instance.getEquip("Engine");
        ShipModelMaterialData.shipEngine techEngine = ShipModelMaterialData.instance.getEngine(equipEngine);
        if (usingDefault)
            techEngine = ShipModelMaterialData.instance.getEngine("default");
        for (int i = 0; i < engine.childCount; ++i)
        {
            Destroy(engine.GetChild(i).gameObject);
        }
        Instantiate(techEngine.getEngine(), engine);
        onModelUpdate.Invoke(equipEngine);
        // laser
        laser.SetActive(usingDefault ? false : TechStorage.instance.isEquipedLaser());
        // solar battery
        int levelOfBattery = TechStorage.instance.getSolarCellNum();
        for (int i = 0; i < solarBattery.childCount; ++i)
        {
            Destroy(solarBattery.GetChild(i).gameObject);
        }
            
        GameObject newBattery = ShipModelMaterialData.instance.getSolarBattery(levelOfBattery);
        if (levelOfBattery > 0 && levelOfBattery <= 2 && !usingDefault)
        {
            Transform battery = Instantiate(newBattery, solarBattery).transform;
        }
        // servicing
        servicing.SetActive(usingDefault ? false : TechStorage.instance.isEquipedServicing());
    }

    public static void updateShipModel()
    {
        foreach (ShipModelChange ship in instances)
        {
            ship.updateModel();
        }
    }
}