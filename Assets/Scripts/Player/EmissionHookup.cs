using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EmissionHookup : MonoBehaviour
{
    ParticleSystem ps;
    private float rate;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        rate = rot.constant;
        rot.constant = 0;
        ems.rateOverTime = rot;
    }


    public void SetEmissionRate(float percent) {
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        rot.constant = percent * rate;
        ems.rateOverTime = rot;
    }
}
