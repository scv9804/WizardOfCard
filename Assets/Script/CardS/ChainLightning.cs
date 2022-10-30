using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Card
{
    [Header("공격 횟수"), SerializeField] int i_attackCount;
	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Append("\n{3}번 더 반복합니다.\n");
		sb.Replace("{3}", (i_attackCount - 1).ToString());

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

		yield return StartCoroutine(Repeat(() => Attack_RandomEnemy(_target_enemy, ApplyManaAffinity_Instance(i_damage)), i_attackCount));

		yield return StartCoroutine(EndUsingCard());
	}
}
