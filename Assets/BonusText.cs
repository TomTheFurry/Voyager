using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BonusText : MonoBehaviour
{
    public TextMeshProUGUI text;
    float disableTimer = -1f;
    public void setDisableTimer(float f) => disableTimer = f;
    
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
        text.text = $"{current}/{total}";
    }

    private void Update()
    {
        disableTimer -= Time.unscaledDeltaTime;
        if (disableTimer < 0f) gameObject.SetActive(false);
    }
}
