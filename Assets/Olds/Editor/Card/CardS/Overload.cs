using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Card
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ��ο� Ƚ��"), SerializeField] int[] drawCount = new int[3];

	int DrawCount
	{
		get { return drawCount[i_upgraded]; }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", DrawCount.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		for (int i = 0; i < DrawCount; i++)
		{
			CardManager.Inst.AddCard();

			yield return new WaitForSeconds(0.1f);
		}

		//yield return StartCoroutine(Repeat(() => CardManager.Inst.AddCard(), i_damage));

		Player.Debuff_CannotDrawCard = true;

		yield return StartCoroutine(EndUsingCard());
	}
}
