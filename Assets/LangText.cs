using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LangText : MonoBehaviour
{
    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = LangSystem.parseText(text.text);
    }
}
