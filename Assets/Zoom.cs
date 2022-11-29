using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
	Vector3 originScale;

	void Start()
	{
		originScale = this.transform.localScale;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		this.transform.localScale = originScale;
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log(11212);
		this.transform.localScale *= 2f;
	}
}
