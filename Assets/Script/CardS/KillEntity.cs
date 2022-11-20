using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEntity : Card
{
	// <<22-10-28 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			Attack(_target_enemy, 9999);

		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			Attack(_target_player, 9999);
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => Attack(_target_enemy, 9999), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
