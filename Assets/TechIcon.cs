using System;
using UnityEngine;
using UnityEngine.UI;

public class TechIcon : MonoBehaviour
{
    public Tech reference;
    Button bgButton;
    Image icon;
    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    private int state = 0;

    public void Start()
    {
        if (TechStorage.instance == null) throw new Exception();
        if (reference == null) reference = TechStorage.instance.getTechByIdentifier(gameObject.name);

        TechStorage.instance.onTechStatusChanged.AddListener(Refresh);

        bgButton = GetComponent<Button>();
        icon = transform.GetChild(0).gameObject.GetComponent<Image>();
        icon.sprite = reference.icon;
    }

    void Refresh()
    {
        int preState = state;
        int newState;
        if (TechStorage.instance.isTechUnlocked(nextTech)) newState = 2;
        else if (TechStorage.instance.canTechBeUnlocked(nextTech)) newState = 1;
        else newState = 0;
        if (preState != newState)
        {
            state = newState;
            SetState(state);
        }
    }

    public void SetState(int i)
    {
        switch (i)
        {
            case 0: _SetSprite(sprite0); break;
            case 1: _SetSprite(sprite1); break;
            case 2: _SetSprite(sprite2); break;
            default: throw new System.Exception();
        }
    }

    private void _SetSprite(Sprite spr)
    {
        bgButton.sprite = spr;
    }



}
