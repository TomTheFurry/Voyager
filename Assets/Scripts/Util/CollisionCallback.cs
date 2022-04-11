using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SendMessageUpwards("OnCollision", collision, SendMessageOptions.DontRequireReceiver);
    }
}
