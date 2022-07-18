using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2StarControl : MonoBehaviour
{
    private Star[] childStars;
    Level2GoalTracker goalTracker;
    TimeTracker timeTracker;
    MovementControl2 control2;
    bool hasStartRan = false;
    bool delayedRefresh = false;

    public Scoreboard scoreboard;
    const string langFile = "Stars";
    const string langPrefix = "Level2Star";

    void Start()
    {
        Debug.Log("Level2StarControl Start");
        childStars = GetComponentsInChildren<Star>(true);
        if (childStars.Length != 3) throw new System.Exception("Level2StarControl: Star count is not 3");
        goalTracker = FindObjectOfType<Level2GoalTracker>(true);
        if (goalTracker == null)
            throw new System.Exception("Level2StarControl: Level2GoalTracker not found");
        timeTracker = FindObjectOfType<TimeTracker>(true);
        if (timeTracker == null)
            throw new System.Exception("Level2StarControl: TimeTracker not found");
        control2 = FindObjectOfType<MovementControl2>(true);
        if (control2 == null)
            throw new System.Exception("Level2StarControl: MovementControl2 not found");

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

        // star[1]: Perfectly hit the goal with more than 15 m/s
        childStars[1].isCompleted = goalTracker.hasGoodFinish &&
            control2.childObject.GetComponent<Rigidbody>().velocity.magnitude >= goalTracker.speedThresold;

        // star[2]: Fly past the Voyager within a close distance
        childStars[2].isCompleted = goalTracker.seenVoyager;

        bool[] doneStars = new bool[childStars.Length];
        for (int i = 0; i < childStars.Length; i++)
        {
            if (childStars[i].isCompleted)
            {
                doneStars[i] = true;
            }
        }
        float time = timeTracker.GetTime();

        int newStars = PlayerData.AddLevelData(2, doneStars, time);
        int points = newStars * 3;
        PlayerData.GetData().stars += points;
        // TODO: Score based new stars
        Debug.Log("Level2StarControl:\n  New stars: " + newStars + "\n  Time: " + time);
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
