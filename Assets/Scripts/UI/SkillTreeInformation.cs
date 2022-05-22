using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeInformation : MonoBehaviour
{
    public GameObject information;
    public string objName;
    public GameObject icon;
    public Sprite image;
    public GameObject description;

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
                child.GetComponent<Text>().text = gameObject.GetComponent<Text>().text;
            }
        }
        
    }
}
