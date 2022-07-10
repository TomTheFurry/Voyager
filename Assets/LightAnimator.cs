using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightAnimator : MonoBehaviour
{
    public AnimationCurve lightCurve;
    public float timeScale = 1f;
    private float startTime;
    private float scalerValue;
    private Light lightComp;
    
    void Start()
    {
        lightComp = GetComponent<Light>();
        startTime = Time.time;
        scalerValue = lightComp.intensity;
    }

    void Update()
    {
        lightComp.intensity = lightCurve.Evaluate(Time.time - startTime) * scalerValue;
    }
}
