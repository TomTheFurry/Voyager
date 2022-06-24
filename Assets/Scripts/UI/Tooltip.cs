using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : UICallback {
    [TextArea(1, 10)]
    public string text;
    public float alpha = 100/255f;
    void Start() {
        onHoverEnter.AddListener(_OnPointerEnter);
        onHoverExit.AddListener(_OnPointerExit);
    }

    void _OnPointerEnter()
    {
        if (text.Trim().Length == 0)
            return;
        Debug.Log("Activating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.ShowTooltip(this, alpha);
    }

    void _OnPointerExit()
    {
        if (text.Trim().Length == 0)
            return;
        Debug.Log("Deactivating Tooltip: " + text);
        if (TooltipManager.Instance != null) TooltipManager.Instance.HideTooltip(this);
    }
}
