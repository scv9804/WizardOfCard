using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("���� ģȭ�� ���� ���� ���"), SerializeField] int[] criteriaCost = new int[3];

	#region ������Ƽ

	#endregion

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		// �� ����Ʈ���� �� ī�� �� ã�� �ϴ� ��

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}
}
