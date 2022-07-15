using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SolarBattery : MonoBehaviour
{
    public bool openWhenStart = false;

    private Animator anim;
    private InputActionReference actionPlayer;
    private InputActionReference actionUi;

    private float timer = 0f;
    private void Start()
    {
        anim = GetComponent<Animator>();

        //transform.Rotate(new Vector3(0, -45f, 0), Space.Self);
        // adding
        actionPlayer = new InputActionReference();
        actionUi = new InputActionReference();
        actionPlayer.Set(FindObjectOfType<InputSystemUIInputModule>().actionsAsset.FindActionMap("Player").FindAction("Skill"));
        actionUi.Set(FindObjectOfType<InputSystemUIInputModule>().actionsAsset.FindActionMap("UI").FindAction("Skill"));
        actionPlayer.action.started += _onClick;
        actionUi.action.started += _onClick;
        // adding end

        if (openWhenStart)
            openBattery();
    }

    private void Update()
    {
        if (timer <= 0f)
            return;

        timer -= Time.deltaTime;
        if (!closeBattery())
            openBattery();
    }

    private void OnDestroy()
    {
        actionPlayer.action.started -= _onClick;
        actionUi.action.started -= _onClick;
    }

    public void _onClick(InputAction.CallbackContext cc)
    {
        if (gameObject.activeInHierarchy)
        {
            timer = 0.6f;
        }
    }

    private bool closeBattery()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait_Opened"))
        {
            anim.Play("Close");
            return true;
        }
        return false;
    }

    private bool openBattery()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait_Closed"))
        {
            anim.Play("Open");
            return true;
        }
        return false;
    }
}
