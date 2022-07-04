using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdvancedTooltip : Tooltip
{
    public GameObject customTooltip;

    AdvancedTooltip()
    {
        onHoverEnter.AddListener(_OnPointerEnter);
        onHoverExit.AddListener(_OnPointerExit);
    }

    void _OnPointerEnter()
    {
        Debug.Log("Activating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.ShowTooltip(this);
    }

    void _OnPointerExit()
    {
        Debug.Log("Deactivating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip(this);
    }
}
