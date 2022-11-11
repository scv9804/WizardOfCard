using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("공격 횟수"), SerializeField] int[] attackCount = new int[3];

	#region 프로퍼티

	int I_AttackCount
	{
		get
		{
			return attackCount[i_upgraded];
		}

		//set
		//{
		//    attackCount[i_upgraded] = value;
		//}
	}

	int I_Damage
	{
		get
		{
			return ApplyMagicAffinity(i_damage);
		}

		//set
		//{
		//    I_Damage = value;
		//}
	}

	#endregion

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{3}", (I_AttackCount - 1).ToString());

		explainTMP.text = sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		yield return StartCoroutine(Repeat(() => Attack_RandomEnemy(_target_enemy, I_Damage), I_AttackCount));

		yield return StartCoroutine(EndUsingCard());
	}
}
