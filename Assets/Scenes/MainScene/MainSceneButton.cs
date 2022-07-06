using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneButton : MonoBehaviour
{

	public void StartButton()
	{
		SceneManager.LoadScene("Stage 1-1");
	}

	public void Fun()
    {
		SceneManager.LoadScene("Have a Fun!");
    }
	public void GameOver()
    {
		Application.Quit();
	}


}
