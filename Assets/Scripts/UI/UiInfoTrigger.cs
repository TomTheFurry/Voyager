using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInfoTrigger : MonoBehaviour
{
    public string key;
    private GameObject thisObj;
    public GameObject nextObj;

    private void Start()
    {
        thisObj = gameObject;
    }

    public void moveBack()
    {
        UiInfo.moveBack(key, thisObj);
    }

    public void moveToNext()
    {
        UiInfo.moveToNext(key, nextObj);
    }

    public void moveCreat()
    {
        new UiInfo(key, thisObj, nextObj);
    }
}
