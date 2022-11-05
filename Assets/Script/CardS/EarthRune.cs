using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRune : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		Protection(i_damage);
		Shield(PlayerEntity.Inst.Status_Protection);

		yield return StartCoroutine(EndUsingCard());
	}
}
