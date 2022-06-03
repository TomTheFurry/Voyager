using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechEquipButton : MonoBehaviour
{
    public Tech tech;
    private Sprite defaultSpr;

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);
        defaultSpr = transform.GetChild(0).GetComponent<Image>().sprite;

        TechEquipInterfaceController.instance.onResetEquip.AddListener(resetEquip);

        TechEquipInterfaceController.instance.onLoadEquip.AddListener(loadEquip);
        loadEquip();
    }
    public void ButtonClicked()
    {
        TechEquipInterfaceController.instance.openInterface(this);
    }

    public void resetEquip()
    {
        tech = null;
        updateIcon();
        TechStorage.instance.techEquip(tech, name);
    }

    public void techSelect(Tech tech)
    {
        this.tech = tech;
        updateIcon();
        TechStorage.instance.techEquip(tech, name);
    }

    public void updateIcon()
    {
        Transform img = transform.GetChild(0);
        img.GetComponent<Image>().sprite = tech != null ? tech.icon : defaultSpr;
    }

    public void loadEquip()
    {
        tech = TechStorage.instance.getEquip(name);
        updateIcon();
    }
}
