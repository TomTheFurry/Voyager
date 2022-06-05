using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TechEquipInterfaceController : MonoBehaviour
{
    public static TechEquipInterfaceController instance;
    
    public GameObject selectedTechInterfacePrefab;
    private GameObject selectInterface;

    private TechEquipButton equipBtn;
    public static string type = "";
    
    public UnityEvent onResetEquip;
    public UnityEvent onLoadEquip;

    private void Start()
    {
        instance = this;
    }

    public void leaveInterface()
    {
        UiInfo.moveBack(gameObject);
    }

    public void UnsaveEquip()
    {
        onLoadEquip.Invoke();
        leaveInterface();
    }

    public void saveEquip()
    {
        TechStorage.instance.saveEquipChange();
        leaveInterface();
    }

    public void resetEquip()
    {
        onResetEquip.Invoke();
    }

    public void openInterface(TechEquipButton btn)
    {
        if (selectInterface != null)
            Destroy(selectInterface);

        equipBtn = btn;
        type = equipBtn.gameObject.name;

        Transform sti = Instantiate(selectedTechInterfacePrefab, transform).transform;
        sti.localPosition = Vector3.zero;
        selectInterface = sti.gameObject;
    }

    public void closeInterface(GameObject selectInterface, GameObject img)
    {
        selectInterface.GetComponent<Animator>().SetTrigger("Close");
        for (int i = 0; i < img.transform.childCount; i++)
        {
            TechEquipIcon sti = img.transform.GetChild(i).GetComponent<TechEquipIcon>();
            sti.colseAnim();
        }
        this.selectInterface = null;
    }

    public void techSelect(Tech tech)
    {
        equipBtn.techSelect(tech);
    }
}
