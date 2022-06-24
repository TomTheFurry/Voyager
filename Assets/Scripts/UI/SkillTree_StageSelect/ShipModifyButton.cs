using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipModifyButton : MonoBehaviour
{
    public GameObject shipModiftUi;
    public bool isLock = false;
    public bool showSuggest = false;
    public bool keepSuggest = false;
    public List<Tech> suggestTech = new List<Tech>();

    private void Start()
    {
        changeButtonState();
    }

    public void openShipModifyUi()
    {
        if (!keepSuggest)
        {
            ShipModify shipModify = shipModiftUi.GetComponent<ShipModify>();
            shipModify.showSuggestIcon = showSuggest;
            shipModify.suggestTechs = suggestTech;
        }
            
        new UiInfo("Equip or skill tree", gameObject, shipModiftUi);
    }

    public void changeButtonState()
    {
        GetComponent<Button>().interactable = !isLock;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(!isLock);
        }
    }

    public void changeButtonState(bool isLock)
    {
        this.isLock = isLock;
        changeButtonState();
    }
}
