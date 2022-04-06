using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public void OnStartGame(int SceneNumber)
    {
        SceneManager.LoadScene(SceneNumber);
    }
}