using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneRay : Card
{
	[Header("데미지 증가에 필요한 카드"), SerializeField] int[] damagePerCard = new int[3];

    #region 프로퍼티

    public int i_damagePerCard
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

	public int i_usedCardCount
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

		sb.Replace("{3}", i_damagePerCard.ToString());

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyManaAffinity(i_damage + (int)(i_usedCardCount / i_damagePerCard)).ToString());

		explainTMP.text = sb.ToString();
	}

    public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
    {
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage + (int) (i_usedCardCount / i_damagePerCard)));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, ApplyManaAffinity_Instance(i_damage + (int)(i_usedCardCount / i_damagePerCard)));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, ApplyManaAffinity_Instance(i_damage + (int)(i_usedCardCount / i_damagePerCard))), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
