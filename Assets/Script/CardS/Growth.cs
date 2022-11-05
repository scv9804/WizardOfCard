using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : Card
{
	[Header("���� ģȭ�� ���� ���� ���"), SerializeField] int[] criteriaCost = new int[3];

	#region ������Ƽ

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

	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		// �� ����Ʈ���� �� ī�� �� ã�� �ϴ� ��

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}
}
