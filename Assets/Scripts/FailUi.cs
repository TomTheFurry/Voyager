using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailUi : MonoBehaviour
{
    private Text text;
    //public int targetLevel = 0;
    public int starIndex = 0;

    const string langFile = "fail";

    private void Start()
    {
        text = transform.GetComponentInChildren<Text>();
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (text != null)
            text.text = LangSystem.GetLang(langFile, string.Format("Level{0}Fail{1}", StarUI.targetLevel, starIndex));
    }
}
