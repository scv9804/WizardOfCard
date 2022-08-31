using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/Wand_01")]
public class Wand_01 : ItemEffect
{
	public override bool ExcuteRole()
	{
		PlayerEntity.Inst.Status_MaxHealth += 5;
		return true;
	}
}
