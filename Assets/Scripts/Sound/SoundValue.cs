using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundValue : MonoBehaviour
{
    public Slider slider;
    Text text;
    private float maxValue;

    private void Start()
    {
        text = GetComponent<Text>();
        maxValue = slider.maxValue;
    }
    void Update()
    {
        text.text = string.Format("{0,3}%", Mathf.Round(slider.value * 100 / maxValue ));
    }
}
