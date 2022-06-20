using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;

public class MainMenuStartButton : UICallback
{
    public GameObject gameStartMenu;
    public GameObject mainMenu;

    void Start()
    {
        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        current.actionsAsset.FindActionMap("UI").FindAction("Any").started += (cc) => {
            if (gameObject.activeInHierarchy && isHovering)
            {
                gameStartMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        };
    }
}
