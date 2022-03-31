using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public bool isPaused = false;
    private bool stateChanged = true;
    public GameObject pauseMenu;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (!stateChanged) return;
        stateChanged = false;
        if (isPaused) {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            // Loop over all root game objects in current scene and broadcast OnPauseMenuStart event
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                go.BroadcastMessage("OnPauseMenuStart", SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            // Loop over all root game objects in current scene and broadcast OnPauseMenuEnd event
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                go.BroadcastMessage("OnPauseMenuEnd", SendMessageOptions.DontRequireReceiver);
            }
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
}
