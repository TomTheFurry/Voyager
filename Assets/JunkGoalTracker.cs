using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JunkGoalTracker : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Lazer lazer;
    public bool seenJWST = false;
    public bool seenGPS = false;
    public void SetSeenJWST() { seenJWST = true; }
    public void SetSeenGPS() { seenGPS = true; }

    public int targetCount = 200;
    public int count = 0;
    
    private void Start()
    {
        lazer.onDestroyObject.AddListener(UpdateText);
        text.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        lazer.onDestroyObject.RemoveListener(UpdateText);
    }

    void UpdateText(GameObject obj)
    {
        count++;
        text.gameObject.SetActive(true);
        text.text = string.Format("{0}/{1}", count, targetCount);
    }
}
