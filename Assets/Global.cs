using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Global : MonoBehaviour
{
    public static Global instance;
    public GameObject mainMenu;
    public Sprite sprLocked;
    public Sprite sprLock;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        LangSystem.LoadLang("eng");
    }

    public static Transform getChildByName(Transform instance, string name)
    {
        return getChildByName(instance.gameObject, name).transform;
    }
    public static GameObject getChildByName(GameObject instance, string name)
    {
        if (instance.transform.childCount == 0) return null;

        for (int i = 0; i < instance.transform.childCount; i++)
        {
            Transform child = instance.transform.GetChild(i);
            if (child.name.Equals(name))
                return child.gameObject;
        }

        return null;
    }

    public static string langPath(string file, string key)
    {
        return "$lang/" + file + "/" + key + "$";
    }

    public static Transform getCanvas(Transform ins)
    {
        while (ins.GetComponent<Canvas>() != null)
            ins = ins.parent;

        return ins;
    }
}
