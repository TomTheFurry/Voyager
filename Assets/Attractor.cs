using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Attractor : MonoBehaviour
{
    SphereCollider sph;
    public float attractionForce;
    void Start()
    {
        sph = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        foreach (Collider other in Physics.OverlapSphere(transform.position, sph.radius))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector3 direction = transform.position - rb.position;
                float distance = direction.magnitude;
                float force = attractionForce / (distance * distance);
                rb.AddForce(direction.normalized * force);
            }
        }
        
    }
}
