using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorHooker : MonoBehaviour
{
    public ArrowIndicator indicator;
    public RotationBar speedBar;
    public Tooltip speedBarTooltip;
    public GameObject target;
    
    void Update()
    {
        Vector3 localVelocity = target.transform.InverseTransformVector(target.GetComponent<Rigidbody>().velocity);
        indicator.setValue(localVelocity);
        speedBar.SetValue(localVelocity.magnitude);
        speedBarTooltip.text = string.Format("Speed: {0:0} m/s", localVelocity.magnitude);

    }
}
