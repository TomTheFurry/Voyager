using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{
    public Text txtTechNum;

    private void Start()
    {
        txtTechNum.text = TechStorage.instance.techNumCanBeUnlocked.ToString();
        TechStorage.instance.onTechStatusChanged.AddListener(updateNumber);
        updateNumber();
    }

    public void updateNumber()
    {
        TechStorage.instance.collectTechNumCanBeUnlocked();
        int num = TechStorage.instance.techNumCanBeUnlocked;
        if (num == 0)
        {
            txtTechNum.transform.parent.gameObject.SetActive(false);
        } else
        {
            txtTechNum.transform.parent.gameObject.SetActive(true);
            txtTechNum.text = num.ToString();
        }
    }
}
