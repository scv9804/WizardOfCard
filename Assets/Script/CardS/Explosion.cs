using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Card, IAttack
{
	public int Damage
	{
		get { return 0; }
	}

	// <<22-10-28 장형용 :: 수정>>
	// <<22-11-09 장형용 :: 데미지 0을 주는 연타 공격으로 매커니즘 수정>>
	// <<22-11-24 장형용 :: 수정>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
		yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // 단일 대상
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy), _target_enemy.i_burning));
		}
		else if (_target_enemy == null && _target_player != null) // 자신 대상
		{
			StartCoroutine(Repeat(() => Attack(_target_player), _target_player.Debuff_Burning));
		}
		else // 광역 또는 무작위 대상 (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_player), _target_enemy.i_burning)), ref _target_enemy);
		}

		yield return StartCoroutine(EndUsingCard());
	}

	public void Attack(Entity _target)
	{
		if (!_target.is_die)
		{
			_target?.Damaged(Damage, enemyDamageSprite, this);

			StartCoroutine(PlayAttackSprite);

			MusicManager.inst.PlayerDefultSoundEffect();
		}
	}

	public void Attack(PlayerEntity _target)
	{
		_target?.Damaged(Damage, this);

		StartCoroutine(PlayAttackSprite);
		_target?.SetDamagedSprite(enemyDamageSprite);

		MusicManager.inst.PlayerDefultSoundEffect();
	}
}
