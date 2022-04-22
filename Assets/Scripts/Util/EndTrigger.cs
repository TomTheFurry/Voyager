using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTrigger : MonoBehaviour
{
    public UnityEvent onEndTrigger;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.attachedRigidbody.velocity.magnitude < 1) {
            onEndTrigger.Invoke();
            enabled = false;
        }
    }

}
