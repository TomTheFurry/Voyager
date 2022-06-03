using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInfo
{
    static Dictionary<string, UiInfo> uiInfos = new Dictionary<string, UiInfo>();

    private string key;
    private Stack<GameObject> uiLocation = new Stack<GameObject>();

    public static void moveBack(GameObject thisLocation)
    {
        moveBack(getKey(thisLocation));
    }
    public static void moveBack(string key)
    {
        if (!uiInfos.ContainsKey(key)) return;
        UiInfo location = uiInfos[key];
        location.moveBack();
    }

    public static void moveToNext(GameObject thisLocation, GameObject nextLocation)
    {
        moveToNext(getKey(thisLocation), nextLocation);
    }
    public static void moveToNext(string key, GameObject nextLocation)
    {
        if (!uiInfos.ContainsKey(key)) return;
        UiInfo location = uiInfos[key];
        location.moveToNext(nextLocation);
    }

    public static string getKey(GameObject go)
    {
        Transform instance = go.transform;
        while (!instance.parent.name.Equals("Canvas"))
        {
            instance = instance.parent;
        }
        return instance.name;
    }

    public UiInfo(GameObject startLocation, GameObject nextLocation)
    {
        
        this.key = getKey(nextLocation);

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
        nextLocation.SetActive(true);
    }

    public void moveBack()
    {
        uiLocation.Peek().SetActive(false);
        uiLocation.Pop();
        uiLocation.Peek().SetActive(true);
        if (uiLocation.Count == 1)
            uiInfos.Remove(key);
    }
}
