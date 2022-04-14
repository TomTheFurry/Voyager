using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationBar : MonoBehaviour
{
    public float maxValue = 1;
    public float currentValue = 1;
    public float minValue = 0;
    public float minAngle = 0;
    public float maxAngle = 360;
    private RawImage bar;

    void Start()
    {
        bar = GetComponent<RawImage>();
    }

    void Update()
    {
        float percent = (currentValue - minValue) / (maxValue - minValue);
        if (float.IsNaN(percent))
        {
            percent = 0;
        }
        percent = Mathf.Clamp01(percent);
        bar.rectTransform.localRotation = Quaternion.Euler(0, 0, minAngle + (maxAngle - minAngle) * percent);
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
