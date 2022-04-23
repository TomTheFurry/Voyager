using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EmissionHookup : MonoBehaviour
{
    ParticleSystem ps;
    private float rate;
    private float rate2;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        var rod = ems.rateOverDistance;
        rate = rot.constant;
        rot.constant = 0;
        rate2 = rod.constant;
        rod.constant = 0;
        ems.rateOverTime = rot;
        ems.rateOverDistance = rod;
    }


    public void SetEmissionRate(float percent) {
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        var rod = ems.rateOverDistance;
        rot.constant = percent * rate;
        ems.rateOverTime = rot;
        rod.constant = percent * rate2;
        ems.rateOverDistance = rod;
    }
}
