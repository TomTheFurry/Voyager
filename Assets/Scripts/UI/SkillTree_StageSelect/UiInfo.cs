using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInfo : MonoBehaviour
{
    static Dictionary<string, UiInfo> uiInfos = new Dictionary<string, UiInfo>();

    private string key;
    private Stack<GameObject> uiLocation = new Stack<GameObject>();

    public static void moveBack(string key)
    {
        UiInfo location = getUiInfo(key);
        location.moveBack();
    }

    public static UiInfo getUiInfo(string key)
    {
        UiInfo temp;
        uiInfos.TryGetValue(key, out temp);
        return temp;
    }

    public UiInfo(string key, GameObject startLocation)
    {
        Debug.Log("UiInfo created");
        UiInfo temp = getUiInfo(key);
        
        if (temp != null)
        {
            uiInfos.Remove(key);
        }

        this.key = key;
        uiInfos.Add(key, this);
        uiLocation.Push(startLocation);
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
