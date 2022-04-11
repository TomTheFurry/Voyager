using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHooker : MonoBehaviour
{
    public GameObject indicator;
    public GameObject target;
    
    void Update()
    {
        indicator.GetComponent<ArrowIndicator>().setValue(target.transform.InverseTransformVector(target.GetComponent<Rigidbody>().velocity));

    }
}
