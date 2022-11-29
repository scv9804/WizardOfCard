using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffDebuffImageSpawn : MonoBehaviour
{
	string buffDebuffName;
	public int value = 0;
	int code;
	[SerializeField] Image buffImage;
	[SerializeField] TMP_Text text_TMP;

	public int BuffDebuffCode { get { return code; } }
	public string Name { get { return buffDebuffName; } }

	public void SetValue()
	{
		text_TMP.text= value.ToString();
	}

	public void Setup(Sprite _sprite ,string _buffDebuffName ,int _value, int _code)
	{
		code = _code;
		value = _value;
		buffDebuffName = _buffDebuffName;
		buffImage.sprite = _sprite;
		text_TMP.text = _value.ToString();
	}
}
