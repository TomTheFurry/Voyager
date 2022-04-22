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

    public GameObject tmpObj; //textMeshPro Obj

    public void ShowTooltip(Tooltip t) {
        if (currentTooltip != null && currentTooltip.GetType() == typeof(AdvancedTooltip)) {
            ((AdvancedTooltip)currentTooltip).customTooltip.SetActive(false);
        }
        currentTooltip = t;
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
        }
    }

    private void Update()
    {
        if (currentTooltip == null)
        {
            img.enabled = false;
            tmpObj.SetActive(false);
        }
        else if (currentTooltip.GetType() == typeof(AdvancedTooltip)) {
            img.enabled = false;
            tmpObj.SetActive(false);
            GameObject ui = ((AdvancedTooltip)currentTooltip).customTooltip;
            ui.SetActive(true);
            RectTransform rect = ui.GetComponent<RectTransform>();

            // Get MousePos using new InputSystem
            Vector2 mousePos = Mouse.current.position.ReadValue();
            ui.transform.position = mousePos;
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
        else
        {
            img.enabled = true;
            tmpObj.SetActive(true);
            RectTransform rect = GetComponent<RectTransform>();
            tmpObj.GetComponent<TextMeshProUGUI>().text = currentTooltip.text;
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
