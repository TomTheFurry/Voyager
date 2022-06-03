using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechEquipIcon : MonoBehaviour
{
    public Tech tech;
    private Animator anim;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(techSelect);
        anim = GetComponent<Animator>();
    }

    public void techSelect()
    {
        TechEquipInterfaceController.instance.techSelect(tech);
    }

    public void colseAnim()
    {
        anim.SetTrigger("Close");
    }
}
