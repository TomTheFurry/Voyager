using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechEquipButton : MonoBehaviour
{
    public bool canRepeat = true;
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
        TechEquipInterfaceController.instance.openInterface(this, canRepeat);
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
    }

    private TechEquip getTechEquip()
    {
        return TechStorage.instance.getEquipByIdentifier(name);
    }
}
