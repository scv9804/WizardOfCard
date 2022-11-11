using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRune : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("화상"), SerializeField] int[] applyBurning = new int[3];

	#region 프로퍼티

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

	int I_MagicAffinity_Turn
	{
		get
		{
			return ApplyMagicAffinity(i_damage);
		}

		//set
		//{
		//    I_MagicAffinity_Turn = value;
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

        PlayerEntity.Inst.SpellEnchaneReset();

        TargetAll(() => Add_Burning(_target_enemy, I_Burning), ref _target_enemy);
        Add_MagicAffinity_Turn(I_MagicAffinity_Turn);

        yield return StartCoroutine(EndUsingCard());
    }
}
