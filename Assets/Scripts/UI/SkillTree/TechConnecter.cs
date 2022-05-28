using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechConnecter : MonoBehaviour
{
    public Tech[] prerequisites;
    private int state = 0;
    private int type;

    private void Start()
    {
        setType();
    }

    public void updateState(Tech[] nexts)
    {
        if (state == 2) return;

        if (!TechTree.isTechsUnlocked(prerequisites)) return;
        if (state < 1) updateState(1);

        if (!TechTree.isTechsUnlocked(nexts)) return;
        updateState(2);
    }

    public void updateState(int state)
    {
        this.state = state;
        gameObject.GetComponent<Image>().sprite = TechTree.instance.getConnecterIcon(state * 3 + type);
    }

    private void setType()
    {
        switch (gameObject.tag)
        {
            case "TECH_c":  type = 0; break;
            case "TECH_f":  type = 1; break;
            case "TECH_t":  type = 2; break;
            default:        type = 0; break;
        }
    }
}
