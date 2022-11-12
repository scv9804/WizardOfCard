using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneRay : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("데미지 증가에 필요한 카드 수"), SerializeField] int[] damagePerCard = new int[3];

	#region Properties

	int I_DamagePerCard
    {
		get
        {
			return damagePerCard[i_upgraded];
		}

        //set
        //{
		//	damagePerCard[i_upgraded] = value;
		//}
    }

	int I_Damage
    {
		get
        {
			return ApplyMagicAffinity(i_damage + I_UsedCardCount / I_DamagePerCard);
		}

		//set
		//{
		//    I_Damage = value;
		//}
	}

    int I_UsedCardCount
    {
        get
        {
            return CardManager.i_usedCardCount;
        }

        //set
        //{
        //    damagePerCard[i_upgraded] = value;
        //}
    }

	#endregion

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{3}", I_DamagePerCard.ToString());

		sb.Replace("{4}", "<color=#ff0000>{4}</color>");
		sb.Replace("{4}", I_Damage.ToString());

		explainTMP.text = sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
    {
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, I_Damage);
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, I_Damage);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, I_Damage), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
