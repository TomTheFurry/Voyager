using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2GoalTracker : MonoBehaviour
{
    public bool hasGoodFinish = false;
    public bool seenVoyager = false;
    public float speedThresold;

    public void SetGoodFinish() { hasGoodFinish = true; }
    public void SetSeenVoyager() { seenVoyager = true; }
}
