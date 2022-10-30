using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Card
{
	[Header("화상"), SerializeField] int i_applyBurning;

	public override void ExplainRefresh()
    {
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyEnhanceValue(i_applyBurning).ToString());

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Add_Burning(_target_enemy, i_applyBurning);
			Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage));

		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Add_Burning(_target_player, i_applyBurning);
			Attack(_target_player, ApplyManaAffinity_Instance(i_damage));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Add_Burning(_target_enemy, i_applyBurning), ref _target_enemy);
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
