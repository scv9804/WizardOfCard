using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calm : Card
{
	[Header("추가 마나 친화성 증가량"), SerializeField] int[] extraManaAffinity = new int[3];

	#region 프로퍼티

	public int i_extraManaAffinity
	{
		get
		{
			return extraManaAffinity[i_upgraded];
		}

		//set
		//{
		//	extraManaAffinity[i_upgraded] = value;
		//}
	}

	#endregion

	public override void ExplainRefresh()
	{
		base.ExplainRefresh();

		sb.Replace("{4}", "<color=#ff00ff>{4}</color>");
		sb.Replace("{4}", ApplyEnhanceValue(i_extraManaAffinity + i_damage).ToString());

		explainTMP.text = sb.ToString();
	}

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		Add_MagicAffinity_Battle(i_damage);

		for(int i = 0; i < CardManager.Inst.myCards.Count; i++)
        {
			if(CardManager.Inst.myCards[i].i_manaCost >= 3)
            {
				Add_MagicAffinity_Battle(i_extraManaAffinity);
				break;
			}
        }

		yield return StartCoroutine(EndUsingCard());
	}
}
