using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{

	public void GotoMainMenu()
	{
        SceneManager.LoadScene("MainScene");

		BETA.GameManager.Instance.GameEnd();


		//BETA.GameManager.Instance.Loading("MainScene", BETA.GameManager.Instance.GameEnd);
	}


}
