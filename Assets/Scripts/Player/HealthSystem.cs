using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 1000;
    public float health = 1000;
    public float healthPerForce = 0.1f;
    public float healthExpValue = 2.0f;

    public Tooltip textObj = null;
    public UIBar bar = null;
    public GameObject player;
    public UnityEvent onDeath;
    public UnityEvent onRespawn;

    void Update()
    {
        if (textObj != null) textObj.text = "Health: " + health.ToString("F2") + " / " + maxHealth.ToString();
        if (bar != null)
        {
            bar.SetValue(health);
            bar.SetMinValue(0);
            bar.SetMaxValue(maxHealth);
        }
    }
    
    public void RecordCollision(Collision collision) // Public facing function
    {
        float damage = collision.relativeVelocity.magnitude * healthPerForce;
        health -= Mathf.Pow(damage, healthExpValue);
        if (health <= 0)
        {
            player.SetActive(false);
            onDeath.Invoke();
        }
    }

    public void OnCheatHeal(InputValue value) //InputSystem event
    {
        if (value.isPressed)
        {
            Debug.Log("Healed!");
            if (health <= 0)
            {
                health = maxHealth;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                player.SetActive(true);
                onRespawn.Invoke();
            }
            else
            {
                health = maxHealth;
            }
        }
    }
}
