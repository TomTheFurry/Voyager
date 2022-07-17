using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Star : MonoBehaviour
{
    private Image childImg = null;
    private TextMeshProUGUI tmpText = null;

    public Sprite completeStar;
    public Sprite incompleteStar;
    public bool isCompleted
    {
        get
        {
            return getImg().sprite == completeStar;
        }
        set
        {
            if (value)
            {
                getImg().sprite = completeStar;
            }
            else
            {
                getImg().sprite = incompleteStar;
            }
        }
    }
    public string text {
        get { return getText().text; }
        set { getText().text = LangSystem.parseText(value); }
    }

    private TextMeshProUGUI getText() {
        if (tmpText == null) tmpText = transform.GetComponentInChildren<TextMeshProUGUI>();
        return tmpText;
    }
    private Image getImg()
    {
        if (childImg == null) childImg = transform.GetComponentInChildren<Image>();
        return childImg;
    }
}
