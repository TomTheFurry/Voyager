using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraImage : UICallback
{
    public float zoomRate = 100f;
    public float maxSize = 600f;
    public float minSize = 450f;

    private bool onPointer = false;
    RectTransform rt;

    void Start()
    {
        onHoverEnter.AddListener(_OnPointerEnter);
        onHoverExit.AddListener(_OnPointerExit);
        rt = GetComponent<RectTransform>();
        changeSize(minSize, minSize);
    }

    void Update()
    {
        float addSize = zoomRate * Time.deltaTime;

        if (onPointer)
        {
            increaseSize(addSize, addSize);
        }
        else
        {
            decreaseSize(addSize, addSize);
        }
    }

    void increaseSize(float x, float y)
    {
        float sX = rt.sizeDelta.x;
        float sY = rt.sizeDelta.y;

        if (sY == maxSize)
            return;

        if (sY > maxSize)
        {
            rt.sizeDelta = new Vector2(maxSize, maxSize);
            return;
        }

        changeSize(sX + x, sY + y);
    }

    void decreaseSize(float x, float y)
    {
        float sX = rt.sizeDelta.x;
        float sY = rt.sizeDelta.y;

        if (sY == minSize)
            return;

        if (sY < minSize)
        {
            rt.sizeDelta = new Vector2(minSize, minSize);
            return;
        }

        changeSize(sX - x, sY - y);
    }

    void changeSize(float x, float y)
    {
        rt.sizeDelta = new Vector2(x, y);
    }

    void _OnPointerEnter()
    {
        onPointer = true;
    }

    void _OnPointerExit()
    {
        onPointer = false;
    }
}
