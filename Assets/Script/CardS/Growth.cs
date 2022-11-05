using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : Card
{
	[Header("마나 친화성 증가 기준 비용"), SerializeField] int[] criteriaCost = new int[3];

	#region 프로퍼티

	public int i_criteriaCost
	{
		get
		{
			return criteriaCost[i_upgraded];
		}

		//set
		//{
		//	criteriaCost[i_upgraded] = value;
		//}
	}

	#endregion

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		// 왜 리스트에서 이 카드 못 찾지 일단 런

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}
}
