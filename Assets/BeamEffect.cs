using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(ParticleSystemForceField))]
public class BeamEffect : MonoBehaviour
{
    ParticleSystem particle;
    ParticleSystemForceField forceField;
    float defaultRate;
    private void Start()
    {
        forceField = GetComponent<ParticleSystemForceField>();
        particle = GetComponent<ParticleSystem>();
        defaultRate = particle.emission.rateOverTimeMultiplier;
    }

    public void SetLengthAndRotation(float length, Quaternion rotation, Vector3 centerPos) {
        transform.rotation = rotation;
        transform.position = centerPos;
        var shape = particle.shape;
        shape.length = length;
        shape.position = new Vector3(0, length/2f, 0);
        var emission = particle.emission;
        emission.rateOverTimeMultiplier = defaultRate*length;
        forceField.length = length;
    }

    public void StopEffect() {
        var emission = particle.emission;
        emission.rateOverTimeMultiplier = 0;
        forceField.length = 0;
    }
}
