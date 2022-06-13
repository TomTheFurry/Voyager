using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TechUnlockButton : MonoBehaviour
{
    private Tech tech;
    void Start()
    {
        if(tech == null) updateState(false, null);
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ButtonClicked);
    }

    void OnSubmit()
    {
        //Debug.Log("Submit");
        Button btn = GetComponent<Button>();
        if (btn.interactable == true)
            ButtonClicked();
    }

    public void updateState(bool state, Tech currentTech)
    {
        tech = currentTech;
        Color disableColor = GetComponent<Button>().colors.disabledColor;
        GetComponent<Button>().interactable = state;
        Global.getChildByName(gameObject, "Text (TMP)").GetComponent<TextMeshProUGUI>().faceColor = state ? Color.white : disableColor;
    }

    public void ButtonClicked() {
        if (tech != null) {
            bool unlock = TechStorage.instance.unlockTech(tech);
            if (unlock) 
                updateState(false, tech);
        }
    }
}
