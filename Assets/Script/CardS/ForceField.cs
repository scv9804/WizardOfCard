using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : Card
{
	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		Protection(i_damage);

		yield return StartCoroutine(EndUsingCard());
	}
}
