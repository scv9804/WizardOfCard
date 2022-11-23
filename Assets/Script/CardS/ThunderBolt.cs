using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("최댓값"), SerializeField] int[] maxDamage = new int[3];

	#region Properties

	int I_MaxDamage
	{
		get
		{
			return ApplyMagicAffinity(maxDamage[i_upgraded]);
		}

		//set
		//{
		//	maxDamage[i_upgraded] = value;
		//}
	}

	#endregion

	public override string ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff0000>{4}</color>");
		sb.Replace("{4}", I_MaxDamage.ToString());

		explainTMP.text = sb.ToString();

		return sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		int damage = ApplyMagicAffinity(Random.Range(i_damage, maxDamage[i_upgraded] + 1)); // 여러 대상 간 데미지 통일

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, damage);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, damage);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
