using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;
using System;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPTooltip : UICallback
{
    // To add links into TMP textbox, add this:
    // 
    //     ...Hi here is a <link="Tip:TooltipObjectName">abc</link>.
    //
    // Or use AutoTip, like this:
    //
    //     ...Hi here is a <link="Autotip">abc</link>.
    //
    // Which links to the tooltip object named 'abc'
    // (Note that while keyword cases doesn't matter, the object name needs to match exactly.)
    // The Matcher will first try and find the object in the hinted container,
    //   then it will search this object's child,
    //   then this object's parent,
    //   then globally find the object with the name,
    //   then if all else fails, throw Console error logs



    static Regex r0 = new Regex(@"Tip:\s*(.+)\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled); // Tip:ABC
    static Regex r1 = new Regex(@"Autotip", RegexOptions.IgnoreCase | RegexOptions.Compiled); // Autotip
    static Regex linkFormat0 = new Regex(@"((?:<link=""Tip:.*?"">)|(?:<link=""Autotip"">))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static string linkFormat0Replace = "<u><b>$1";
    static Regex linkFormat1 = new Regex(@"(<\/link>)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    static string linkFormat1Replace = "$1</b></u>";

    TextMeshProUGUI tmpText;
    Tooltip activeTooltip = null;
    public Transform tooltipContainerHint; //A hint to where the tooltips are contained in.

    public void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        onHover.AddListener(_OnHoverTick);
        onClick.AddListener(_Click);
        onHoverExit.AddListener(_Unselect);
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(_textChanged);
    }

    private bool isChanged = false;

    public void LateUpdate()
    {
        if (isChanged) {
            string text = tmpText.text;
            text = linkFormat0.Replace(text, linkFormat0Replace);
            text = linkFormat1.Replace(text, linkFormat1Replace);
            tmpText.text = text;
            tmpText.ForceMeshUpdate();
        }
        isChanged = false;
    }

    void _textChanged(UnityEngine.Object obj) {
        if (obj == (UnityEngine.Object)tmpText) {
            isChanged = true;
        }
    }

    private void _Unselect() {
        if (activeTooltip != null)
        {
            activeTooltip.OnPointerExit(lastData);
            Debug.Log("Unselecting TMP Tooltip " + activeTooltip);
            activeTooltip.gameObject.SetActive(false);
            activeTooltip = null;
        }
    }

    private void _Select(Tooltip newTip) {
        if (activeTooltip == newTip) return;
        _Unselect();
        activeTooltip = newTip;
        activeTooltip.gameObject.SetActive(true);
        Debug.Log("Selecting TMP Tooltip " + activeTooltip);
        activeTooltip.OnPointerEnter(lastData);
    }

    private void _Click() {
        if (activeTooltip != null) activeTooltip.OnPointerClick(lastData);
    }

    private Tooltip _FindTip(string name) {
        // First, search using the hints.
        if (tooltipContainerHint != null)
        {
            Tooltip[] hintTips = tooltipContainerHint.GetComponentsInChildren<Tooltip>(true);
            var t = Array.Find(hintTips, (t) => t.name.Equals(name));
            if (t != null) return t;
            hintTips = tooltipContainerHint.GetComponentsInChildren<AdvancedTooltip>(true);
            t = Array.Find(hintTips, (t) => t.name.Equals(name));
            if (t != null) return t;
        }

        // Second, search self childs with name.
        {
            Tooltip[] directTips = gameObject.transform.GetComponentsInChildren<Tooltip>(true);
            var t = Array.Find(directTips, (t) => t.name.Equals(name));
            if (t != null) return t;
            directTips = gameObject.transform.GetComponentsInChildren<AdvancedTooltip>(true);
            t = Array.Find(directTips, (t) => t.name.Equals(name));
            if (t != null) return t;
        }
        // Then, search parents
        {
            Tooltip[] parentTips = gameObject.transform.GetComponentsInParent<Tooltip>(true);
            var t = Array.Find(parentTips, (t) => t.name.Equals(name));
            if (t != null) return t;
            parentTips = gameObject.transform.GetComponentsInParent<AdvancedTooltip>(true);
            t = Array.Find(parentTips, (t) => t.name.Equals(name));
            if (t != null) return t;
        }
        // Then, search globally with name
        {
            var obj = GameObject.Find(name);
            if (obj != null && obj.GetComponent<Tooltip>() != null) return obj.GetComponent<Tooltip>();
            if (obj != null && obj.GetComponent<AdvancedTooltip>() != null) return obj.GetComponent<AdvancedTooltip>();
        }
        // Finally, search ALL tooltips to try and find it.
        {
            var objs = FindObjectsOfType(typeof(Tooltip));
            var obj = Array.Find(objs, (obj) => obj.name.Equals(name) && obj is GameObject @gobj && @gobj.GetComponent<Tooltip>() != null);
            if (obj!=null) return ((GameObject)obj).GetComponent<Tooltip>();
            objs = FindObjectsOfType(typeof(AdvancedTooltip));
            obj = Array.Find(objs, (obj) => obj.name.Equals(name) && obj is GameObject @gobj && @gobj.GetComponent<AdvancedTooltip>() != null);
            if (obj != null) return ((GameObject)obj).GetComponent<AdvancedTooltip>();
        }
        // Otherwise, return null, and also throw Debug error
        Debug.LogError("Failed to find Tooltip obj named '" + name + "'");
        return null;
    }

    private void _OnHoverTick() {
        var data = lastData;
        var idx = TMP_TextUtilities.FindIntersectingLink(tmpText, data.position, data.pressEventCamera);
        if (idx == -1) {
            _Unselect();
            return;
        }
        TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[idx];
        var mth = r0.Match(linkInfo.GetLinkID());
        string id = "";
        if (mth.Success && mth.Groups.Count >= 2)
        {
            id = mth.Groups[1].Value;
        }
        else if (r1.IsMatch(linkInfo.GetLinkID()))
        {
            id = linkInfo.GetLinkText().Trim();
            if (id.Length == 0)
            {
                _Unselect();
                return;
            }
            id = id.Substring(0, 1).ToUpper() + id.Substring(1);
            // Convert this: "here is a thing" to "HereIsAThing"
            for (int i = 0; i < id.Length; i++)
            {
                if (id[i] == ' ')
                {
                    id = id.Substring(0, i) + id.Substring(i + 1, 1).ToUpper() + id.Substring(i + 2);
                }
            }

        }
        if (id.Length == 0) {
            _Unselect();
            return;
        }

        Tooltip tip = _FindTip(id);
        if (tip == null)
        {
            _Unselect();
        }
        else {
            _Select(tip);
        }
    }
}
