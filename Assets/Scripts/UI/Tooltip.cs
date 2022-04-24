using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : UICallback {
    [TextArea(1, 10)]
    public string text;
    void Start() {
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
