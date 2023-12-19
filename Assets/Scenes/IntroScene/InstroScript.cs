using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class InstroScript : MonoBehaviour
{
    [SerializeField] Image MainSprite;
    [SerializeField] PlayableDirector playable;

    void Start()
    {
        StartCoroutine(IntroStart());   
    }

	private void Update()
	{
        if (playable.state == PlayState.Paused)
        {
            SceneManager.LoadScene("MainScene");
        }
           
	}

	public void SkipButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    IEnumerator IntroStart()
	{
        yield return new WaitForSeconds(0.1f); ;
        Sequence sequence1 = DOTween.Sequence()
        .Append(MainSprite.DOFade(1, 0.5f));
        yield return new WaitForSeconds(1.2f); ;
        Sequence sequence2 = DOTween.Sequence()
        .Append(MainSprite.DOFade(0, 0.5f));
    }

}
