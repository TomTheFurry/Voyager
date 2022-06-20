using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSliderSelectSprite : UICallback
{
    public Image imgHandle;
    private Sprite sprHandleNoraml;
    public Sprite sprHandleSelected;
    public Image imgBackground;
    private Sprite sprBackgroundNormal;
    public Sprite sprBackgroundSelected;

    private void Start()
    {
        sprHandleNoraml = imgHandle.sprite;
        sprBackgroundNormal = imgBackground.sprite;
        onHover.AddListener(changeSelectedSprite);
        onHoverExit.AddListener(changeNormalSprite);
    }

    public void changeSelectedSprite()
    {
        imgHandle.sprite = sprHandleSelected;
        imgBackground.sprite = sprBackgroundSelected;
    }

    public void changeNormalSprite()
    {
        imgHandle.sprite = sprHandleNoraml;
        imgBackground.sprite = sprBackgroundNormal;
    }
}
