using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpear : Card
{
	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//if (_target_enemy != null && _target_player == null) // 단일 대상
		//{
		//	Attack_SingleEnemy(_target_enemy, ApplyManaAffinity(i_damage));
		//}
		//else if (_target_enemy == null && _target_player != null) // 자신 대상
		//{
		//	Attack_PlayerSelf(_target_player, ApplyManaAffinity(i_damage));
		//}
		//else // 광역 또는 무작위 대상 (?)
		//{
		//	for (int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
		//	{
		//		Attack_SingleEnemy(EntityManager.Inst.enemyEntities[i], ApplyManaAffinity(i_damage));
		//	}
		//}

		//PlayerEntity.Inst.Status_Aether++;

		//BattleCalculater.Inst.SpellEnchaneReset();
	}
	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			T_Attack(_target_enemy, i_damage);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			T_Attack(_target_player, i_damage);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			T_Attack_AllEnemy(_target_enemy, i_damage);
		}

		T_ResotreAether(1);

		yield return StartCoroutine(T_EndUsingCard());
	}
}
