using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UICallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector]
    public UnityEvent onHover = new UnityEvent();
    [HideInInspector]
    public UnityEvent onHoverEnter = new UnityEvent();
    [HideInInspector]
    public UnityEvent onHoverExit = new UnityEvent();
    [HideInInspector]
    public UnityEvent onClick = new UnityEvent();
    [HideInInspector]
    public bool isHovering = false;
    [HideInInspector]
    public PointerEventData lastData;

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
        lastData = eventData;
        if (!isHovering)
        {
            isHovering = true;
            onHoverEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lastData = eventData;
        if (isHovering)
        {
            isHovering = false;
            onHoverExit.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        lastData = eventData;
        onClick.Invoke();
    }
}
