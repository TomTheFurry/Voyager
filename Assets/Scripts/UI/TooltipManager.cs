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

    public GameObject tmpObj;

    public void ShowTooltip(Tooltip t) {
        currentTooltip = t;
    }

    public void HideTooltip(Tooltip t)
    {
        if (currentTooltip == t) currentTooltip = null;
    }
    
    private void Update()
    {
        if (currentTooltip == null)
        {
            img.enabled = false;
            tmpObj.SetActive(false);
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
            Vector3 pos = rect.position;
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
