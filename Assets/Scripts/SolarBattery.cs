using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBattery : MonoBehaviour
{
    private void Start()
    {
        Animation anim =  GetComponent<Animation>();
        anim.Play("Take 001");
    }
}
