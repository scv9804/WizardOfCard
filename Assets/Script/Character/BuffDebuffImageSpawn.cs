using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffDebuffImageSpawn : MonoBehaviour
{
	[SerializeField] Image buffImage;
	[SerializeField] TMP_Text text_TMP;

	public void Setup(Sprite _sprite , int _value)
	{
		buffImage.sprite = _sprite;
		text_TMP.text = _value.ToString();
	}
}
