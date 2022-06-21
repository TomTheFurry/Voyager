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
    private System.Action<InputAction.CallbackContext> inputAction;

    private void Start()
    {
        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        inputAction = (cc) => {
            if (gameObject.activeInHierarchy && triggerEnable)
            {
                //Debug.Log("BackTriggered for " + GetInstanceID());
                GetComponent<Button>().onClick.Invoke();
            }
        };
        current.cancel.action.started += inputAction;
    }

    private void OnDestroy()
    {
        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        current.cancel.action.started -= inputAction;
    }
}