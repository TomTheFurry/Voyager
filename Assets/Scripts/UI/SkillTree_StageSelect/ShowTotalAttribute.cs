using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowTotalAttribute : MonoBehaviour
{
    public string attributeType;
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        TechStorage.instance.onTechEquipChanging.AddListener(updateAttribute);
        updateAttribute();
    }

    public void updateAttribute()
    {
        TechStorage.TotalAttribute attribute =  TechStorage.instance.getTotalAttribute(attributeType);
        text.text = LangSystem.GetLang("UI", "Attribute" + attributeType.Replace(" ", "")) + ":\n";
        string atteributeAdd = attribute.atteributeAdd < 0 ? Mathf.Round(attribute.atteributeAdd).ToString() : "+" + Mathf.Round(attribute.atteributeAdd).ToString();
        string atteributePercentage = attribute.atteributePercentage < 0 ? attribute.atteributePercentage.ToString("F2") : "+" + attribute.atteributePercentage.ToString("F2");

        text.text += string.Format("({0}) ({1}%)", atteributeAdd, atteributePercentage);
    }
}
