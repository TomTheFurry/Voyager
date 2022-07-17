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
        float atteributeAdd = attribute.atteributeAdd;
        float atteributePercentage = (attribute.atteributePercentage - 1f) * 100f;
        string strAtteributeAdd = atteributeAdd < 0 ? Mathf.Round(atteributeAdd).ToString() : "+" + Mathf.Round(atteributeAdd).ToString();
        string strAtteributePercentage = atteributePercentage < 0 ? atteributePercentage.ToString("F2") : "+" + atteributePercentage.ToString("F2");

        text.text += string.Format("({0}) ({1}%)", strAtteributeAdd, strAtteributePercentage);
    }
}
