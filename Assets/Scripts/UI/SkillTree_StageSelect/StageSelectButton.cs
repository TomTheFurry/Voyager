using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    public bool enable;
    public string langFile;
    public int targetLevel;
    public string level;
    public GameObject stageSelect;
    public bool shipModifyIsLock = false;

    public TextMeshProUGUI information;
    public ShipModifyButton shipModifyButton;
    public LevelSwitch playButton;

    void Start()
    {
        updateButtonState();
    }

    public void changeToSelectUI()
    {
        StarUI.targetLevel = targetLevel;

        information.text = LangSystem.parseText(Global.langPath(langFile, level + "_Information"));
        shipModifyButton.changeButtonState(shipModifyIsLock);
        playButton.levelName = level;

        stageSelect.SetActive(true);
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
