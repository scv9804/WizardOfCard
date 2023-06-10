using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateStorage : MonoBehaviour
{
	public static CharacterStateStorage Inst { get; set; }

	[HideInInspector]public float maxHealth;
	public int money;

	int Aether;
	public int aether { get { return Aether; } set{ Aether = value; } }


	private void Start()
	{
		Inst = this;
		DontDestroyOnLoad(this);

		UIManager.Inst.money_TMP.text = money.ToString("D3");
	}

}