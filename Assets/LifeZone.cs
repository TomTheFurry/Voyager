using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LifeZone : MonoBehaviour
{
    public HealthSystem playerHealth;

    public float healthSubMul = 1.01f;
    public float healthSub = 0.1f;

    private float currentHealthSub = 0f;

    public TextFlash OutOfBoundText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentHealthSub = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentHealthSub = healthSub;
        }
    }

    void FixedUpdate()
    {
        if (currentHealthSub > 0f)
        {
            OutOfBoundText.gameObject.SetActive(true);
            currentHealthSub *= healthSubMul;
            playerHealth.TakeDamage(currentHealthSub);
        } else
        {
            OutOfBoundText.gameObject.SetActive(false);
        }
    }
}
