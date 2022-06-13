using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GoBackButton : MonoBehaviour
{
    public static bool enable = true;

    void OnRightClick(InputValue value) {
        if (!value.isPressed)
            return;
        if (!enable)
            return;
        GetComponent<Button>().onClick.Invoke();
    }
}