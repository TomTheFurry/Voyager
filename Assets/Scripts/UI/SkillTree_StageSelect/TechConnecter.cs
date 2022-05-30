using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechConnecter : MonoBehaviour
{
    public GameObject[] prerequisitesObject;
    public GameObject[] groupConnecter;

    private int state = 0;
    private int type;
    private List<Tech> prerequisites;

    private void Start()
    {
        prerequisites = new List<Tech>();
        type = getType(gameObject);
        foreach (GameObject instance in prerequisitesObject)
        {
            prerequisites.Add(instance.GetComponent<Tech>());
        }
    }

    public void updateState(Tech[] nexts)
    {
        if (state == 2) return;

        if (!TechTree.instance.isTechsUnlocked(prerequisites.ToArray())) return;
        if (state < 1) updateState(1);

        if (!TechTree.instance.isTechsUnlocked(nexts)) return;
        updateState(2);
    }

    public void updateState(int state)
    {
        this.state = state;
        gameObject.GetComponent<Image>().sprite = TechTree.instance.getConnecterIcon(state * 3 + type);
        groupUndateState(state);
    }

    private void groupUndateState(int state)
    {
        foreach (GameObject instance in groupConnecter)
        {
            if (instance.GetComponent<TechConnecter>() != null)
                continue;

            instance.GetComponent<Image>().sprite = TechTree.instance.getConnecterIcon(state * 3 + getType(instance));
        }
    }

    private int getType(GameObject instance)
    {
        switch (instance.tag)
        {
            case "TECH_c":
                return 0;
            case "TECH_f":
                return 1;
            case "TECH_t":
                return 2;
            default:
                return 0;
        }
    }
}
