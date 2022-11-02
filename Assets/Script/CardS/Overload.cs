using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 장형용 :: 수정>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		yield return StartCoroutine(Repeat(() => CardManager.Inst.AddCard(), i_damage));

		//PlayerEntity.Inst.b_cannotDrawCard = true; <<22-11-01 장형용 :: 일단 나중에 구현>>

		yield return StartCoroutine(EndUsingCard());
	}
}
