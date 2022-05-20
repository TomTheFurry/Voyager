using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeInformation : MonoBehaviour
{
    public GameObject informationIcon;
    public GameObject icon;

    public void showInformation()
    {
        informationIcon.GetComponent<Image>().sprite = icon.GetComponent<Image>().sprite;
    }
}
