using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(MovementControl2))]
public class TimeTracker : MonoBehaviour
{
    public float deltaTime = 0;
    public bool isPaused = false;
    public float levelMaxTime = 0f;

    private Timer timer;

    void Start()
    {
        timer = FindObjectOfType<CanvasHandler>().timer;
        timer.setTime(0);
    }

    public void Update()
    {
        if (isPaused) return;
        deltaTime += Time.deltaTime;
        timer.setTime(deltaTime, levelMaxTime);

        if (levelMaxTime != 0f && deltaTime >= levelMaxTime)
        {
            isPaused = true;
            GetComponent<MovementControl2>().DisableSpaceshipInput();
            FindObjectOfType<CanvasHandler>().OnFail();
        }
    }

    public float GetTime()
    {
        return deltaTime;
    }
}