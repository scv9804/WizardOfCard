using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour
{
	Vector3 originScale;
	Vector3 originPos;
	bool toogle = false;

	void Start()
	{
		originScale = this.transform.localScale;
		originPos = this.GetComponent<RectTransform>().anchoredPosition3D;
		this.gameObject.AddComponent<Button>().onClick.AddListener(() =>backPos() );
	}

	void backPos()
	{

		if (!toogle)
		{
			this.transform.localScale *= 4;
			this.transform.localPosition = Vector3.zero;

		}
		else
		{
			this.transform.localScale = originScale;
			this.GetComponent<RectTransform>().anchoredPosition3D = originPos;
		}

		toogle = !toogle;
	}
}
