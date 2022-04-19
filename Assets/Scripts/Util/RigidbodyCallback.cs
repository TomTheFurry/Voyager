using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyCallback : MonoBehaviour
{
    public UnityEvent<Collision> onCollisionEnter;

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke(collision);
    }
    
    public void ResetRigidBody()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

}
