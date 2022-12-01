using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableTome : Card
{
	[Header("카드 추가 가변 데이터")]
	[Tooltip("카드 드로우 횟수"), SerializeField] int[] drawCount = new int[3];

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

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		for (int i = 0; i < DrawCount; i++)
        {
			AddCardAndCostDescrease();

			yield return new WaitForSeconds(0.1f);
		}

		yield return StartCoroutine(EndUsingCard());
	}

	protected void AddCardAndCostDescrease()
	{
		if (CardManager.Inst.myDeck.Count > 0)
		{
			CardManager.Inst.AddCard();

			MyHandCards[MyHandCards.Count].i_manaCost = 0;
			MyHandCards[MyHandCards.Count].b_isExile = true;
		}
	}
}
