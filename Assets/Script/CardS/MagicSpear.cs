using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpear : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, ApplyManaAffinity_Instance(i_damage));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), ref _target_enemy);
		}

		RestoreAether(1);

		yield return StartCoroutine(EndUsingCard());
	}
}
