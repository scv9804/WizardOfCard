using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calm : Card
{
	[Header("카드 추가 데이터")]
	[Tooltip("추가 마나 친화성 증가량"), SerializeField] int[] extraManaAffinity = new int[3];

	#region Properties

	int I_ExtraManaAffinity_Battle
	{
		get
		{
			return ApplyEnhanceValue(extraManaAffinity[i_upgraded]);
		}

		//set
		//{
		//    I_ExtraManaAffinity_Battle = value;
		//}
	}

	int I_MagicAffinity_Battle
    {
		get
		{
			return ApplyEnhanceValue(i_damage);
		}

		//set
		//{
		//    I_MagicAffinity_Battle = value;
		//}
	}

	#endregion

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", (I_MagicAffinity_Battle + I_ExtraManaAffinity_Battle).ToString());

		explainTMP.text = sb.ToString();
	}

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		Add_MagicAffinity_Battle(I_MagicAffinity_Battle);

		for(int i = 0; i < CardManager.Inst.myCards.Count; i++)
        {
			if(CardManager.Inst.myCards[i].i_manaCost >= 3)
            {
				Add_MagicAffinity_Battle(I_ExtraManaAffinity_Battle);
				break;
			}
        }

		yield return StartCoroutine(EndUsingCard());
	}
}
