using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEntity : Card
{
	// <<22-10-28 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			Attack(_target_enemy, 9999);

		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			Attack(_target_player, 9999);
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => Attack(_target_enemy, 9999), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
