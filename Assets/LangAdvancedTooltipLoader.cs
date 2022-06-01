using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LangAdvancedTooltipLoader : MonoBehaviour
{
    public string langFile;

    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            string name = child.gameObject.name;
            TextMeshProUGUI tmpText = child.GetComponentInChildren<TextMeshProUGUI>();
            tmpText.text = LangSystem.GetLang(langFile, name);
        }
    }
}
