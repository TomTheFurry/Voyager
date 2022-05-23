using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeTooltipsHighligher : MonoBehaviour
{
    private GameObject myParent;
    void Start()
    {
        myParent = gameObject.transform.parent.gameObject;
        gameObject.SetActive(false);
    }

    public void resetParent()
    {
        gameObject.transform.parent = myParent.transform;
        gameObject.SetActive(false);
    }

    public void setParent(Transform newParent)
    {
        gameObject.transform.parent = newParent;
    }
}
