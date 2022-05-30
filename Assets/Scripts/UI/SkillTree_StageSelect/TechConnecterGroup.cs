using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TechTree;

public class TechConnecterGroup : MonoBehaviour
{
    Image[] groupConnecter;

    private int state = 0;
    public Tech nextTech;

    private void Start()
    {
        TechStorage.instance.onTechStatusChanged.AddListener(Refresh);
    }

    void Refresh()
    {
        int preState = state;
        int newState;
        if (TechStorage.instance.isTechUnlocked(nextTech)) newState = 2;
        else if (TechStorage.instance.canTechBeUnlocked(nextTech)) newState = 1;
        else newState = 0;
        if (preState != newState) {
            state = newState;
            foreach (TechConnecter tc in groupConnecter.transform.GetComponentsInChildren<TechConnecter>())
            {
                tc.SetState(state);
            }
        }
    }
}
