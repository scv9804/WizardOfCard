using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapse : Card
{
	[Header("쉴드 소지 시 데미지 배율"), SerializeField] int i_DamageEnhanceValue;

	public override void ExplainRefresh()
    {
        base.ExplainRefresh();

		sb.Replace("{3}", i_DamageEnhanceValue.ToString());

		explainTMP.text = sb.ToString();
    }

	int ShieldBreak(Entity _target, int _value)
	{
		if (_target.i_shield > 0)
		{
			return _value * i_DamageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}

	int ShieldBreak(PlayerEntity _target, int _value)
	{
		if (_target.Status_Shiled > 0)
		{
			return _value * i_DamageEnhanceValue;
		}
		else
		{
			return _value;
		}
	}
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage)));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage)));
		}
		else // 광역 또는 무작위 대상 (?) + 이 카드는 특성상 모듈 못씀;;;
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(ShieldBreak(_target_enemy, i_damage))), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
