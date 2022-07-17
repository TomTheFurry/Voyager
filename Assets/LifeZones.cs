using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeZones : MonoBehaviour
{
    public HealthSystem playerHealth;

    public float healthSubMul = 1.01f;
    public float healthSub = 0.1f;

    private float currentHealthSub = 0f;

    public TextFlash OutOfBoundText;

    public int playerInZone = 0;

    public void OnPlayerEnterZone()
    {
        playerInZone++;
    }
    public void OnPlayerExitZone()
    {
        playerInZone--;
    }

    void FixedUpdate()
    {
        if (playerInZone == 0)
        {
            if (currentHealthSub == 0f) currentHealthSub = healthSub;
            OutOfBoundText.gameObject.SetActive(true);
            currentHealthSub *= healthSubMul;
            playerHealth.TakeDamage(currentHealthSub);
        } else
        {
            currentHealthSub = 0f;
            OutOfBoundText.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Array.ForEach(GetComponentsInChildren<LifeSubZone>(true), x => x.parentZones = this);
    }
}
