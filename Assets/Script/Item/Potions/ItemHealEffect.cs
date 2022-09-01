using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "itemEffect/Custom/health")]
public class ItemHealEffect : ItemEffect
{
	public int HealPoint = 0;

	public override bool ExcuteRole()
	{
        float CurrentHealthPoint = PlayerEntity.Inst.Status_Health + HealPoint;

        PlayerEntity.Inst.Status_Health = CurrentHealthPoint <= PlayerEntity.Inst.Status_MaxHealth ? CurrentHealthPoint : PlayerEntity.Inst.Status_MaxHealth;

        return true;
    }
}
