using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class GoBackButton : MonoBehaviour
{
    public static bool triggerEnable = true;
    private InputActionReference action;

    private void _onCancel(InputAction.CallbackContext cc)
    {
        if (gameObject.activeInHierarchy && triggerEnable)
        {
            Debug.Log("BackTriggered for " + GetInstanceID() + " : " + name);
            GetComponent<Button>().onClick.Invoke();
        }
    }

    private void Start()
    {
        action = FindObjectOfType<InputSystemUIInputModule>().cancel;
        action.action.started += _onCancel;
    }

    private void OnDestroy()
    {
        action.action.started -= _onCancel;
    }
}