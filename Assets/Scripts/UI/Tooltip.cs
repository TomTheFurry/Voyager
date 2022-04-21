using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(1, 10)]
    public string text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Activating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.ShowTooltip(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Deactivating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip(this);
    }

    private void OnDestroy()
    {
        if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip(this);
    }
    private void OnDisable()
    {
        if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip(this);
    }
}
