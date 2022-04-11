using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public RawImage bar;
    public float maxValue = 1;
    public float currentValue = 1;
    public float minValue = 0;

    void Start()
    {
        //get child's RawImage component
        bar = transform.GetChild(0).GetComponent<RawImage>();
    }

    void Update()
    {
        float percent = (currentValue - minValue) / (maxValue - minValue);
        if (float.IsNaN(percent))
        {
            percent = 0;
        }
        percent = Mathf.Clamp01(percent);
        bar.uvRect = new Rect(0, 0, percent, 1);
        bar.rectTransform.localScale = new Vector3(percent, 1, 1);
    }

    public void SetValue(float value)
    {
        currentValue = value;
    }
    public void SetMinValue(float value)
    {
        minValue = value;
    }
    public void SetMaxValue(float value)
    {
        maxValue = value;
    }
}
