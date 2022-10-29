using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBombardment : Card
{
	[Header("공격 횟수"), SerializeField] int i_attackCount;
	[Header("무작위 적 추가공격 여부"), SerializeField] bool b_canExtraAttack;

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		if (i_attackCount > 1)
		{
			sb.Append("\n{3}번 더 반복합니다.\n");
			sb.Replace("{3}", (i_attackCount - 1).ToString());
		}

		if (b_canExtraAttack)
        {
			sb.Append("그 후 무작위 적에게\n 데미지를 <color=#ff0000>{1}</color> 줍니다.");
			sb.Replace("{1}", ApplyManaAffinity(i_damage).ToString());
		}

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

        yield return StartCoroutine(Repeat(() => Attack_AllEnemy(_target_enemy, i_damage), i_attackCount));

        if (b_canExtraAttack)
		{
			Attack_RandomEnemy(_target_enemy, i_damage);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
