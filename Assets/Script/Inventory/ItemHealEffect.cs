 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/health")]
public class ItemHealEffect : ItemEffect
{
	public int AmmorPoint = 0;

	public override bool ExcuteRole()
	{
		Debug.Log("PlayerHp Add:" + AmmorPoint);
		return true;
	}
}
