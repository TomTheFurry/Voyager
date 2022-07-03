using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public string levelName;
    public void Trigger() {
        if (levelName.Length == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
