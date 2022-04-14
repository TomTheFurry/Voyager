using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHooker : MonoBehaviour
{
    public ArrowIndicator indicator;
    public RotationBar speedBar;
    public GameObject target;
    
    void Update()
    {
        indicator.setValue(target.transform.InverseTransformVector(target.GetComponent<Rigidbody>().velocity));
        speedBar.SetValue(target.transform.InverseTransformVector(target.GetComponent<Rigidbody>().velocity).magnitude);
    }
}
