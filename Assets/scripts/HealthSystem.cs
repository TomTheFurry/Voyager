using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 1000;
    public float health = 1000;
    public float healthPerForce = 0.1f;
    public float healthExpValue = 2.0f;

    public Text textObj = null;
    public UIBar bar = null;
    public GameObject player;

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
    
    public void OnCollision(Collision collision)
    {
        float damage = collision.relativeVelocity.magnitude * healthPerForce;
        health -= Mathf.Pow(damage, healthExpValue);
        if (health <= 0)
        {
            BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            player.SetActive(false);
        }
    }

    public void OnCheatHeal(InputValue value)
    {
        if (value.isPressed)
        {
            if (health <= 0)
            {
                health = maxHealth;
                BroadcastMessage("OnReset", SendMessageOptions.DontRequireReceiver);
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                player.SetActive(true);
            }
            else
            {
                health = maxHealth;
                BroadcastMessage("OnHeal", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
