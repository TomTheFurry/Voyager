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

    public Scoreboard scoreboard;
    const string langFile = "Stars";
    const string langPrefix = "Level1Star";

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
        childStars[0].text = LangSystem.GetLang(langFile, langPrefix + "0");
        childStars[1].text = LangSystem.GetLang(langFile, langPrefix + "1");
        childStars[2].text = LangSystem.GetLang(langFile, langPrefix + "2");
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

        // star[2]: Find the two hidden space object
        childStars[2].isCompleted = junkTracker.seenGPS && junkTracker.seenJWST;

        bool[] doneStars = new bool[childStars.Length];
        for (int i = 0; i < childStars.Length; i++)
        {
            if (childStars[i].isCompleted)
            {
                doneStars[i] = true;
            }
        }
        float time = timeTracker.GetTime();

        int newStars = PlayerData.AddLevelData(1, doneStars, time);
        int points = newStars * 3;
        PlayerData.GetData().stars += points;
        // TODO: Score based new stars
        Debug.Log("Level1StarControl:\n  New stars: " + newStars + "\n  Time: " + time);
        PlayerData.Save();

        scoreboard.DisplayUpdate(time);
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
        if (hasStartRan) doUpdate();
        else delayedRefresh = true;
    }

    public void OnDisable()
    {
        // Resume timeTracker
        timeTracker.isPaused = false;
    }
}
