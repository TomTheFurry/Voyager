using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeInformation : MonoBehaviour
{
    public GameObject information;
    public GameObject icon;
    public Sprite image;
    public Transform customTooltips;
    public GameObject temp;

    public string langFile = "Information";
    private Tech myTech;
    private void Start()
    {
        myTech = GetComponent<TechIcon>().reference;
    }

    public void showInformation()
    {
        for (int i = 0; i < information.transform.childCount; ++i)
        {
            Transform child = information.transform.GetChild(i);
            string childName = child.gameObject.name;

            if (string.Equals(childName, "Name"))
            {
                child.GetComponent<Text>().text = gameObject.GetComponent<Tooltip>().text;
            }
            else if (string.Equals(childName, "Icon"))
            {
                child.GetComponent<Image>().sprite = icon.GetComponent<Image>().sprite;
            }
            else if (string.Equals(childName, "Image"))
            {
                child.GetComponent<Image>().sprite = image;
            }
            else if (string.Equals(childName, "Attribute"))
            {
                string[] desc = myTech.getAttributeDescription();
                TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
                int index = 1;
                float baseLinePos;

                text.text = "";
                foreach (string str in desc)
                {
                    text.text += str;
                    text.ForceMeshUpdate();
                    int newIndex = text.textInfo.characterInfo.Length;
                    baseLinePos = text.textInfo.characterInfo[index - 1].baseLine;
                    float newBaseLinePos = text.textInfo.characterInfo[newIndex - 1].baseLine;

                    if (baseLinePos != newBaseLinePos)
                    {
                        Debug.Log("new line");
                        text.text = text.text.Insert(text.text.Length - str.Length, "\n");
                    }
                    text.text += "  ";
                    text.ForceMeshUpdate();
                    index = text.textInfo.characterInfo.Length;
                }
            }
            else if (string.Equals(childName, "Description"))
            {

                child.GetComponent<TextMeshProUGUI>().text = LangSystem.parseText(Global.langPath(langFile, name + "_Description"));

                int size = 0;
                int.TryParse(LangSystem.parseText(Global.langPath(langFile, name + "_FontSize")), out size);
                child.GetComponent<TextMeshProUGUI>().fontSize = (size != 0 ? size : 24);
                child.GetComponent<TMPTooltip>().tooltipContainerHint = customTooltips;
            }
            else if (string.Equals(childName, "TechUnlockButton"))
            {
                bool canBeUnlocked = TechStorage.instance.canTechBeUnlocked(myTech);
                child.GetComponent<TechUnlockButton>().updateState(canBeUnlocked, myTech);
            }
        }
    }
}
