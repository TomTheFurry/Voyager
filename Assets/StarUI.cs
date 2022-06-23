using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    public static int targetLevel = 0;

    public Sprite completedStar;
    public Sprite incompleteStar;
    //public int targetLevel = 0;
    public int starIndex = 0;

    private void Start()
    {
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
        } else
        {
            GetComponent<Image>().sprite = incompleteStar;
        }
    }
}
