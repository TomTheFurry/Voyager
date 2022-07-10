using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;

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
            prefabSpawner.Spawn(player.transform);
            control2.spaceshipTerminated();
            player.SetActive(false);
            canvas.OnFail();
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
