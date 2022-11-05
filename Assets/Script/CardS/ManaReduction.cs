using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaReduction : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		int random;

		do
		{
			random = UnityEngine.Random.Range(0, CardManager.Inst.myCards.Count);
			Debug.Log(random);

			if (CardManager.Inst.myCards[random] != this)
				break;
		} 
		while (true);

		CardManager.Inst.myCards[random].i_manaCost++;

		RestoreAether(i_damage);

		if (i_upgraded == 2)
			CardManager.Inst.AddCard();

		CardManager.Inst.RefreshMyHands();

		yield return StartCoroutine(EndUsingCard());
	}
}
