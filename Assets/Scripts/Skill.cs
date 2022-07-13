using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private void Start()
    {
        SkillController.instance.skills.Add(this);
    }

    public abstract void _onClick();
}
