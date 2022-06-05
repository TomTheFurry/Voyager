using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipModifyButton : MonoBehaviour
{
    public GameObject shipModiftUi;
    public bool isLock = false;

    private void Start()
    {
        changeButtonState();
    }

    public void openShipModifyUi()
    {
        new UiInfo(transform.parent.gameObject, shipModiftUi);
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
