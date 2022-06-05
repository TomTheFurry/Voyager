using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTracker : MonoBehaviour
{
    public int bonuses = 0;
    public int totalBonuses = 0;
    public float displayTime = 2;
    private float timer = 0;
    public GameObject bonusUI;

    public void AquireBonus()
    {
        bonusUI.SetActive(false); // In case the last bonus UI is still active
        timer = displayTime;
        bonuses++;
        bonusUI.SetActive(true);
    }

    void Update()
    {
        if (timer < 0) {
            bonusUI.SetActive(false);
        }
        timer -= Time.deltaTime;
    }

    public bool isAllBonusAquired()
    {
        return bonuses >= totalBonuses;
    }
}
