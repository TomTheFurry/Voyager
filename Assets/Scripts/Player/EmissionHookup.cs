using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class EmissionHookup : MonoBehaviour
{
    ParticleSystem ps;
    private float rate;
    private float rate2;
    private AudioSource audioSource; // Nullable
    private float maxVolume;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        var rod = ems.rateOverDistance;
        rate = rot.constant;
        rot.constant = 0;
        rate2 = rod.constant;
        rod.constant = 0;
        ems.rateOverTime = rot;
        ems.rateOverDistance = rod;
        if (audioSource != null)
        {
            maxVolume = audioSource.volume;
            audioSource.volume = 0;
        }
    }


    public void SetEmissionRate(float percent) {
        var ems = ps.emission;
        var rot = ems.rateOverTime;
        var rod = ems.rateOverDistance;
        rot.constant = percent * rate;
        ems.rateOverTime = rot;
        rod.constant = percent * rate2;
        ems.rateOverDistance = rod;
        if (audioSource != null) {
            audioSource.volume = maxVolume * percent;
        }
    }
}
