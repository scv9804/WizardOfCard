using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Card
{
    [Header("공격 횟수"), SerializeField] int[] attackCount = new int[3];

	#region 프로퍼티

	public int i_attackCount
	{
		get
		{
			return attackCount[i_upgraded];
		}

		//set
		//{
		//	attackCount[i_upgraded] = value;
		//}
	}

	#endregion

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

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
