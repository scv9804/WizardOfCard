using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Card
{
	// <<22-10-28 ������ :: ����>>
	// <<22-11-09 ������ :: ������ 0�� �ִ� ��Ÿ �������� ��Ŀ���� ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy, 0), _target_enemy.i_burning));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_player, 0), _target_player.Debuff_Burning));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_player, 0), _target_enemy.i_burning)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}
}
