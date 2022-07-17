using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TmpLangText : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = LangSystem.parseText(GetComponent<TextMeshProUGUI>().text);
    }
}
