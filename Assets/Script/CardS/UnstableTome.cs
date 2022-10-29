using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class UnstableTome : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		for (int i = 0; i < i_damage; i++)
        {
			AddCardAndCostDescrease();

			yield return new WaitForSeconds(0.1f);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
