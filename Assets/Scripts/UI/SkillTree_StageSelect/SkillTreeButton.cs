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
    }

    public void updateNumber()
    {
        TechStorage.instance.collectTechNumCanBeUnlocked();
        txtTechNum.text = TechStorage.instance.techNumCanBeUnlocked.ToString();
    }
}
