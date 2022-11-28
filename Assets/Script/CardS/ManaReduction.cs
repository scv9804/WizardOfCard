using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaReduction : Card, IRestoreAether
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ���� ȸ�� ��ġ"), SerializeField] int[] aether = new int[3];

	int index;

	public int Aether
	{
		get { return aether[i_upgraded]; }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", Aether.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (MyHandCards.Count > 1)
        {
			do
			{
				index = Random.Range(0, MyHandCards.Count);

				if (MyHandCards[index] != this)
					break;
			}
			while (true);

			MyHandCards[index].i_manaCost++;

			RestoreAether();

			if (i_upgraded == 2)
				CardManager.Inst.AddCard();

			CardManager.Inst.RefreshMyHands();
		}

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
	}

	public void RestoreAether()
	{
		Player.Status_Aether += Aether;
	}
}
