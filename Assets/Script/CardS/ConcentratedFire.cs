using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcentratedFire : Card, IAttack
{
	[Header("ī�� �߰� ���� ������")]
	[Tooltip("ī�� ������"), SerializeField] int[] damage = new int[3];

	public int Damage
	{
		get { return ApplyMagicAffinity(damage[i_upgraded]); }
	}

	int AttackCount
	{
		get { return MyHandCards.Count + 1; }
	}

	public override string GetCardExplain()
	{
		base.GetCardExplain();

		sb.Replace("{0}", "<color=#ff0000>{0}</color>");
		sb.Replace("{0}", Damage.ToString());

		return sb.ToString();
	}

	// <<22-10-28 ������ :: ����>>
	// <<22-11-24 ������ :: ����>>
	public override IEnumerator UseCard(Entity _target_enemy, PlayerEntity _target_player = null)
	{
        yield return StartCoroutine(base.UseCard(_target_enemy, _target_player));

		if (_target_enemy != null && _target_player == null) // ���� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_enemy), AttackCount));
		}
		else if (_target_enemy == null && _target_player != null) // �ڽ� ���
		{
			StartCoroutine(Repeat(() => Attack(_target_player), AttackCount));
		}
		else // ���� �Ǵ� ������ ��� (?)
		{
			TargetAll(() => StartCoroutine(Repeat(() => Attack(_target_enemy), AttackCount)), ref _target_enemy);
		}

		#region EndUsingCard

		CardManager.i_usingCardCount--;

		RefreshMyHandsExplain();

		yield return null;

		#endregion
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
