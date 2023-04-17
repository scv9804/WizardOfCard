using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] Sprite []turnSprite;
    Image turnImage;

    public void Show(bool _myTurn)
    {
		if (turnImage == null)
		{
            turnImage = GameObject.Find("Notification Panel").GetComponent<Image>();
        }




        if (_myTurn)
		{
            turnImage.sprite = turnSprite[0];
		}
		else
        {
            turnImage.sprite = turnSprite[1];
        }

        //Ease Google ÂüÁ¶.
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one *0.7f , 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

  

    [ContextMenu("SclaeOne")]
    void ScaleOne()
    {
        transform.localScale = Vector3.one;
    }
    [ContextMenu("SclaeZero")]
    void ScaleZero()
    {
        transform.localScale = Vector3.zero;
    }

}
