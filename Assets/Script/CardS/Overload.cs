using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Card
{

	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		yield return StartCoroutine(Repeat(() => CardManager.Inst.AddCard(), i_damage));

        PlayerEntity.Inst.Debuff_CannotDrawCard = true;

        yield return StartCoroutine(EndUsingCard());
	}
}
