using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(MovementControl2), typeof(PrefabSpawner))]
public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 1000;
    public float health = 1000;
    public float healthPerForce = 0.1f;
    public float healthExpValue = 2.0f;
    public UnityEvent onDeath;
    public UnityEvent onRespawn;

    Tooltip textObj;
    UIBar bar;
    GameObject player;
    MovementControl2 control2;
    CanvasHandler canvas;
    PrefabSpawner prefabSpawner;

    void Start()
    {
        control2 = GetComponent<MovementControl2>();
        player = control2.childObject;
        canvas = FindObjectOfType<CanvasHandler>();
        textObj = canvas.cornerHud.healthBar.GetComponent<Tooltip>();
        bar = canvas.cornerHud.healthBar.GetComponent<UIBar>();
        prefabSpawner = GetComponent<PrefabSpawner>();
    }

    internal void TakeDamage(float damage)
    {
        if (health <= 0) return;
        health -= damage;
        if (health <= 0)
        {
            prefabSpawner.Spawn(player.transform);
            control2.spaceshipTerminated();
            player.SetActive(false);
            canvas.OnFail();
            onDeath.Invoke();
        }
    }

    void Update()
    {
        string langText = LangSystem.GetLang("UI", "HudHealthTip");

        if (textObj != null) textObj.text = langText + " " + health.ToString("F2") + " / " + maxHealth.ToString();
        if (bar != null)
        {
            bar.SetValue(health);
            bar.SetMinValue(0);
            bar.SetMaxValue(maxHealth);
        }
    }
    
    public void RecordCollision(Collision collision) // Public facing function
    {
        if (health <= 0) return;
        float damage = collision.relativeVelocity.magnitude * healthPerForce;
        TakeDamage(Mathf.Pow(damage, healthExpValue));
    }

    public void OnCheatHeal(InputValue value) //InputSystem event
    {
        if (value.isPressed)
        {
            Debug.Log("Healed!");
            if (health <= 0)
            {
                health = maxHealth;
                player.GetComponent<RigidbodyCallback>().ResetRigidBody();
                player.SetActive(true);
                canvas.DebugUndoFail();
                onRespawn.Invoke();
            }
            else
            {
                health = maxHealth;
            }
        }
    }
}
