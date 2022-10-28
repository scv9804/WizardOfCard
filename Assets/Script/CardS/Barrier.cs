using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//PlayerEntity.Inst.Status_Shiled += ApplyMagicResistance(i_damage);

		//BattleCalculater.Inst.SpellEnchaneReset();
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***����(����� �Ҿ����� �� ����)*** <<22-10-27 ������ :: �߰�>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		T_Shield(i_damage);

		yield return StartCoroutine(T_EndUsingCard());
	}
}
