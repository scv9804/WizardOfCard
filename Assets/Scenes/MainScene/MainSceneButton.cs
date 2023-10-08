using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Timeline;

using DG.Tweening;

public class MainSceneButton : MonoBehaviour
{
	public PlayableDirector playable;
	public TimelineAsset timeline;
	float time;
	[SerializeField] SpriteRenderer titleSpriteRenderer;
	[SerializeField] Button[] buttons;
	[SerializeField] GameObject[] timelineObject;

	[SerializeField] Image Fade;

    private void Start()
    {
		StartCoroutine(Main());

		IEnumerator Main()
		{
			Fade.DOFade(0.0f, 0.5f);

			yield return new WaitForSeconds(0.5f);

			Fade.gameObject.SetActive(false);


			yield return null;
		}
	}

    private void Update()
	{

		if (Input.anyKey)
		{
			time = 0;
		}
		if (Input.GetAxis("Mouse X") > 1f)
		{
			time = 0;
		}
		if (Input.GetAxis("Mouse Y") > 1f)
		{
			time = 0;
		}



		time += Time.deltaTime;
		if (time > 10f)
		{
			playable.Play();
			ButtonSetDeactive();
		}
		else
		{
			playable.Stop();
			ButtonSetActive();
		}
	}


	void ButtonSetDeactive()
	{
		foreach (var tmpt in buttons)
		{
			tmpt.gameObject.SetActive(false);
		}

		titleSpriteRenderer.gameObject.SetActive(false);
	}

	void ButtonSetActive()
	{
		foreach (var tmpt in buttons)
		{
			tmpt.gameObject.SetActive(true);
		}
		foreach (var tmpt in timelineObject)
		{
			tmpt.gameObject.SetActive(false);
		}
		titleSpriteRenderer.gameObject.SetActive(true);
	}


	public void StartButton()
	{
		StartCoroutine(Main());

		IEnumerator Main()
        {
            Fade.gameObject.SetActive(true);
            Fade.DOFade(1.0f, 2.5f);

			yield return new WaitForSeconds(2.5f);

			LoadSceneManager.LoadScene("Stage 1-1");

			yield return null;
		}
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
