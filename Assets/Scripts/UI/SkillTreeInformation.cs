using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeInformation : MonoBehaviour
{
    public GameObject information;
    public string objName;
    public GameObject icon;
    public Sprite image;
    public int size = 24;
    [TextArea(1, 10)]
    public string description;
    public GameObject highlighter;

    public void showInformation()
    {
        Transform[] children = information.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            string childName = child.gameObject.name;

            if (string.Equals(childName, "Name"))
            {
                child.GetComponent<Text>().text = objName;
            }
            else if (string.Equals(childName, "Icon"))
            {
                child.GetComponent<Image>().sprite = icon.GetComponent<Image>().sprite;
            }
            else if (string.Equals(childName, "Image"))
            {
                child.GetComponent<Image>().sprite = image;
            }
            else if (string.Equals(childName, "Description"))
            {
                child.GetComponent<TextMeshProUGUI>().text = description;
                child.GetComponent<TextMeshProUGUI>().fontSize = size;

                if (child.childCount > 0)
                {
                    for (int i = 0; i < child.childCount; i++)
                    {
                        var temp = child.GetChild(i).GetComponent<SkillTreeTooltipsHighligher>();
                        if (temp != null)
                        {
                            temp.resetParent();
                        }
                    }
                }

                do
                {
                    if (highlighter == null)
                        break;

                    highlighter.gameObject.SetActive(true);
                    highlighter.GetComponent<SkillTreeTooltipsHighligher>().setParent(child);
                } while (false);
            }
        }
        
    }
}
