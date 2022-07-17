using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechEquipButton : MonoBehaviour
{
    public bool canEmpty = true;
    private Sprite defaultSpr;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(openInterface);
        defaultSpr = transform.GetChild(0).GetComponent<Image>().sprite;

        TechEquipInterfaceController.instance.onResetEquip.AddListener(resetEquip);
        TechEquipInterfaceController.instance.onLoadEquip.AddListener(loadEquip);

        updateIcon();
    }
    public void openInterface()
    {
        TechEquipInterfaceController.instance.openInterface(this, canEmpty);
    }

    public void resetEquip()
    {
        TechStorage.instance.techEquip(null, name);
        updateIcon();
    }

    public void loadEquip()
    {
        TechStorage.instance.loadEquip(name);
        updateIcon();
    }

    public void techSelect(Tech tech)
    {
        TechStorage.instance.techEquip(tech, name);
        updateIcon();
    }

    public void updateIcon()
    {
        Transform img = transform.GetChild(0);
        Tech equip = getTechEquip().equip;
        img.GetComponent<Image>().sprite = equip != null ? equip.icon : defaultSpr;

        string[] descs = equip.getAttributeDescription();
        string desc = "";
        foreach (string str in descs)
        {
            desc += str + "\n";
        }
        GetComponent<Tooltip>().text = desc;
    }

    private TechEquip getTechEquip()
    {
        return TechStorage.instance.getEquipByIdentifier(name);
    }
}
