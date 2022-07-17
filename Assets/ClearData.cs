using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearData : MonoBehaviour
{
    public void Clear() {
        Global.ResetDataAndRestart();
    }
}
