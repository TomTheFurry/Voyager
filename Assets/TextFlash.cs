using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextFlash : MonoBehaviour
{
    TextMeshProUGUI text;
    public float cycleTime = 1f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.alpha = 1f - Mathf.Pow(Mathf.PingPong(Time.time / cycleTime, 1),2);
        text.alpha = 1f - Mathf.Pow(Mathf.PingPong(Time.time / cycleTime, 1),2);
    }
}
