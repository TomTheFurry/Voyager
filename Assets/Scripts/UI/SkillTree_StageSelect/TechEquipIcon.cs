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
        if (tech == null)
            tech = TechStorage.instance.getTechByIdentifier("EmptyTech");
        GetComponent<Button>().onClick.AddListener(techSelect);
        anim = GetComponent<Animator>();
    }

    public void techSelect()
    {
        TechEquipInterfaceController.instance.techSelect(tech);
        TechEquipInterface teInterface = transform.parent.parent.GetComponent<TechEquipInterface>();
        teInterface.setSelectedIcon(this);
    }

    public void colseAnim()
    {
        anim.SetTrigger("Close");
    }

    public void setIcon(Sprite spr1, Sprite spr2)
    {
        Button btn = GetComponent<Button>();
        btn.image.sprite = spr1;
        SpriteState ss = btn.spriteState;
        ss.highlightedSprite = spr2;
        ss.selectedSprite = spr2;
        btn.spriteState = ss;
    }
}
