using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundValue : MonoBehaviour
{
    Slider slider;
    Text text;
    private void Start()
    {
        slider = transform.parent.GetComponent<Slider>();
        text = GetComponent<Text>();
    }
    void Update()
    {
        text.text = string.Format("{0,3}%", Mathf.Round(slider.value * 100 / 2));
    }
}
