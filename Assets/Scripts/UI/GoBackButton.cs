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

    private void Start()
    {
        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        current.cancel.action.started += (cc) => {
            if (gameObject.activeInHierarchy && triggerEnable)
            {
                //Debug.Log("BackTriggered for " + GetInstanceID());
                GetComponent<Button>().onClick.Invoke();
            }
        };
    }
}