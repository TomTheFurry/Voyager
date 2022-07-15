using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI listText;

    public void DisplayUpdate(float currentRecord) {
        PlayerData.LevelData data = PlayerData.GetData().levelData[level];
        float[] records = (float[])data.timeRecords.Clone();
        Array.Sort(records);
        bool hasHighlighted = false;
        StringBuilder builder = new StringBuilder("");
        int num = 1;
        foreach (float v in records) {
            string str = num++ + ": " + format(v); 
            if (!hasHighlighted && v==currentRecord) {
                builder.Append("<color=yellow>");
                builder.Append(str);
                builder.Append("</color>");
                builder.Append('\n');
                hasHighlighted = true;
            } else {
                builder.Append(str);
                builder.Append('\n');
            }
            if (num > 50) break;
        }
        listText.text = builder.ToString();
    }

    string format(float v) {
        int mins = (int)Math.Floor(v/60);
        int secs = (int)(v%60);
        int microsecs = (int)((v%1)*1000);
        return string.Format("{0:#0}:{1:00}.{2:000}", mins, secs, microsecs);
    }
}
