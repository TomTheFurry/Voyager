using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	public void NextScene (string sceneName) {
		SceneManager.LoadScene(sceneName);
	}
}


