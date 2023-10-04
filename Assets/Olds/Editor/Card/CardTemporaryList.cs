using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTemporaryList : MonoBehaviour
{
	public static CardTemporaryList Inst { get; set; }

	public List<Card> temporaryList;



	private void Start()
	{
		Inst = this;
		DontDestroyOnLoad(this);
	}

}
