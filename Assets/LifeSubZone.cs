using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LifeSubZone : MonoBehaviour
{
    public LifeZones parentZones;
    private bool isEntered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isEntered)
            {
                Debug.LogWarning("Double Player Enter LifeSubZone!");
                return;
            }
            isEntered = true;
            parentZones.OnPlayerEnterZone();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isEntered)
            {
                Debug.LogWarning("Double Player Exit LifeSubZone!");
                return;
            }
            isEntered = false;
            parentZones.OnPlayerExitZone();
        }
    }
}
