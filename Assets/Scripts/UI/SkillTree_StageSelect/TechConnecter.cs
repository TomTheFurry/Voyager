using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechConnecter : MonoBehaviour
{
    Image image;
    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;

    public void Start() {
        image = GetComponent<Image>();
    }

    public void SetState(int i) {
        switch (i)
        {
            case 0: _SetSprite(sprite0); break;
            case 1: _SetSprite(sprite1); break;
            case 2: _SetSprite(sprite2); break;
            default: throw new System.Exception();
        }
    }

    private void _SetSprite(Sprite spr) {
        image.sprite = spr;
    }
    
}
