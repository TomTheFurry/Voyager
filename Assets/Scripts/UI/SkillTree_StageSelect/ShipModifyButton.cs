using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipModifyButton : MonoBehaviour
{
    public GameObject shipModiftUi;
    public bool isLock = true;

    private void Start()
    {
        changeButtonState();
    }

    public void openShipModifyUi()
    {
        UiInfo location = new UiInfo("Ship modify", transform.parent.gameObject);
        location.moveToNext(shipModiftUi);
    }

    public void changeButtonState()
    {
        GetComponent<Button>().interactable = !isLock;
    }

    public void changeButtonState(bool isLock)
    {
        this.isLock = isLock;
        changeButtonState();
    }
}
