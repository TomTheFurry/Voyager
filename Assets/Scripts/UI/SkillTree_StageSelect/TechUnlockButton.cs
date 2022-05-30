using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechUnlockButton : MonoBehaviour
{
    private Tech tech;
    void Start()
    {
        if(tech == null) updateState(false);

    }

    public void updateState(bool state)
    {
        Color disableColor = GetComponent<Button>().colors.disabledColor;
        GetComponent<Button>().interactable = state;
        Global.getChildByName(gameObject, "Text (TMP)").GetComponent<TextMeshProUGUI>().faceColor = state ? Color.white : disableColor;
    }
}
