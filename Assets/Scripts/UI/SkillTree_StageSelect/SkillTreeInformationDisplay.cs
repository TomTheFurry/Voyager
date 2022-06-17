using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeInformationDisplay : MonoBehaviour
{
    public string langFile = "Information";

    public Text txtName;
    public TechUnlockButton btnUnlock;
    public Image imgImage;
    public TextMeshProUGUI tmpAttribute;
    public TextMeshProUGUI tmpDescription;

    public void updateInformation(string techName, Sprite image, Tech tech, Transform customTooltips)
    {
        txtName.text = techName;
        imgImage.sprite = image;
        // attribute
        string[] desc = tech.getAttributeDescription();
        int index = 1;
        float baseLinePos;

        tmpAttribute.text = "";
        foreach (string str in desc)
        {
            tmpAttribute.text += str;
            tmpAttribute.ForceMeshUpdate();
            int newIndex = tmpAttribute.text.Length;
            baseLinePos = tmpAttribute.textInfo.characterInfo[index - 1].baseLine;
            float newBaseLinePos = tmpAttribute.textInfo.characterInfo[newIndex - 1].baseLine;

            if (baseLinePos != newBaseLinePos)
            {
                //Debug.Log("new line");
                tmpAttribute.text = tmpAttribute.text.Insert(tmpAttribute.text.Length - str.Length, "\n");
            }
            tmpAttribute.text += "  ";
            tmpAttribute.ForceMeshUpdate();
            index = tmpAttribute.text.Length;
        }
        // end attribute

        tmpDescription.text = LangSystem.parseText(Global.langPath(langFile, tech.identifier + "_Description"));
        int size = 0;
        int.TryParse(LangSystem.parseText(Global.langPath(langFile, tech.identifier + "_FontSize")), out size);
        tmpDescription.fontSize = (size != 0 ? size : 24);
        tmpDescription.GetComponent<TMPTooltip>().tooltipContainerHint = customTooltips;
        // unlock button
        btnUnlock.updateState(tech);

    }
}
