using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTracker : MonoBehaviour
{
    public int bonuses = 0;
    public int totalBonuses = 0;
    public float displayTime = 2;

    private CanvasHandler canvas;

    private void Start()
    {
        canvas = FindObjectOfType<CanvasHandler>();
    }


    public void AquireBonus()
    {
        bonuses++;
        canvas.OnBonusPopup(displayTime);
    }

    public bool isAllBonusAquired()
    {
        return bonuses >= totalBonuses;
    }
}
