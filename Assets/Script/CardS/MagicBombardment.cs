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

	public override void UseCard(Entity _target_enemy = null, PlayerEntity _target_player = null)
	{
		base.UseCard(_target_enemy, _target_player);

		//StartCoroutine(Repeat(() => Attack_AllEnemy(ApplyManaAffinity(i_damage)), i_attackCount));
		////StartCoroutine(RepeatC_AAE(i_attackCount));

		//if (b_canExtraAttack)
		//{
		//	Attack_SingleEnemy(EntityManager.Inst.enemyEntities[Random.Range(0, EntityManager.Inst.enemyEntities.Count)], ApplyManaAffinity(i_damage));
		//}

		//BattleCalculater.Inst.SpellEnchaneReset();
	}

	public override IEnumerator T_UseCard(Entity _target_enemy, PlayerEntity _target_player = null)  // ***실험(기능이 불안정할 수 있음)*** <<22-10-27 장형용 :: 추가>>
	{
		yield return StartCoroutine(base.T_UseCard(_target_enemy, _target_player));

		BattleCalculater.Inst.SpellEnchaneReset();

        yield return StartCoroutine(Repeat(() => T_Attack_AllEnemy(_target_enemy, i_damage), i_attackCount));

        if (b_canExtraAttack)
		{
			T_Attack_RandomEnemy(_target_enemy, i_damage);
		}

		Debug.Log("아니 카드가 먼저 퇴근한다니까?");

		yield return StartCoroutine(T_EndUsingCard());
	}
}
