using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarUI : MonoBehaviour
{
    public static int targetLevel = 0;

    private Text text;

    public Sprite completedStar;
    public Sprite incompleteStar;
    //public int targetLevel = 0;
    public int starIndex = 0;

    const string langFile = "Stars";

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
        bool completed = PlayerData.IsLevelStarCompleted(targetLevel, starIndex);
        if (completed)
        {
            GetComponent<Image>().sprite = completedStar;
        }
        else
        {
            GetComponent<Image>().sprite = incompleteStar;
        }

        if (text != null)
            text.text = LangSystem.GetLang(langFile, string.Format("Level{0}Star{1}", targetLevel, starIndex));
    }
}
