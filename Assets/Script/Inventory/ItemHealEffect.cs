 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/health")]
public class ItemHealEffect : ItemEffect
{
	public int healPoint = 0;

	public override bool ExcuteRole()
	{
		Debug.Log("PlayerHp Add:" + healPoint);
		return true;
	}
}
