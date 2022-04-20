
using UnityEngine;
using TMPro;

public class TextResizer : MonoBehaviour
{
    public float textBuffer = 10;
    public float maxWidth = 500;

    public void ResizeText(RectTransform targetTransform, TextMeshProUGUI tmp)
    {
        targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
        tmp.ForceMeshUpdate(true);
        Vector3 size = tmp.textBounds.size;
        //Debug.Log("Text size: " + size);
        if (size.x <= 0 || size.y <= 0)
        {
            size.x = 0;
            size.y = 0;
        }
        targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x + textBuffer * 2);
        targetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y + textBuffer * 2);
    }
}
