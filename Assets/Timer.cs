using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeDisabledText;
    public UIBar timeBar;

    public void setTime(float time, float max = 0f)
    {
        string formatedTime; //Format should be: 1:24
        if (time < 60)
        {
            if (time < 10)
                formatedTime = "0:0" + Mathf.FloorToInt(time % 60);
            else
                formatedTime = "0:" + Mathf.FloorToInt(time % 60);
        }
        else
        {
            if (time % 60 < 10)
                formatedTime = Mathf.FloorToInt(time / 60) + ":0" + Mathf.FloorToInt(time % 60);
            else
                formatedTime = Mathf.FloorToInt(time / 60) + ":" + Mathf.FloorToInt(time % 60);
        }
        timeText.text = formatedTime;
        
        if (max == 0f)
        {
            timeDisabledText.gameObject.SetActive(true);
            timeBar.gameObject.SetActive(false);
        }
        else
        {
            timeDisabledText.gameObject.SetActive(false);
            timeBar.gameObject.SetActive(true);
            timeBar.minValue = 0f;
            timeBar.maxValue = max;
            timeBar.currentValue = time;
        }
    }
}
