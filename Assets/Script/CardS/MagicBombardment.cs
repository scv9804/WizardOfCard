using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBombardment : Card
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

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

        yield return StartCoroutine(Repeat(() => TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage)), ref _target_enemy), i_attackCount)); // 가독성;;;

        if (i_upgraded == 2)
		{
			Attack_RandomEnemy(_target_enemy, ApplyManaAffinity_Instance(i_damage));
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
