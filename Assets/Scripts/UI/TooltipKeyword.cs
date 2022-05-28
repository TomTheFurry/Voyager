using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipKeyword : MonoBehaviour
{
    public string keyword;
    public string langFile = "Information";
    private void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            if (t.name.Equals("Text (TMP)"))
            {
                TextMeshProUGUI text = t.gameObject.GetComponent<TextMeshProUGUI>();
                text.text = LangSystem.parseText("$lang/" + langFile + "/" + keyword.Replace(" ", "") + "$");
            }
        }
    }
}
