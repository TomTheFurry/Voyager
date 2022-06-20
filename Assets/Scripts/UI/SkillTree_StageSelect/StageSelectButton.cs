using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public bool enable;
    public string langFile;
    public string level;
    public GameObject stageSelect;
    public bool shipModifyIsLock = false;
    void Start()
    {
        updateButtonState();
    }

    public void changeToSelectUI()
    {
        stageSelect.SetActive(true);
        GameObject information = Global.getChildByName(stageSelect, "Information");
        information = Global.getChildByName(information, "Text");
        information.GetComponent<TextMeshProUGUI>().text =
            LangSystem.parseText(Global.langPath(langFile, level + "_Information"));

        Global.getChildByName(stageSelect, "Ship").GetComponent<ShipModifyButton>().changeButtonState(shipModifyIsLock);
        Global.getChildByName(stageSelect, "Play_button").GetComponent<LevelSwitch>().levelName = level;
    }

    private void changeButtonState(bool btnState)
    {
        GetComponent<Button>().interactable = btnState;
        int childNum = gameObject.transform.childCount;
        if (childNum > 0)
        {
            for (int i = 0; i < childNum; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                if (string.Equals(child.name, "Image"))
                {
                    child.gameObject.SetActive(!btnState);
                }
            }
        }
    }

    public void lockButton()
    {
        changeButtonState(false);
    }

    public void unlockButton()
    {
        changeButtonState(true);
    }

    public void updateButtonState()
    {
        changeButtonState(enable);
    }
}
