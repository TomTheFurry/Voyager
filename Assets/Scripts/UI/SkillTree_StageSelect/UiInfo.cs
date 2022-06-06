using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInfo
{
    static Dictionary<string, UiInfo> uiInfos = new Dictionary<string, UiInfo>();

    private string key;
    private Stack<GameObject> uiLocation = new Stack<GameObject>();

    public static void moveBack(string key, GameObject thisLocation)
    {
        if (!uiInfos.ContainsKey(key)) return;
        UiInfo location = uiInfos[key];
        thisLocation = getKeyObj(thisLocation);
        location.moveBack(thisLocation);
    }

    public static void moveToNext(string key, GameObject nextLocation)
    {
        if (!uiInfos.ContainsKey(key)) return;
        UiInfo location = uiInfos[key];
        nextLocation = getKeyObj(nextLocation);
        location.moveToNext(nextLocation);
    }

    public static GameObject getKeyObj(GameObject obj)
    {
        Transform instance = obj.transform;
        while (!instance.parent.name.Equals("Canvas"))
        {
            instance = instance.parent;
        }
        return instance.gameObject;
    }

    public UiInfo(string key, GameObject startLocation, GameObject nextLocation)
    {
        startLocation = getKeyObj(startLocation);
        nextLocation = getKeyObj(nextLocation);
        this.key = key;

        if (uiInfos.ContainsKey(key))
        {
            uiInfos.Remove(key);
        }

        uiInfos.Add(key, this);
        uiLocation.Push(startLocation);
        moveToNext(nextLocation);
    }

    public void moveToNext(GameObject nextLocation)
    {
        uiLocation.Peek().SetActive(false);
        uiLocation.Push(nextLocation);
        uiLocation.Peek().SetActive(true);
    }

    public void moveBack(GameObject thisLocation)
    {
        thisLocation.SetActive(false);
        if (uiLocation.Peek() == thisLocation)
            uiLocation.Pop();
        uiLocation.Peek().SetActive(true);
        uiLocation.Pop();
        if (uiLocation.Count == 1)
            uiInfos.Remove(key);
    }
}
