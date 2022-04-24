using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FullScreen : MonoBehaviour
{
    public void toggleFullScreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Toggled fullscreen to " + Screen.fullScreen);
    }

    public void OnFullScreen(InputValue value)
    {
        if (value.isPressed)
        {
            toggleFullScreen();
        }
    }
}
