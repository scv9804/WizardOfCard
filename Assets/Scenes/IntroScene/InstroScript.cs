using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstroScript : MonoBehaviour
{
    [SerializeField] Image MainSprite;
    void Start()
    {
        StartCoroutine(IntroStart());   
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
