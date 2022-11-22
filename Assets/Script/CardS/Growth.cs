using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : Card
{
	[Header("ī�� �߰� ������")]
	[Tooltip("���� ģȭ�� ���� ���� ���"), SerializeField] int[] criteriaCost = new int[3];

	#region Properties

	#endregion

	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.ResetEnhanceValue();

		// �� ����Ʈ���� �� ī�� �� ã�� �ϴ� ��

		CardManager.Inst.myCards.Find((Card card) => { return this; });

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}

	public bool findcard(Card card)
    {
		return this;
    }
}
