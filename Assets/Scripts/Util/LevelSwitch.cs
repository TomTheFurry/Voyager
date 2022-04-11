using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public string levelName;
    public void Trigger() {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
