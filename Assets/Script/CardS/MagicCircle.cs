using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		PlayerEntity.Inst.Status_EnchaneValue *= i_damage;

		for(int i = 0; i < CardManager.Inst.myCards.Count; i++)
        {
			CardManager.Inst.myCards[i].ExplainRefresh();
        }
	}
}
