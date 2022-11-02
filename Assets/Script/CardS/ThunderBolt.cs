using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Card
{
    [Header("최댓값"), SerializeField] int i_maxDamage;

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff0000>{4}</color>");
		sb.Replace("{4}", ApplyManaAffinity(i_maxDamage).ToString());

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		int totalDamage = ApplyManaAffinity_Instance(Random.Range(i_damage, i_maxDamage + 1));

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, totalDamage);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, totalDamage);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, totalDamage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
