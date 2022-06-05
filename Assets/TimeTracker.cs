using UnityEngine;
using System;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    public float deltaTime = 0;
    public bool isPaused = false;
    public TextMeshProUGUI tmpText;

    public void Update()
    {
        if (!isPaused) deltaTime += Time.deltaTime; //TODO: Improve this???? Somehow?
        if (tmpText != null)
        {
            string formatedTime; //Format should be: 1:24
            if (deltaTime < 60)
            {
                if (deltaTime < 10)
                    formatedTime = "0:0" + Mathf.FloorToInt(deltaTime % 60);
                else
                    formatedTime = "0:" + Mathf.FloorToInt(deltaTime % 60);
            }
            else
            {
                if (deltaTime % 60 < 10)
                    formatedTime = Mathf.FloorToInt(deltaTime / 60) + ":0" + Mathf.FloorToInt(deltaTime % 60);
                else
                    formatedTime = Mathf.FloorToInt(deltaTime / 60) + ":" + Mathf.FloorToInt(deltaTime % 60);
            }
            tmpText.text = formatedTime;
        }
    }

    public float GetTime()
    {
        return deltaTime;
    }

}