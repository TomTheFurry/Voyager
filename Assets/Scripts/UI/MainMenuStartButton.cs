using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenuStartButton : UICallback
{
    public GameObject gameStartMenu;
    public GameObject mainMenu;

    InputActionReference action;
    

    void Start()
    {
        action = new InputActionReference();
        action.Set(FindObjectOfType<InputSystemUIInputModule>().actionsAsset.FindActionMap("UI").FindAction("Any"));
        action.action.started += _actionStart;
    }
    void _actionStart(InputAction.CallbackContext cc)
    {
        if (gameObject.activeInHierarchy && isHovering)
        {
            gameStartMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        action.action.started -= _actionStart;
    }
}
