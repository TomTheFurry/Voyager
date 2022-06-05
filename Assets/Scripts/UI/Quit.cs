using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quit : MonoBehaviour
{
    public void GUI()
    {

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void openQuitInterface()
    {
        gameObject.SetActive(true);
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Open");
    }

    public void closeQuitInterface()
    {
        Debug.Log("Aadaegeg");
        gameObject.SetActive(false);
        Animator anim = GetComponent<Animator>();
        anim.ResetTrigger("Close");
    }
}