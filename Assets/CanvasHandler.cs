using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasHandler : MonoBehaviour
{
    public InputActionReference actionPause;
    public CornerHud cornerHud;
    private CursorLockMode lastCursorMode;
    private bool lastCursorVisible;
    public bool isPaused {
        get { return pauseMenu.activeSelf; }
        set
        {
            value &= !successMenu.activeSelf && !failMenu.activeSelf;
            bool prev = isPaused;
            if (value == prev) return;
            if (value)
            {
                _pushMouseState();
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                //hud.SetActive(false);
            }
            else
            {
                _popMouseState();
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                //hud.SetActive(true);
            }
        }
    }
    public GameObject pauseMenu;
    public GameObject successMenu;
    public GameObject failMenu;
    public GameObject hud;
    public BonusText bonusPopup;
    public Timer timer;

    private void _pushMouseState()
    {
        lastCursorMode = Cursor.lockState;
        lastCursorVisible = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void _popMouseState()
    {
        Cursor.lockState = lastCursorMode;
        Cursor.visible = lastCursorVisible;
    }

    public void OnFail()
    {
        if (failMenu.activeSelf)
        {
            Debug.LogWarning("OnFail already called before!");
            return;
        }
        if (successMenu.activeInHierarchy) return;
        _pushMouseState();
        failMenu.SetActive(true);
        hud.SetActive(false);
    }
    public void DebugUndoFail()
    {
        _popMouseState();
        failMenu.SetActive(false);
        hud.SetActive(true);
    }

    public void OnSuccess()
    {
        if (successMenu.activeSelf)
        {
            Debug.LogWarning("OnSuccess already called before!");
            return;
        }
        _pushMouseState();
        successMenu.SetActive(true);
        hud.SetActive(false);
    }

    public void OnBonusPopup(float displayTime)
    {
        bonusPopup.gameObject.SetActive(false);
        bonusPopup.setDisableTimer(displayTime);
        bonusPopup.gameObject.SetActive(true);
    }

    private void _OnPause(InputAction.CallbackContext ctx) => isPaused = !isPaused;

    private void Start()
    {
        actionPause.action.performed += _OnPause;
    }
    private void OnDestroy()
    {
        actionPause.action.performed -= _OnPause;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

}
