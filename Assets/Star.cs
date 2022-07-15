using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Star : MonoBehaviour
{
    private Image childImg;
    private TextMeshProUGUI tmpText;

    public Sprite completeStar;
    public Sprite incompleteStar;
    public bool isCompleted
    {
        get
        {
            return childImg.sprite == completeStar;
        }
        set
        {
            if (value)
            {
                childImg.sprite = completeStar;
            }
            else
            {
                childImg.sprite = incompleteStar;
            }
        }
    }
    public string text {
        get { return tmpText.text; }
        set { tmpText.text = value; }
    }


    public void Start() {
        childImg = transform.GetComponentInChildren<Image>();
        tmpText = transform.GetComponentInChildren<TextMeshProUGUI>();
    }
}
