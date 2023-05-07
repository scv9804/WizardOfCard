using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStateSO", menuName = "StateStorage/StateSO")]
public class PlayerStateSO : ScriptableObject
{
	[SerializeField]
	private int money;
	[SerializeField]
	private float maxHP;
	[SerializeField]
	private float HP;
	private int MP;
	private int maxMP;

	public void Update_State(PlayerEntity player)
	{
		Debug.Log("체력 확인"+ HP);
		maxHP = player.Status_MaxHealth;
		HP = player.Status_Health;
	}
}
