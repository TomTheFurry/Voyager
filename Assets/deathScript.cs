using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class deathScript : MonoBehaviour
{
    SphereCollider sph;
    HealthSystem health;
    void Start()
    {
        sph = GetComponent<SphereCollider>();
        health = GetComponent<HealthSystem>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody == null) return;
        if (collision.gameObject.tag == "Player")
        {
            health.TakeDamage(1000000000f);
        } else
        {
            Destroy(collision.gameObject);
        }
    }
}
