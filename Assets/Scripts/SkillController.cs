using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SkillController : MonoBehaviour
{
    public static SkillController instance;

    public List<Skill> skills = new List<Skill>();

    private InputActionReference action;
    private float timer = 0f;

    private void Start()
    {
        if (instance == null)
            instance = this;

        string actionMap = FindObjectOfType<PlayerInput>().defaultActionMap;
        action = new InputActionReference();
        action.Set(FindObjectOfType<InputSystemUIInputModule>().actionsAsset.FindActionMap(actionMap).FindAction("Skill"));
        action.action.started += _onClick;
    }

    private void Update()
    {
        if (timer <= 0f)
            return;

        timer -= Time.deltaTime;
        foreach (Skill skill in skills)
        {
            skill._onClick();
        }
    }

    private void _onClick(InputAction.CallbackContext cc)
    {
        Debug.Log("Click Skill");
        if (gameObject.activeInHierarchy)
        {

            timer = 0.6f;
        }
    }

    private void OnDestroy()
    {
        instance = null;
        action.action.started -= _onClick;
    }
}
