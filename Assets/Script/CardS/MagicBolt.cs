using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, ApplyManaAffinity_Instance(i_damage));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
