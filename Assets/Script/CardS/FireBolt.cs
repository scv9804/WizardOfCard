using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBolt : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("화상"), SerializeField] int[] applyBurning = new int[3];

	#region Properties

	int I_Burning
	{
		get
		{
			return ApplyEnhanceValue(applyBurning[i_upgraded]);
		}

		//set
		//{
		//    I_Burning = value;
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

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyEnhanceValue(I_Burning).ToString());

		explainTMP.text = sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Add_Burning(_target_enemy, I_Burning);
			Attack(_target_enemy, I_Damage);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Add_Burning(_target_player, I_Burning);
			Attack(_target_player, I_Damage);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Add_Burning(_target_enemy, I_Burning), ref _target_enemy);
			TargetAll(() => Attack(_target_enemy, I_Damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
