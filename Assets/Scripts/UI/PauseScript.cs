using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PauseScript : MonoBehaviour
{
    public bool isPaused = false;
    private bool stateChanged = true;

    public UnityEvent onPauseMenuStart;
    public UnityEvent onPauseMenuEnd;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!stateChanged) return;
        stateChanged = false;
        if (isPaused) {
            Time.timeScale = 0;
            onPauseMenuStart.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            onPauseMenuEnd.Invoke();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        stateChanged = true;
    }

    public void SetPause(bool pause)
    {
        if (isPaused == pause) return;
        isPaused = pause;
        stateChanged = true;
    }

    public void OnSceneExit() {
        Time.timeScale = 1;
    }
}
