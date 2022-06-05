using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BonusText : MonoBehaviour
{
    private void OnEnable()
    {
        BonusTracker tracker = FindObjectOfType<BonusTracker>();
        if (tracker == null)
        {
            Debug.LogError("BonusTracker not found on enabling bonus text");
            return;
        }
        int total = tracker.totalBonuses;
        int current = tracker.bonuses;
        var text = GetComponent<TextMeshProUGUI>();
        text.text = $"{current}/{total}";
    }
}
