using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRune : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		PlayerEntity.Inst.SpellEnchaneReset();

		for(int i = 0; i < EntityManager.Inst.enemyEntities.Count; i++)
        {
			RestoreHealth(i_damage);

			CardManager.Inst.AddCard();

			yield return new WaitForSeconds(0.15f);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
