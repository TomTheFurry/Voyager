using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class JunkRemover : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Obj exited");
        if (other.gameObject.CompareTag("Junk"))
        {
            Destroy(other.gameObject);
        }
    }

}
