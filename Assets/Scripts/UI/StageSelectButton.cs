using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour
{
    public bool enable;
    void Start()
    {
        if (!enable)
        {
            changeButtonState(false);
        }
    }

    private void changeButtonState(bool btnState)
    {
        int childNum = gameObject.transform.childCount;
        if (childNum > 0)
        {
            for (int i = 0; i < childNum; i++)
            {
                Transform child = gameObject.transform.GetChild(i);
                if (string.Equals(child.name, "Image"))
                {
                    if (btnState)
                        child.gameObject.SetActive(true);
                    else
                        child.gameObject.SetActive(false);
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
