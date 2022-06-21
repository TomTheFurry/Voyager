using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class TechUnlockButton : MonoBehaviour
{
    private Tech tech;
    public Image icon;

    private System.Action<InputAction.CallbackContext> inputAction;

    void Start()
    {
        if(tech == null) updateState(TechStorage.instance.getTechByIdentifier("EmptyTech"));
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);

        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        inputAction = (cc) => {
            if (gameObject.activeInHierarchy) 
            {
                Button btn = GetComponent<Button>();
                if (btn.interactable == true)
                    ButtonClicked();
            }
        };
        current.submit.action.started += inputAction;
    }

    private void OnDestroy()
    {
        InputSystemUIInputModule current = FindObjectOfType<InputSystemUIInputModule>();
        current.submit.action.started -= inputAction;
    }

    public void updateState(Tech currentTech, bool playUnlockAnim = false)
    {
        tech = currentTech;

        Button btn = GetComponent<Button>();
        
        bool canUnlock = TechStorage.instance.canTechBeUnlocked(tech);
        bool isUnlock = TechStorage.instance.isTechUnlocked(tech);

        btn.image.sprite = tech.icon;
        Color color = btn.image.color;

        Animator anim = GetComponent<Animator>();

        do {
            icon.enabled = true;
            if (anim.GetInteger("State") == 2)
                anim.SetTrigger("Reset State");

            if (!canUnlock && !isUnlock)
            {
                anim.SetInteger("State", 0);
                //color.a = 0f;
                btn.interactable = false;
                icon.sprite = Global.instance.sprLocked; 
                break;
            }

            if (!isUnlock)
            {
                anim.SetInteger("State", 1);
                //color.a = 80f/256f;
                btn.interactable = true;
                icon.sprite = Global.instance.sprLock;
                break;
            }

            if (isUnlock)
            {
                if (playUnlockAnim)
                {
                    anim.SetTrigger("Unlock"); 
                }
                anim.SetInteger("State", 2);
                //color.a = 1f;
                btn.interactable = false;
                break;
            }
        } while(false);
        btn.image.color = color;
    }

    public void ButtonClicked() {
        if (tech != null) {
            bool unlock = TechStorage.instance.unlockTech(tech);
            if (unlock) 
                updateState(tech, true);
        }
    }

}
