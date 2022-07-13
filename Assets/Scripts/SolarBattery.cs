using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SolarBattery : Skill
{
    public bool openWhenStart = false;

    private Animator anim;
    private InputActionReference action;

    private float timer = 0f;
    private void Start()
    {
        anim = GetComponent<Animator>();

        //transform.Rotate(new Vector3(0, -45f, 0), Space.Self);

        if (openWhenStart)
            openBattery();
    }

    public override void _onClick()
    {
        if (!closeBattery())
            openBattery();
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
