using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentratedFire : Card
{
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        int _count = CardManager.Inst.myCards.Count + 1;

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
    }
}
