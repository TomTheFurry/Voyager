using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1StarControl : MonoBehaviour
{
    private Star[] childStars;
    JunkGoalTracker junkTracker;

    TimeTracker timeTracker;
    bool hasStartRan = false;
    bool delayedRefresh = false;

    void Start()
    {
        Debug.Log("Level1StarControl Start");
        childStars = GetComponentsInChildren<Star>(true);
        if (childStars.Length != 3) throw new System.Exception("Level1StarControl: Star count is not 3");
        junkTracker = FindObjectOfType<JunkGoalTracker>(true);
        if (junkTracker == null)
            throw new System.Exception("Level1StarControl: JunkGoalTracker not found");
        timeTracker = FindObjectOfType<TimeTracker>(true);
        if (timeTracker == null)
            throw new System.Exception("Level1StarControl: TimeTracker not found");
        hasStartRan = true;
    }

    private void doUpdate()
    {
        if (!hasStartRan) Start();
        Debug.Log("Level1StarControl OnEnable");
        // Pause timeTracker
        timeTracker.isPaused = true;

        // Update star status
        
        // star[0]: Reached the goal (Always true)
        childStars[0].isCompleted = true;

        // star[1]: Destroyed 200 space junks
        childStars[1].isCompleted = junkTracker.count >= junkTracker.targetCount;

        // star[2]: Complete level under 50% of allocated time
        childStars[2].isCompleted = timeTracker.GetTime() <= timeTracker.levelMaxTime * 0.5f;

        bool[] doneStars = new bool[childStars.Length];
        for (int i = 0; i < childStars.Length; i++)
        {
            if (childStars[i].isCompleted)
            {
                doneStars[i] = true;
            }
        }

        int newStars = PlayerData.AddLevelData(1, doneStars, timeTracker.GetTime());
        PlayerData.GetData().stars += newStars;
        // TODO: Score based new stars
        Debug.Log("Level1StarControl:\n  New stars: " + newStars + "\n  Time: " + timeTracker.GetTime());
        PlayerData.Save();
    }

    private void Update()
    {
        if (delayedRefresh)
        {
            doUpdate();
            delayedRefresh = false;
        }
    }

    private void OnEnable()
    {
        if (hasStartRan) doUpdate(); else delayedRefresh = true;
    }

    public void OnDisable()
    {
        // Resume timeTracker
        timeTracker.isPaused = false;
    }
}
