using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSpark : MonoBehaviour
{
    public ParticleSystem particle;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var ems = particle.emission;
        ems.enabled = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        particle.transform.position = collision.GetContact(0).point;
    }

    private void OnCollisionExit(Collision collision)
    {
        var ems = particle.emission;
        ems.enabled = false;
    }
}
