using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(TextResizer))]
[RequireComponent(typeof(Image))]
public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public Tooltip currentTooltip = null;

    private TextResizer rs;
    private Image img;

    private float alpha = 100f/255f;

    public GameObject tmpObj; //textMeshPro Obj

    public void ShowTooltip(Tooltip t, float alpha = 100f/255f)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;

        if (currentTooltip != null && currentTooltip.GetType() == typeof(AdvancedTooltip)) {
            GameObject obj = ((AdvancedTooltip)currentTooltip).customTooltip;
            obj.SetActive(false);
        }
        currentTooltip = t;
        Update();
    }

    public void HideTooltip(Tooltip t)
    {
        if (currentTooltip == t)
        {
            if (currentTooltip != null && currentTooltip.GetType() == typeof(AdvancedTooltip))
            {
                ((AdvancedTooltip)currentTooltip).customTooltip.SetActive(false);
            }
            currentTooltip = null;
            Update();
        }
    }

    private void Update()
    {
        RectTransform thisRt = GetComponent<RectTransform>();
        Canvas canvas = thisRt.parent.GetComponent<Canvas>();

        if (currentTooltip == null)
        {
            img.enabled = false;
            tmpObj.SetActive(false);
        }
        else if (currentTooltip.GetType() == typeof(AdvancedTooltip)) {
            img.enabled = false;
            tmpObj.SetActive(false);
            GameObject ui = ((AdvancedTooltip)currentTooltip).customTooltip;
            RectTransform rect = ui.GetComponent<RectTransform>();

            // Get MousePos using new InputSystem
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ui.transform.position = mousePos;
            //Debug.Log("mouse:"+mousePos+",min:" + rect.offsetMin + ",max:" + rect.offsetMax);
            Vector2 maxWidths = canvas.renderingDisplaySize;
            Vector2 minRect = rect.rect.min;
            Vector2 maxRect = rect.rect.max;
            //Debug.Log("A!maxWidth: " + maxWidths + ",minRect: " + minRect + ",maxRect" + maxRect);
            minRect = rect.localToWorldMatrix * new Vector4(minRect.x, minRect.y, 0, 1);
            maxRect = rect.localToWorldMatrix * new Vector4(maxRect.x, maxRect.y, 0, 1);
            //Debug.Log("maxWidth: " + maxWidths + ",minRect: " + minRect + ",maxRect" + maxRect);

            if (minRect.x < 0)
            {
                Vector3 pos = rect.position;
                pos.x -= minRect.x;
                rect.position = pos;
            }
            else if (maxRect.x > maxWidths.x)
            {
                Vector3 pos = rect.position;
                pos.x -= maxRect.x-maxWidths.x;
                rect.position = pos;
            }
            ui.SetActive(true);
        }
        else
        {
            img.enabled = true;
            tmpObj.SetActive(true);
            RectTransform rect = GetComponent<RectTransform>();
            tmpObj.GetComponent<TextMeshProUGUI>().text = LangSystem.parseText(currentTooltip.text);
            rs.ResizeText(rect, tmpObj.GetComponent<TextMeshProUGUI>());
            // Get MousePos using new InputSystem
            Vector2 mousePos = Mouse.current.position.ReadValue();
            transform.position = mousePos;
            //Debug.Log("mouse:"+mousePos+",min:" + rect.offsetMin + ",max:" + rect.offsetMax);
            if (rect.offsetMin.x < 0)
            {
                rect.offsetMax = new Vector2(rect.offsetMax.x - rect.offsetMin.x, rect.offsetMax.y);
                rect.offsetMin = new Vector2(0, rect.offsetMin.y);
            }
            else if (rect.offsetMax.x > 0)
            {
                rect.offsetMin = new Vector2(rect.offsetMin.x - rect.offsetMax.x, rect.offsetMin.y);
                rect.offsetMax = new Vector2(0, rect.offsetMax.y);
            }
        }
    }




    void Start()
    {
        rs = GetComponent<TextResizer>();
        img = GetComponent<Image>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Cannot have more than one TooltipManager in the scene!");
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
