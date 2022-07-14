using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JunkGoalTracker : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Lazer lazer;

    public int targetCount = 200;
    private int count = 0;
    
    private void Start()
    {
        lazer.onDestroyObject.AddListener(UpdateText);
    }

    private void OnDestroy()
    {
        lazer.onDestroyObject.RemoveListener(UpdateText);
    }

    void UpdateText(GameObject obj)
    {
        count++;
        text.text = string.Format("{0}/{1}", count, targetCount);
    }
}
