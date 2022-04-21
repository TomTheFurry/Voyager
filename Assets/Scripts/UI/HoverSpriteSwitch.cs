using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HoverSpriteSwitch : UICallback
{
    public Sprite onHoverSprite;
    private Image img;
    private Sprite _normSprite;

    void Start()
    {
        img = GetComponent<Image>();
        _normSprite = img.sprite;
        onHoverEnter.AddListener(_OnHoverEnter);
        onHoverExit.AddListener(_OnHoverExit);
    }

    void _OnHoverEnter() {
        img.sprite = onHoverSprite;
    }
    void _OnHoverExit()
    {
        img.sprite = _normSprite;
    }
}
