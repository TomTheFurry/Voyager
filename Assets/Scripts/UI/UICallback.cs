using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UICallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected UnityEvent onHover = new UnityEvent();
    protected UnityEvent onHoverEnter = new UnityEvent();
    protected UnityEvent onHoverExit = new UnityEvent();
    protected UnityEvent onClick = new UnityEvent();
    private bool isHovering = false;


    void Update()
    {
        if (isHovering) {
            onHover.Invoke();
        }
    }

    // Below 2 calls fix bug where IHoverExit didn't trigger on obj destroy/disable
    // Note that the reverse (obj enable) does hoever trigger IHoverEnter
    void OnDestroy()
    {
        if (isHovering)
        {
            isHovering = false;
            onHoverExit.Invoke();
        }
    }
    void OnDisable()
    {
        if (isHovering)
        {
            isHovering = false;
            onHoverExit.Invoke();
        }
    }

    // Internal Callback Hooks
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering)
        {
            isHovering = true;
            onHoverEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovering)
        {
            isHovering = false;
            onHoverExit.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick.Invoke();
    }
}
