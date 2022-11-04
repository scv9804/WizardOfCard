using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Card
{
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null) // <<22-10-28 ������ :: ����>>
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			StartCoroutine(Repeat(() => Explode(_target_enemy), _target_enemy.i_burning));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			StartCoroutine(Repeat(() => Explode(_target_player), _target_player.i_burning));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Explode(_target_enemy), _target_enemy.i_burning)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}

	void Explode(Entity _target)
    {
		if(!_target.is_die)
        {
			_target?.Burning();

			StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackSprite));
			StartCoroutine(_target?.DamagedEffectCorutin(enemyDamageSprite));
		}
	}

	void Explode(PlayerEntity _target)
	{
		_target?.Burning();

		StartCoroutine(PlayerEntity.Inst.AttackSprite(PlayerEntity.Inst.playerChar.MagicBoltSprite, playerAttackSprite));
		_target?.SetDamagedSprite(enemyDamageSprite);
	}
}
