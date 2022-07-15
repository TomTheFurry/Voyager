using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStarControl : MonoBehaviour
{
    private Star[] childStars;

    BonusTracker bonusTracker;
    TimeTracker timeTracker;
    bool hasStartRan = false;
    bool delayedRefresh = false;

    public Scoreboard scoreboard;
    const string langFile = "Stars";
    const string langPrefix = "Level0Star";

    void Start()
    {
        Debug.Log("TutorialStarControl Start");
        childStars = GetComponentsInChildren<Star>(true);
        if (childStars.Length != 3) throw new System.Exception("TutorialStarControl: Star count is not 3");
        bonusTracker = FindObjectOfType<BonusTracker>(true);
        if (bonusTracker == null)
            throw new System.Exception("TutorialStarControl: BonusTracker not found");
        timeTracker = FindObjectOfType<TimeTracker>(true);
        if (timeTracker == null)
            throw new System.Exception("TutorialStarControl: TimeTracker not found");
        childStars[0].text = LangSystem.GetLang(langFile, langPrefix+"0");
        childStars[1].text = LangSystem.GetLang(langFile, langPrefix + "1");
        childStars[2].text = LangSystem.GetLang(langFile, langPrefix + "2");
        hasStartRan = true;
    }

    private void doUpdate()
    {
        if (!hasStartRan) Start();
        Debug.Log("TutorialStarControl OnEnable");
        // Pause timeTracker
        timeTracker.isPaused = true;

        // Update star status

        // star[0]: Reached the goal (Always true)
        childStars[0].isCompleted = true;

        // star[1]: Collected all bonuses
        childStars[1].isCompleted = bonusTracker.isAllBonusAquired();

        // star[2]: Complete level under 5 minutes
        childStars[2].isCompleted = timeTracker.GetTime() <= 5 * 60; // Secs. Use <= to give some leeway

        bool[] doneStars = new bool[childStars.Length];
        for (int i = 0; i < childStars.Length; i++)
        {
            if (childStars[i].isCompleted)
            {
                doneStars[i] = true;
            }
        }
        float time = timeTracker.GetTime();

        int newStars = PlayerData.AddLevelData(0, doneStars, time);
        PlayerData.GetData().stars += newStars;
        // TODO: Score based new stars
        Debug.Log("TutorialStarControl:\n  New stars: " + newStars + "\n  Time: " + time);
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
        if (hasStartRan) doUpdate(); else delayedRefresh = true;
    }

    public void OnDisable()
    {
        // Resume timeTracker
        timeTracker.isPaused = false;
    }
}