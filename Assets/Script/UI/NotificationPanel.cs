using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text notificationTMP;

    public void Show(string _message)
    {
        notificationTMP.text = _message;
        //Ease Google ÂüÁ¶.
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    private void Start()
    {

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
