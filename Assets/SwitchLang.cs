using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLang : MonoBehaviour
{
    public void ToggleLang()
    {
        string lang = PlayerPrefs.GetString("lang", "eng");
        if (lang == "eng")
        {
            lang = "chn";
            PlayerPrefs.SetString("lang", lang);
        }
        else
        {
            lang = "eng";
            PlayerPrefs.SetString("lang", lang);
        }
        Global.Restart();
    }
}
