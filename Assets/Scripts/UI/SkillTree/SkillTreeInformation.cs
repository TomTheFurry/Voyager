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
    public GameObject[] customTooltips;
    public GameObject temp;

    private string langPath;
    private string myLangPath;
    private void Start()
    {
        langPath = "$lang/SkillTreeInfo/";
        myLangPath = langPath + gameObject.name;
    }

    public void showInformation()
    {
        Transform[] children = information.GetComponentsInChildren<Transform>();
        if (temp != null) Debug.Log(temp.GetComponent<Test1>().temp);

        foreach (Transform child in children)
        {
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
            else if (string.Equals(childName, "Description"))
            {

                child.GetComponent<TextMeshProUGUI>().text = LangSystem.parseText(myLangPath + "_Description$");

                int size = 0;
                int.TryParse(LangSystem.parseText(myLangPath + "_FontSize$"), out size);
                child.GetComponent<TextMeshProUGUI>().fontSize = (size != 0 ? size : 24);

                child.GetComponent<TooltipAuto>().clearTooltips();
                foreach (GameObject tooltips in customTooltips)
                {
                    //TooltipKeyword_ 
                    string keyword = LangSystem.parseText(langPath + tooltips.name.Substring(15).Trim() + "_Keyword$").ToLower().Replace("\r","").Replace("\n","");
                    child.GetComponent<TooltipAuto>().addTooltips(keyword, tooltips);
                }
            }
        }
    }
}
