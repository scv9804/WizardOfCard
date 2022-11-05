using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentratedFire : Card
{
    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

        PlayerEntity.Inst.SpellEnchaneReset();

        int _count = CardManager.Inst.myCards.Count + 1;

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), _count)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
    }
}
