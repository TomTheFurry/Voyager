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
    public Sprite sprite3;
    public Sprite sprite4;
    public Sprite sprite5;
    private int state = -1;

    public void Start()
    {
        if (TechStorage.instance == null) throw new Exception();
        if (reference == null) reference = TechStorage.instance.getTechByIdentifier(gameObject.name);

        TechStorage.instance.onTechStatusChanged.AddListener(Refresh);

        bgButton = transform.GetChild(0).GetComponent<Button>();
        icon = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        icon.sprite = reference.icon;
        Refresh();
    }

    void Refresh()
    {
        int preState = state;
        int newState;
        if (TechStorage.instance.isTechUnlocked(reference)) newState = 2;
        else if (TechStorage.instance.canTechBeUnlocked(reference)) newState = 1;
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
            case 0: 
                _SetSprite(sprite0, sprite1);
                setIcon(false);
                break;
            case 1: 
                _SetSprite(sprite2, sprite3); 
                setIcon(true);
                break;
            case 2: 
                _SetSprite(sprite4, sprite5);
                setIcon(true);
                break;
            default: throw new Exception();
        }
    }

    private void _SetSprite(Sprite sprNormal, Sprite sprHighlight)
    {
        bgButton.image.sprite = sprNormal;
        
        SpriteState st = new SpriteState();
        st.highlightedSprite = sprHighlight;
        st.selectedSprite = sprHighlight;
        bgButton.GetComponent<Button>().spriteState = st;
    }

    private void setIcon(bool isUnlock = true)
    {
        if (isUnlock)
            icon.sprite = reference.icon;
        else
            icon.sprite = Global.instance.sprLocked;
    }
}
